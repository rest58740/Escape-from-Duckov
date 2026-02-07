using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A92 RID: 2706
	public interface ICollection<T> : IEnumerable<!0>, IEnumerable
	{
		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x0600610F RID: 24847
		int Count { get; }

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x06006110 RID: 24848
		bool IsReadOnly { get; }

		// Token: 0x06006111 RID: 24849
		void Add(T item);

		// Token: 0x06006112 RID: 24850
		void Clear();

		// Token: 0x06006113 RID: 24851
		bool Contains(T item);

		// Token: 0x06006114 RID: 24852
		void CopyTo(T[] array, int arrayIndex);

		// Token: 0x06006115 RID: 24853
		bool Remove(T item);
	}
}
