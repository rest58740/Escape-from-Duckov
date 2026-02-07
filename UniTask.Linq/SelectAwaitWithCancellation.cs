using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000057 RID: 87
	internal sealed class SelectAwaitWithCancellation<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000338 RID: 824 RVA: 0x0000C0F5 File Offset: 0x0000A2F5
		public SelectAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TResult>> selector)
		{
			this.source = source;
			this.selector = selector;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000C10B File Offset: 0x0000A30B
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SelectAwaitWithCancellation<TSource, TResult>._SelectAwaitWithCancellation(this.source, this.selector, cancellationToken);
		}

		// Token: 0x04000139 RID: 313
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400013A RID: 314
		private readonly Func<TSource, CancellationToken, UniTask<TResult>> selector;

		// Token: 0x0200018F RID: 399
		private sealed class _SelectAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000777 RID: 1911 RVA: 0x0003F5F5 File Offset: 0x0003D7F5
			public _SelectAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TResult>> selector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector = selector;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x06000778 RID: 1912 RVA: 0x0003F62B File Offset: 0x0003D82B
			// (set) Token: 0x06000779 RID: 1913 RVA: 0x0003F633 File Offset: 0x0003D833
			public TResult Current { get; private set; }

			// Token: 0x0600077A RID: 1914 RVA: 0x0003F63C File Offset: 0x0003D83C
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

			// Token: 0x0600077B RID: 1915 RVA: 0x0003F680 File Offset: 0x0003D880
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
						goto IL_DE;
					default:
						goto IL_10B;
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
						goto IL_10B;
					}
					this.awaiter2 = this.selector(this.enumerator.Current, this.cancellationToken).GetAwaiter();
					if (!this.awaiter2.IsCompleted)
					{
						this.state = 2;
						this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
						return;
					}
					IL_DE:
					this.Current = this.awaiter2.GetResult();
					goto IL_121;
				}
				catch (Exception ex)
				{
					this.state = -2;
					this.completionSource.TrySetException(ex);
					return;
				}
				IL_10B:
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_121:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x0600077C RID: 1916 RVA: 0x0003F7D4 File Offset: 0x0003D9D4
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04000EE5 RID: 3813
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000EE6 RID: 3814
			private readonly Func<TSource, CancellationToken, UniTask<TResult>> selector;

			// Token: 0x04000EE7 RID: 3815
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000EE8 RID: 3816
			private int state = -1;

			// Token: 0x04000EE9 RID: 3817
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000EEA RID: 3818
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000EEB RID: 3819
			private UniTask<TResult>.Awaiter awaiter2;

			// Token: 0x04000EEC RID: 3820
			private Action moveNextAction;
		}
	}
}
