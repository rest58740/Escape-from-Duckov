using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000182 RID: 386
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetConnectionRealTimeStatus_t
	{
		// Token: 0x04000A4F RID: 2639
		public ESteamNetworkingConnectionState m_eState;

		// Token: 0x04000A50 RID: 2640
		public int m_nPing;

		// Token: 0x04000A51 RID: 2641
		public float m_flConnectionQualityLocal;

		// Token: 0x04000A52 RID: 2642
		public float m_flConnectionQualityRemote;

		// Token: 0x04000A53 RID: 2643
		public float m_flOutPacketsPerSec;

		// Token: 0x04000A54 RID: 2644
		public float m_flOutBytesPerSec;

		// Token: 0x04000A55 RID: 2645
		public float m_flInPacketsPerSec;

		// Token: 0x04000A56 RID: 2646
		public float m_flInBytesPerSec;

		// Token: 0x04000A57 RID: 2647
		public int m_nSendRateBytesPerSecond;

		// Token: 0x04000A58 RID: 2648
		public int m_cbPendingUnreliable;

		// Token: 0x04000A59 RID: 2649
		public int m_cbPendingReliable;

		// Token: 0x04000A5A RID: 2650
		public int m_cbSentUnackedReliable;

		// Token: 0x04000A5B RID: 2651
		public SteamNetworkingMicroseconds m_usecQueueTime;

		// Token: 0x04000A5C RID: 2652
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public uint[] reserved;
	}
}
