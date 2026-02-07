using System;
using System.Collections.Generic;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000CB RID: 203
	public class FastTypeComparer : IEqualityComparer<Type>
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x0002A011 File Offset: 0x00028211
		public bool Equals(Type x, Type y)
		{
			return x == y || x == y;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0002A020 File Offset: 0x00028220
		public int GetHashCode(Type obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x0400020F RID: 527
		public static readonly FastTypeComparer Instance = new FastTypeComparer();
	}
}
