using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A4 RID: 164
	[CallbackIdentity(1252)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetworkingMessagesSessionFailed_t
	{
		// Token: 0x040001BC RID: 444
		public const int k_iCallback = 1252;

		// Token: 0x040001BD RID: 445
		public SteamNetConnectionInfo_t m_info;
	}
}
