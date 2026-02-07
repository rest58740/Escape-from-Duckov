using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F0 RID: 240
	[CallbackIdentity(1106)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoreUploaded_t
	{
		// Token: 0x040002F1 RID: 753
		public const int k_iCallback = 1106;

		// Token: 0x040002F2 RID: 754
		public byte m_bSuccess;

		// Token: 0x040002F3 RID: 755
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040002F4 RID: 756
		public int m_nScore;

		// Token: 0x040002F5 RID: 757
		public byte m_bScoreChanged;

		// Token: 0x040002F6 RID: 758
		public int m_nGlobalRankNew;

		// Token: 0x040002F7 RID: 759
		public int m_nGlobalRankPrevious;
	}
}
