using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017E RID: 382
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardEntry_t
	{
		// Token: 0x04000A37 RID: 2615
		public CSteamID m_steamIDUser;

		// Token: 0x04000A38 RID: 2616
		public int m_nGlobalRank;

		// Token: 0x04000A39 RID: 2617
		public int m_nScore;

		// Token: 0x04000A3A RID: 2618
		public int m_cDetails;

		// Token: 0x04000A3B RID: 2619
		public UGCHandle_t m_hUGC;
	}
}
