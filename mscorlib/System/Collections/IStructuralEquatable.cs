using System;

namespace System.Collections
{
	// Token: 0x02000A1B RID: 2587
	public interface IStructuralEquatable
	{
		// Token: 0x06005BA3 RID: 23459
		bool Equals(object other, IEqualityComparer comparer);

		// Token: 0x06005BA4 RID: 23460
		int GetHashCode(IEqualityComparer comparer);
	}
}
