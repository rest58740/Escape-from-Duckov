using System;

namespace Steamworks
{
	// Token: 0x0200001C RID: 28
	public static class SteamNetworkingMessages
	{
		// Token: 0x06000339 RID: 825 RVA: 0x00008D92 File Offset: 0x00006F92
		public static EResult SendMessageToUser(ref SteamNetworkingIdentity identityRemote, IntPtr pubData, uint cubData, int nSendFlags, int nRemoteChannel)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingMessages_SendMessageToUser(CSteamAPIContext.GetSteamNetworkingMessages(), ref identityRemote, pubData, cubData, nSendFlags, nRemoteChannel);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00008DA9 File Offset: 0x00006FA9
		public static int ReceiveMessagesOnChannel(int nLocalChannel, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			InteropHelp.TestIfAvailableClient();
			if (ppOutMessages != null && ppOutMessages.Length != nMaxMessages)
			{
				throw new ArgumentException("ppOutMessages must be the same size as nMaxMessages!");
			}
			return NativeMethods.ISteamNetworkingMessages_ReceiveMessagesOnChannel(CSteamAPIContext.GetSteamNetworkingMessages(), nLocalChannel, ppOutMessages, nMaxMessages);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00008DD1 File Offset: 0x00006FD1
		public static bool AcceptSessionWithUser(ref SteamNetworkingIdentity identityRemote)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingMessages_AcceptSessionWithUser(CSteamAPIContext.GetSteamNetworkingMessages(), ref identityRemote);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00008DE3 File Offset: 0x00006FE3
		public static bool CloseSessionWithUser(ref SteamNetworkingIdentity identityRemote)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingMessages_CloseSessionWithUser(CSteamAPIContext.GetSteamNetworkingMessages(), ref identityRemote);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00008DF5 File Offset: 0x00006FF5
		public static bool CloseChannelWithUser(ref SteamNetworkingIdentity identityRemote, int nLocalChannel)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingMessages_CloseChannelWithUser(CSteamAPIContext.GetSteamNetworkingMessages(), ref identityRemote, nLocalChannel);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00008E08 File Offset: 0x00007008
		public static ESteamNetworkingConnectionState GetSessionConnectionInfo(ref SteamNetworkingIdentity identityRemote, out SteamNetConnectionInfo_t pConnectionInfo, out SteamNetConnectionRealTimeStatus_t pQuickStatus)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingMessages_GetSessionConnectionInfo(CSteamAPIContext.GetSteamNetworkingMessages(), ref identityRemote, out pConnectionInfo, out pQuickStatus);
		}
	}
}
