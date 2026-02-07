using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A9B RID: 2715
	public interface IEqualityComparer<in T>
	{
		// Token: 0x06006129 RID: 24873
		bool Equals(T x, T y);

		// Token: 0x0600612A RID: 24874
		int GetHashCode(T obj);
	}
}
