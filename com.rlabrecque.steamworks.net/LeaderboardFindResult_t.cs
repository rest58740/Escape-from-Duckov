using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EE RID: 238
	[CallbackIdentity(1104)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardFindResult_t
	{
		// Token: 0x040002EA RID: 746
		public const int k_iCallback = 1104;

		// Token: 0x040002EB RID: 747
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040002EC RID: 748
		public byte m_bLeaderboardFound;
	}
}
