using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200008B RID: 139
	internal sealed class Zip<TFirst, TSecond, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x0000E059 File Offset: 0x0000C259
		public Zip(IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
		{
			this.first = first;
			this.second = second;
			this.resultSelector = resultSelector;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000E076 File Offset: 0x0000C276
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Zip<TFirst, TSecond, TResult>._Zip(this.first, this.second, this.resultSelector, cancellationToken);
		}

		// Token: 0x0400019D RID: 413
		private readonly IUniTaskAsyncEnumerable<TFirst> first;

		// Token: 0x0400019E RID: 414
		private readonly IUniTaskAsyncEnumerable<TSecond> second;

		// Token: 0x0400019F RID: 415
		private readonly Func<TFirst, TSecond, TResult> resultSelector;

		// Token: 0x020001FF RID: 511
		private sealed class _Zip : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000903 RID: 2307 RVA: 0x0004E905 File Offset: 0x0004CB05
			public _Zip(IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.first = first;
				this.second = second;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000904 RID: 2308 RVA: 0x0004E92A File Offset: 0x0004CB2A
			// (set) Token: 0x06000905 RID: 2309 RVA: 0x0004E932 File Offset: 0x0004CB32
			public TResult Current { get; private set; }

			// Token: 0x06000906 RID: 2310 RVA: 0x0004E93C File Offset: 0x0004CB3C
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
					Zip<TFirst, TSecond, TResult>._Zip.FirstMoveNextCore(this);
				}
				else
				{
					this.firstAwaiter.SourceOnCompleted(Zip<TFirst, TSecond, TResult>._Zip.firstMoveNextCoreDelegate, this);
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000907 RID: 2311 RVA: 0x0004E9DC File Offset: 0x0004CBDC
			private static void FirstMoveNextCore(object state)
			{
				Zip<TFirst, TSecond, TResult>._Zip zip = (Zip<TFirst, TSecond, TResult>._Zip)state;
				bool flag;
				if (zip.TryGetResult<bool>(zip.firstAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							zip.secondAwaiter = zip.secondEnumerator.MoveNextAsync().GetAwaiter();
						}
						catch (Exception ex)
						{
							zip.completionSource.TrySetException(ex);
							return;
						}
						if (zip.secondAwaiter.IsCompleted)
						{
							Zip<TFirst, TSecond, TResult>._Zip.SecondMoveNextCore(zip);
							return;
						}
						zip.secondAwaiter.SourceOnCompleted(Zip<TFirst, TSecond, TResult>._Zip.secondMoveNextCoreDelegate, zip);
						return;
					}
					else
					{
						zip.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x06000908 RID: 2312 RVA: 0x0004EA74 File Offset: 0x0004CC74
			private static void SecondMoveNextCore(object state)
			{
				Zip<TFirst, TSecond, TResult>._Zip zip = (Zip<TFirst, TSecond, TResult>._Zip)state;
				bool flag;
				if (zip.TryGetResult<bool>(zip.secondAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							zip.Current = zip.resultSelector(zip.firstEnumerator.Current, zip.secondEnumerator.Current);
						}
						catch (Exception ex)
						{
							zip.completionSource.TrySetException(ex);
						}
						if (zip.cancellationToken.IsCancellationRequested)
						{
							zip.completionSource.TrySetCanceled(zip.cancellationToken);
							return;
						}
						zip.completionSource.TrySetResult(true);
						return;
					}
					else
					{
						zip.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x06000909 RID: 2313 RVA: 0x0004EB20 File Offset: 0x0004CD20
			public UniTask DisposeAsync()
			{
				Zip<TFirst, TSecond, TResult>._Zip.<DisposeAsync>d__18 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<Zip<TFirst, TSecond, TResult>._Zip.<DisposeAsync>d__18>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x04001352 RID: 4946
			private static readonly Action<object> firstMoveNextCoreDelegate = new Action<object>(Zip<TFirst, TSecond, TResult>._Zip.FirstMoveNextCore);

			// Token: 0x04001353 RID: 4947
			private static readonly Action<object> secondMoveNextCoreDelegate = new Action<object>(Zip<TFirst, TSecond, TResult>._Zip.SecondMoveNextCore);

			// Token: 0x04001354 RID: 4948
			private readonly IUniTaskAsyncEnumerable<TFirst> first;

			// Token: 0x04001355 RID: 4949
			private readonly IUniTaskAsyncEnumerable<TSecond> second;

			// Token: 0x04001356 RID: 4950
			private readonly Func<TFirst, TSecond, TResult> resultSelector;

			// Token: 0x04001357 RID: 4951
			private CancellationToken cancellationToken;

			// Token: 0x04001358 RID: 4952
			private IUniTaskAsyncEnumerator<TFirst> firstEnumerator;

			// Token: 0x04001359 RID: 4953
			private IUniTaskAsyncEnumerator<TSecond> secondEnumerator;

			// Token: 0x0400135A RID: 4954
			private UniTask<bool>.Awaiter firstAwaiter;

			// Token: 0x0400135B RID: 4955
			private UniTask<bool>.Awaiter secondAwaiter;
		}
	}
}
