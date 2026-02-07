using System;

namespace LeTai
{
	// Token: 0x02000004 RID: 4
	public static class HashUtils
	{
		// Token: 0x06000010 RID: 16 RVA: 0x0000230E File Offset: 0x0000050E
		public static int CombineHashCodes(int h1, int h2)
		{
			return (h1 << 5) + h1 ^ h2;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002317 File Offset: 0x00000517
		public static int CombineHashCodes(int h1, int h2, int h3)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2), h3);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002326 File Offset: 0x00000526
		public static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2), HashUtils.CombineHashCodes(h3, h4));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000233B File Offset: 0x0000053B
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4), h5);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000234D File Offset: 0x0000054D
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4), HashUtils.CombineHashCodes(h5, h6));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002366 File Offset: 0x00000566
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4), HashUtils.CombineHashCodes(h5, h6, h7));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002381 File Offset: 0x00000581
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4), HashUtils.CombineHashCodes(h5, h6, h7, h8));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000239E File Offset: 0x0000059E
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7, h8), h9);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023B8 File Offset: 0x000005B8
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7, h8), HashUtils.CombineHashCodes(h9, h10));
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023D9 File Offset: 0x000005D9
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7, h8), HashUtils.CombineHashCodes(h9, h10, h11));
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023FC File Offset: 0x000005FC
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7, h8), HashUtils.CombineHashCodes(h9, h10, h11, h12));
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002421 File Offset: 0x00000621
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12, int h13)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7, h8), HashUtils.CombineHashCodes(h9, h10, h11, h12, h13));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002448 File Offset: 0x00000648
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12, int h13, int h14)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7, h8), HashUtils.CombineHashCodes(h9, h10, h11, h12, h13, h14));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002471 File Offset: 0x00000671
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12, int h13, int h14, int h15)
		{
			return HashUtils.CombineHashCodes(HashUtils.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7, h8), HashUtils.CombineHashCodes(h9, h10, h11, h12, h13, h14, h15));
		}
	}
}
