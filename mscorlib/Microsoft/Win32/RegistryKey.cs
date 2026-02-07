using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Unity;

namespace Microsoft.Win32
{
	// Token: 0x020000A3 RID: 163
	public sealed class RegistryKey : MarshalByRefObject, IDisposable
	{
		// Token: 0x06000409 RID: 1033 RVA: 0x00015B49 File Offset: 0x00013D49
		private void ClosePerfDataKey()
		{
			Interop.Advapi32.RegCloseKey(RegistryKey.HKEY_PERFORMANCE_DATA);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00015B56 File Offset: 0x00013D56
		private void FlushCore()
		{
			if (this._hkey != null && this.IsDirty())
			{
				Interop.Advapi32.RegFlushKey(this._hkey);
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00015B78 File Offset: 0x00013D78
		private RegistryKey CreateSubKeyInternalCore(string subkey, RegistryKeyPermissionCheck permissionCheck, object registrySecurityObj, RegistryOptions registryOptions)
		{
			Interop.Kernel32.SECURITY_ATTRIBUTES security_ATTRIBUTES = default(Interop.Kernel32.SECURITY_ATTRIBUTES);
			int num = 0;
			SafeRegistryHandle safeRegistryHandle = null;
			int num2 = Interop.Advapi32.RegCreateKeyEx(this._hkey, subkey, 0, null, (int)registryOptions, RegistryKey.GetRegistryKeyAccess(permissionCheck != RegistryKeyPermissionCheck.ReadSubTree) | (int)this._regView, ref security_ATTRIBUTES, out safeRegistryHandle, out num);
			if (num2 == 0 && !safeRegistryHandle.IsInvalid)
			{
				RegistryKey registryKey = new RegistryKey(safeRegistryHandle, permissionCheck != RegistryKeyPermissionCheck.ReadSubTree, false, this._remoteKey, false, this._regView);
				registryKey._checkMode = permissionCheck;
				if (subkey.Length == 0)
				{
					registryKey._keyName = this._keyName;
				}
				else
				{
					registryKey._keyName = this._keyName + "\\" + subkey;
				}
				return registryKey;
			}
			if (num2 != 0)
			{
				this.Win32Error(num2, this._keyName + "\\" + subkey);
			}
			return null;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00015C50 File Offset: 0x00013E50
		private void DeleteSubKeyCore(string subkey, bool throwOnMissingSubKey)
		{
			int num = Interop.Advapi32.RegDeleteKeyEx(this._hkey, subkey, (int)this._regView, 0);
			if (num != 0)
			{
				if (num == 2)
				{
					if (throwOnMissingSubKey)
					{
						ThrowHelper.ThrowArgumentException("Cannot delete a subkey tree because the subkey does not exist.");
						return;
					}
				}
				else
				{
					this.Win32Error(num, null);
				}
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00015C94 File Offset: 0x00013E94
		private void DeleteSubKeyTreeCore(string subkey)
		{
			int num = Interop.Advapi32.RegDeleteKeyEx(this._hkey, subkey, (int)this._regView, 0);
			if (num != 0)
			{
				this.Win32Error(num, null);
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00015CC4 File Offset: 0x00013EC4
		private void DeleteValueCore(string name, bool throwOnMissingValue)
		{
			int num = Interop.Advapi32.RegDeleteValue(this._hkey, name);
			if (num == 2 || num == 206)
			{
				if (throwOnMissingValue)
				{
					ThrowHelper.ThrowArgumentException("No value exists with that name.");
					return;
				}
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00015CFC File Offset: 0x00013EFC
		private static RegistryKey OpenBaseKeyCore(RegistryHive hKeyHive, RegistryView view)
		{
			IntPtr intPtr = (IntPtr)((int)hKeyHive);
			int num = (int)intPtr & 268435455;
			bool flag = intPtr == RegistryKey.HKEY_PERFORMANCE_DATA;
			return new RegistryKey(new SafeRegistryHandle(intPtr, flag), true, true, false, flag, view)
			{
				_checkMode = RegistryKeyPermissionCheck.Default,
				_keyName = RegistryKey.s_hkeyNames[num]
			};
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00015D50 File Offset: 0x00013F50
		private static RegistryKey OpenRemoteBaseKeyCore(RegistryHive hKey, string machineName, RegistryView view)
		{
			int num = (int)(hKey & (RegistryHive)268435455);
			if (num < 0 || num >= RegistryKey.s_hkeyNames.Length || ((long)hKey & (long)((ulong)-16)) != (long)((ulong)-2147483648))
			{
				throw new ArgumentException("Registry HKEY was out of the legal range.");
			}
			SafeRegistryHandle safeRegistryHandle = null;
			int num2 = Interop.Advapi32.RegConnectRegistry(machineName, new SafeRegistryHandle(new IntPtr((int)hKey), false), out safeRegistryHandle);
			if (num2 == 1114)
			{
				throw new ArgumentException("One machine may not have remote administration enabled, or both machines may not be running the remote registry service.");
			}
			if (num2 != 0)
			{
				RegistryKey.Win32ErrorStatic(num2, null);
			}
			if (safeRegistryHandle.IsInvalid)
			{
				throw new ArgumentException(SR.Format("No remote connection to '{0}' while trying to read the registry.", machineName));
			}
			return new RegistryKey(safeRegistryHandle, true, false, true, (IntPtr)((int)hKey) == RegistryKey.HKEY_PERFORMANCE_DATA, view)
			{
				_checkMode = RegistryKeyPermissionCheck.Default,
				_keyName = RegistryKey.s_hkeyNames[num]
			};
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00015E0C File Offset: 0x0001400C
		private RegistryKey InternalOpenSubKeyCore(string name, RegistryKeyPermissionCheck permissionCheck, int rights, bool throwOnPermissionFailure)
		{
			SafeRegistryHandle safeRegistryHandle = null;
			int num = Interop.Advapi32.RegOpenKeyEx(this._hkey, name, 0, rights | (int)this._regView, out safeRegistryHandle);
			if (num == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new RegistryKey(safeRegistryHandle, permissionCheck == RegistryKeyPermissionCheck.ReadWriteSubTree, false, this._remoteKey, false, this._regView)
				{
					_keyName = this._keyName + "\\" + name,
					_checkMode = permissionCheck
				};
			}
			if (throwOnPermissionFailure && (num == 5 || num == 1346))
			{
				ThrowHelper.ThrowSecurityException("Requested registry access is not allowed.");
			}
			return null;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00015EA0 File Offset: 0x000140A0
		private RegistryKey InternalOpenSubKeyCore(string name, bool writable, bool throwOnPermissionFailure)
		{
			SafeRegistryHandle safeRegistryHandle = null;
			int num = Interop.Advapi32.RegOpenKeyEx(this._hkey, name, 0, RegistryKey.GetRegistryKeyAccess(writable) | (int)this._regView, out safeRegistryHandle);
			if (num == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new RegistryKey(safeRegistryHandle, writable, false, this._remoteKey, false, this._regView)
				{
					_checkMode = this.GetSubKeyPermissionCheck(writable),
					_keyName = this._keyName + "\\" + name
				};
			}
			if (throwOnPermissionFailure && (num == 5 || num == 1346))
			{
				ThrowHelper.ThrowSecurityException("Requested registry access is not allowed.");
			}
			return null;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00015F3C File Offset: 0x0001413C
		internal RegistryKey InternalOpenSubKeyWithoutSecurityChecksCore(string name, bool writable)
		{
			SafeRegistryHandle safeRegistryHandle = null;
			if (Interop.Advapi32.RegOpenKeyEx(this._hkey, name, 0, RegistryKey.GetRegistryKeyAccess(writable) | (int)this._regView, out safeRegistryHandle) == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new RegistryKey(safeRegistryHandle, writable, false, this._remoteKey, false, this._regView)
				{
					_keyName = this._keyName + "\\" + name
				};
			}
			return null;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00015FAC File Offset: 0x000141AC
		private SafeRegistryHandle SystemKeyHandle
		{
			get
			{
				int num = 6;
				IntPtr hKey = (IntPtr)0;
				string keyName = this._keyName;
				if (!(keyName == "HKEY_CLASSES_ROOT"))
				{
					if (!(keyName == "HKEY_CURRENT_USER"))
					{
						if (!(keyName == "HKEY_LOCAL_MACHINE"))
						{
							if (!(keyName == "HKEY_USERS"))
							{
								if (!(keyName == "HKEY_PERFORMANCE_DATA"))
								{
									if (!(keyName == "HKEY_CURRENT_CONFIG"))
									{
										this.Win32Error(num, null);
									}
									else
									{
										hKey = RegistryKey.HKEY_CURRENT_CONFIG;
									}
								}
								else
								{
									hKey = RegistryKey.HKEY_PERFORMANCE_DATA;
								}
							}
							else
							{
								hKey = RegistryKey.HKEY_USERS;
							}
						}
						else
						{
							hKey = RegistryKey.HKEY_LOCAL_MACHINE;
						}
					}
					else
					{
						hKey = RegistryKey.HKEY_CURRENT_USER;
					}
				}
				else
				{
					hKey = RegistryKey.HKEY_CLASSES_ROOT;
				}
				SafeRegistryHandle safeRegistryHandle;
				num = Interop.Advapi32.RegOpenKeyEx(hKey, null, 0, RegistryKey.GetRegistryKeyAccess(this.IsWritable()) | (int)this._regView, out safeRegistryHandle);
				if (num == 0 && !safeRegistryHandle.IsInvalid)
				{
					return safeRegistryHandle;
				}
				this.Win32Error(num, null);
				throw new IOException(Interop.Kernel32.GetMessage(num), num);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00016094 File Offset: 0x00014294
		private int InternalSubKeyCountCore()
		{
			int result = 0;
			int num = 0;
			int num2 = Interop.Advapi32.RegQueryInfoKey(this._hkey, null, null, IntPtr.Zero, ref result, null, null, ref num, null, null, null, null);
			if (num2 != 0)
			{
				this.Win32Error(num2, null);
			}
			return result;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000160D0 File Offset: 0x000142D0
		private string[] InternalGetSubKeyNamesCore(int subkeys)
		{
			List<string> list = new List<string>(subkeys);
			char[] array = ArrayPool<char>.Shared.Rent(256);
			try
			{
				int length = array.Length;
				int num;
				while ((num = Interop.Advapi32.RegEnumKeyEx(this._hkey, list.Count, array, ref length, null, null, null, null)) != 259)
				{
					if (num == 0)
					{
						list.Add(new string(array, 0, length));
						length = array.Length;
					}
					else
					{
						this.Win32Error(num, null);
					}
				}
			}
			finally
			{
				ArrayPool<char>.Shared.Return(array, false);
			}
			return list.ToArray();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00016164 File Offset: 0x00014364
		private int InternalValueCountCore()
		{
			int result = 0;
			int num = 0;
			int num2 = Interop.Advapi32.RegQueryInfoKey(this._hkey, null, null, IntPtr.Zero, ref num, null, null, ref result, null, null, null, null);
			if (num2 != 0)
			{
				this.Win32Error(num2, null);
			}
			return result;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000161A0 File Offset: 0x000143A0
		private unsafe string[] GetValueNamesCore(int values)
		{
			List<string> list = new List<string>(values);
			char[] array = ArrayPool<char>.Shared.Rent(100);
			try
			{
				int length = array.Length;
				int num;
				while ((num = Interop.Advapi32.RegEnumValue(this._hkey, list.Count, array, ref length, IntPtr.Zero, null, null, null)) != 259)
				{
					if (num != 0)
					{
						if (num != 234)
						{
							this.Win32Error(num, null);
						}
						else
						{
							if (this.IsPerfDataKey())
							{
								try
								{
									fixed (char* ptr = &array[0])
									{
										char* value = ptr;
										list.Add(new string(value));
										goto IL_92;
									}
								}
								finally
								{
									char* ptr = null;
								}
							}
							char[] array2 = array;
							int num2 = array2.Length;
							array = null;
							ArrayPool<char>.Shared.Return(array2, false);
							array = ArrayPool<char>.Shared.Rent(checked(num2 * 2));
						}
					}
					else
					{
						list.Add(new string(array, 0, length));
					}
					IL_92:
					length = array.Length;
				}
			}
			finally
			{
				if (array != null)
				{
					ArrayPool<char>.Shared.Return(array, false);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000162A4 File Offset: 0x000144A4
		private object InternalGetValueCore(string name, object defaultValue, bool doNotExpand)
		{
			object obj = defaultValue;
			int num = 0;
			int num2 = 0;
			int num3 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, null, ref num2);
			if (num3 != 0)
			{
				if (this.IsPerfDataKey())
				{
					int num4 = 65000;
					int num5 = num4;
					byte[] array = new byte[num4];
					int num6;
					while (234 == (num6 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, array, ref num5)))
					{
						if (num4 == 2147483647)
						{
							this.Win32Error(num6, name);
						}
						else if (num4 > 1073741823)
						{
							num4 = int.MaxValue;
						}
						else
						{
							num4 *= 2;
						}
						num5 = num4;
						array = new byte[num4];
					}
					if (num6 != 0)
					{
						this.Win32Error(num6, name);
					}
					return array;
				}
				if (num3 != 234)
				{
					return obj;
				}
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			switch (num)
			{
			case 0:
			case 3:
			case 5:
				break;
			case 1:
			{
				char[] array2;
				checked
				{
					if (num2 % 2 == 1)
					{
						try
						{
							num2++;
						}
						catch (OverflowException innerException)
						{
							throw new IOException("RegistryKey.GetValue does not allow a String that has a length greater than Int32.MaxValue.", innerException);
						}
					}
					array2 = new char[num2 / 2];
					num3 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, array2, ref num2);
				}
				if (array2.Length != 0 && array2[array2.Length - 1] == '\0')
				{
					return new string(array2, 0, array2.Length - 1);
				}
				return new string(array2);
			}
			case 2:
			{
				char[] array3;
				checked
				{
					if (num2 % 2 == 1)
					{
						try
						{
							num2++;
						}
						catch (OverflowException innerException2)
						{
							throw new IOException("RegistryKey.GetValue does not allow a String that has a length greater than Int32.MaxValue.", innerException2);
						}
					}
					array3 = new char[num2 / 2];
					num3 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, array3, ref num2);
				}
				if (array3.Length != 0 && array3[array3.Length - 1] == '\0')
				{
					obj = new string(array3, 0, array3.Length - 1);
				}
				else
				{
					obj = new string(array3);
				}
				if (!doNotExpand)
				{
					return Environment.ExpandEnvironmentVariables((string)obj);
				}
				return obj;
			}
			case 4:
				if (num2 <= 4)
				{
					int num7 = 0;
					num3 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, ref num7, ref num2);
					return num7;
				}
				goto IL_118;
			case 6:
			case 8:
			case 9:
			case 10:
				return obj;
			case 7:
			{
				char[] array4;
				checked
				{
					if (num2 % 2 == 1)
					{
						try
						{
							num2++;
						}
						catch (OverflowException innerException3)
						{
							throw new IOException("RegistryKey.GetValue does not allow a String that has a length greater than Int32.MaxValue.", innerException3);
						}
					}
					array4 = new char[num2 / 2];
					num3 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, array4, ref num2);
				}
				if (array4.Length != 0 && array4[array4.Length - 1] != '\0')
				{
					Array.Resize<char>(ref array4, array4.Length + 1);
				}
				string[] array5 = Array.Empty<string>();
				int num8 = 0;
				int num9 = 0;
				int num10 = array4.Length;
				while (num3 == 0 && num9 < num10)
				{
					int num11 = num9;
					while (num11 < num10 && array4[num11] != '\0')
					{
						num11++;
					}
					string text = null;
					if (num11 < num10)
					{
						if (num11 - num9 > 0)
						{
							text = new string(array4, num9, num11 - num9);
						}
						else if (num11 != num10 - 1)
						{
							text = string.Empty;
						}
					}
					else
					{
						text = new string(array4, num9, num10 - num9);
					}
					num9 = num11 + 1;
					if (text != null)
					{
						if (array5.Length == num8)
						{
							Array.Resize<string>(ref array5, (num8 > 0) ? (num8 * 2) : 4);
						}
						array5[num8++] = text;
					}
				}
				Array.Resize<string>(ref array5, num8);
				return array5;
			}
			case 11:
				goto IL_118;
			default:
				return obj;
			}
			IL_F2:
			byte[] array6 = new byte[num2];
			num3 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, array6, ref num2);
			return array6;
			IL_118:
			if (num2 > 8)
			{
				goto IL_F2;
			}
			long num12 = 0L;
			num3 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, ref num12, ref num2);
			obj = num12;
			return obj;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00016644 File Offset: 0x00014844
		private RegistryValueKind GetValueKindCore(string name)
		{
			int num = 0;
			int num2 = 0;
			int num3 = Interop.Advapi32.RegQueryValueEx(this._hkey, name, null, ref num, null, ref num2);
			if (num3 != 0)
			{
				this.Win32Error(num3, null);
			}
			if (num == 0)
			{
				return RegistryValueKind.None;
			}
			if (Enum.IsDefined(typeof(RegistryValueKind), num))
			{
				return (RegistryValueKind)num;
			}
			return RegistryValueKind.Unknown;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00016694 File Offset: 0x00014894
		private void SetValueCore(string name, object value, RegistryValueKind valueKind)
		{
			int num = 0;
			try
			{
				switch (valueKind)
				{
				case RegistryValueKind.None:
				case RegistryValueKind.Binary:
				{
					byte[] array = (byte[])value;
					num = Interop.Advapi32.RegSetValueEx(this._hkey, name, 0, (valueKind == RegistryValueKind.None) ? RegistryValueKind.Unknown : RegistryValueKind.Binary, array, array.Length);
					break;
				}
				case RegistryValueKind.String:
				case RegistryValueKind.ExpandString:
				{
					string text = value.ToString();
					num = Interop.Advapi32.RegSetValueEx(this._hkey, name, 0, valueKind, text, checked(text.Length * 2 + 2));
					break;
				}
				case RegistryValueKind.DWord:
				{
					int num2 = Convert.ToInt32(value, CultureInfo.InvariantCulture);
					num = Interop.Advapi32.RegSetValueEx(this._hkey, name, 0, RegistryValueKind.DWord, ref num2, 4);
					break;
				}
				case RegistryValueKind.MultiString:
				{
					string[] array2 = (string[])((string[])value).Clone();
					int num3 = 1;
					for (int i = 0; i < array2.Length; i++)
					{
						if (array2[i] == null)
						{
							ThrowHelper.ThrowArgumentException("RegistryKey.SetValue does not allow a String[] that contains a null String reference.");
						}
						checked
						{
							num3 += array2[i].Length + 1;
						}
					}
					int cbData = checked(num3 * 2);
					char[] array3 = new char[num3];
					int num4 = 0;
					for (int j = 0; j < array2.Length; j++)
					{
						int length = array2[j].Length;
						array2[j].CopyTo(0, array3, num4, length);
						num4 += length + 1;
					}
					num = Interop.Advapi32.RegSetValueEx(this._hkey, name, 0, RegistryValueKind.MultiString, array3, cbData);
					break;
				}
				case RegistryValueKind.QWord:
				{
					long num5 = Convert.ToInt64(value, CultureInfo.InvariantCulture);
					num = Interop.Advapi32.RegSetValueEx(this._hkey, name, 0, RegistryValueKind.QWord, ref num5, 8);
					break;
				}
				}
			}
			catch (Exception ex) when (ex is OverflowException || ex is InvalidOperationException || ex is FormatException || ex is InvalidCastException)
			{
				ThrowHelper.ThrowArgumentException("The type of the value object did not match the specified RegistryValueKind or the object could not be properly converted.");
			}
			if (num == 0)
			{
				this.SetDirty();
				return;
			}
			this.Win32Error(num, null);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00016894 File Offset: 0x00014A94
		private void Win32Error(int errorCode, string str)
		{
			switch (errorCode)
			{
			case 2:
				throw new IOException("The specified registry key does not exist.", errorCode);
			case 5:
				throw (str != null) ? new UnauthorizedAccessException(SR.Format("Access to the registry key '{0}' is denied.", str)) : new UnauthorizedAccessException();
			case 6:
				if (!this.IsPerfDataKey())
				{
					this._hkey.SetHandleAsInvalid();
					this._hkey = null;
				}
				break;
			}
			throw new IOException(Interop.Kernel32.GetMessage(errorCode), errorCode);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00016912 File Offset: 0x00014B12
		private static void Win32ErrorStatic(int errorCode, string str)
		{
			if (errorCode == 5)
			{
				throw (str != null) ? new UnauthorizedAccessException(SR.Format("Access to the registry key '{0}' is denied.", str)) : new UnauthorizedAccessException();
			}
			throw new IOException(Interop.Kernel32.GetMessage(errorCode), errorCode);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00016940 File Offset: 0x00014B40
		private static int GetRegistryKeyAccess(bool isWritable)
		{
			int result;
			if (!isWritable)
			{
				result = 131097;
			}
			else
			{
				result = 131103;
			}
			return result;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00016960 File Offset: 0x00014B60
		private static int GetRegistryKeyAccess(RegistryKeyPermissionCheck mode)
		{
			int result = 0;
			if (mode > RegistryKeyPermissionCheck.ReadSubTree)
			{
				if (mode == RegistryKeyPermissionCheck.ReadWriteSubTree)
				{
					result = 131103;
				}
			}
			else
			{
				result = 131097;
			}
			return result;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00016988 File Offset: 0x00014B88
		private RegistryKey(SafeRegistryHandle hkey, bool writable, RegistryView view) : this(hkey, writable, false, false, false, view)
		{
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00016998 File Offset: 0x00014B98
		private RegistryKey(SafeRegistryHandle hkey, bool writable, bool systemkey, bool remoteKey, bool isPerfData, RegistryView view)
		{
			RegistryKey.ValidateKeyView(view);
			this._hkey = hkey;
			this._keyName = "";
			this._remoteKey = remoteKey;
			this._regView = view;
			if (systemkey)
			{
				this._state |= RegistryKey.StateFlags.SystemKey;
			}
			if (writable)
			{
				this._state |= RegistryKey.StateFlags.WriteAccess;
			}
			if (isPerfData)
			{
				this._state |= RegistryKey.StateFlags.PerfData;
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00016A1C File Offset: 0x00014C1C
		public void Flush()
		{
			this.FlushCore();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00016A24 File Offset: 0x00014C24
		public void Close()
		{
			this.Dispose();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00016A2C File Offset: 0x00014C2C
		public void Dispose()
		{
			if (this._hkey != null)
			{
				if (!this.IsSystemKey())
				{
					try
					{
						this._hkey.Dispose();
						return;
					}
					catch (IOException)
					{
						return;
					}
					finally
					{
						this._hkey = null;
					}
				}
				if (this.IsPerfDataKey())
				{
					this.ClosePerfDataKey();
				}
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00016A94 File Offset: 0x00014C94
		public RegistryKey CreateSubKey(string subkey)
		{
			return this.CreateSubKey(subkey, this._checkMode);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00016AA5 File Offset: 0x00014CA5
		public RegistryKey CreateSubKey(string subkey, bool writable)
		{
			return this.CreateSubKeyInternal(subkey, writable ? RegistryKeyPermissionCheck.ReadWriteSubTree : RegistryKeyPermissionCheck.ReadSubTree, null, RegistryOptions.None);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00016AB7 File Offset: 0x00014CB7
		public RegistryKey CreateSubKey(string subkey, bool writable, RegistryOptions options)
		{
			return this.CreateSubKeyInternal(subkey, writable ? RegistryKeyPermissionCheck.ReadWriteSubTree : RegistryKeyPermissionCheck.ReadSubTree, null, options);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00016AC9 File Offset: 0x00014CC9
		public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
		{
			return this.CreateSubKeyInternal(subkey, permissionCheck, null, RegistryOptions.None);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00016AD5 File Offset: 0x00014CD5
		public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions)
		{
			return this.CreateSubKeyInternal(subkey, permissionCheck, null, registryOptions);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00016AE1 File Offset: 0x00014CE1
		public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, RegistrySecurity registrySecurity)
		{
			return this.CreateSubKeyInternal(subkey, permissionCheck, registrySecurity, registryOptions);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00016AEE File Offset: 0x00014CEE
		public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
		{
			return this.CreateSubKeyInternal(subkey, permissionCheck, registrySecurity, RegistryOptions.None);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00016AFC File Offset: 0x00014CFC
		private RegistryKey CreateSubKeyInternal(string subkey, RegistryKeyPermissionCheck permissionCheck, object registrySecurityObj, RegistryOptions registryOptions)
		{
			RegistryKey.ValidateKeyOptions(registryOptions);
			RegistryKey.ValidateKeyName(subkey);
			RegistryKey.ValidateKeyMode(permissionCheck);
			this.EnsureWriteable();
			subkey = RegistryKey.FixupName(subkey);
			if (!this._remoteKey)
			{
				RegistryKey registryKey = this.InternalOpenSubKeyWithoutSecurityChecks(subkey, permissionCheck != RegistryKeyPermissionCheck.ReadSubTree);
				if (registryKey != null)
				{
					registryKey._checkMode = permissionCheck;
					return registryKey;
				}
			}
			return this.CreateSubKeyInternalCore(subkey, permissionCheck, registrySecurityObj, registryOptions);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00016B5C File Offset: 0x00014D5C
		public void DeleteSubKey(string subkey)
		{
			this.DeleteSubKey(subkey, true);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00016B68 File Offset: 0x00014D68
		public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
		{
			RegistryKey.ValidateKeyName(subkey);
			this.EnsureWriteable();
			subkey = RegistryKey.FixupName(subkey);
			RegistryKey registryKey = this.InternalOpenSubKeyWithoutSecurityChecks(subkey, false);
			if (registryKey != null)
			{
				using (registryKey)
				{
					if (registryKey.InternalSubKeyCount() > 0)
					{
						ThrowHelper.ThrowInvalidOperationException("Registry key has subkeys and recursive removes are not supported by this method.");
					}
				}
				this.DeleteSubKeyCore(subkey, throwOnMissingSubKey);
				return;
			}
			if (throwOnMissingSubKey)
			{
				ThrowHelper.ThrowArgumentException("Cannot delete a subkey tree because the subkey does not exist.");
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00016BDC File Offset: 0x00014DDC
		public void DeleteSubKeyTree(string subkey)
		{
			this.DeleteSubKeyTree(subkey, true);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00016BE8 File Offset: 0x00014DE8
		public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
		{
			RegistryKey.ValidateKeyName(subkey);
			if (subkey.Length == 0 && this.IsSystemKey())
			{
				ThrowHelper.ThrowArgumentException("Cannot delete a registry hive's subtree.");
			}
			this.EnsureWriteable();
			subkey = RegistryKey.FixupName(subkey);
			RegistryKey registryKey = this.InternalOpenSubKeyWithoutSecurityChecks(subkey, true);
			if (registryKey != null)
			{
				using (registryKey)
				{
					if (registryKey.InternalSubKeyCount() > 0)
					{
						string[] array = registryKey.InternalGetSubKeyNames();
						for (int i = 0; i < array.Length; i++)
						{
							registryKey.DeleteSubKeyTreeInternal(array[i]);
						}
					}
				}
				this.DeleteSubKeyTreeCore(subkey);
				return;
			}
			if (throwOnMissingSubKey)
			{
				ThrowHelper.ThrowArgumentException("Cannot delete a subkey tree because the subkey does not exist.");
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00016C8C File Offset: 0x00014E8C
		private void DeleteSubKeyTreeInternal(string subkey)
		{
			RegistryKey registryKey = this.InternalOpenSubKeyWithoutSecurityChecks(subkey, true);
			if (registryKey != null)
			{
				using (registryKey)
				{
					if (registryKey.InternalSubKeyCount() > 0)
					{
						string[] array = registryKey.InternalGetSubKeyNames();
						for (int i = 0; i < array.Length; i++)
						{
							registryKey.DeleteSubKeyTreeInternal(array[i]);
						}
					}
				}
				this.DeleteSubKeyTreeCore(subkey);
				return;
			}
			ThrowHelper.ThrowArgumentException("Cannot delete a subkey tree because the subkey does not exist.");
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00016CFC File Offset: 0x00014EFC
		public void DeleteValue(string name)
		{
			this.DeleteValue(name, true);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00016D06 File Offset: 0x00014F06
		public void DeleteValue(string name, bool throwOnMissingValue)
		{
			this.EnsureWriteable();
			this.DeleteValueCore(name, throwOnMissingValue);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00016D16 File Offset: 0x00014F16
		public static RegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
		{
			RegistryKey.ValidateKeyView(view);
			return RegistryKey.OpenBaseKeyCore(hKey, view);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00016D25 File Offset: 0x00014F25
		public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
		{
			return RegistryKey.OpenRemoteBaseKey(hKey, machineName, RegistryView.Default);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00016D2F File Offset: 0x00014F2F
		public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName, RegistryView view)
		{
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			RegistryKey.ValidateKeyView(view);
			return RegistryKey.OpenRemoteBaseKeyCore(hKey, machineName, view);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00016D4D File Offset: 0x00014F4D
		public RegistryKey OpenSubKey(string name)
		{
			return this.OpenSubKey(name, false);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00016D57 File Offset: 0x00014F57
		public RegistryKey OpenSubKey(string name, bool writable)
		{
			RegistryKey.ValidateKeyName(name);
			this.EnsureNotDisposed();
			name = RegistryKey.FixupName(name);
			return this.InternalOpenSubKeyCore(name, writable, true);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00016D76 File Offset: 0x00014F76
		public RegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
		{
			RegistryKey.ValidateKeyMode(permissionCheck);
			return this.InternalOpenSubKey(name, permissionCheck, RegistryKey.GetRegistryKeyAccess(permissionCheck));
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00016D8C File Offset: 0x00014F8C
		public RegistryKey OpenSubKey(string name, RegistryRights rights)
		{
			return this.InternalOpenSubKey(name, this._checkMode, (int)rights);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00016D9E File Offset: 0x00014F9E
		public RegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
		{
			return this.InternalOpenSubKey(name, permissionCheck, (int)rights);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00016DA9 File Offset: 0x00014FA9
		private RegistryKey InternalOpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, int rights)
		{
			RegistryKey.ValidateKeyName(name);
			RegistryKey.ValidateKeyMode(permissionCheck);
			RegistryKey.ValidateKeyRights(rights);
			this.EnsureNotDisposed();
			name = RegistryKey.FixupName(name);
			return this.InternalOpenSubKeyCore(name, permissionCheck, rights, true);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00016DD5 File Offset: 0x00014FD5
		internal RegistryKey InternalOpenSubKeyWithoutSecurityChecks(string name, bool writable)
		{
			RegistryKey.ValidateKeyName(name);
			this.EnsureNotDisposed();
			return this.InternalOpenSubKeyWithoutSecurityChecksCore(name, writable);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00016DEB File Offset: 0x00014FEB
		public RegistrySecurity GetAccessControl()
		{
			return this.GetAccessControl(AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00016DF5 File Offset: 0x00014FF5
		public RegistrySecurity GetAccessControl(AccessControlSections includeSections)
		{
			this.EnsureNotDisposed();
			return new RegistrySecurity(this.Handle, this.Name, includeSections);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00016E0F File Offset: 0x0001500F
		public void SetAccessControl(RegistrySecurity registrySecurity)
		{
			this.EnsureWriteable();
			if (registrySecurity == null)
			{
				throw new ArgumentNullException("registrySecurity");
			}
			registrySecurity.Persist(this.Handle, this.Name);
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00016E37 File Offset: 0x00015037
		public int SubKeyCount
		{
			get
			{
				return this.InternalSubKeyCount();
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00016E3F File Offset: 0x0001503F
		public RegistryView View
		{
			get
			{
				this.EnsureNotDisposed();
				return this._regView;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00016E4F File Offset: 0x0001504F
		public SafeRegistryHandle Handle
		{
			get
			{
				this.EnsureNotDisposed();
				if (!this.IsSystemKey())
				{
					return this._hkey;
				}
				return this.SystemKeyHandle;
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00016E6E File Offset: 0x0001506E
		public static RegistryKey FromHandle(SafeRegistryHandle handle)
		{
			return RegistryKey.FromHandle(handle, RegistryView.Default);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00016E77 File Offset: 0x00015077
		public static RegistryKey FromHandle(SafeRegistryHandle handle, RegistryView view)
		{
			if (handle == null)
			{
				throw new ArgumentNullException("handle");
			}
			RegistryKey.ValidateKeyView(view);
			return new RegistryKey(handle, true, view);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00016E95 File Offset: 0x00015095
		private int InternalSubKeyCount()
		{
			this.EnsureNotDisposed();
			return this.InternalSubKeyCountCore();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00016EA3 File Offset: 0x000150A3
		public string[] GetSubKeyNames()
		{
			return this.InternalGetSubKeyNames();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00016EAC File Offset: 0x000150AC
		private string[] InternalGetSubKeyNames()
		{
			this.EnsureNotDisposed();
			int num = this.InternalSubKeyCount();
			if (num <= 0)
			{
				return Array.Empty<string>();
			}
			return this.InternalGetSubKeyNamesCore(num);
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00016ED7 File Offset: 0x000150D7
		public int ValueCount
		{
			get
			{
				return this.InternalValueCount();
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00016EDF File Offset: 0x000150DF
		private int InternalValueCount()
		{
			this.EnsureNotDisposed();
			return this.InternalValueCountCore();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00016EF0 File Offset: 0x000150F0
		public string[] GetValueNames()
		{
			this.EnsureNotDisposed();
			int num = this.InternalValueCount();
			if (num <= 0)
			{
				return Array.Empty<string>();
			}
			return this.GetValueNamesCore(num);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00016F1B File Offset: 0x0001511B
		public object GetValue(string name)
		{
			return this.InternalGetValue(name, null, false, true);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00016F27 File Offset: 0x00015127
		public object GetValue(string name, object defaultValue)
		{
			return this.InternalGetValue(name, defaultValue, false, true);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00016F34 File Offset: 0x00015134
		public object GetValue(string name, object defaultValue, RegistryValueOptions options)
		{
			if (options < RegistryValueOptions.None || options > RegistryValueOptions.DoNotExpandEnvironmentNames)
			{
				throw new ArgumentException(SR.Format("Illegal enum value: {0}.", (int)options), "options");
			}
			bool doNotExpand = options == RegistryValueOptions.DoNotExpandEnvironmentNames;
			return this.InternalGetValue(name, defaultValue, doNotExpand, true);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00016F73 File Offset: 0x00015173
		private object InternalGetValue(string name, object defaultValue, bool doNotExpand, bool checkSecurity)
		{
			if (checkSecurity)
			{
				this.EnsureNotDisposed();
			}
			return this.InternalGetValueCore(name, defaultValue, doNotExpand);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00016F88 File Offset: 0x00015188
		public RegistryValueKind GetValueKind(string name)
		{
			this.EnsureNotDisposed();
			return this.GetValueKindCore(name);
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00016F97 File Offset: 0x00015197
		public string Name
		{
			get
			{
				this.EnsureNotDisposed();
				return this._keyName;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00016FA7 File Offset: 0x000151A7
		public void SetValue(string name, object value)
		{
			this.SetValue(name, value, RegistryValueKind.Unknown);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00016FB4 File Offset: 0x000151B4
		public void SetValue(string name, object value, RegistryValueKind valueKind)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException("value");
			}
			if (name != null && name.Length > 16383)
			{
				throw new ArgumentException("Registry value names should not be greater than 16,383 characters.", "name");
			}
			if (!Enum.IsDefined(typeof(RegistryValueKind), valueKind))
			{
				throw new ArgumentException("The specified RegistryValueKind is an invalid value.", "valueKind");
			}
			this.EnsureWriteable();
			if (valueKind == RegistryValueKind.Unknown)
			{
				valueKind = this.CalculateValueKind(value);
			}
			this.SetValueCore(name, value, valueKind);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00017030 File Offset: 0x00015230
		private RegistryValueKind CalculateValueKind(object value)
		{
			if (value is int)
			{
				return RegistryValueKind.DWord;
			}
			if (!(value is Array))
			{
				return RegistryValueKind.String;
			}
			if (value is byte[])
			{
				return RegistryValueKind.Binary;
			}
			if (value is string[])
			{
				return RegistryValueKind.MultiString;
			}
			throw new ArgumentException(SR.Format("RegistryKey.SetValue does not support arrays of type '{0}'. Only Byte[] and String[] are supported.", value.GetType().Name));
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00016F97 File Offset: 0x00015197
		public override string ToString()
		{
			this.EnsureNotDisposed();
			return this._keyName;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00017080 File Offset: 0x00015280
		private static string FixupName(string name)
		{
			if (name.IndexOf('\\') == -1)
			{
				return name;
			}
			StringBuilder stringBuilder = new StringBuilder(name);
			RegistryKey.FixupPath(stringBuilder);
			int num = stringBuilder.Length - 1;
			if (num >= 0 && stringBuilder[num] == '\\')
			{
				stringBuilder.Length = num;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000170CC File Offset: 0x000152CC
		private static void FixupPath(StringBuilder path)
		{
			int length = path.Length;
			bool flag = false;
			char maxValue = char.MaxValue;
			for (int i = 1; i < length - 1; i++)
			{
				if (path[i] == '\\')
				{
					i++;
					while (i < length && path[i] == '\\')
					{
						path[i] = maxValue;
						i++;
						flag = true;
					}
				}
			}
			if (flag)
			{
				int i = 0;
				int num = 0;
				while (i < length)
				{
					if (path[i] == maxValue)
					{
						i++;
					}
					else
					{
						path[num] = path[i];
						i++;
						num++;
					}
				}
				path.Length += num - i;
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001716C File Offset: 0x0001536C
		private void EnsureNotDisposed()
		{
			if (this._hkey == null)
			{
				ThrowHelper.ThrowObjectDisposedException(this._keyName, "Cannot access a closed registry key.");
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001718A File Offset: 0x0001538A
		private void EnsureWriteable()
		{
			this.EnsureNotDisposed();
			if (!this.IsWritable())
			{
				ThrowHelper.ThrowUnauthorizedAccessException("Cannot write to the registry key.");
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000171A4 File Offset: 0x000153A4
		private RegistryKeyPermissionCheck GetSubKeyPermissionCheck(bool subkeyWritable)
		{
			if (this._checkMode == RegistryKeyPermissionCheck.Default)
			{
				return this._checkMode;
			}
			if (subkeyWritable)
			{
				return RegistryKeyPermissionCheck.ReadWriteSubTree;
			}
			return RegistryKeyPermissionCheck.ReadSubTree;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000171C0 File Offset: 0x000153C0
		private static void ValidateKeyName(string name)
		{
			if (name == null)
			{
				ThrowHelper.ThrowArgumentNullException("name");
			}
			int num = name.IndexOf("\\", StringComparison.OrdinalIgnoreCase);
			int num2 = 0;
			while (num != -1)
			{
				if (num - num2 > 255)
				{
					ThrowHelper.ThrowArgumentException("Registry key names should not be greater than 255 characters.", "name");
				}
				num2 = num + 1;
				num = name.IndexOf("\\", num2, StringComparison.OrdinalIgnoreCase);
			}
			if (name.Length - num2 > 255)
			{
				ThrowHelper.ThrowArgumentException("Registry key names should not be greater than 255 characters.", "name");
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00017238 File Offset: 0x00015438
		private static void ValidateKeyMode(RegistryKeyPermissionCheck mode)
		{
			if (mode < RegistryKeyPermissionCheck.Default || mode > RegistryKeyPermissionCheck.ReadWriteSubTree)
			{
				ThrowHelper.ThrowArgumentException("The specified RegistryKeyPermissionCheck value is invalid.", "mode");
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00017251 File Offset: 0x00015451
		private static void ValidateKeyOptions(RegistryOptions options)
		{
			if (options < RegistryOptions.None || options > RegistryOptions.Volatile)
			{
				ThrowHelper.ThrowArgumentException("The specified RegistryOptions value is invalid.", "options");
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001726A File Offset: 0x0001546A
		private static void ValidateKeyView(RegistryView view)
		{
			if (view != RegistryView.Default && view != RegistryView.Registry32 && view != RegistryView.Registry64)
			{
				ThrowHelper.ThrowArgumentException("The specified RegistryView value is invalid.", "view");
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001728E File Offset: 0x0001548E
		private static void ValidateKeyRights(int rights)
		{
			if ((rights & -983104) != 0)
			{
				ThrowHelper.ThrowSecurityException("Requested registry access is not allowed.");
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000172A3 File Offset: 0x000154A3
		private bool IsDirty()
		{
			return (this._state & RegistryKey.StateFlags.Dirty) > (RegistryKey.StateFlags)0;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000172B2 File Offset: 0x000154B2
		private bool IsSystemKey()
		{
			return (this._state & RegistryKey.StateFlags.SystemKey) > (RegistryKey.StateFlags)0;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000172C1 File Offset: 0x000154C1
		private bool IsWritable()
		{
			return (this._state & RegistryKey.StateFlags.WriteAccess) > (RegistryKey.StateFlags)0;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000172D0 File Offset: 0x000154D0
		private bool IsPerfDataKey()
		{
			return (this._state & RegistryKey.StateFlags.PerfData) > (RegistryKey.StateFlags)0;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000172DF File Offset: 0x000154DF
		private void SetDirty()
		{
			this._state |= RegistryKey.StateFlags.Dirty;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000173AD File Offset: 0x000155AD
		internal RegistryKey()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04000F55 RID: 3925
		internal static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(int.MinValue);

		// Token: 0x04000F56 RID: 3926
		internal static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);

		// Token: 0x04000F57 RID: 3927
		internal static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);

		// Token: 0x04000F58 RID: 3928
		internal static readonly IntPtr HKEY_USERS = new IntPtr(-2147483645);

		// Token: 0x04000F59 RID: 3929
		internal static readonly IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(-2147483644);

		// Token: 0x04000F5A RID: 3930
		internal static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);

		// Token: 0x04000F5B RID: 3931
		internal static readonly IntPtr HKEY_DYN_DATA = new IntPtr(-2147483642);

		// Token: 0x04000F5C RID: 3932
		private static readonly string[] s_hkeyNames = new string[]
		{
			"HKEY_CLASSES_ROOT",
			"HKEY_CURRENT_USER",
			"HKEY_LOCAL_MACHINE",
			"HKEY_USERS",
			"HKEY_PERFORMANCE_DATA",
			"HKEY_CURRENT_CONFIG",
			"HKEY_DYN_DATA"
		};

		// Token: 0x04000F5D RID: 3933
		private const int MaxKeyLength = 255;

		// Token: 0x04000F5E RID: 3934
		private const int MaxValueLength = 16383;

		// Token: 0x04000F5F RID: 3935
		private volatile SafeRegistryHandle _hkey;

		// Token: 0x04000F60 RID: 3936
		private volatile string _keyName;

		// Token: 0x04000F61 RID: 3937
		private volatile bool _remoteKey;

		// Token: 0x04000F62 RID: 3938
		private volatile RegistryKey.StateFlags _state;

		// Token: 0x04000F63 RID: 3939
		private volatile RegistryKeyPermissionCheck _checkMode;

		// Token: 0x04000F64 RID: 3940
		private volatile RegistryView _regView;

		// Token: 0x020000A4 RID: 164
		[Flags]
		private enum StateFlags
		{
			// Token: 0x04000F66 RID: 3942
			Dirty = 1,
			// Token: 0x04000F67 RID: 3943
			SystemKey = 2,
			// Token: 0x04000F68 RID: 3944
			WriteAccess = 4,
			// Token: 0x04000F69 RID: 3945
			PerfData = 8
		}
	}
}
