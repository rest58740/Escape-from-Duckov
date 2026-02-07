using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017B RID: 379
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionState_t
	{
		// Token: 0x04000A12 RID: 2578
		public byte m_bConnectionActive;

		// Token: 0x04000A13 RID: 2579
		public byte m_bConnecting;

		// Token: 0x04000A14 RID: 2580
		public byte m_eP2PSessionError;

		// Token: 0x04000A15 RID: 2581
		public byte m_bUsingRelay;

		// Token: 0x04000A16 RID: 2582
		public int m_nBytesQueuedForSend;

		// Token: 0x04000A17 RID: 2583
		public int m_nPacketsQueuedForSend;

		// Token: 0x04000A18 RID: 2584
		public uint m_nRemoteIP;

		// Token: 0x04000A19 RID: 2585
		public ushort m_nRemotePort;
	}
}
