using System;

namespace System.Collections
{
	// Token: 0x02000A38 RID: 2616
	public static class StructuralComparisons
	{
		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06005CFA RID: 23802 RVA: 0x00138D20 File Offset: 0x00136F20
		public static IComparer StructuralComparer
		{
			get
			{
				IComparer comparer = StructuralComparisons.s_StructuralComparer;
				if (comparer == null)
				{
					comparer = new StructuralComparer();
					StructuralComparisons.s_StructuralComparer = comparer;
				}
				return comparer;
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06005CFB RID: 23803 RVA: 0x00138D48 File Offset: 0x00136F48
		public static IEqualityComparer StructuralEqualityComparer
		{
			get
			{
				IEqualityComparer equalityComparer = StructuralComparisons.s_StructuralEqualityComparer;
				if (equalityComparer == null)
				{
					equalityComparer = new StructuralEqualityComparer();
					StructuralComparisons.s_StructuralEqualityComparer = equalityComparer;
				}
				return equalityComparer;
			}
		}

		// Token: 0x040038CE RID: 14542
		private static volatile IComparer s_StructuralComparer;

		// Token: 0x040038CF RID: 14543
		private static volatile IEqualityComparer s_StructuralEqualityComparer;
	}
}
