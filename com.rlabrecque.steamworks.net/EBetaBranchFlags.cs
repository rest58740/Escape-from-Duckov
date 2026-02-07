using System;

namespace Steamworks
{
	// Token: 0x02000162 RID: 354
	[Flags]
	public enum EBetaBranchFlags
	{
		// Token: 0x040008E2 RID: 2274
		k_EBetaBranch_None = 0,
		// Token: 0x040008E3 RID: 2275
		k_EBetaBranch_Default = 1,
		// Token: 0x040008E4 RID: 2276
		k_EBetaBranch_Available = 2,
		// Token: 0x040008E5 RID: 2277
		k_EBetaBranch_Private = 4,
		// Token: 0x040008E6 RID: 2278
		k_EBetaBranch_Selected = 8,
		// Token: 0x040008E7 RID: 2279
		k_EBetaBranch_Installed = 16
	}
}
