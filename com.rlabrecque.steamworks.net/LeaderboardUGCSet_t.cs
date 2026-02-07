using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F5 RID: 245
	[CallbackIdentity(1111)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardUGCSet_t
	{
		// Token: 0x04000305 RID: 773
		public const int k_iCallback = 1111;

		// Token: 0x04000306 RID: 774
		public EResult m_eResult;

		// Token: 0x04000307 RID: 775
		public SteamLeaderboard_t m_hSteamLeaderboard;
	}
}
