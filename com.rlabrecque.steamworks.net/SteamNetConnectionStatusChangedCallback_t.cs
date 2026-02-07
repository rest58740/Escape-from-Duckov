using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A5 RID: 165
	[CallbackIdentity(1221)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetConnectionStatusChangedCallback_t
	{
		// Token: 0x040001BE RID: 446
		public const int k_iCallback = 1221;

		// Token: 0x040001BF RID: 447
		public HSteamNetConnection m_hConn;

		// Token: 0x040001C0 RID: 448
		public SteamNetConnectionInfo_t m_info;

		// Token: 0x040001C1 RID: 449
		public ESteamNetworkingConnectionState m_eOldState;
	}
}
