using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200008C RID: 140
	internal sealed class ZipAwait<TFirst, TSecond, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x060003D8 RID: 984 RVA: 0x0000E090 File Offset: 0x0000C290
		public ZipAwait(IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, UniTask<TResult>> resultSelector)
		{
			this.first = first;
			this.second = second;
			this.resultSelector = resultSelector;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000E0AD File Offset: 0x0000C2AD
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ZipAwait<TFirst, TSecond, TResult>._ZipAwait(this.first, this.second, this.resultSelector, cancellationToken);
		}

		// Token: 0x040001A0 RID: 416
		private readonly IUniTaskAsyncEnumerable<TFirst> first;

		// Token: 0x040001A1 RID: 417
		private readonly IUniTaskAsyncEnumerable<TSecond> second;

		// Token: 0x040001A2 RID: 418
		private readonly Func<TFirst, TSecond, UniTask<TResult>> resultSelector;

		// Token: 0x02000200 RID: 512
		private sealed class _ZipAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600090B RID: 2315 RVA: 0x0004EB87 File Offset: 0x0004CD87
			public _ZipAwait(IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, UniTask<TResult>> resultSelector, CancellationToken cancellationToken)
			{
				this.first = first;
				this.second = second;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x0600090C RID: 2316 RVA: 0x0004EBAC File Offset: 0x0004CDAC
			// (set) Token: 0x0600090D RID: 2317 RVA: 0x0004EBB4 File Offset: 0x0004CDB4
			public TResult Current { get; private set; }

			// Token: 0x0600090E RID: 2318 RVA: 0x0004EBC0 File Offset: 0x0004CDC0
			public UniTask<bool> MoveNextAsync()
			{
				this.completionSource.Reset();
				if (this.firstEnumerator == null)
				{
					this.firstEnumerator = this.first.GetAsyncEnumerator(this.cancellationToken);
					this.secondEnumerator = this.second.GetAsyncEnumerator(this.cancellationToken);
				}
				this.firstAwaiter = this.firstEnumerator.MoveNextAsync().GetAwaiter();
				if (this.firstAwaiter.IsCompleted)
				{
					ZipAwait<TFirst, TSecond, TResult>._ZipAwait.FirstMoveNextCore(this);
				}
				else
				{
					this.firstAwaiter.SourceOnCompleted(ZipAwait<TFirst, TSecond, TResult>._ZipAwait.firstMoveNextCoreDelegate, this);
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x0600090F RID: 2319 RVA: 0x0004EC60 File Offset: 0x0004CE60
			private static void FirstMoveNextCore(object state)
			{
				ZipAwait<TFirst, TSecond, TResult>._ZipAwait zipAwait = (ZipAwait<TFirst, TSecond, TResult>._ZipAwait)state;
				bool flag;
				if (zipAwait.TryGetResult<bool>(zipAwait.firstAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							zipAwait.secondAwaiter = zipAwait.secondEnumerator.MoveNextAsync().GetAwaiter();
						}
						catch (Exception ex)
						{
							zipAwait.completionSource.TrySetException(ex);
							return;
						}
						if (zipAwait.secondAwaiter.IsCompleted)
						{
							ZipAwait<TFirst, TSecond, TResult>._ZipAwait.SecondMoveNextCore(zipAwait);
							return;
						}
						zipAwait.secondAwaiter.SourceOnCompleted(ZipAwait<TFirst, TSecond, TResult>._ZipAwait.secondMoveNextCoreDelegate, zipAwait);
						return;
					}
					else
					{
						zipAwait.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x06000910 RID: 2320 RVA: 0x0004ECF8 File Offset: 0x0004CEF8
			private static void SecondMoveNextCore(object state)
			{
				ZipAwait<TFirst, TSecond, TResult>._ZipAwait zipAwait = (ZipAwait<TFirst, TSecond, TResult>._ZipAwait)state;
				bool flag;
				if (zipAwait.TryGetResult<bool>(zipAwait.secondAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							zipAwait.resultAwaiter = zipAwait.resultSelector(zipAwait.firstEnumerator.Current, zipAwait.secondEnumerator.Current).GetAwaiter();
							if (zipAwait.resultAwaiter.IsCompleted)
							{
								ZipAwait<TFirst, TSecond, TResult>._ZipAwait.ResultAwaitCore(zipAwait);
							}
							else
							{
								zipAwait.resultAwaiter.SourceOnCompleted(ZipAwait<TFirst, TSecond, TResult>._ZipAwait.resultAwaitCoreDelegate, zipAwait);
							}
							return;
						}
						catch (Exception ex)
						{
							zipAwait.completionSource.TrySetException(ex);
							return;
						}
					}
					zipAwait.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x06000911 RID: 2321 RVA: 0x0004EDA4 File Offset: 0x0004CFA4
			private static void ResultAwaitCore(object state)
			{
				ZipAwait<TFirst, TSecond, TResult>._ZipAwait zipAwait = (ZipAwait<TFirst, TSecond, TResult>._ZipAwait)state;
				TResult value;
				if (zipAwait.TryGetResult<TResult>(zipAwait.resultAwaiter, ref value))
				{
					zipAwait.Current = value;
					if (zipAwait.cancellationToken.IsCancellationRequested)
					{
						zipAwait.completionSource.TrySetCanceled(zipAwait.cancellationToken);
						return;
					}
					zipAwait.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x06000912 RID: 2322 RVA: 0x0004EDFC File Offset: 0x0004CFFC
			public UniTask DisposeAsync()
			{
				ZipAwait<TFirst, TSecond, TResult>._ZipAwait.<DisposeAsync>d__21 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<ZipAwait<TFirst, TSecond, TResult>._ZipAwait.<DisposeAsync>d__21>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0400135D RID: 4957
			private static readonly Action<object> firstMoveNextCoreDelegate = new Action<object>(ZipAwait<TFirst, TSecond, TResult>._ZipAwait.FirstMoveNextCore);

			// Token: 0x0400135E RID: 4958
			private static readonly Action<object> secondMoveNextCoreDelegate = new Action<object>(ZipAwait<TFirst, TSecond, TResult>._ZipAwait.SecondMoveNextCore);

			// Token: 0x0400135F RID: 4959
			private static readonly Action<object> resultAwaitCoreDelegate = new Action<object>(ZipAwait<TFirst, TSecond, TResult>._ZipAwait.ResultAwaitCore);

			// Token: 0x04001360 RID: 4960
			private readonly IUniTaskAsyncEnumerable<TFirst> first;

			// Token: 0x04001361 RID: 4961
			private readonly IUniTaskAsyncEnumerable<TSecond> second;

			// Token: 0x04001362 RID: 4962
			private readonly Func<TFirst, TSecond, UniTask<TResult>> resultSelector;

			// Token: 0x04001363 RID: 4963
			private CancellationToken cancellationToken;

			// Token: 0x04001364 RID: 4964
			private IUniTaskAsyncEnumerator<TFirst> firstEnumerator;

			// Token: 0x04001365 RID: 4965
			private IUniTaskAsyncEnumerator<TSecond> secondEnumerator;

			// Token: 0x04001366 RID: 4966
			private UniTask<bool>.Awaiter firstAwaiter;

			// Token: 0x04001367 RID: 4967
			private UniTask<bool>.Awaiter secondAwaiter;

			// Token: 0x04001368 RID: 4968
			private UniTask<TResult>.Awaiter resultAwaiter;
		}
	}
}
