using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000092 RID: 146
	public static class DictionaryKeyUtility
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x0001EF7C File Offset: 0x0001D17C
		static DictionaryKeyUtility()
		{
			var enumerable = from n in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly ass) => from attr in ass.SafeGetCustomAttributes(typeof(RegisterDictionaryKeyPathProviderAttribute), false)
			select new
			{
				Assembly = ass,
				Attribute = (RegisterDictionaryKeyPathProviderAttribute)attr
			})
			where n.Attribute.ProviderType != null
			select n;
			foreach (var <>f__AnonymousType in enumerable)
			{
				Assembly assembly = <>f__AnonymousType.Assembly;
				Type providerType = <>f__AnonymousType.Attribute.ProviderType;
				if (providerType.IsAbstract)
				{
					DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Type cannot be abstract");
				}
				else if (providerType.IsInterface)
				{
					DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Type cannot be an interface");
				}
				else if (!providerType.ImplementsOpenGenericInterface(typeof(IDictionaryKeyPathProvider<>)))
				{
					DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Type must implement the " + typeof(IDictionaryKeyPathProvider<>).GetNiceName() + " interface");
				}
				else if (providerType.IsGenericType)
				{
					DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Type cannot be generic");
				}
				else if (providerType.GetConstructor(Type.EmptyTypes) == null)
				{
					DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Type must have a public parameterless constructor");
				}
				else
				{
					Type type = providerType.GetArgumentsOfInheritedOpenGenericInterface(typeof(IDictionaryKeyPathProvider<>))[0];
					if (!type.IsValueType)
					{
						DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Key type to support '" + type.GetNiceFullName() + "' must be a value type - support for extending dictionaries with reference type keys may come at a later time");
					}
					else if (DictionaryKeyUtility.TypeToKeyPathProviders.ContainsKey(type))
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"Ignoring dictionary key path provider '",
							providerType.GetNiceFullName(),
							"' registered on assembly '",
							assembly.GetName().Name,
							"': A previous provider '",
							DictionaryKeyUtility.TypeToKeyPathProviders[type].GetType().GetNiceFullName(),
							"' was already registered for the key type '",
							type.GetNiceFullName(),
							"'."
						}));
					}
					else
					{
						IDictionaryKeyPathProvider dictionaryKeyPathProvider;
						try
						{
							dictionaryKeyPathProvider = (IDictionaryKeyPathProvider)Activator.CreateInstance(providerType);
						}
						catch (Exception ex)
						{
							Debug.LogException(ex);
							string[] array = new string[7];
							array[0] = "Ignoring dictionary key path provider '";
							array[1] = providerType.GetNiceFullName();
							array[2] = "' registered on assembly '";
							array[3] = assembly.GetName().Name;
							array[4] = "': An exception of type '";
							int num = 5;
							Type type2 = ex.GetType();
							array[num] = ((type2 != null) ? type2.ToString() : null);
							array[6] = "' was thrown when trying to instantiate a provider instance.";
							Debug.LogWarning(string.Concat(array));
							continue;
						}
						string providerID;
						try
						{
							providerID = dictionaryKeyPathProvider.ProviderID;
						}
						catch (Exception ex2)
						{
							Debug.LogException(ex2);
							string[] array2 = new string[7];
							array2[0] = "Ignoring dictionary key path provider '";
							array2[1] = providerType.GetNiceFullName();
							array2[2] = "' registered on assembly '";
							array2[3] = assembly.GetName().Name;
							array2[4] = "': An exception of type '";
							int num2 = 5;
							Type type3 = ex2.GetType();
							array2[num2] = ((type3 != null) ? type3.ToString() : null);
							array2[6] = "' was thrown when trying to get the provider ID string.";
							Debug.LogWarning(string.Concat(array2));
							continue;
						}
						if (providerID == null)
						{
							DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Provider ID is null");
						}
						else if (providerID.Length == 0)
						{
							DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Provider ID is an empty string");
						}
						else
						{
							for (int i = 0; i < providerID.Length; i++)
							{
								if (!char.IsLetterOrDigit(providerID.get_Chars(i)))
								{
									DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, "Provider ID '" + providerID + "' cannot contain characters which are not letters or digits");
								}
							}
							if (DictionaryKeyUtility.IDToKeyPathProviders.ContainsKey(providerID))
							{
								DictionaryKeyUtility.LogInvalidKeyPathProvider(providerType, assembly, string.Concat(new string[]
								{
									"Provider ID '",
									providerID,
									"' is already in use for the provider '",
									DictionaryKeyUtility.IDToKeyPathProviders[providerID].GetType().GetNiceFullName(),
									"'"
								}));
							}
							else
							{
								DictionaryKeyUtility.TypeToKeyPathProviders[type] = dictionaryKeyPathProvider;
								DictionaryKeyUtility.IDToKeyPathProviders[providerID] = dictionaryKeyPathProvider;
								DictionaryKeyUtility.ProviderToID[dictionaryKeyPathProvider] = providerID;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001F544 File Offset: 0x0001D744
		private static void LogInvalidKeyPathProvider(Type type, Assembly assembly, string reason)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Invalid dictionary key path provider '",
				type.GetNiceFullName(),
				"' registered on assembly '",
				assembly.GetName().Name,
				"': ",
				reason
			}));
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001F594 File Offset: 0x0001D794
		public static IEnumerable<Type> GetPersistentPathKeyTypes()
		{
			foreach (Type type in DictionaryKeyUtility.BaseSupportedDictionaryKeyTypes)
			{
				yield return type;
			}
			HashSet<Type>.Enumerator enumerator = default(HashSet<Type>.Enumerator);
			foreach (Type type2 in DictionaryKeyUtility.TypeToKeyPathProviders.Keys)
			{
				yield return type2;
			}
			Dictionary<Type, IDictionaryKeyPathProvider>.KeyCollection.Enumerator enumerator2 = default(Dictionary<Type, IDictionaryKeyPathProvider>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001F5A0 File Offset: 0x0001D7A0
		public static bool KeyTypeSupportsPersistentPaths(Type type)
		{
			bool flag;
			if (!DictionaryKeyUtility.GetSupportedDictionaryKeyTypesResults.TryGetValue(type, ref flag))
			{
				flag = DictionaryKeyUtility.PrivateIsSupportedDictionaryKeyType(type);
				DictionaryKeyUtility.GetSupportedDictionaryKeyTypesResults.Add(type, flag);
			}
			return flag;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001F5D0 File Offset: 0x0001D7D0
		private static bool PrivateIsSupportedDictionaryKeyType(Type type)
		{
			return type.IsEnum || DictionaryKeyUtility.BaseSupportedDictionaryKeyTypes.Contains(type) || DictionaryKeyUtility.TypeToKeyPathProviders.ContainsKey(type);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001F5F4 File Offset: 0x0001D7F4
		public static string GetDictionaryKeyString(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			Type type = key.GetType();
			if (!DictionaryKeyUtility.KeyTypeSupportsPersistentPaths(type))
			{
				string text;
				if (!DictionaryKeyUtility.ObjectsToTempKeys.TryGetValue(key, ref text))
				{
					long num = DictionaryKeyUtility.tempKeyCounter;
					DictionaryKeyUtility.tempKeyCounter = num + 1L;
					long num2 = num;
					text = num2.ToString();
					string text2 = "{temp:" + text + "}";
					DictionaryKeyUtility.ObjectsToTempKeys[key] = text2;
					DictionaryKeyUtility.TempKeysToObjects[text2] = key;
				}
				return text;
			}
			IDictionaryKeyPathProvider dictionaryKeyPathProvider;
			if (DictionaryKeyUtility.TypeToKeyPathProviders.TryGetValue(type, ref dictionaryKeyPathProvider))
			{
				string pathStringFromKey = dictionaryKeyPathProvider.GetPathStringFromKey(key);
				string text3 = null;
				bool flag = true;
				if (pathStringFromKey == null || pathStringFromKey.Length == 0)
				{
					flag = false;
					text3 = "String is null or empty";
				}
				if (flag)
				{
					for (int i = 0; i < pathStringFromKey.Length; i++)
					{
						char c = pathStringFromKey.get_Chars(i);
						if (!char.IsLetterOrDigit(c) && !DictionaryKeyUtility.AllowedSpecialKeyStrChars.Contains(c))
						{
							flag = false;
							text3 = "Invalid character '" + c.ToString() + "' at index " + i.ToString();
							break;
						}
					}
				}
				if (!flag)
				{
					throw new ArgumentException(string.Concat(new string[]
					{
						"Invalid key path '",
						pathStringFromKey,
						"' given by provider '",
						dictionaryKeyPathProvider.GetType().GetNiceFullName(),
						"': ",
						text3
					}));
				}
				return string.Concat(new string[]
				{
					"{id:",
					DictionaryKeyUtility.ProviderToID[dictionaryKeyPathProvider],
					":",
					pathStringFromKey,
					"}"
				});
			}
			else if (type.IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (underlyingType == typeof(ulong))
				{
					return "{" + Convert.ToUInt64(key).ToString("D", CultureInfo.InvariantCulture) + "eu}";
				}
				return "{" + Convert.ToInt64(key).ToString("D", CultureInfo.InvariantCulture) + "es}";
			}
			else
			{
				if (type == typeof(string))
				{
					return "{\"" + ((key != null) ? key.ToString() : null) + "\"}";
				}
				if (type == typeof(char))
				{
					return "{'" + ((char)key).ToString(CultureInfo.InvariantCulture) + "'}";
				}
				if (type == typeof(byte))
				{
					return "{" + ((byte)key).ToString("D", CultureInfo.InvariantCulture) + "ub}";
				}
				if (type == typeof(sbyte))
				{
					return "{" + ((sbyte)key).ToString("D", CultureInfo.InvariantCulture) + "sb}";
				}
				if (type == typeof(ushort))
				{
					return "{" + ((ushort)key).ToString("D", CultureInfo.InvariantCulture) + "us}";
				}
				if (type == typeof(short))
				{
					return "{" + ((short)key).ToString("D", CultureInfo.InvariantCulture) + "ss}";
				}
				if (type == typeof(uint))
				{
					return "{" + ((uint)key).ToString("D", CultureInfo.InvariantCulture) + "ui}";
				}
				if (type == typeof(int))
				{
					return "{" + ((int)key).ToString("D", CultureInfo.InvariantCulture) + "si}";
				}
				if (type == typeof(ulong))
				{
					return "{" + ((ulong)key).ToString("D", CultureInfo.InvariantCulture) + "ul}";
				}
				if (type == typeof(long))
				{
					return "{" + ((long)key).ToString("D", CultureInfo.InvariantCulture) + "sl}";
				}
				if (type == typeof(float))
				{
					return "{" + ((float)key).ToString("R", CultureInfo.InvariantCulture) + "fl}";
				}
				if (type == typeof(double))
				{
					return "{" + ((double)key).ToString("R", CultureInfo.InvariantCulture) + "dl}";
				}
				if (type == typeof(decimal))
				{
					return "{" + ((decimal)key).ToString("G", CultureInfo.InvariantCulture) + "dc}";
				}
				if (type == typeof(Guid))
				{
					return "{" + ((Guid)key).ToString("N", CultureInfo.InvariantCulture) + "gu}";
				}
				throw new NotImplementedException("Support has not been implemented for the supported dictionary key type '" + type.GetNiceName() + "'.");
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001FB3C File Offset: 0x0001DD3C
		public static object GetDictionaryKeyValue(string keyStr, Type expectedType)
		{
			if (keyStr == null)
			{
				throw new ArgumentNullException("keyStr");
			}
			if (keyStr.Length < 4 || keyStr.get_Chars(0) != '{' || keyStr.get_Chars(keyStr.Length - 1) != '}')
			{
				throw new ArgumentException("Invalid key string: " + keyStr);
			}
			if (keyStr.get_Chars(1) == '"')
			{
				if (keyStr.get_Chars(keyStr.Length - 2) != '"')
				{
					throw new ArgumentException("Invalid key string: " + keyStr);
				}
				return keyStr.Substring(2, keyStr.Length - 4);
			}
			else if (keyStr.get_Chars(1) == '\'')
			{
				if (keyStr.Length != 5 || keyStr.get_Chars(keyStr.Length - 2) != '\'')
				{
					throw new ArgumentException("Invalid key string: " + keyStr);
				}
				return keyStr.get_Chars(2);
			}
			else if (keyStr.StartsWith("{temp:"))
			{
				object result;
				if (!DictionaryKeyUtility.TempKeysToObjects.TryGetValue(keyStr, ref result))
				{
					throw new ArgumentException("The temp dictionary key '" + keyStr + "' has not been allocated yet.");
				}
				return result;
			}
			else if (keyStr.StartsWith("{id:"))
			{
				int num = keyStr.IndexOf(':', 4);
				if (num == -1 || num > keyStr.Length - 3)
				{
					throw new ArgumentException("Invalid key string: " + keyStr);
				}
				string text = keyStr.FromTo(4, num);
				string pathStr = keyStr.FromTo(num + 1, keyStr.Length - 1);
				IDictionaryKeyPathProvider dictionaryKeyPathProvider;
				if (!DictionaryKeyUtility.IDToKeyPathProviders.TryGetValue(text, ref dictionaryKeyPathProvider))
				{
					throw new ArgumentException(string.Concat(new string[]
					{
						"No provider found for provider ID '",
						text,
						"' in key string '",
						keyStr,
						"'."
					}));
				}
				return dictionaryKeyPathProvider.GetKeyFromPathString(pathStr);
			}
			else
			{
				if (keyStr.EndsWith("ub}"))
				{
					return byte.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("sb}"))
				{
					return sbyte.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("us}"))
				{
					return ushort.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("ss}"))
				{
					return short.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("ui}"))
				{
					return uint.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("si}"))
				{
					return int.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("ul}"))
				{
					return ulong.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("sl}"))
				{
					return long.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("fl}"))
				{
					return float.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("dl}"))
				{
					return double.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("dc}"))
				{
					return decimal.Parse(keyStr.Substring(1, keyStr.Length - 4), 511);
				}
				if (keyStr.EndsWith("gu}"))
				{
					return new Guid(keyStr.Substring(1, keyStr.Length - 4));
				}
				if (keyStr.EndsWith("es}"))
				{
					return Enum.ToObject(expectedType, long.Parse(keyStr.Substring(1, keyStr.Length - 4), 511));
				}
				if (keyStr.EndsWith("eu}"))
				{
					return Enum.ToObject(expectedType, ulong.Parse(keyStr.Substring(1, keyStr.Length - 4), 511));
				}
				throw new ArgumentException("Invalid key string: " + keyStr);
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001FF57 File Offset: 0x0001E157
		private static string FromTo(this string str, int from, int to)
		{
			return str.Substring(from, to - from);
		}

		// Token: 0x04000186 RID: 390
		private static readonly Dictionary<Type, bool> GetSupportedDictionaryKeyTypesResults = new Dictionary<Type, bool>();

		// Token: 0x04000187 RID: 391
		private static readonly HashSet<Type> BaseSupportedDictionaryKeyTypes = new HashSet<Type>
		{
			typeof(string),
			typeof(char),
			typeof(byte),
			typeof(sbyte),
			typeof(ushort),
			typeof(short),
			typeof(uint),
			typeof(int),
			typeof(ulong),
			typeof(long),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(Guid)
		};

		// Token: 0x04000188 RID: 392
		private static readonly HashSet<char> AllowedSpecialKeyStrChars = new HashSet<char>
		{
			',',
			'(',
			')',
			'\\',
			'|',
			'-',
			'+'
		};

		// Token: 0x04000189 RID: 393
		private static readonly Dictionary<Type, IDictionaryKeyPathProvider> TypeToKeyPathProviders = new Dictionary<Type, IDictionaryKeyPathProvider>();

		// Token: 0x0400018A RID: 394
		private static readonly Dictionary<string, IDictionaryKeyPathProvider> IDToKeyPathProviders = new Dictionary<string, IDictionaryKeyPathProvider>();

		// Token: 0x0400018B RID: 395
		private static readonly Dictionary<IDictionaryKeyPathProvider, string> ProviderToID = new Dictionary<IDictionaryKeyPathProvider, string>();

		// Token: 0x0400018C RID: 396
		private static readonly Dictionary<object, string> ObjectsToTempKeys = new Dictionary<object, string>();

		// Token: 0x0400018D RID: 397
		private static readonly Dictionary<string, object> TempKeysToObjects = new Dictionary<string, object>();

		// Token: 0x0400018E RID: 398
		private static long tempKeyCounter = 0L;

		// Token: 0x02000108 RID: 264
		private class UnityObjectKeyComparer<T> : IComparer<T>
		{
			// Token: 0x060006D5 RID: 1749 RVA: 0x0002B96C File Offset: 0x00029B6C
			public int Compare(T x, T y)
			{
				Object @object = (Object)((object)x);
				Object object2 = (Object)((object)y);
				if (@object == null && object2 == null)
				{
					return 0;
				}
				if (@object == null)
				{
					return 1;
				}
				if (object2 == null)
				{
					return -1;
				}
				return @object.name.CompareTo(object2.name);
			}
		}

		// Token: 0x02000109 RID: 265
		private class FallbackKeyComparer<T> : IComparer<T>
		{
			// Token: 0x060006D7 RID: 1751 RVA: 0x0002B9CC File Offset: 0x00029BCC
			public int Compare(T x, T y)
			{
				return DictionaryKeyUtility.GetDictionaryKeyString(x).CompareTo(DictionaryKeyUtility.GetDictionaryKeyString(y));
			}
		}

		// Token: 0x0200010A RID: 266
		public class KeyComparer<T> : IComparer<T>
		{
			// Token: 0x060006D9 RID: 1753 RVA: 0x0002B9EC File Offset: 0x00029BEC
			public KeyComparer()
			{
				IDictionaryKeyPathProvider dictionaryKeyPathProvider;
				if (DictionaryKeyUtility.TypeToKeyPathProviders.TryGetValue(typeof(T), ref dictionaryKeyPathProvider))
				{
					this.actualComparer = (IComparer<T>)dictionaryKeyPathProvider;
					return;
				}
				if (typeof(IComparable).IsAssignableFrom(typeof(T)) || typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
				{
					this.actualComparer = Comparer<T>.Default;
					return;
				}
				if (typeof(Object).IsAssignableFrom(typeof(T)))
				{
					this.actualComparer = new DictionaryKeyUtility.UnityObjectKeyComparer<T>();
					return;
				}
				this.actualComparer = new DictionaryKeyUtility.FallbackKeyComparer<T>();
			}

			// Token: 0x060006DA RID: 1754 RVA: 0x0002BA98 File Offset: 0x00029C98
			public int Compare(T x, T y)
			{
				return this.actualComparer.Compare(x, y);
			}

			// Token: 0x040002D5 RID: 725
			public static readonly DictionaryKeyUtility.KeyComparer<T> Default = new DictionaryKeyUtility.KeyComparer<T>();

			// Token: 0x040002D6 RID: 726
			private readonly IComparer<T> actualComparer;
		}
	}
}
