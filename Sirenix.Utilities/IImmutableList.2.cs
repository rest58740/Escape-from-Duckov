using System;
using System.Collections;
using System.Collections.Generic;

namespace Sirenix.Utilities
{
	// Token: 0x02000024 RID: 36
	public interface IImmutableList<T> : IImmutableList, IList, ICollection, IEnumerable, IList<T>, ICollection<T>, IEnumerable<!0>
	{
		// Token: 0x1700000E RID: 14
		T this[int index]
		{
			get;
		}
	}
}
