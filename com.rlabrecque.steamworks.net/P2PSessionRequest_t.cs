using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A0 RID: 160
	[CallbackIdentity(1202)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionRequest_t
	{
		// Token: 0x040001B0 RID: 432
		public const int k_iCallback = 1202;

		// Token: 0x040001B1 RID: 433
		public CSteamID m_steamIDRemote;
	}
}
