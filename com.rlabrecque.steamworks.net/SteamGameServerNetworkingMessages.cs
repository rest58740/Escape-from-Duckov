using System;

namespace Steamworks
{
	// Token: 0x0200000B RID: 11
	public static class SteamGameServerNetworkingMessages
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00004D7B File Offset: 0x00002F7B
		public static EResult SendMessageToUser(ref SteamNetworkingIdentity identityRemote, IntPtr pubData, uint cubData, int nSendFlags, int nRemoteChannel)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_SendMessageToUser(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote, pubData, cubData, nSendFlags, nRemoteChannel);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00004D92 File Offset: 0x00002F92
		public static int ReceiveMessagesOnChannel(int nLocalChannel, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			InteropHelp.TestIfAvailableGameServer();
			if (ppOutMessages != null && ppOutMessages.Length != nMaxMessages)
			{
				throw new ArgumentException("ppOutMessages must be the same size as nMaxMessages!");
			}
			return NativeMethods.ISteamNetworkingMessages_ReceiveMessagesOnChannel(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), nLocalChannel, ppOutMessages, nMaxMessages);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00004DBA File Offset: 0x00002FBA
		public static bool AcceptSessionWithUser(ref SteamNetworkingIdentity identityRemote)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_AcceptSessionWithUser(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004DCC File Offset: 0x00002FCC
		public static bool CloseSessionWithUser(ref SteamNetworkingIdentity identityRemote)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_CloseSessionWithUser(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004DDE File Offset: 0x00002FDE
		public static bool CloseChannelWithUser(ref SteamNetworkingIdentity identityRemote, int nLocalChannel)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_CloseChannelWithUser(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote, nLocalChannel);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004DF1 File Offset: 0x00002FF1
		public static ESteamNetworkingConnectionState GetSessionConnectionInfo(ref SteamNetworkingIdentity identityRemote, out SteamNetConnectionInfo_t pConnectionInfo, out SteamNetConnectionRealTimeStatus_t pQuickStatus)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_GetSessionConnectionInfo(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote, out pConnectionInfo, out pQuickStatus);
		}
	}
}
