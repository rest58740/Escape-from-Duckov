using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace WyvrnSDK
{
	// Token: 0x02000005 RID: 5
	public static class WyvrnAPI
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002244 File Offset: 0x00000444
		static WyvrnAPI()
		{
			try
			{
				string[] array = new string[]
				{
					Path.Combine(Environment.GetFolderPath(38), "Razer Chroma SDK", "bin", "RzChromatic64.dll"),
					Path.Combine(Environment.GetFolderPath(36), "System32", "RzChromatic64.dll")
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (!WyvrnAPI.IsProductionVersionAvailable(array[i]))
					{
						return;
					}
				}
				WyvrnAPI._sIsChromaticAvailable = true;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(string.Format("The ChromaSDK is not available! Exception={0}", ex));
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022DC File Offset: 0x000004DC
		private static bool IsProductionVersionAvailable(string fileName)
		{
			bool result;
			try
			{
				if (!new FileInfo(fileName).Exists)
				{
					result = false;
				}
				else
				{
					string[] array = FileVersionInfo.GetVersionInfo(fileName).FileVersion.Split(".".ToCharArray());
					int num;
					int num2;
					int num3;
					int num4;
					if (array.Length < 4)
					{
						result = false;
					}
					else if (!int.TryParse(array[0], ref num))
					{
						result = false;
					}
					else if (!int.TryParse(array[1], ref num2))
					{
						result = false;
					}
					else if (!int.TryParse(array[2], ref num3))
					{
						result = false;
					}
					else if (!int.TryParse(array[3], ref num4))
					{
						result = false;
					}
					else if (num < 2)
					{
						result = false;
					}
					else if (num == 2 && num2 < 0)
					{
						result = false;
					}
					else if (num == 2 && num2 == 0 && num3 < 2)
					{
						result = false;
					}
					else if (num == 2 && num2 == 0 && num3 == 2 && num4 < 0)
					{
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(string.Format("The ChromaSDK is not available! Exception={0}", ex));
				result = false;
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023D8 File Offset: 0x000005D8
		public static bool IsWyvrnSDKAvailable()
		{
			return WyvrnAPI._sIsChromaticAvailable;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000023E0 File Offset: 0x000005E0
		private static IntPtr GetUnicodeIntPtr(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return IntPtr.Zero;
			}
			byte[] bytes = Encoding.Unicode.GetBytes(str + "\0");
			IntPtr intPtr = Marshal.AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, intPtr, bytes.Length);
			return intPtr;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002426 File Offset: 0x00000626
		private static void FreeIntPtr(IntPtr lpData)
		{
			if (lpData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(lpData);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000243B File Offset: 0x0000063B
		public static int CoreInitSDK(ref APPINFOTYPE appInfo)
		{
			appInfo.SupportedDevice = 63U;
			if (!WyvrnAPI._sIsChromaticAvailable)
			{
				return -1;
			}
			if (WyvrnAPI._sInitialized)
			{
				return 0;
			}
			int num = WyvrnAPI.PluginCoreInitSDK(ref appInfo);
			if (num == 0)
			{
				WyvrnAPI._sInitialized = true;
			}
			return num;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002468 File Offset: 0x00000668
		public static int CoreSetEventName(string name)
		{
			if (!WyvrnAPI._sIsChromaticAvailable)
			{
				return -1;
			}
			if (!WyvrnAPI._sInitialized)
			{
				return -1;
			}
			IntPtr unicodeIntPtr = WyvrnAPI.GetUnicodeIntPtr(name);
			int result = WyvrnAPI.PluginCoreSetEventName(unicodeIntPtr);
			WyvrnAPI.FreeIntPtr(unicodeIntPtr);
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000249A File Offset: 0x0000069A
		public static int CoreUnInit()
		{
			if (!WyvrnAPI._sIsChromaticAvailable)
			{
				return 0;
			}
			if (!WyvrnAPI._sInitialized)
			{
				return 0;
			}
			int num = WyvrnAPI.PluginCoreUnInit();
			if (num == 0)
			{
				WyvrnAPI._sInitialized = false;
			}
			return num;
		}

		// Token: 0x0600000D RID: 13
		[DllImport("RzChromatic64", CallingConvention = 2)]
		private static extern int PluginCoreInitSDK(ref APPINFOTYPE appInfo);

		// Token: 0x0600000E RID: 14
		[DllImport("RzChromatic64", CallingConvention = 2)]
		private static extern int PluginCoreSetEventName(IntPtr name);

		// Token: 0x0600000F RID: 15
		[DllImport("RzChromatic64", CallingConvention = 2)]
		private static extern int PluginCoreUnInit();

		// Token: 0x0400001A RID: 26
		private const string DLL_NAME = "RzChromatic64";

		// Token: 0x0400001B RID: 27
		private static bool _sIsChromaticAvailable = false;

		// Token: 0x0400001C RID: 28
		private static bool _sInitialized;
	}
}
