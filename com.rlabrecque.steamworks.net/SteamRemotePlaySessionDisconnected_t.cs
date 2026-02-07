using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AA RID: 170
	[CallbackIdentity(5702)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamRemotePlaySessionDisconnected_t
	{
		// Token: 0x040001CE RID: 462
		public const int k_iCallback = 5702;

		// Token: 0x040001CF RID: 463
		public RemotePlaySessionID_t m_unSessionID;
	}
}
