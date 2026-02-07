using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000008 RID: 8
	internal abstract class AsyncEnumeratorBase<TSource, TResult> : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x0000728E File Offset: 0x0000548E
		public AsyncEnumeratorBase(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			this.source = source;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x060001F1 RID: 497
		protected abstract bool TryMoveNextCore(bool sourceHasCurrent, out bool result);

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000072A4 File Offset: 0x000054A4
		protected TSource SourceCurrent
		{
			get
			{
				return this.enumerator.Current;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000072B1 File Offset: 0x000054B1
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x000072B9 File Offset: 0x000054B9
		public TResult Current { get; protected set; }

		// Token: 0x060001F5 RID: 501 RVA: 0x000072C4 File Offset: 0x000054C4
		public UniTask<bool> MoveNextAsync()
		{
			if (this.enumerator == null)
			{
				this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
			}
			this.completionSource.Reset();
			if (!this.OnFirstIteration())
			{
				this.SourceMoveNext();
			}
			return new UniTask<bool>(this, this.completionSource.Version);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000731A File Offset: 0x0000551A
		protected virtual bool OnFirstIteration()
		{
			return false;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007320 File Offset: 0x00005520
		protected void SourceMoveNext()
		{
			bool flag;
			for (;;)
			{
				this.sourceMoveNext = this.enumerator.MoveNextAsync().GetAwaiter();
				if (this.sourceMoveNext.IsCompleted)
				{
					flag = false;
					try
					{
						if (!this.TryMoveNextCore(this.sourceMoveNext.GetResult(), out flag))
						{
							continue;
						}
					}
					catch (Exception ex)
					{
						this.completionSource.TrySetException(ex);
						return;
					}
					break;
				}
				goto IL_7F;
			}
			if (this.cancellationToken.IsCancellationRequested)
			{
				this.completionSource.TrySetCanceled(this.cancellationToken);
				return;
			}
			this.completionSource.TrySetResult(flag);
			return;
			IL_7F:
			this.sourceMoveNext.SourceOnCompleted(AsyncEnumeratorBase<TSource, TResult>.moveNextCallbackDelegate, this);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000073D0 File Offset: 0x000055D0
		private static void MoveNextCallBack(object state)
		{
			AsyncEnumeratorBase<TSource, TResult> asyncEnumeratorBase = (AsyncEnumeratorBase<TSource, TResult>)state;
			bool flag;
			try
			{
				if (!asyncEnumeratorBase.TryMoveNextCore(asyncEnumeratorBase.sourceMoveNext.GetResult(), out flag))
				{
					asyncEnumeratorBase.SourceMoveNext();
					return;
				}
			}
			catch (Exception ex)
			{
				asyncEnumeratorBase.completionSource.TrySetException(ex);
				return;
			}
			if (asyncEnumeratorBase.cancellationToken.IsCancellationRequested)
			{
				asyncEnumeratorBase.completionSource.TrySetCanceled(asyncEnumeratorBase.cancellationToken);
				return;
			}
			asyncEnumeratorBase.completionSource.TrySetResult(flag);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007450 File Offset: 0x00005650
		public virtual UniTask DisposeAsync()
		{
			if (this.enumerator != null)
			{
				return this.enumerator.DisposeAsync();
			}
			return default(UniTask);
		}

		// Token: 0x04000004 RID: 4
		private static readonly Action<object> moveNextCallbackDelegate = new Action<object>(AsyncEnumeratorBase<TSource, TResult>.MoveNextCallBack);

		// Token: 0x04000005 RID: 5
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000006 RID: 6
		protected CancellationToken cancellationToken;

		// Token: 0x04000007 RID: 7
		private IUniTaskAsyncEnumerator<TSource> enumerator;

		// Token: 0x04000008 RID: 8
		private UniTask<bool>.Awaiter sourceMoveNext;
	}
}
