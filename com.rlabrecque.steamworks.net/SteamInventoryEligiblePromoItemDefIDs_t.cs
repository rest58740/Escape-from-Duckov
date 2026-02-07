using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000075 RID: 117
	[CallbackIdentity(4703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryEligiblePromoItemDefIDs_t
	{
		// Token: 0x04000128 RID: 296
		public const int k_iCallback = 4703;

		// Token: 0x04000129 RID: 297
		public EResult m_result;

		// Token: 0x0400012A RID: 298
		public CSteamID m_steamID;

		// Token: 0x0400012B RID: 299
		public int m_numEligiblePromoItemDefs;

		// Token: 0x0400012C RID: 300
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
