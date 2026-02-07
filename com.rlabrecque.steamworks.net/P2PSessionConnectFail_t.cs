using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A1 RID: 161
	[CallbackIdentity(1203)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct P2PSessionConnectFail_t
	{
		// Token: 0x040001B2 RID: 434
		public const int k_iCallback = 1203;

		// Token: 0x040001B3 RID: 435
		public CSteamID m_steamIDRemote;

		// Token: 0x040001B4 RID: 436
		public byte m_eP2PSessionError;
	}
}
