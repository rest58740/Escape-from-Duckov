using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A94 RID: 2708
	public interface IComparer<in T>
	{
		// Token: 0x06006118 RID: 24856
		int Compare(T x, T y);
	}
}
