using System;

namespace Steamworks
{
	// Token: 0x02000020 RID: 32
	public static class SteamRemotePlay
	{
		// Token: 0x0600038F RID: 911 RVA: 0x0000962E File Offset: 0x0000782E
		public static uint GetSessionCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetSessionCount(CSteamAPIContext.GetSteamRemotePlay());
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000963F File Offset: 0x0000783F
		public static RemotePlaySessionID_t GetSessionID(int iSessionIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (RemotePlaySessionID_t)NativeMethods.ISteamRemotePlay_GetSessionID(CSteamAPIContext.GetSteamRemotePlay(), iSessionIndex);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00009656 File Offset: 0x00007856
		public static CSteamID GetSessionSteamID(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamRemotePlay_GetSessionSteamID(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000966D File Offset: 0x0000786D
		public static string GetSessionClientName(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamRemotePlay_GetSessionClientName(CSteamAPIContext.GetSteamRemotePlay(), unSessionID));
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00009684 File Offset: 0x00007884
		public static ESteamDeviceFormFactor GetSessionClientFormFactor(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetSessionClientFormFactor(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00009696 File Offset: 0x00007896
		public static bool BGetSessionClientResolution(RemotePlaySessionID_t unSessionID, out int pnResolutionX, out int pnResolutionY)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_BGetSessionClientResolution(CSteamAPIContext.GetSteamRemotePlay(), unSessionID, out pnResolutionX, out pnResolutionY);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000096AA File Offset: 0x000078AA
		public static bool BStartRemotePlayTogether(bool bShowOverlay = true)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_BStartRemotePlayTogether(CSteamAPIContext.GetSteamRemotePlay(), bShowOverlay);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000096BC File Offset: 0x000078BC
		public static bool BSendRemotePlayTogetherInvite(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_BSendRemotePlayTogetherInvite(CSteamAPIContext.GetSteamRemotePlay(), steamIDFriend);
		}
	}
}
