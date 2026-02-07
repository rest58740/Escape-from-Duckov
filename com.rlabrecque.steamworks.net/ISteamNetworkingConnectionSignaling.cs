using System;

namespace Steamworks
{
	// Token: 0x020001B4 RID: 436
	[Serializable]
	public struct ISteamNetworkingConnectionSignaling
	{
		// Token: 0x06000A8D RID: 2701 RVA: 0x00010121 File Offset: 0x0000E321
		public bool SendSignal(HSteamNetConnection hConn, ref SteamNetConnectionInfo_t info, IntPtr pMsg, int cbMsg)
		{
			return NativeMethods.SteamAPI_ISteamNetworkingConnectionSignaling_SendSignal(ref this, hConn, ref info, pMsg, cbMsg);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0001012E File Offset: 0x0000E32E
		public void Release()
		{
			NativeMethods.SteamAPI_ISteamNetworkingConnectionSignaling_Release(ref this);
		}
	}
}
