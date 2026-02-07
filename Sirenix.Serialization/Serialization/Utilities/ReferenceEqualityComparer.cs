using System;
using System.Collections.Generic;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000D6 RID: 214
	internal class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
	{
		// Token: 0x06000654 RID: 1620 RVA: 0x0002A618 File Offset: 0x00028818
		public bool Equals(T x, T y)
		{
			return x == y;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0002A628 File Offset: 0x00028828
		public int GetHashCode(T obj)
		{
			int result;
			try
			{
				result = obj.GetHashCode();
			}
			catch (NullReferenceException)
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x0400022B RID: 555
		public static readonly ReferenceEqualityComparer<T> Default = new ReferenceEqualityComparer<T>();
	}
}
