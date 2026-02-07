using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200006E RID: 110
	internal sealed class TakeUntilCanceled<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000396 RID: 918 RVA: 0x0000D639 File Offset: 0x0000B839
		public TakeUntilCanceled(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			this.source = source;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000D64F File Offset: 0x0000B84F
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TakeUntilCanceled<TSource>._TakeUntilCanceled(this.source, this.cancellationToken, cancellationToken);
		}

		// Token: 0x04000168 RID: 360
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000169 RID: 361
		private readonly CancellationToken cancellationToken;

		// Token: 0x020001D5 RID: 469
		private sealed class _TakeUntilCanceled : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600085A RID: 2138 RVA: 0x00049768 File Offset: 0x00047968
			public _TakeUntilCanceled(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken1, CancellationToken cancellationToken2)
			{
				this.source = source;
				this.cancellationToken1 = cancellationToken1;
				this.cancellationToken2 = cancellationToken2;
				if (cancellationToken1.CanBeCanceled)
				{
					this.cancellationTokenRegistration1 = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken1, TakeUntilCanceled<TSource>._TakeUntilCanceled.CancelDelegate1, this);
				}
				if (cancellationToken1 != cancellationToken2 && cancellationToken2.CanBeCanceled)
				{
					this.cancellationTokenRegistration2 = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken2, TakeUntilCanceled<TSource>._TakeUntilCanceled.CancelDelegate2, this);
				}
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x0600085B RID: 2139 RVA: 0x000497CF File Offset: 0x000479CF
			// (set) Token: 0x0600085C RID: 2140 RVA: 0x000497D7 File Offset: 0x000479D7
			public TSource Current { get; private set; }

			// Token: 0x0600085D RID: 2141 RVA: 0x000497E0 File Offset: 0x000479E0
			public UniTask<bool> MoveNextAsync()
			{
				if (this.cancellationToken1.IsCancellationRequested)
				{
					this.isCanceled = true;
				}
				if (this.cancellationToken2.IsCancellationRequested)
				{
					this.isCanceled = true;
				}
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken2);
				}
				if (this.isCanceled)
				{
					return CompletedTasks.False;
				}
				this.completionSource.Reset();
				this.SourceMoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x0600085E RID: 2142 RVA: 0x00049864 File Offset: 0x00047A64
			private void SourceMoveNext()
			{
				try
				{
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (this.awaiter.IsCompleted)
					{
						TakeUntilCanceled<TSource>._TakeUntilCanceled.MoveNextCore(this);
					}
					else
					{
						this.awaiter.SourceOnCompleted(TakeUntilCanceled<TSource>._TakeUntilCanceled.MoveNextCoreDelegate, this);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x0600085F RID: 2143 RVA: 0x000498D4 File Offset: 0x00047AD4
			private static void MoveNextCore(object state)
			{
				TakeUntilCanceled<TSource>._TakeUntilCanceled takeUntilCanceled = (TakeUntilCanceled<TSource>._TakeUntilCanceled)state;
				bool flag;
				if (takeUntilCanceled.TryGetResult<bool>(takeUntilCanceled.awaiter, ref flag))
				{
					if (flag)
					{
						if (takeUntilCanceled.isCanceled)
						{
							takeUntilCanceled.completionSource.TrySetResult(false);
							return;
						}
						takeUntilCanceled.Current = takeUntilCanceled.enumerator.Current;
						takeUntilCanceled.completionSource.TrySetResult(true);
						return;
					}
					else
					{
						takeUntilCanceled.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x06000860 RID: 2144 RVA: 0x00049940 File Offset: 0x00047B40
			private static void OnCanceled1(object state)
			{
				TakeUntilCanceled<TSource>._TakeUntilCanceled takeUntilCanceled = (TakeUntilCanceled<TSource>._TakeUntilCanceled)state;
				if (!takeUntilCanceled.isCanceled)
				{
					takeUntilCanceled.cancellationTokenRegistration2.Dispose();
					takeUntilCanceled.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x06000861 RID: 2145 RVA: 0x00049974 File Offset: 0x00047B74
			private static void OnCanceled2(object state)
			{
				TakeUntilCanceled<TSource>._TakeUntilCanceled takeUntilCanceled = (TakeUntilCanceled<TSource>._TakeUntilCanceled)state;
				if (!takeUntilCanceled.isCanceled)
				{
					takeUntilCanceled.cancellationTokenRegistration1.Dispose();
					takeUntilCanceled.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x06000862 RID: 2146 RVA: 0x000499A8 File Offset: 0x00047BA8
			public UniTask DisposeAsync()
			{
				this.cancellationTokenRegistration1.Dispose();
				this.cancellationTokenRegistration2.Dispose();
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x040011DD RID: 4573
			private static readonly Action<object> CancelDelegate1 = new Action<object>(TakeUntilCanceled<TSource>._TakeUntilCanceled.OnCanceled1);

			// Token: 0x040011DE RID: 4574
			private static readonly Action<object> CancelDelegate2 = new Action<object>(TakeUntilCanceled<TSource>._TakeUntilCanceled.OnCanceled2);

			// Token: 0x040011DF RID: 4575
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(TakeUntilCanceled<TSource>._TakeUntilCanceled.MoveNextCore);

			// Token: 0x040011E0 RID: 4576
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040011E1 RID: 4577
			private CancellationToken cancellationToken1;

			// Token: 0x040011E2 RID: 4578
			private CancellationToken cancellationToken2;

			// Token: 0x040011E3 RID: 4579
			private CancellationTokenRegistration cancellationTokenRegistration1;

			// Token: 0x040011E4 RID: 4580
			private CancellationTokenRegistration cancellationTokenRegistration2;

			// Token: 0x040011E5 RID: 4581
			private bool isCanceled;

			// Token: 0x040011E6 RID: 4582
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040011E7 RID: 4583
			private UniTask<bool>.Awaiter awaiter;
		}
	}
}
