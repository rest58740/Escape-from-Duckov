using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200000A RID: 10
	public static class StateExtensions
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000027A6 File Offset: 0x000009A6
		public static ReadOnlyAsyncReactiveProperty<T> ToReadOnlyAsyncReactiveProperty<T>(this IUniTaskAsyncEnumerable<T> source, CancellationToken cancellationToken)
		{
			return new ReadOnlyAsyncReactiveProperty<T>(source, cancellationToken);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000027AF File Offset: 0x000009AF
		public static ReadOnlyAsyncReactiveProperty<T> ToReadOnlyAsyncReactiveProperty<T>(this IUniTaskAsyncEnumerable<T> source, T initialValue, CancellationToken cancellationToken)
		{
			return new ReadOnlyAsyncReactiveProperty<T>(initialValue, source, cancellationToken);
		}
	}
}
