using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000020 RID: 32
	public static class UniTaskAsyncEnumerableExtensions
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00002EEB File Offset: 0x000010EB
		public static UniTaskCancelableAsyncEnumerable<T> WithCancellation<T>(this IUniTaskAsyncEnumerable<T> source, CancellationToken cancellationToken)
		{
			return new UniTaskCancelableAsyncEnumerable<T>(source, cancellationToken);
		}
	}
}
