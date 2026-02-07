using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200006D RID: 109
	internal sealed class TakeUntil<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000394 RID: 916 RVA: 0x0000D5E7 File Offset: 0x0000B7E7
		public TakeUntil(IUniTaskAsyncEnumerable<TSource> source, UniTask other, Func<CancellationToken, UniTask> other2)
		{
			this.source = source;
			this.other = other;
			this.other2 = other2;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000D604 File Offset: 0x0000B804
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (this.other2 != null)
			{
				return new TakeUntil<TSource>._TakeUntil(this.source, this.other2(cancellationToken), cancellationToken);
			}
			return new TakeUntil<TSource>._TakeUntil(this.source, this.other, cancellationToken);
		}

		// Token: 0x04000165 RID: 357
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000166 RID: 358
		private readonly UniTask other;

		// Token: 0x04000167 RID: 359
		private readonly Func<CancellationToken, UniTask> other2;

		// Token: 0x020001D4 RID: 468
		private sealed class _TakeUntil : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000850 RID: 2128 RVA: 0x000494B0 File Offset: 0x000476B0
			public _TakeUntil(IUniTaskAsyncEnumerable<TSource> source, UniTask other, CancellationToken cancellationToken1)
			{
				this.source = source;
				this.cancellationToken1 = cancellationToken1;
				if (cancellationToken1.CanBeCanceled)
				{
					this.cancellationTokenRegistration1 = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken1, TakeUntil<TSource>._TakeUntil.CancelDelegate1, this);
				}
				this.RunOther(other).Forget();
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x06000851 RID: 2129 RVA: 0x000494FB File Offset: 0x000476FB
			// (set) Token: 0x06000852 RID: 2130 RVA: 0x00049503 File Offset: 0x00047703
			public TSource Current { get; private set; }

			// Token: 0x06000853 RID: 2131 RVA: 0x0004950C File Offset: 0x0004770C
			public UniTask<bool> MoveNextAsync()
			{
				if (this.completed)
				{
					return CompletedTasks.False;
				}
				if (this.exception != null)
				{
					return UniTask.FromException<bool>(this.exception);
				}
				if (this.cancellationToken1.IsCancellationRequested)
				{
					return UniTask.FromCanceled<bool>(this.cancellationToken1);
				}
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken1);
				}
				this.completionSource.Reset();
				this.SourceMoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000854 RID: 2132 RVA: 0x00049598 File Offset: 0x00047798
			private void SourceMoveNext()
			{
				try
				{
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (this.awaiter.IsCompleted)
					{
						TakeUntil<TSource>._TakeUntil.MoveNextCore(this);
					}
					else
					{
						this.awaiter.SourceOnCompleted(TakeUntil<TSource>._TakeUntil.MoveNextCoreDelegate, this);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x06000855 RID: 2133 RVA: 0x00049608 File Offset: 0x00047808
			private static void MoveNextCore(object state)
			{
				TakeUntil<TSource>._TakeUntil takeUntil = (TakeUntil<TSource>._TakeUntil)state;
				bool flag;
				if (takeUntil.TryGetResult<bool>(takeUntil.awaiter, ref flag))
				{
					if (flag)
					{
						if (takeUntil.exception != null)
						{
							takeUntil.completionSource.TrySetException(takeUntil.exception);
							return;
						}
						if (takeUntil.cancellationToken1.IsCancellationRequested)
						{
							takeUntil.completionSource.TrySetCanceled(takeUntil.cancellationToken1);
							return;
						}
						takeUntil.Current = takeUntil.enumerator.Current;
						takeUntil.completionSource.TrySetResult(true);
						return;
					}
					else
					{
						takeUntil.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x06000856 RID: 2134 RVA: 0x00049698 File Offset: 0x00047898
			private UniTaskVoid RunOther(UniTask other)
			{
				TakeUntil<TSource>._TakeUntil.<RunOther>d__17 <RunOther>d__;
				<RunOther>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunOther>d__.<>4__this = this;
				<RunOther>d__.other = other;
				<RunOther>d__.<>1__state = -1;
				<RunOther>d__.<>t__builder.Start<TakeUntil<TSource>._TakeUntil.<RunOther>d__17>(ref <RunOther>d__);
				return <RunOther>d__.<>t__builder.Task;
			}

			// Token: 0x06000857 RID: 2135 RVA: 0x000496E4 File Offset: 0x000478E4
			private static void OnCanceled1(object state)
			{
				TakeUntil<TSource>._TakeUntil takeUntil = (TakeUntil<TSource>._TakeUntil)state;
				takeUntil.completionSource.TrySetCanceled(takeUntil.cancellationToken1);
			}

			// Token: 0x06000858 RID: 2136 RVA: 0x0004970C File Offset: 0x0004790C
			public UniTask DisposeAsync()
			{
				this.cancellationTokenRegistration1.Dispose();
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x040011D3 RID: 4563
			private static readonly Action<object> CancelDelegate1 = new Action<object>(TakeUntil<TSource>._TakeUntil.OnCanceled1);

			// Token: 0x040011D4 RID: 4564
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(TakeUntil<TSource>._TakeUntil.MoveNextCore);

			// Token: 0x040011D5 RID: 4565
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040011D6 RID: 4566
			private CancellationToken cancellationToken1;

			// Token: 0x040011D7 RID: 4567
			private CancellationTokenRegistration cancellationTokenRegistration1;

			// Token: 0x040011D8 RID: 4568
			private bool completed;

			// Token: 0x040011D9 RID: 4569
			private Exception exception;

			// Token: 0x040011DA RID: 4570
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040011DB RID: 4571
			private UniTask<bool>.Awaiter awaiter;
		}
	}
}
