using System;
using System.Collections.Generic;

namespace Animancer
{
	// Token: 0x02000007 RID: 7
	public sealed class FastReferenceComparer : IEqualityComparer<object>
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00002F4D File Offset: 0x0000114D
		bool IEqualityComparer<object>.Equals(object x, object y)
		{
			return x == y;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002F53 File Offset: 0x00001153
		int IEqualityComparer<object>.GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000007 RID: 7
		public static readonly FastReferenceComparer Instance = new FastReferenceComparer();
	}
}
