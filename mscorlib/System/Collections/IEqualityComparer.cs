using System;

namespace System.Collections
{
	// Token: 0x02000A18 RID: 2584
	public interface IEqualityComparer
	{
		// Token: 0x06005B95 RID: 23445
		bool Equals(object x, object y);

		// Token: 0x06005B96 RID: 23446
		int GetHashCode(object obj);
	}
}
