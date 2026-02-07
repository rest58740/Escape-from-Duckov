using System;
using System.Collections.Generic;

namespace Sirenix.Utilities
{
	// Token: 0x02000030 RID: 48
	public class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
	{
		// Token: 0x0600022C RID: 556 RVA: 0x0000CF71 File Offset: 0x0000B171
		public bool Equals(T x, T y)
		{
			return x == y;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000CF84 File Offset: 0x0000B184
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

		// Token: 0x04000068 RID: 104
		public static readonly ReferenceEqualityComparer<T> Default = new ReferenceEqualityComparer<T>();
	}
}
