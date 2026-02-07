using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F2 RID: 242
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsUnloaded_t
	{
		// Token: 0x040002FB RID: 763
		public const int k_iCallback = 1108;

		// Token: 0x040002FC RID: 764
		public CSteamID m_steamIDUser;
	}
}
