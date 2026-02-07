using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000007 RID: 7
	public interface IAsyncReactiveProperty<T> : IReadOnlyAsyncReactiveProperty<T>, IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001A RID: 26
		// (set) Token: 0x0600001B RID: 27
		T Value { get; set; }
	}
}
