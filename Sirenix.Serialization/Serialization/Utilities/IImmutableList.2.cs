using System;
using System.Collections;
using System.Collections.Generic;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000CF RID: 207
	internal interface IImmutableList<T> : IImmutableList, IList, ICollection, IEnumerable, IList<!0>, ICollection<!0>, IEnumerable<!0>
	{
		// Token: 0x17000063 RID: 99
		T this[int index]
		{
			get;
		}
	}
}
