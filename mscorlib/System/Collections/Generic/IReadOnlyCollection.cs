using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A9D RID: 2717
	public interface IReadOnlyCollection<out T> : IEnumerable<!0>, IEnumerable
	{
		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x06006130 RID: 24880
		int Count { get; }
	}
}
