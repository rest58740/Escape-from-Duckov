using System;
using System.Collections.Generic;

namespace Animancer
{
	// Token: 0x02000006 RID: 6
	public sealed class FastComparer : IEqualityComparer<object>
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00002F28 File Offset: 0x00001128
		bool IEqualityComparer<object>.Equals(object x, object y)
		{
			return object.Equals(x, y);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00002F31 File Offset: 0x00001131
		int IEqualityComparer<object>.GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000006 RID: 6
		public static readonly FastComparer Instance = new FastComparer();
	}
}
