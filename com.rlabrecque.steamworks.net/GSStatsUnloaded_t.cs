using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000053 RID: 83
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSStatsUnloaded_t
	{
		// Token: 0x04000098 RID: 152
		public const int k_iCallback = 1108;

		// Token: 0x04000099 RID: 153
		public CSteamID m_steamIDUser;
	}
}
