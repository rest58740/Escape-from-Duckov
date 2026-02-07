using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F6 RID: 246
	[CallbackIdentity(1112)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalStatsReceived_t
	{
		// Token: 0x04000308 RID: 776
		public const int k_iCallback = 1112;

		// Token: 0x04000309 RID: 777
		public ulong m_nGameID;

		// Token: 0x0400030A RID: 778
		public EResult m_eResult;
	}
}
