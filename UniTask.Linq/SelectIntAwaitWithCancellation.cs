using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000058 RID: 88
	internal sealed class SelectIntAwaitWithCancellation<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600033A RID: 826 RVA: 0x0000C11F File Offset: 0x0000A31F
		public SelectIntAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<TResult>> selector)
		{
			this.source = source;
			this.selector = selector;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000C135 File Offset: 0x0000A335
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SelectIntAwaitWithCancellation<TSource, TResult>._SelectAwaitWithCancellation(this.source, this.selector, cancellationToken);
		}

		// Token: 0x0400013B RID: 315
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400013C RID: 316
		private readonly Func<TSource, int, CancellationToken, UniTask<TResult>> selector;

		// Token: 0x02000190 RID: 400
		private sealed class _SelectAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600077D RID: 1917 RVA: 0x0003F7E1 File Offset: 0x0003D9E1
			public _SelectAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<TResult>> selector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector = selector;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x0600077E RID: 1918 RVA: 0x0003F817 File Offset: 0x0003DA17
			// (set) Token: 0x0600077F RID: 1919 RVA: 0x0003F81F File Offset: 0x0003DA1F
			public TResult Current { get; private set; }

			// Token: 0x06000780 RID: 1920 RVA: 0x0003F828 File Offset: 0x0003DA28
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

			// Token: 0x06000781 RID: 1921 RVA: 0x0003F86C File Offset: 0x0003DA6C
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
						goto IL_EF;
					default:
						goto IL_11E;
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
						goto IL_11E;
					}
					Func<TSource, int, CancellationToken, UniTask<TResult>> func = this.selector;
					TSource arg = this.enumerator.Current;
					int num = this.index;
					this.index = checked(num + 1);
					this.awaiter2 = func(arg, num, this.cancellationToken).GetAwaiter();
					if (!this.awaiter2.IsCompleted)
					{
						this.state = 2;
						this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
						return;
					}
					IL_EF:
					this.Current = this.awaiter2.GetResult();
					goto IL_134;
				}
				catch (Exception ex)
				{
					this.state = -2;
					this.completionSource.TrySetException(ex);
					return;
				}
				IL_11E:
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_134:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000782 RID: 1922 RVA: 0x0003F9E0 File Offset: 0x0003DBE0
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04000EEE RID: 3822
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000EEF RID: 3823
			private readonly Func<TSource, int, CancellationToken, UniTask<TResult>> selector;

			// Token: 0x04000EF0 RID: 3824
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000EF1 RID: 3825
			private int state = -1;

			// Token: 0x04000EF2 RID: 3826
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000EF3 RID: 3827
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000EF4 RID: 3828
			private UniTask<TResult>.Awaiter awaiter2;

			// Token: 0x04000EF5 RID: 3829
			private Action moveNextAction;

			// Token: 0x04000EF6 RID: 3830
			private int index;
		}
	}
}
