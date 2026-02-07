using System;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x02000061 RID: 97
	internal static class InternalConstants
	{
		// Token: 0x0400034D RID: 845
		internal static readonly int MAX_BITS = 15;

		// Token: 0x0400034E RID: 846
		internal static readonly int BL_CODES = 19;

		// Token: 0x0400034F RID: 847
		internal static readonly int D_CODES = 30;

		// Token: 0x04000350 RID: 848
		internal static readonly int LITERALS = 256;

		// Token: 0x04000351 RID: 849
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x04000352 RID: 850
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x04000353 RID: 851
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x04000354 RID: 852
		internal static readonly int REP_3_6 = 16;

		// Token: 0x04000355 RID: 853
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x04000356 RID: 854
		internal static readonly int REPZ_11_138 = 18;
	}
}
