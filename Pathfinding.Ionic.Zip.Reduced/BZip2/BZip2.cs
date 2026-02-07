using System;

namespace Pathfinding.Ionic.BZip2
{
	// Token: 0x02000041 RID: 65
	internal static class BZip2
	{
		// Token: 0x06000331 RID: 817 RVA: 0x000147DC File Offset: 0x000129DC
		internal static T[][] InitRectangularArray<T>(int d1, int d2)
		{
			T[][] array = new T[d1][];
			for (int i = 0; i < d1; i++)
			{
				array[i] = new T[d2];
			}
			return array;
		}

		// Token: 0x040001D7 RID: 471
		public static readonly int BlockSizeMultiple = 100000;

		// Token: 0x040001D8 RID: 472
		public static readonly int MinBlockSize = 1;

		// Token: 0x040001D9 RID: 473
		public static readonly int MaxBlockSize = 9;

		// Token: 0x040001DA RID: 474
		public static readonly int MaxAlphaSize = 258;

		// Token: 0x040001DB RID: 475
		public static readonly int MaxCodeLength = 23;

		// Token: 0x040001DC RID: 476
		public static readonly char RUNA = '\0';

		// Token: 0x040001DD RID: 477
		public static readonly char RUNB = '\u0001';

		// Token: 0x040001DE RID: 478
		public static readonly int NGroups = 6;

		// Token: 0x040001DF RID: 479
		public static readonly int G_SIZE = 50;

		// Token: 0x040001E0 RID: 480
		public static readonly int N_ITERS = 4;

		// Token: 0x040001E1 RID: 481
		public static readonly int MaxSelectors = 2 + 900000 / BZip2.G_SIZE;

		// Token: 0x040001E2 RID: 482
		public static readonly int NUM_OVERSHOOT_BYTES = 20;

		// Token: 0x040001E3 RID: 483
		internal static readonly int QSORT_STACK_SIZE = 1000;
	}
}
