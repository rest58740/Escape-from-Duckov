using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200006C RID: 108
	internal sealed class TakeLast<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000392 RID: 914 RVA: 0x0000D5BD File Offset: 0x0000B7BD
		public TakeLast(IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			this.source = source;
			this.count = count;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000D5D3 File Offset: 0x0000B7D3
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TakeLast<TSource>._TakeLast(this.source, this.count, cancellationToken);
		}

		// Token: 0x04000163 RID: 355
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000164 RID: 356
		private readonly int count;

		// Token: 0x020001D3 RID: 467
		private sealed class _TakeLast : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000848 RID: 2120 RVA: 0x00049265 File Offset: 0x00047465
			public _TakeLast(IUniTaskAsyncEnumerable<TSource> source, int count, CancellationToken cancellationToken)
			{
				this.source = source;
				this.count = count;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x06000849 RID: 2121 RVA: 0x00049282 File Offset: 0x00047482
			// (set) Token: 0x0600084A RID: 2122 RVA: 0x0004928A File Offset: 0x0004748A
			public TSource Current { get; private set; }

			// Token: 0x0600084B RID: 2123 RVA: 0x00049294 File Offset: 0x00047494
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

			// Token: 0x0600084C RID: 2124 RVA: 0x000492F8 File Offset: 0x000474F8
			private void SourceMoveNext()
			{
				if (!this.iterateCompleted)
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
							TakeLast<TSource>._TakeLast.MoveNextCore(this);
							if (!this.continueNext)
							{
								goto IL_99;
							}
							this.continueNext = false;
						}
						this.awaiter.SourceOnCompleted(TakeLast<TSource>._TakeLast.MoveNextCoreDelegate, this);
						IL_99:;
					}
					catch (Exception ex)
					{
						this.completionSource.TrySetException(ex);
					}
					return;
				}
				if (this.queue.Count > 0)
				{
					this.Current = this.queue.Dequeue();
					this.completionSource.TrySetResult(true);
					return;
				}
				this.completionSource.TrySetResult(false);
			}

			// Token: 0x0600084D RID: 2125 RVA: 0x000493C0 File Offset: 0x000475C0
			private static void MoveNextCore(object state)
			{
				TakeLast<TSource>._TakeLast takeLast = (TakeLast<TSource>._TakeLast)state;
				bool flag;
				if (takeLast.TryGetResult<bool>(takeLast.awaiter, ref flag))
				{
					if (!flag)
					{
						takeLast.continueNext = false;
						takeLast.iterateCompleted = true;
						takeLast.SourceMoveNext();
						return;
					}
					if (takeLast.queue.Count < takeLast.count)
					{
						takeLast.queue.Enqueue(takeLast.enumerator.Current);
						if (!takeLast.continueNext)
						{
							takeLast.SourceMoveNext();
							return;
						}
					}
					else
					{
						takeLast.queue.Dequeue();
						takeLast.queue.Enqueue(takeLast.enumerator.Current);
						if (!takeLast.continueNext)
						{
							takeLast.SourceMoveNext();
							return;
						}
					}
				}
				else
				{
					takeLast.continueNext = false;
				}
			}

			// Token: 0x0600084E RID: 2126 RVA: 0x00049470 File Offset: 0x00047670
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x040011C9 RID: 4553
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(TakeLast<TSource>._TakeLast.MoveNextCore);

			// Token: 0x040011CA RID: 4554
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040011CB RID: 4555
			private readonly int count;

			// Token: 0x040011CC RID: 4556
			private CancellationToken cancellationToken;

			// Token: 0x040011CD RID: 4557
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040011CE RID: 4558
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040011CF RID: 4559
			private Queue<TSource> queue;

			// Token: 0x040011D0 RID: 4560
			private bool iterateCompleted;

			// Token: 0x040011D1 RID: 4561
			private bool continueNext;
		}
	}
}
