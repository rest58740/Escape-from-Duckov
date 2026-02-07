using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200002A RID: 42
	internal sealed class Do<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000273 RID: 627 RVA: 0x000091DC File Offset: 0x000073DC
		public Do(IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
		{
			this.source = source;
			this.onNext = onNext;
			this.onError = onError;
			this.onCompleted = onCompleted;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00009201 File Offset: 0x00007401
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Do<TSource>._Do(this.source, this.onNext, this.onError, this.onCompleted, cancellationToken);
		}

		// Token: 0x040000B9 RID: 185
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000BA RID: 186
		private readonly Action<TSource> onNext;

		// Token: 0x040000BB RID: 187
		private readonly Action<Exception> onError;

		// Token: 0x040000BC RID: 188
		private readonly Action onCompleted;

		// Token: 0x020000FE RID: 254
		private sealed class _Do : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005B5 RID: 1461 RVA: 0x000267D1 File Offset: 0x000249D1
			public _Do(IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted, CancellationToken cancellationToken)
			{
				this.source = source;
				this.onNext = onNext;
				this.onError = onError;
				this.onCompleted = onCompleted;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x060005B6 RID: 1462 RVA: 0x000267FE File Offset: 0x000249FE
			// (set) Token: 0x060005B7 RID: 1463 RVA: 0x00026806 File Offset: 0x00024A06
			public TSource Current { get; private set; }

			// Token: 0x060005B8 RID: 1464 RVA: 0x00026810 File Offset: 0x00024A10
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				bool flag = false;
				try
				{
					if (this.enumerator == null)
					{
						this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
					}
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					flag = this.awaiter.IsCompleted;
				}
				catch (Exception ex)
				{
					this.CallTrySetExceptionAfterNotification(ex);
					return new UniTask<bool>(this, this.completionSource.Version);
				}
				if (flag)
				{
					Do<TSource>._Do.MoveNextCore(this);
				}
				else
				{
					this.awaiter.SourceOnCompleted(Do<TSource>._Do.MoveNextCoreDelegate, this);
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x000268D8 File Offset: 0x00024AD8
			private void CallTrySetExceptionAfterNotification(Exception ex)
			{
				if (this.onError != null)
				{
					try
					{
						this.onError(ex);
					}
					catch (Exception ex2)
					{
						this.completionSource.TrySetException(ex2);
						return;
					}
				}
				this.completionSource.TrySetException(ex);
			}

			// Token: 0x060005BA RID: 1466 RVA: 0x00026928 File Offset: 0x00024B28
			private bool TryGetResultWithNotification<T>(UniTask<T>.Awaiter awaiter, out T result)
			{
				bool result2;
				try
				{
					result = awaiter.GetResult();
					result2 = true;
				}
				catch (Exception ex)
				{
					this.CallTrySetExceptionAfterNotification(ex);
					result = default(T);
					result2 = false;
				}
				return result2;
			}

			// Token: 0x060005BB RID: 1467 RVA: 0x0002696C File Offset: 0x00024B6C
			private static void MoveNextCore(object state)
			{
				Do<TSource>._Do @do = (Do<TSource>._Do)state;
				bool flag;
				if (@do.TryGetResultWithNotification<bool>(@do.awaiter, out flag))
				{
					if (flag)
					{
						TSource tsource = @do.enumerator.Current;
						if (@do.onNext != null)
						{
							try
							{
								@do.onNext(tsource);
							}
							catch (Exception ex)
							{
								@do.CallTrySetExceptionAfterNotification(ex);
							}
						}
						@do.Current = tsource;
						@do.completionSource.TrySetResult(true);
						return;
					}
					if (@do.onCompleted != null)
					{
						try
						{
							@do.onCompleted();
						}
						catch (Exception ex2)
						{
							@do.CallTrySetExceptionAfterNotification(ex2);
							return;
						}
					}
					@do.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x00026A20 File Offset: 0x00024C20
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x040008C2 RID: 2242
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(Do<TSource>._Do.MoveNextCore);

			// Token: 0x040008C3 RID: 2243
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040008C4 RID: 2244
			private readonly Action<TSource> onNext;

			// Token: 0x040008C5 RID: 2245
			private readonly Action<Exception> onError;

			// Token: 0x040008C6 RID: 2246
			private readonly Action onCompleted;

			// Token: 0x040008C7 RID: 2247
			private CancellationToken cancellationToken;

			// Token: 0x040008C8 RID: 2248
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040008C9 RID: 2249
			private UniTask<bool>.Awaiter awaiter;
		}
	}
}
