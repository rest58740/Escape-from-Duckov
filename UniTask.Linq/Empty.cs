using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200002C RID: 44
	internal class Empty<T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x06000276 RID: 630 RVA: 0x0000927F File Offset: 0x0000747F
		private Empty()
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00009287 File Offset: 0x00007487
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return Empty<T>._Empty.Instance;
		}

		// Token: 0x040000BD RID: 189
		public static readonly IUniTaskAsyncEnumerable<T> Instance = new Empty<T>();

		// Token: 0x02000100 RID: 256
		private class _Empty : IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005C0 RID: 1472 RVA: 0x00026CBE File Offset: 0x00024EBE
			private _Empty()
			{
			}

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00026CC8 File Offset: 0x00024EC8
			public T Current
			{
				get
				{
					return default(T);
				}
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x00026CDE File Offset: 0x00024EDE
			public UniTask<bool> MoveNextAsync()
			{
				return CompletedTasks.False;
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x00026CE8 File Offset: 0x00024EE8
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x040008D8 RID: 2264
			public static readonly IUniTaskAsyncEnumerator<T> Instance = new Empty<T>._Empty();
		}
	}
}
