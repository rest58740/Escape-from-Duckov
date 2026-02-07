using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000028 RID: 40
	public static class SteamVideo
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x0000C5D2 File Offset: 0x0000A7D2
		public static void GetVideoURL(AppId_t unVideoAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamVideo_GetVideoURL(CSteamAPIContext.GetSteamVideo(), unVideoAppID);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		public static bool IsBroadcasting(out int pnNumViewers)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamVideo_IsBroadcasting(CSteamAPIContext.GetSteamVideo(), out pnNumViewers);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000C5F6 File Offset: 0x0000A7F6
		public static void GetOPFSettings(AppId_t unVideoAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamVideo_GetOPFSettings(CSteamAPIContext.GetSteamVideo(), unVideoAppID);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000C608 File Offset: 0x0000A808
		public static bool GetOPFStringForApp(AppId_t unVideoAppID, out string pchBuffer, ref int pnBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(pnBufferSize);
			bool flag = NativeMethods.ISteamVideo_GetOPFStringForApp(CSteamAPIContext.GetSteamVideo(), unVideoAppID, intPtr, ref pnBufferSize);
			pchBuffer = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}
	}
}
