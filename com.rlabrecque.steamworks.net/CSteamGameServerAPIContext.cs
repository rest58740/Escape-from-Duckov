using System;

namespace Steamworks
{
	// Token: 0x02000198 RID: 408
	internal static class CSteamGameServerAPIContext
	{
		// Token: 0x06000962 RID: 2402 RVA: 0x0000EA48 File Offset: 0x0000CC48
		internal static void Clear()
		{
			CSteamGameServerAPIContext.m_pSteamClient = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamGameServer = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamUtils = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworking = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamGameServerStats = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamHTTP = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamInventory = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamUGC = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworkingUtils = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworkingSockets = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworkingMessages = IntPtr.Zero;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0000EAC4 File Offset: 0x0000CCC4
		internal static bool Init()
		{
			HSteamUser hsteamUser = GameServer.GetHSteamUser();
			HSteamPipe hsteamPipe = GameServer.GetHSteamPipe();
			if (hsteamPipe == (HSteamPipe)0)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle("SteamClient021"))
			{
				CSteamGameServerAPIContext.m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(utf8StringHandle);
			}
			if (CSteamGameServerAPIContext.m_pSteamClient == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamGameServer = SteamGameServerClient.GetISteamGameServer(hsteamUser, hsteamPipe, "SteamGameServer015");
			if (CSteamGameServerAPIContext.m_pSteamGameServer == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamUtils = SteamGameServerClient.GetISteamUtils(hsteamPipe, "SteamUtils010");
			if (CSteamGameServerAPIContext.m_pSteamUtils == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamNetworking = SteamGameServerClient.GetISteamNetworking(hsteamUser, hsteamPipe, "SteamNetworking006");
			if (CSteamGameServerAPIContext.m_pSteamNetworking == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamGameServerStats = SteamGameServerClient.GetISteamGameServerStats(hsteamUser, hsteamPipe, "SteamGameServerStats001");
			if (CSteamGameServerAPIContext.m_pSteamGameServerStats == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamHTTP = SteamGameServerClient.GetISteamHTTP(hsteamUser, hsteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
			if (CSteamGameServerAPIContext.m_pSteamHTTP == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamInventory = SteamGameServerClient.GetISteamInventory(hsteamUser, hsteamPipe, "STEAMINVENTORY_INTERFACE_V003");
			if (CSteamGameServerAPIContext.m_pSteamInventory == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamUGC = SteamGameServerClient.GetISteamUGC(hsteamUser, hsteamPipe, "STEAMUGC_INTERFACE_VERSION020");
			if (CSteamGameServerAPIContext.m_pSteamUGC == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle("SteamNetworkingUtils004"))
			{
				CSteamGameServerAPIContext.m_pSteamNetworkingUtils = ((NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle2) != IntPtr.Zero) ? NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle2) : NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hsteamUser, utf8StringHandle2));
			}
			if (CSteamGameServerAPIContext.m_pSteamNetworkingUtils == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle("SteamNetworkingSockets012"))
			{
				CSteamGameServerAPIContext.m_pSteamNetworkingSockets = NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hsteamUser, utf8StringHandle3);
			}
			if (CSteamGameServerAPIContext.m_pSteamNetworkingSockets == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle("SteamNetworkingMessages002"))
			{
				CSteamGameServerAPIContext.m_pSteamNetworkingMessages = NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hsteamUser, utf8StringHandle4);
			}
			return !(CSteamGameServerAPIContext.m_pSteamNetworkingMessages == IntPtr.Zero);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0000ED18 File Offset: 0x0000CF18
		internal static IntPtr GetSteamClient()
		{
			return CSteamGameServerAPIContext.m_pSteamClient;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0000ED1F File Offset: 0x0000CF1F
		internal static IntPtr GetSteamGameServer()
		{
			return CSteamGameServerAPIContext.m_pSteamGameServer;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0000ED26 File Offset: 0x0000CF26
		internal static IntPtr GetSteamUtils()
		{
			return CSteamGameServerAPIContext.m_pSteamUtils;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0000ED2D File Offset: 0x0000CF2D
		internal static IntPtr GetSteamNetworking()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworking;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0000ED34 File Offset: 0x0000CF34
		internal static IntPtr GetSteamGameServerStats()
		{
			return CSteamGameServerAPIContext.m_pSteamGameServerStats;
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0000ED3B File Offset: 0x0000CF3B
		internal static IntPtr GetSteamHTTP()
		{
			return CSteamGameServerAPIContext.m_pSteamHTTP;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0000ED42 File Offset: 0x0000CF42
		internal static IntPtr GetSteamInventory()
		{
			return CSteamGameServerAPIContext.m_pSteamInventory;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0000ED49 File Offset: 0x0000CF49
		internal static IntPtr GetSteamUGC()
		{
			return CSteamGameServerAPIContext.m_pSteamUGC;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0000ED50 File Offset: 0x0000CF50
		internal static IntPtr GetSteamNetworkingUtils()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworkingUtils;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0000ED57 File Offset: 0x0000CF57
		internal static IntPtr GetSteamNetworkingSockets()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworkingSockets;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0000ED5E File Offset: 0x0000CF5E
		internal static IntPtr GetSteamNetworkingMessages()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworkingMessages;
		}

		// Token: 0x04000AA8 RID: 2728
		private static IntPtr m_pSteamClient;

		// Token: 0x04000AA9 RID: 2729
		private static IntPtr m_pSteamGameServer;

		// Token: 0x04000AAA RID: 2730
		private static IntPtr m_pSteamUtils;

		// Token: 0x04000AAB RID: 2731
		private static IntPtr m_pSteamNetworking;

		// Token: 0x04000AAC RID: 2732
		private static IntPtr m_pSteamGameServerStats;

		// Token: 0x04000AAD RID: 2733
		private static IntPtr m_pSteamHTTP;

		// Token: 0x04000AAE RID: 2734
		private static IntPtr m_pSteamInventory;

		// Token: 0x04000AAF RID: 2735
		private static IntPtr m_pSteamUGC;

		// Token: 0x04000AB0 RID: 2736
		private static IntPtr m_pSteamNetworkingUtils;

		// Token: 0x04000AB1 RID: 2737
		private static IntPtr m_pSteamNetworkingSockets;

		// Token: 0x04000AB2 RID: 2738
		private static IntPtr m_pSteamNetworkingMessages;
	}
}
