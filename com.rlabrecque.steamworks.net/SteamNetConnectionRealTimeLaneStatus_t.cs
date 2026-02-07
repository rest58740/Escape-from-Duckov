using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000183 RID: 387
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetConnectionRealTimeLaneStatus_t
	{
		// Token: 0x04000A5D RID: 2653
		public int m_cbPendingUnreliable;

		// Token: 0x04000A5E RID: 2654
		public int m_cbPendingReliable;

		// Token: 0x04000A5F RID: 2655
		public int m_cbSentUnackedReliable;

		// Token: 0x04000A60 RID: 2656
		public int _reservePad1;

		// Token: 0x04000A61 RID: 2657
		public SteamNetworkingMicroseconds m_usecQueueTime;

		// Token: 0x04000A62 RID: 2658
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public uint[] reserved;
	}
}
