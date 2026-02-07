using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EF RID: 239
	[CallbackIdentity(1105)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoresDownloaded_t
	{
		// Token: 0x040002ED RID: 749
		public const int k_iCallback = 1105;

		// Token: 0x040002EE RID: 750
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040002EF RID: 751
		public SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;

		// Token: 0x040002F0 RID: 752
		public int m_cEntryCount;
	}
}
