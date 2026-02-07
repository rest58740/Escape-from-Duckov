using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200001D RID: 29
	public static class SteamNetworkingSockets
	{
		// Token: 0x0600033F RID: 831 RVA: 0x00008E1C File Offset: 0x0000701C
		public static HSteamListenSocket CreateListenSocketIP(ref SteamNetworkingIPAddr localAddress, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamListenSocket)NativeMethods.ISteamNetworkingSockets_CreateListenSocketIP(CSteamAPIContext.GetSteamNetworkingSockets(), ref localAddress, nOptions, pOptions);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00008E35 File Offset: 0x00007035
		public static HSteamNetConnection ConnectByIPAddress(ref SteamNetworkingIPAddr address, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetConnection)NativeMethods.ISteamNetworkingSockets_ConnectByIPAddress(CSteamAPIContext.GetSteamNetworkingSockets(), ref address, nOptions, pOptions);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00008E4E File Offset: 0x0000704E
		public static HSteamListenSocket CreateListenSocketP2P(int nLocalVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamListenSocket)NativeMethods.ISteamNetworkingSockets_CreateListenSocketP2P(CSteamAPIContext.GetSteamNetworkingSockets(), nLocalVirtualPort, nOptions, pOptions);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00008E67 File Offset: 0x00007067
		public static HSteamNetConnection ConnectP2P(ref SteamNetworkingIdentity identityRemote, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetConnection)NativeMethods.ISteamNetworkingSockets_ConnectP2P(CSteamAPIContext.GetSteamNetworkingSockets(), ref identityRemote, nRemoteVirtualPort, nOptions, pOptions);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00008E81 File Offset: 0x00007081
		public static EResult AcceptConnection(HSteamNetConnection hConn)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_AcceptConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00008E94 File Offset: 0x00007094
		public static bool CloseConnection(HSteamNetConnection hPeer, int nReason, string pszDebug, bool bEnableLinger)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszDebug))
			{
				result = NativeMethods.ISteamNetworkingSockets_CloseConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer, nReason, utf8StringHandle, bEnableLinger);
			}
			return result;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00008EDC File Offset: 0x000070DC
		public static bool CloseListenSocket(HSteamListenSocket hSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_CloseListenSocket(CSteamAPIContext.GetSteamNetworkingSockets(), hSocket);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00008EEE File Offset: 0x000070EE
		public static bool SetConnectionUserData(HSteamNetConnection hPeer, long nUserData)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_SetConnectionUserData(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer, nUserData);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00008F01 File Offset: 0x00007101
		public static long GetConnectionUserData(HSteamNetConnection hPeer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetConnectionUserData(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00008F14 File Offset: 0x00007114
		public static void SetConnectionName(HSteamNetConnection hPeer, string pszName)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszName))
			{
				NativeMethods.ISteamNetworkingSockets_SetConnectionName(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer, utf8StringHandle);
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00008F58 File Offset: 0x00007158
		public static bool GetConnectionName(HSteamNetConnection hPeer, out string pszName, int nMaxLen)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(nMaxLen);
			bool flag = NativeMethods.ISteamNetworkingSockets_GetConnectionName(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer, intPtr, nMaxLen);
			pszName = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00008F94 File Offset: 0x00007194
		public static EResult SendMessageToConnection(HSteamNetConnection hConn, IntPtr pData, uint cbData, int nSendFlags, out long pOutMessageNumber)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_SendMessageToConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, pData, cbData, nSendFlags, out pOutMessageNumber);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00008FAB File Offset: 0x000071AB
		public static void SendMessages(int nMessages, IntPtr[] pMessages, long[] pOutMessageNumberOrResult)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingSockets_SendMessages(CSteamAPIContext.GetSteamNetworkingSockets(), nMessages, pMessages, pOutMessageNumberOrResult);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00008FBF File Offset: 0x000071BF
		public static EResult FlushMessagesOnConnection(HSteamNetConnection hConn)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_FlushMessagesOnConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00008FD1 File Offset: 0x000071D1
		public static int ReceiveMessagesOnConnection(HSteamNetConnection hConn, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			InteropHelp.TestIfAvailableClient();
			if (ppOutMessages != null && ppOutMessages.Length != nMaxMessages)
			{
				throw new ArgumentException("ppOutMessages must be the same size as nMaxMessages!");
			}
			return NativeMethods.ISteamNetworkingSockets_ReceiveMessagesOnConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, ppOutMessages, nMaxMessages);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00008FF9 File Offset: 0x000071F9
		public static bool GetConnectionInfo(HSteamNetConnection hConn, out SteamNetConnectionInfo_t pInfo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetConnectionInfo(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, out pInfo);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000900C File Offset: 0x0000720C
		public static EResult GetConnectionRealTimeStatus(HSteamNetConnection hConn, ref SteamNetConnectionRealTimeStatus_t pStatus, int nLanes, ref SteamNetConnectionRealTimeLaneStatus_t pLanes)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetConnectionRealTimeStatus(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, ref pStatus, nLanes, ref pLanes);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00009024 File Offset: 0x00007224
		public static int GetDetailedConnectionStatus(HSteamNetConnection hConn, out string pszBuf, int cbBuf)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cbBuf);
			int num = NativeMethods.ISteamNetworkingSockets_GetDetailedConnectionStatus(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, intPtr, cbBuf);
			pszBuf = ((num != -1) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00009061 File Offset: 0x00007261
		public static bool GetListenSocketAddress(HSteamListenSocket hSocket, out SteamNetworkingIPAddr address)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetListenSocketAddress(CSteamAPIContext.GetSteamNetworkingSockets(), hSocket, out address);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00009074 File Offset: 0x00007274
		public static bool CreateSocketPair(out HSteamNetConnection pOutConnection1, out HSteamNetConnection pOutConnection2, bool bUseNetworkLoopback, ref SteamNetworkingIdentity pIdentity1, ref SteamNetworkingIdentity pIdentity2)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_CreateSocketPair(CSteamAPIContext.GetSteamNetworkingSockets(), out pOutConnection1, out pOutConnection2, bUseNetworkLoopback, ref pIdentity1, ref pIdentity2);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000908B File Offset: 0x0000728B
		public static EResult ConfigureConnectionLanes(HSteamNetConnection hConn, int nNumLanes, int[] pLanePriorities, ushort[] pLaneWeights)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_ConfigureConnectionLanes(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, nNumLanes, pLanePriorities, pLaneWeights);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000090A0 File Offset: 0x000072A0
		public static bool GetIdentity(out SteamNetworkingIdentity pIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetIdentity(CSteamAPIContext.GetSteamNetworkingSockets(), out pIdentity);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000090B2 File Offset: 0x000072B2
		public static ESteamNetworkingAvailability InitAuthentication()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_InitAuthentication(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000090C3 File Offset: 0x000072C3
		public static ESteamNetworkingAvailability GetAuthenticationStatus(out SteamNetAuthenticationStatus_t pDetails)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetAuthenticationStatus(CSteamAPIContext.GetSteamNetworkingSockets(), out pDetails);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000090D5 File Offset: 0x000072D5
		public static HSteamNetPollGroup CreatePollGroup()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetPollGroup)NativeMethods.ISteamNetworkingSockets_CreatePollGroup(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000090EB File Offset: 0x000072EB
		public static bool DestroyPollGroup(HSteamNetPollGroup hPollGroup)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_DestroyPollGroup(CSteamAPIContext.GetSteamNetworkingSockets(), hPollGroup);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000090FD File Offset: 0x000072FD
		public static bool SetConnectionPollGroup(HSteamNetConnection hConn, HSteamNetPollGroup hPollGroup)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_SetConnectionPollGroup(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, hPollGroup);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00009110 File Offset: 0x00007310
		public static int ReceiveMessagesOnPollGroup(HSteamNetPollGroup hPollGroup, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			InteropHelp.TestIfAvailableClient();
			if (ppOutMessages != null && ppOutMessages.Length != nMaxMessages)
			{
				throw new ArgumentException("ppOutMessages must be the same size as nMaxMessages!");
			}
			return NativeMethods.ISteamNetworkingSockets_ReceiveMessagesOnPollGroup(CSteamAPIContext.GetSteamNetworkingSockets(), hPollGroup, ppOutMessages, nMaxMessages);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00009138 File Offset: 0x00007338
		public static bool ReceivedRelayAuthTicket(IntPtr pvTicket, int cbTicket, out SteamDatagramRelayAuthTicket pOutParsedTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_ReceivedRelayAuthTicket(CSteamAPIContext.GetSteamNetworkingSockets(), pvTicket, cbTicket, out pOutParsedTicket);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000914C File Offset: 0x0000734C
		public static int FindRelayAuthTicketForServer(ref SteamNetworkingIdentity identityGameServer, int nRemoteVirtualPort, out SteamDatagramRelayAuthTicket pOutParsedTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_FindRelayAuthTicketForServer(CSteamAPIContext.GetSteamNetworkingSockets(), ref identityGameServer, nRemoteVirtualPort, out pOutParsedTicket);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00009160 File Offset: 0x00007360
		public static HSteamNetConnection ConnectToHostedDedicatedServer(ref SteamNetworkingIdentity identityTarget, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetConnection)NativeMethods.ISteamNetworkingSockets_ConnectToHostedDedicatedServer(CSteamAPIContext.GetSteamNetworkingSockets(), ref identityTarget, nRemoteVirtualPort, nOptions, pOptions);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000917A File Offset: 0x0000737A
		public static ushort GetHostedDedicatedServerPort()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetHostedDedicatedServerPort(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000918B File Offset: 0x0000738B
		public static SteamNetworkingPOPID GetHostedDedicatedServerPOPID()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamNetworkingPOPID)NativeMethods.ISteamNetworkingSockets_GetHostedDedicatedServerPOPID(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000091A1 File Offset: 0x000073A1
		public static EResult GetHostedDedicatedServerAddress(out SteamDatagramHostedAddress pRouting)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetHostedDedicatedServerAddress(CSteamAPIContext.GetSteamNetworkingSockets(), out pRouting);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000091B3 File Offset: 0x000073B3
		public static HSteamListenSocket CreateHostedDedicatedServerListenSocket(int nLocalVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamListenSocket)NativeMethods.ISteamNetworkingSockets_CreateHostedDedicatedServerListenSocket(CSteamAPIContext.GetSteamNetworkingSockets(), nLocalVirtualPort, nOptions, pOptions);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x000091CC File Offset: 0x000073CC
		public static EResult GetGameCoordinatorServerLogin(IntPtr pLoginInfo, out int pcbSignedBlob, IntPtr pBlob)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetGameCoordinatorServerLogin(CSteamAPIContext.GetSteamNetworkingSockets(), pLoginInfo, out pcbSignedBlob, pBlob);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000091E0 File Offset: 0x000073E0
		public static HSteamNetConnection ConnectP2PCustomSignaling(out ISteamNetworkingConnectionSignaling pSignaling, ref SteamNetworkingIdentity pPeerIdentity, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetConnection)NativeMethods.ISteamNetworkingSockets_ConnectP2PCustomSignaling(CSteamAPIContext.GetSteamNetworkingSockets(), out pSignaling, ref pPeerIdentity, nRemoteVirtualPort, nOptions, pOptions);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000091FC File Offset: 0x000073FC
		public static bool ReceivedP2PCustomSignal(IntPtr pMsg, int cbMsg, out ISteamNetworkingSignalingRecvContext pContext)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_ReceivedP2PCustomSignal(CSteamAPIContext.GetSteamNetworkingSockets(), pMsg, cbMsg, out pContext);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00009210 File Offset: 0x00007410
		public static bool GetCertificateRequest(out int pcbBlob, IntPtr pBlob, out SteamNetworkingErrMsg errMsg)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetCertificateRequest(CSteamAPIContext.GetSteamNetworkingSockets(), out pcbBlob, pBlob, out errMsg);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00009224 File Offset: 0x00007424
		public static bool SetCertificate(IntPtr pCertificate, int cbCertificate, out SteamNetworkingErrMsg errMsg)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_SetCertificate(CSteamAPIContext.GetSteamNetworkingSockets(), pCertificate, cbCertificate, out errMsg);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00009238 File Offset: 0x00007438
		public static void ResetIdentity(ref SteamNetworkingIdentity pIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingSockets_ResetIdentity(CSteamAPIContext.GetSteamNetworkingSockets(), ref pIdentity);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000924A File Offset: 0x0000744A
		public static void RunCallbacks()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingSockets_RunCallbacks(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000925B File Offset: 0x0000745B
		public static bool BeginAsyncRequestFakeIP(int nNumPorts)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_BeginAsyncRequestFakeIP(CSteamAPIContext.GetSteamNetworkingSockets(), nNumPorts);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000926D File Offset: 0x0000746D
		public static void GetFakeIP(int idxFirstPort, out SteamNetworkingFakeIPResult_t pInfo)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingSockets_GetFakeIP(CSteamAPIContext.GetSteamNetworkingSockets(), idxFirstPort, out pInfo);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00009280 File Offset: 0x00007480
		public static HSteamListenSocket CreateListenSocketP2PFakeIP(int idxFakePort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamListenSocket)NativeMethods.ISteamNetworkingSockets_CreateListenSocketP2PFakeIP(CSteamAPIContext.GetSteamNetworkingSockets(), idxFakePort, nOptions, pOptions);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00009299 File Offset: 0x00007499
		public static EResult GetRemoteFakeIPForConnection(HSteamNetConnection hConn, out SteamNetworkingIPAddr pOutAddr)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetRemoteFakeIPForConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, out pOutAddr);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000092AC File Offset: 0x000074AC
		public static IntPtr CreateFakeUDPPort(int idxFakeServerPort)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_CreateFakeUDPPort(CSteamAPIContext.GetSteamNetworkingSockets(), idxFakeServerPort);
		}
	}
}
