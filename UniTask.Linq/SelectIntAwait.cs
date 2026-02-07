using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000056 RID: 86
	internal sealed class SelectIntAwait<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000336 RID: 822 RVA: 0x0000C0CB File Offset: 0x0000A2CB
		public SelectIntAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<TResult>> selector)
		{
			this.source = source;
			this.selector = selector;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000C0E1 File Offset: 0x0000A2E1
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SelectIntAwait<TSource, TResult>._SelectAwait(this.source, this.selector, cancellationToken);
		}

		// Token: 0x04000137 RID: 311
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000138 RID: 312
		private readonly Func<TSource, int, UniTask<TResult>> selector;

		// Token: 0x0200018E RID: 398
		private sealed class _SelectAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000771 RID: 1905 RVA: 0x0003F3FD File Offset: 0x0003D5FD
			public _SelectAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<TResult>> selector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector = selector;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700003B RID: 59
			// (get) Token: 0x06000772 RID: 1906 RVA: 0x0003F433 File Offset: 0x0003D633
			// (set) Token: 0x06000773 RID: 1907 RVA: 0x0003F43B File Offset: 0x0003D63B
			public TResult Current { get; private set; }

			// Token: 0x06000774 RID: 1908 RVA: 0x0003F444 File Offset: 0x0003D644
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

			// Token: 0x06000775 RID: 1909 RVA: 0x0003F488 File Offset: 0x0003D688
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
						goto IL_E9;
					default:
						goto IL_118;
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
						goto IL_118;
					}
					Func<TSource, int, UniTask<TResult>> func = this.selector;
					TSource arg = this.enumerator.Current;
					int num = this.index;
					this.index = checked(num + 1);
					this.awaiter2 = func(arg, num).GetAwaiter();
					if (!this.awaiter2.IsCompleted)
					{
						this.state = 2;
						this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
						return;
					}
					IL_E9:
					this.Current = this.awaiter2.GetResult();
					goto IL_12E;
				}
				catch (Exception ex)
				{
					this.state = -2;
					this.completionSource.TrySetException(ex);
					return;
				}
				IL_118:
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_12E:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000776 RID: 1910 RVA: 0x0003F5E8 File Offset: 0x0003D7E8
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04000EDB RID: 3803
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000EDC RID: 3804
			private readonly Func<TSource, int, UniTask<TResult>> selector;

			// Token: 0x04000EDD RID: 3805
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000EDE RID: 3806
			private int state = -1;

			// Token: 0x04000EDF RID: 3807
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000EE0 RID: 3808
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000EE1 RID: 3809
			private UniTask<TResult>.Awaiter awaiter2;

			// Token: 0x04000EE2 RID: 3810
			private Action moveNextAction;

			// Token: 0x04000EE3 RID: 3811
			private int index;
		}
	}
}
