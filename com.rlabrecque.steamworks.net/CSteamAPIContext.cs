using System;

namespace Steamworks
{
	// Token: 0x02000197 RID: 407
	internal static class CSteamAPIContext
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x0000E39C File Offset: 0x0000C59C
		internal static void Clear()
		{
			CSteamAPIContext.m_pSteamClient = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUser = IntPtr.Zero;
			CSteamAPIContext.m_pSteamFriends = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUtils = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMatchmaking = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUserStats = IntPtr.Zero;
			CSteamAPIContext.m_pSteamApps = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMatchmakingServers = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworking = IntPtr.Zero;
			CSteamAPIContext.m_pSteamRemoteStorage = IntPtr.Zero;
			CSteamAPIContext.m_pSteamHTTP = IntPtr.Zero;
			CSteamAPIContext.m_pSteamScreenshots = IntPtr.Zero;
			CSteamAPIContext.m_pSteamGameSearch = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
			CSteamAPIContext.m_pController = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUGC = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMusicRemote = IntPtr.Zero;
			CSteamAPIContext.m_pSteamHTMLSurface = IntPtr.Zero;
			CSteamAPIContext.m_pSteamInventory = IntPtr.Zero;
			CSteamAPIContext.m_pSteamVideo = IntPtr.Zero;
			CSteamAPIContext.m_pSteamParentalSettings = IntPtr.Zero;
			CSteamAPIContext.m_pSteamInput = IntPtr.Zero;
			CSteamAPIContext.m_pSteamParties = IntPtr.Zero;
			CSteamAPIContext.m_pSteamRemotePlay = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworkingUtils = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworkingSockets = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworkingMessages = IntPtr.Zero;
			CSteamAPIContext.m_pSteamTimeline = IntPtr.Zero;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0000E4CC File Offset: 0x0000C6CC
		internal static bool Init()
		{
			HSteamUser hsteamUser = SteamAPI.GetHSteamUser();
			HSteamPipe hsteamPipe = SteamAPI.GetHSteamPipe();
			if (hsteamPipe == (HSteamPipe)0)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle("SteamClient021"))
			{
				CSteamAPIContext.m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(utf8StringHandle);
			}
			if (CSteamAPIContext.m_pSteamClient == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUser = SteamClient.GetISteamUser(hsteamUser, hsteamPipe, "SteamUser023");
			if (CSteamAPIContext.m_pSteamUser == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamFriends = SteamClient.GetISteamFriends(hsteamUser, hsteamPipe, "SteamFriends017");
			if (CSteamAPIContext.m_pSteamFriends == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUtils = SteamClient.GetISteamUtils(hsteamPipe, "SteamUtils010");
			if (CSteamAPIContext.m_pSteamUtils == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMatchmaking = SteamClient.GetISteamMatchmaking(hsteamUser, hsteamPipe, "SteamMatchMaking009");
			if (CSteamAPIContext.m_pSteamMatchmaking == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMatchmakingServers = SteamClient.GetISteamMatchmakingServers(hsteamUser, hsteamPipe, "SteamMatchMakingServers002");
			if (CSteamAPIContext.m_pSteamMatchmakingServers == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUserStats = SteamClient.GetISteamUserStats(hsteamUser, hsteamPipe, "STEAMUSERSTATS_INTERFACE_VERSION013");
			if (CSteamAPIContext.m_pSteamUserStats == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamApps = SteamClient.GetISteamApps(hsteamUser, hsteamPipe, "STEAMAPPS_INTERFACE_VERSION008");
			if (CSteamAPIContext.m_pSteamApps == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamNetworking = SteamClient.GetISteamNetworking(hsteamUser, hsteamPipe, "SteamNetworking006");
			if (CSteamAPIContext.m_pSteamNetworking == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamRemoteStorage = SteamClient.GetISteamRemoteStorage(hsteamUser, hsteamPipe, "STEAMREMOTESTORAGE_INTERFACE_VERSION016");
			if (CSteamAPIContext.m_pSteamRemoteStorage == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamScreenshots = SteamClient.GetISteamScreenshots(hsteamUser, hsteamPipe, "STEAMSCREENSHOTS_INTERFACE_VERSION003");
			if (CSteamAPIContext.m_pSteamScreenshots == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamGameSearch = SteamClient.GetISteamGameSearch(hsteamUser, hsteamPipe, "SteamMatchGameSearch001");
			if (CSteamAPIContext.m_pSteamGameSearch == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamHTTP = SteamClient.GetISteamHTTP(hsteamUser, hsteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
			if (CSteamAPIContext.m_pSteamHTTP == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUGC = SteamClient.GetISteamUGC(hsteamUser, hsteamPipe, "STEAMUGC_INTERFACE_VERSION020");
			if (CSteamAPIContext.m_pSteamUGC == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMusic = SteamClient.GetISteamMusic(hsteamUser, hsteamPipe, "STEAMMUSIC_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamMusic == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMusicRemote = SteamClient.GetISteamMusicRemote(hsteamUser, hsteamPipe, "STEAMMUSICREMOTE_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamMusicRemote == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamHTMLSurface = SteamClient.GetISteamHTMLSurface(hsteamUser, hsteamPipe, "STEAMHTMLSURFACE_INTERFACE_VERSION_005");
			if (CSteamAPIContext.m_pSteamHTMLSurface == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamInventory = SteamClient.GetISteamInventory(hsteamUser, hsteamPipe, "STEAMINVENTORY_INTERFACE_V003");
			if (CSteamAPIContext.m_pSteamInventory == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamVideo = SteamClient.GetISteamVideo(hsteamUser, hsteamPipe, "STEAMVIDEO_INTERFACE_V007");
			if (CSteamAPIContext.m_pSteamVideo == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamParentalSettings = SteamClient.GetISteamParentalSettings(hsteamUser, hsteamPipe, "STEAMPARENTALSETTINGS_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamParentalSettings == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamInput = SteamClient.GetISteamInput(hsteamUser, hsteamPipe, "SteamInput006");
			if (CSteamAPIContext.m_pSteamInput == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamParties = SteamClient.GetISteamParties(hsteamUser, hsteamPipe, "SteamParties002");
			if (CSteamAPIContext.m_pSteamParties == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamRemotePlay = SteamClient.GetISteamRemotePlay(hsteamUser, hsteamPipe, "STEAMREMOTEPLAY_INTERFACE_VERSION002");
			if (CSteamAPIContext.m_pSteamRemotePlay == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle("SteamNetworkingUtils004"))
			{
				CSteamAPIContext.m_pSteamNetworkingUtils = ((NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle2) != IntPtr.Zero) ? NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle2) : NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hsteamUser, utf8StringHandle2));
			}
			if (CSteamAPIContext.m_pSteamNetworkingUtils == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle("SteamNetworkingSockets012"))
			{
				CSteamAPIContext.m_pSteamNetworkingSockets = NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle3);
			}
			if (CSteamAPIContext.m_pSteamNetworkingSockets == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle("SteamNetworkingMessages002"))
			{
				CSteamAPIContext.m_pSteamNetworkingMessages = NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle4);
			}
			if (CSteamAPIContext.m_pSteamNetworkingMessages == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle5 = new InteropHelp.UTF8StringHandle("STEAMTIMELINE_INTERFACE_V004"))
			{
				CSteamAPIContext.m_pSteamTimeline = NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle5);
			}
			return !(CSteamAPIContext.m_pSteamTimeline == IntPtr.Zero);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0000E984 File Offset: 0x0000CB84
		internal static IntPtr GetSteamClient()
		{
			return CSteamAPIContext.m_pSteamClient;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0000E98B File Offset: 0x0000CB8B
		internal static IntPtr GetSteamUser()
		{
			return CSteamAPIContext.m_pSteamUser;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0000E992 File Offset: 0x0000CB92
		internal static IntPtr GetSteamFriends()
		{
			return CSteamAPIContext.m_pSteamFriends;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0000E999 File Offset: 0x0000CB99
		internal static IntPtr GetSteamUtils()
		{
			return CSteamAPIContext.m_pSteamUtils;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0000E9A0 File Offset: 0x0000CBA0
		internal static IntPtr GetSteamMatchmaking()
		{
			return CSteamAPIContext.m_pSteamMatchmaking;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0000E9A7 File Offset: 0x0000CBA7
		internal static IntPtr GetSteamUserStats()
		{
			return CSteamAPIContext.m_pSteamUserStats;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0000E9AE File Offset: 0x0000CBAE
		internal static IntPtr GetSteamApps()
		{
			return CSteamAPIContext.m_pSteamApps;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0000E9B5 File Offset: 0x0000CBB5
		internal static IntPtr GetSteamMatchmakingServers()
		{
			return CSteamAPIContext.m_pSteamMatchmakingServers;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0000E9BC File Offset: 0x0000CBBC
		internal static IntPtr GetSteamNetworking()
		{
			return CSteamAPIContext.m_pSteamNetworking;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0000E9C3 File Offset: 0x0000CBC3
		internal static IntPtr GetSteamRemoteStorage()
		{
			return CSteamAPIContext.m_pSteamRemoteStorage;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0000E9CA File Offset: 0x0000CBCA
		internal static IntPtr GetSteamScreenshots()
		{
			return CSteamAPIContext.m_pSteamScreenshots;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0000E9D1 File Offset: 0x0000CBD1
		internal static IntPtr GetSteamGameSearch()
		{
			return CSteamAPIContext.m_pSteamGameSearch;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
		internal static IntPtr GetSteamHTTP()
		{
			return CSteamAPIContext.m_pSteamHTTP;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0000E9DF File Offset: 0x0000CBDF
		internal static IntPtr GetSteamController()
		{
			return CSteamAPIContext.m_pController;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0000E9E6 File Offset: 0x0000CBE6
		internal static IntPtr GetSteamUGC()
		{
			return CSteamAPIContext.m_pSteamUGC;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0000E9ED File Offset: 0x0000CBED
		internal static IntPtr GetSteamMusic()
		{
			return CSteamAPIContext.m_pSteamMusic;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
		internal static IntPtr GetSteamMusicRemote()
		{
			return CSteamAPIContext.m_pSteamMusicRemote;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0000E9FB File Offset: 0x0000CBFB
		internal static IntPtr GetSteamHTMLSurface()
		{
			return CSteamAPIContext.m_pSteamHTMLSurface;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0000EA02 File Offset: 0x0000CC02
		internal static IntPtr GetSteamInventory()
		{
			return CSteamAPIContext.m_pSteamInventory;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0000EA09 File Offset: 0x0000CC09
		internal static IntPtr GetSteamVideo()
		{
			return CSteamAPIContext.m_pSteamVideo;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0000EA10 File Offset: 0x0000CC10
		internal static IntPtr GetSteamParentalSettings()
		{
			return CSteamAPIContext.m_pSteamParentalSettings;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0000EA17 File Offset: 0x0000CC17
		internal static IntPtr GetSteamInput()
		{
			return CSteamAPIContext.m_pSteamInput;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0000EA1E File Offset: 0x0000CC1E
		internal static IntPtr GetSteamParties()
		{
			return CSteamAPIContext.m_pSteamParties;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0000EA25 File Offset: 0x0000CC25
		internal static IntPtr GetSteamRemotePlay()
		{
			return CSteamAPIContext.m_pSteamRemotePlay;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0000EA2C File Offset: 0x0000CC2C
		internal static IntPtr GetSteamNetworkingUtils()
		{
			return CSteamAPIContext.m_pSteamNetworkingUtils;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0000EA33 File Offset: 0x0000CC33
		internal static IntPtr GetSteamNetworkingSockets()
		{
			return CSteamAPIContext.m_pSteamNetworkingSockets;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0000EA3A File Offset: 0x0000CC3A
		internal static IntPtr GetSteamNetworkingMessages()
		{
			return CSteamAPIContext.m_pSteamNetworkingMessages;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0000EA41 File Offset: 0x0000CC41
		internal static IntPtr GetSteamTimeline()
		{
			return CSteamAPIContext.m_pSteamTimeline;
		}

		// Token: 0x04000A8C RID: 2700
		private static IntPtr m_pSteamClient;

		// Token: 0x04000A8D RID: 2701
		private static IntPtr m_pSteamUser;

		// Token: 0x04000A8E RID: 2702
		private static IntPtr m_pSteamFriends;

		// Token: 0x04000A8F RID: 2703
		private static IntPtr m_pSteamUtils;

		// Token: 0x04000A90 RID: 2704
		private static IntPtr m_pSteamMatchmaking;

		// Token: 0x04000A91 RID: 2705
		private static IntPtr m_pSteamUserStats;

		// Token: 0x04000A92 RID: 2706
		private static IntPtr m_pSteamApps;

		// Token: 0x04000A93 RID: 2707
		private static IntPtr m_pSteamMatchmakingServers;

		// Token: 0x04000A94 RID: 2708
		private static IntPtr m_pSteamNetworking;

		// Token: 0x04000A95 RID: 2709
		private static IntPtr m_pSteamRemoteStorage;

		// Token: 0x04000A96 RID: 2710
		private static IntPtr m_pSteamScreenshots;

		// Token: 0x04000A97 RID: 2711
		private static IntPtr m_pSteamGameSearch;

		// Token: 0x04000A98 RID: 2712
		private static IntPtr m_pSteamHTTP;

		// Token: 0x04000A99 RID: 2713
		private static IntPtr m_pController;

		// Token: 0x04000A9A RID: 2714
		private static IntPtr m_pSteamUGC;

		// Token: 0x04000A9B RID: 2715
		private static IntPtr m_pSteamMusic;

		// Token: 0x04000A9C RID: 2716
		private static IntPtr m_pSteamMusicRemote;

		// Token: 0x04000A9D RID: 2717
		private static IntPtr m_pSteamHTMLSurface;

		// Token: 0x04000A9E RID: 2718
		private static IntPtr m_pSteamInventory;

		// Token: 0x04000A9F RID: 2719
		private static IntPtr m_pSteamVideo;

		// Token: 0x04000AA0 RID: 2720
		private static IntPtr m_pSteamParentalSettings;

		// Token: 0x04000AA1 RID: 2721
		private static IntPtr m_pSteamInput;

		// Token: 0x04000AA2 RID: 2722
		private static IntPtr m_pSteamParties;

		// Token: 0x04000AA3 RID: 2723
		private static IntPtr m_pSteamRemotePlay;

		// Token: 0x04000AA4 RID: 2724
		private static IntPtr m_pSteamNetworkingUtils;

		// Token: 0x04000AA5 RID: 2725
		private static IntPtr m_pSteamNetworkingSockets;

		// Token: 0x04000AA6 RID: 2726
		private static IntPtr m_pSteamNetworkingMessages;

		// Token: 0x04000AA7 RID: 2727
		private static IntPtr m_pSteamTimeline;
	}
}
