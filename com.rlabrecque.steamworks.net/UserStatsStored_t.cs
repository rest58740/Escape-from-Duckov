using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EC RID: 236
	[CallbackIdentity(1102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsStored_t
	{
		// Token: 0x040002E1 RID: 737
		public const int k_iCallback = 1102;

		// Token: 0x040002E2 RID: 738
		public ulong m_nGameID;

		// Token: 0x040002E3 RID: 739
		public EResult m_eResult;
	}
}
