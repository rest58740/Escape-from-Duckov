using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000054 RID: 84
	internal sealed class SelectInt<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000332 RID: 818 RVA: 0x0000C077 File Offset: 0x0000A277
		public SelectInt(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			this.source = source;
			this.selector = selector;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000C08D File Offset: 0x0000A28D
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SelectInt<TSource, TResult>._Select(this.source, this.selector, cancellationToken);
		}

		// Token: 0x04000133 RID: 307
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000134 RID: 308
		private readonly Func<TSource, int, TResult> selector;

		// Token: 0x0200018C RID: 396
		private sealed class _Select : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000765 RID: 1893 RVA: 0x0003F065 File Offset: 0x0003D265
			public _Select(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector = selector;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x06000766 RID: 1894 RVA: 0x0003F09B File Offset: 0x0003D29B
			// (set) Token: 0x06000767 RID: 1895 RVA: 0x0003F0A3 File Offset: 0x0003D2A3
			public TResult Current { get; private set; }

			// Token: 0x06000768 RID: 1896 RVA: 0x0003F0AC File Offset: 0x0003D2AC
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

			// Token: 0x06000769 RID: 1897 RVA: 0x0003F0F0 File Offset: 0x0003D2F0
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
						goto IL_7A;
					default:
						goto IL_B6;
					}
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (!this.awaiter.IsCompleted)
					{
						this.state = 1;
						this.awaiter.UnsafeOnCompleted(this.moveNextAction);
						return;
					}
					IL_7A:
					if (this.awaiter.GetResult())
					{
						Func<TSource, int, TResult> func = this.selector;
						TSource arg = this.enumerator.Current;
						int num = this.index;
						this.index = checked(num + 1);
						this.Current = func(arg, num);
						goto IL_E6;
					}
					IL_B6:;
				}
				catch (Exception ex)
				{
					this.state = -2;
					this.completionSource.TrySetException(ex);
					return;
				}
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_E6:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x0600076A RID: 1898 RVA: 0x0003F208 File Offset: 0x0003D408
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04000EC9 RID: 3785
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000ECA RID: 3786
			private readonly Func<TSource, int, TResult> selector;

			// Token: 0x04000ECB RID: 3787
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000ECC RID: 3788
			private int state = -1;

			// Token: 0x04000ECD RID: 3789
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000ECE RID: 3790
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000ECF RID: 3791
			private Action moveNextAction;

			// Token: 0x04000ED0 RID: 3792
			private int index;
		}
	}
}
