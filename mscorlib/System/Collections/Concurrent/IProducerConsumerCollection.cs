using System;
using System.Collections.Generic;

namespace System.Collections.Concurrent
{
	// Token: 0x02000A61 RID: 2657
	public interface IProducerConsumerCollection<T> : IEnumerable<!0>, IEnumerable, ICollection
	{
		// Token: 0x06005F75 RID: 24437
		void CopyTo(T[] array, int index);

		// Token: 0x06005F76 RID: 24438
		bool TryAdd(T item);

		// Token: 0x06005F77 RID: 24439
		bool TryTake(out T item);

		// Token: 0x06005F78 RID: 24440
		T[] ToArray();
	}
}
