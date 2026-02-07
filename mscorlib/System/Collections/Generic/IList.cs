using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A9C RID: 2716
	public interface IList<T> : ICollection<!0>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x17001146 RID: 4422
		T this[int index]
		{
			get;
			set;
		}

		// Token: 0x0600612D RID: 24877
		int IndexOf(T item);

		// Token: 0x0600612E RID: 24878
		void Insert(int index, T item);

		// Token: 0x0600612F RID: 24879
		void RemoveAt(int index);
	}
}
