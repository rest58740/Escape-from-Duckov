using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x02000195 RID: 405
	public static class GameServer
	{
		// Token: 0x06000932 RID: 2354 RVA: 0x0000E108 File Offset: 0x0000C308
		public static ESteamAPIInitResult InitEx(uint unIP, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString, out string OutSteamErrMsg)
		{
			InteropHelp.TestIfPlatformSupported();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SteamUtils010").Append('\0');
			stringBuilder.Append("SteamNetworkingUtils004").Append('\0');
			stringBuilder.Append("SteamGameServer015").Append('\0');
			stringBuilder.Append("SteamGameServerStats001").Append('\0');
			stringBuilder.Append("STEAMHTTP_INTERFACE_VERSION003").Append('\0');
			stringBuilder.Append("STEAMINVENTORY_INTERFACE_V003").Append('\0');
			stringBuilder.Append("SteamNetworking006").Append('\0');
			stringBuilder.Append("SteamNetworkingMessages002").Append('\0');
			stringBuilder.Append("SteamNetworkingSockets012").Append('\0');
			stringBuilder.Append("STEAMUGC_INTERFACE_VERSION020").Append('\0');
			ESteamAPIInitResult result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(stringBuilder.ToString()))
				{
					IntPtr intPtr = Marshal.AllocHGlobal(1024);
					ESteamAPIInitResult esteamAPIInitResult = NativeMethods.SteamInternal_GameServer_Init_V2(unIP, usGamePort, usQueryPort, eServerMode, utf8StringHandle, utf8StringHandle2, intPtr);
					OutSteamErrMsg = InteropHelp.PtrToStringUTF8(intPtr);
					Marshal.FreeHGlobal(intPtr);
					if (esteamAPIInitResult == ESteamAPIInitResult.k_ESteamAPIInitResult_OK)
					{
						if (CSteamGameServerAPIContext.Init())
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
			}
			return result;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0000E268 File Offset: 0x0000C468
		public static bool Init(uint unIP, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString)
		{
			InteropHelp.TestIfPlatformSupported();
			string text;
			return GameServer.InitEx(unIP, usGamePort, usQueryPort, eServerMode, pchVersionString, out text) == ESteamAPIInitResult.k_ESteamAPIInitResult_OK;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0000E28A File Offset: 0x0000C48A
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_Shutdown();
			CSteamGameServerAPIContext.Clear();
			CallbackDispatcher.Shutdown();
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
		public static void RunCallbacks()
		{
			CallbackDispatcher.RunFrame(true);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0000E2A8 File Offset: 0x0000C4A8
		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_ReleaseCurrentThreadMemory();
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0000E2B4 File Offset: 0x0000C4B4
		public static bool BSecure()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamGameServer_BSecure();
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0000E2C0 File Offset: 0x0000C4C0
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfPlatformSupported();
			return (CSteamID)NativeMethods.SteamGameServer_GetSteamID();
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0000E2D1 File Offset: 0x0000C4D1
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamGameServer_GetHSteamPipe();
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0000E2E2 File Offset: 0x0000C4E2
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamGameServer_GetHSteamUser();
		}
	}
}
