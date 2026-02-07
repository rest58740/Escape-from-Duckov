using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A9 RID: 169
	[CallbackIdentity(5701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamRemotePlaySessionConnected_t
	{
		// Token: 0x040001CC RID: 460
		public const int k_iCallback = 5701;

		// Token: 0x040001CD RID: 461
		public RemotePlaySessionID_t m_unSessionID;
	}
}
