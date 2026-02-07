using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A9F RID: 2719
	public interface IReadOnlyList<out T> : IReadOnlyCollection<T>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x1700114B RID: 4427
		T this[int index]
		{
			get;
		}
	}
}
