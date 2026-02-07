using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000055 RID: 85
	internal sealed class SelectAwait<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000334 RID: 820 RVA: 0x0000C0A1 File Offset: 0x0000A2A1
		public SelectAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector)
		{
			this.source = source;
			this.selector = selector;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000C0B7 File Offset: 0x0000A2B7
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SelectAwait<TSource, TResult>._SelectAwait(this.source, this.selector, cancellationToken);
		}

		// Token: 0x04000135 RID: 309
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000136 RID: 310
		private readonly Func<TSource, UniTask<TResult>> selector;

		// Token: 0x0200018D RID: 397
		private sealed class _SelectAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600076B RID: 1899 RVA: 0x0003F215 File Offset: 0x0003D415
			public _SelectAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector = selector;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700003A RID: 58
			// (get) Token: 0x0600076C RID: 1900 RVA: 0x0003F24B File Offset: 0x0003D44B
			// (set) Token: 0x0600076D RID: 1901 RVA: 0x0003F253 File Offset: 0x0003D453
			public TResult Current { get; private set; }

			// Token: 0x0600076E RID: 1902 RVA: 0x0003F25C File Offset: 0x0003D45C
			public UniTask<bool> MoveNextAsync()
			{
				if (this.state == -2)
				{
					return default(UniTask<bool>);
				}
				this.completionSource.Reset();
				this.MoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x0600076F RID: 1903 RVA: 0x0003F2A0 File Offset: 0x0003D4A0
			private void MoveNext()
			{
				try
				{
					switch (this.state)
					{
					case -1:
						this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
						break;
					case 0:
						break;
					case 1:
						goto IL_7E;
					case 2:
						goto IL_D8;
					default:
						goto IL_105;
					}
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (!this.awaiter.IsCompleted)
					{
						this.state = 1;
						this.awaiter.UnsafeOnCompleted(this.moveNextAction);
						return;
					}
					IL_7E:
					if (!this.awaiter.GetResult())
					{
						goto IL_105;
					}
					this.awaiter2 = this.selector(this.enumerator.Current).GetAwaiter();
					if (!this.awaiter2.IsCompleted)
					{
						this.state = 2;
						this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
						return;
					}
					IL_D8:
					this.Current = this.awaiter2.GetResult();
					goto IL_11B;
				}
				catch (Exception ex)
				{
					this.state = -2;
					this.completionSource.TrySetException(ex);
					return;
				}
				IL_105:
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_11B:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000770 RID: 1904 RVA: 0x0003F3F0 File Offset: 0x0003D5F0
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04000ED2 RID: 3794
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000ED3 RID: 3795
			private readonly Func<TSource, UniTask<TResult>> selector;

			// Token: 0x04000ED4 RID: 3796
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000ED5 RID: 3797
			private int state = -1;

			// Token: 0x04000ED6 RID: 3798
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000ED7 RID: 3799
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000ED8 RID: 3800
			private UniTask<TResult>.Awaiter awaiter2;

			// Token: 0x04000ED9 RID: 3801
			private Action moveNextAction;
		}
	}
}
