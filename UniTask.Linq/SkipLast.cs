using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200005F RID: 95
	internal sealed class SkipLast<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600034C RID: 844 RVA: 0x0000C471 File Offset: 0x0000A671
		public SkipLast(IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			this.source = source;
			this.count = count;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000C487 File Offset: 0x0000A687
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SkipLast<TSource>._SkipLast(this.source, this.count, cancellationToken);
		}

		// Token: 0x0400014B RID: 331
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400014C RID: 332
		private readonly int count;

		// Token: 0x0200019A RID: 410
		private sealed class _SkipLast : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060007B4 RID: 1972 RVA: 0x00041717 File Offset: 0x0003F917
			public _SkipLast(IUniTaskAsyncEnumerable<TSource> source, int count, CancellationToken cancellationToken)
			{
				this.source = source;
				this.count = count;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00041734 File Offset: 0x0003F934
			// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0004173C File Offset: 0x0003F93C
			public TSource Current { get; private set; }

			// Token: 0x060007B7 RID: 1975 RVA: 0x00041748 File Offset: 0x0003F948
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
					this.queue = new Queue<TSource>();
				}
				this.completionSource.Reset();
				this.SourceMoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060007B8 RID: 1976 RVA: 0x000417AC File Offset: 0x0003F9AC
			private void SourceMoveNext()
			{
				try
				{
					for (;;)
					{
						this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
						if (!this.awaiter.IsCompleted)
						{
							break;
						}
						this.continueNext = true;
						SkipLast<TSource>._SkipLast.MoveNextCore(this);
						if (!this.continueNext)
						{
							goto IL_55;
						}
						this.continueNext = false;
					}
					this.awaiter.SourceOnCompleted(SkipLast<TSource>._SkipLast.MoveNextCoreDelegate, this);
					IL_55:;
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x060007B9 RID: 1977 RVA: 0x00041830 File Offset: 0x0003FA30
			private static void MoveNextCore(object state)
			{
				SkipLast<TSource>._SkipLast skipLast = (SkipLast<TSource>._SkipLast)state;
				bool flag;
				if (skipLast.TryGetResult<bool>(skipLast.awaiter, ref flag))
				{
					if (!flag)
					{
						skipLast.continueNext = false;
						skipLast.completionSource.TrySetResult(false);
						return;
					}
					if (skipLast.queue.Count == skipLast.count)
					{
						skipLast.continueNext = false;
						TSource value = skipLast.queue.Dequeue();
						skipLast.Current = value;
						skipLast.queue.Enqueue(skipLast.enumerator.Current);
						skipLast.completionSource.TrySetResult(true);
						return;
					}
					skipLast.queue.Enqueue(skipLast.enumerator.Current);
					if (!skipLast.continueNext)
					{
						skipLast.SourceMoveNext();
						return;
					}
				}
				else
				{
					skipLast.continueNext = false;
				}
			}

			// Token: 0x060007BA RID: 1978 RVA: 0x000418EC File Offset: 0x0003FAEC
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x04000F7A RID: 3962
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(SkipLast<TSource>._SkipLast.MoveNextCore);

			// Token: 0x04000F7B RID: 3963
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000F7C RID: 3964
			private readonly int count;

			// Token: 0x04000F7D RID: 3965
			private CancellationToken cancellationToken;

			// Token: 0x04000F7E RID: 3966
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000F7F RID: 3967
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000F80 RID: 3968
			private Queue<TSource> queue;

			// Token: 0x04000F81 RID: 3969
			private bool continueNext;
		}
	}
}
