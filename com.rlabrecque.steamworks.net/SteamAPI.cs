using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x02000194 RID: 404
	public static class SteamAPI
	{
		// Token: 0x06000929 RID: 2345 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
		public static ESteamAPIInitResult InitEx(out string OutSteamErrMsg)
		{
			InteropHelp.TestIfPlatformSupported();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SteamUtils010").Append("\0");
			stringBuilder.Append("SteamNetworkingUtils004").Append("\0");
			stringBuilder.Append("STEAMAPPS_INTERFACE_VERSION008").Append("\0");
			stringBuilder.Append("SteamFriends017").Append("\0");
			stringBuilder.Append("SteamMatchGameSearch001").Append("\0");
			stringBuilder.Append("STEAMHTMLSURFACE_INTERFACE_VERSION_005").Append("\0");
			stringBuilder.Append("STEAMHTTP_INTERFACE_VERSION003").Append("\0");
			stringBuilder.Append("SteamInput006").Append("\0");
			stringBuilder.Append("STEAMINVENTORY_INTERFACE_V003").Append("\0");
			stringBuilder.Append("SteamMatchMakingServers002").Append("\0");
			stringBuilder.Append("SteamMatchMaking009").Append("\0");
			stringBuilder.Append("STEAMMUSICREMOTE_INTERFACE_VERSION001").Append("\0");
			stringBuilder.Append("STEAMMUSIC_INTERFACE_VERSION001").Append("\0");
			stringBuilder.Append("SteamNetworkingMessages002").Append("\0");
			stringBuilder.Append("SteamNetworkingSockets012").Append("\0");
			stringBuilder.Append("SteamNetworking006").Append("\0");
			stringBuilder.Append("STEAMPARENTALSETTINGS_INTERFACE_VERSION001").Append("\0");
			stringBuilder.Append("SteamParties002").Append("\0");
			stringBuilder.Append("STEAMREMOTEPLAY_INTERFACE_VERSION002").Append("\0");
			stringBuilder.Append("STEAMREMOTESTORAGE_INTERFACE_VERSION016").Append("\0");
			stringBuilder.Append("STEAMSCREENSHOTS_INTERFACE_VERSION003").Append("\0");
			stringBuilder.Append("STEAMUGC_INTERFACE_VERSION020").Append("\0");
			stringBuilder.Append("STEAMUSERSTATS_INTERFACE_VERSION013").Append("\0");
			stringBuilder.Append("SteamUser023").Append("\0");
			stringBuilder.Append("STEAMVIDEO_INTERFACE_V007").Append("\0");
			ESteamAPIInitResult result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(stringBuilder.ToString()))
			{
				IntPtr intPtr = Marshal.AllocHGlobal(1024);
				ESteamAPIInitResult esteamAPIInitResult = NativeMethods.SteamInternal_SteamAPI_Init(utf8StringHandle, intPtr);
				OutSteamErrMsg = InteropHelp.PtrToStringUTF8(intPtr);
				Marshal.FreeHGlobal(intPtr);
				if (esteamAPIInitResult == ESteamAPIInitResult.k_ESteamAPIInitResult_OK)
				{
					if (CSteamAPIContext.Init())
					{
						CallbackDispatcher.Initialize();
					}
					else
					{
						esteamAPIInitResult = ESteamAPIInitResult.k_ESteamAPIInitResult_FailedGeneric;
						OutSteamErrMsg = "[Steamworks.NET] Failed to initialize CSteamAPIContext";
					}
				}
				result = esteamAPIInitResult;
			}
			return result;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0000E084 File Offset: 0x0000C284
		public static bool Init()
		{
			InteropHelp.TestIfPlatformSupported();
			string text;
			return SteamAPI.InitEx(out text) == ESteamAPIInitResult.k_ESteamAPIInitResult_OK;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0000E0A0 File Offset: 0x0000C2A0
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_Shutdown();
			CSteamAPIContext.Clear();
			CallbackDispatcher.Shutdown();
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0000E0B6 File Offset: 0x0000C2B6
		public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0000E0C3 File Offset: 0x0000C2C3
		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_ReleaseCurrentThreadMemory();
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0000E0CF File Offset: 0x0000C2CF
		public static void RunCallbacks()
		{
			CallbackDispatcher.RunFrame(false);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0000E0D7 File Offset: 0x0000C2D7
		public static bool IsSteamRunning()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_IsSteamRunning();
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0000E0E3 File Offset: 0x0000C2E3
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamAPI_GetHSteamPipe();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0000E0F4 File Offset: 0x0000C2F4
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamAPI_GetHSteamUser();
		}
	}
}
