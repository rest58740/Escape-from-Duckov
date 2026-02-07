using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000006 RID: 6
	public interface IReadOnlyAsyncReactiveProperty<T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000017 RID: 23
		T Value { get; }

		// Token: 0x06000018 RID: 24
		IUniTaskAsyncEnumerable<T> WithoutCurrent();

		// Token: 0x06000019 RID: 25
		UniTask<T> WaitAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
