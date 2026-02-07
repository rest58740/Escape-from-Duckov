using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200008D RID: 141
	internal sealed class ZipAwaitWithCancellation<TFirst, TSecond, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x060003DA RID: 986 RVA: 0x0000E0C7 File Offset: 0x0000C2C7
		public ZipAwaitWithCancellation(IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, UniTask<TResult>> resultSelector)
		{
			this.first = first;
			this.second = second;
			this.resultSelector = resultSelector;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation(this.first, this.second, this.resultSelector, cancellationToken);
		}

		// Token: 0x040001A3 RID: 419
		private readonly IUniTaskAsyncEnumerable<TFirst> first;

		// Token: 0x040001A4 RID: 420
		private readonly IUniTaskAsyncEnumerable<TSecond> second;

		// Token: 0x040001A5 RID: 421
		private readonly Func<TFirst, TSecond, CancellationToken, UniTask<TResult>> resultSelector;

		// Token: 0x02000201 RID: 513
		private sealed class _ZipAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000914 RID: 2324 RVA: 0x0004EE74 File Offset: 0x0004D074
			public _ZipAwaitWithCancellation(IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, UniTask<TResult>> resultSelector, CancellationToken cancellationToken)
			{
				this.first = first;
				this.second = second;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x06000915 RID: 2325 RVA: 0x0004EE99 File Offset: 0x0004D099
			// (set) Token: 0x06000916 RID: 2326 RVA: 0x0004EEA1 File Offset: 0x0004D0A1
			public TResult Current { get; private set; }

			// Token: 0x06000917 RID: 2327 RVA: 0x0004EEAC File Offset: 0x0004D0AC
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
					ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.FirstMoveNextCore(this);
				}
				else
				{
					this.firstAwaiter.SourceOnCompleted(ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.firstMoveNextCoreDelegate, this);
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000918 RID: 2328 RVA: 0x0004EF4C File Offset: 0x0004D14C
			private static void FirstMoveNextCore(object state)
			{
				ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation zipAwaitWithCancellation = (ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation)state;
				bool flag;
				if (zipAwaitWithCancellation.TryGetResult<bool>(zipAwaitWithCancellation.firstAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							zipAwaitWithCancellation.secondAwaiter = zipAwaitWithCancellation.secondEnumerator.MoveNextAsync().GetAwaiter();
						}
						catch (Exception ex)
						{
							zipAwaitWithCancellation.completionSource.TrySetException(ex);
							return;
						}
						if (zipAwaitWithCancellation.secondAwaiter.IsCompleted)
						{
							ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.SecondMoveNextCore(zipAwaitWithCancellation);
							return;
						}
						zipAwaitWithCancellation.secondAwaiter.SourceOnCompleted(ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.secondMoveNextCoreDelegate, zipAwaitWithCancellation);
						return;
					}
					else
					{
						zipAwaitWithCancellation.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x06000919 RID: 2329 RVA: 0x0004EFE4 File Offset: 0x0004D1E4
			private static void SecondMoveNextCore(object state)
			{
				ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation zipAwaitWithCancellation = (ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation)state;
				bool flag;
				if (zipAwaitWithCancellation.TryGetResult<bool>(zipAwaitWithCancellation.secondAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							zipAwaitWithCancellation.resultAwaiter = zipAwaitWithCancellation.resultSelector(zipAwaitWithCancellation.firstEnumerator.Current, zipAwaitWithCancellation.secondEnumerator.Current, zipAwaitWithCancellation.cancellationToken).GetAwaiter();
							if (zipAwaitWithCancellation.resultAwaiter.IsCompleted)
							{
								ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.ResultAwaitCore(zipAwaitWithCancellation);
							}
							else
							{
								zipAwaitWithCancellation.resultAwaiter.SourceOnCompleted(ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.resultAwaitCoreDelegate, zipAwaitWithCancellation);
							}
							return;
						}
						catch (Exception ex)
						{
							zipAwaitWithCancellation.completionSource.TrySetException(ex);
							return;
						}
					}
					zipAwaitWithCancellation.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x0600091A RID: 2330 RVA: 0x0004F098 File Offset: 0x0004D298
			private static void ResultAwaitCore(object state)
			{
				ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation zipAwaitWithCancellation = (ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation)state;
				TResult value;
				if (zipAwaitWithCancellation.TryGetResult<TResult>(zipAwaitWithCancellation.resultAwaiter, ref value))
				{
					zipAwaitWithCancellation.Current = value;
					if (zipAwaitWithCancellation.cancellationToken.IsCancellationRequested)
					{
						zipAwaitWithCancellation.completionSource.TrySetCanceled(zipAwaitWithCancellation.cancellationToken);
						return;
					}
					zipAwaitWithCancellation.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x0600091B RID: 2331 RVA: 0x0004F0F0 File Offset: 0x0004D2F0
			public UniTask DisposeAsync()
			{
				ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.<DisposeAsync>d__21 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.<DisposeAsync>d__21>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0400136A RID: 4970
			private static readonly Action<object> firstMoveNextCoreDelegate = new Action<object>(ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.FirstMoveNextCore);

			// Token: 0x0400136B RID: 4971
			private static readonly Action<object> secondMoveNextCoreDelegate = new Action<object>(ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.SecondMoveNextCore);

			// Token: 0x0400136C RID: 4972
			private static readonly Action<object> resultAwaitCoreDelegate = new Action<object>(ZipAwaitWithCancellation<TFirst, TSecond, TResult>._ZipAwaitWithCancellation.ResultAwaitCore);

			// Token: 0x0400136D RID: 4973
			private readonly IUniTaskAsyncEnumerable<TFirst> first;

			// Token: 0x0400136E RID: 4974
			private readonly IUniTaskAsyncEnumerable<TSecond> second;

			// Token: 0x0400136F RID: 4975
			private readonly Func<TFirst, TSecond, CancellationToken, UniTask<TResult>> resultSelector;

			// Token: 0x04001370 RID: 4976
			private CancellationToken cancellationToken;

			// Token: 0x04001371 RID: 4977
			private IUniTaskAsyncEnumerator<TFirst> firstEnumerator;

			// Token: 0x04001372 RID: 4978
			private IUniTaskAsyncEnumerator<TSecond> secondEnumerator;

			// Token: 0x04001373 RID: 4979
			private UniTask<bool>.Awaiter firstAwaiter;

			// Token: 0x04001374 RID: 4980
			private UniTask<bool>.Awaiter secondAwaiter;

			// Token: 0x04001375 RID: 4981
			private UniTask<TResult>.Awaiter resultAwaiter;
		}
	}
}
