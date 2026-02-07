using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200008A RID: 138
	internal sealed class WhereIntAwaitWithCancellation<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060003D4 RID: 980 RVA: 0x0000E02F File Offset: 0x0000C22F
		public WhereIntAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000E045 File Offset: 0x0000C245
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new WhereIntAwaitWithCancellation<TSource>._WhereAwaitWithCancellation(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x0400019B RID: 411
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400019C RID: 412
		private readonly Func<TSource, int, CancellationToken, UniTask<bool>> predicate;

		// Token: 0x020001FE RID: 510
		private sealed class _WhereAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008FD RID: 2301 RVA: 0x0004E6E5 File Offset: 0x0004C8E5
			public _WhereAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken)
			{
				this.source = source;
				this.predicate = predicate;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x060008FE RID: 2302 RVA: 0x0004E71B File Offset: 0x0004C91B
			// (set) Token: 0x060008FF RID: 2303 RVA: 0x0004E723 File Offset: 0x0004C923
			public TSource Current { get; private set; }

			// Token: 0x06000900 RID: 2304 RVA: 0x0004E72C File Offset: 0x0004C92C
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

			// Token: 0x06000901 RID: 2305 RVA: 0x0004E770 File Offset: 0x0004C970
			private void MoveNext()
			{
				for (;;)
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
							goto IL_7F;
						case 2:
							goto IL_FC;
						default:
							goto IL_131;
						}
						this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
						if (!this.awaiter.IsCompleted)
						{
							this.state = 1;
							this.awaiter.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_7F:
						if (!this.awaiter.GetResult())
						{
							break;
						}
						this.Current = this.enumerator.Current;
						Func<TSource, int, CancellationToken, UniTask<bool>> func = this.predicate;
						TSource arg = this.Current;
						int num = this.index;
						this.index = checked(num + 1);
						this.awaiter2 = func(arg, num, this.cancellationToken).GetAwaiter();
						if (!this.awaiter2.IsCompleted)
						{
							this.state = 2;
							this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_FC:
						if (this.awaiter2.GetResult())
						{
							goto IL_147;
						}
						this.state = 0;
						continue;
					}
					catch (Exception ex)
					{
						this.state = -2;
						this.completionSource.TrySetException(ex);
						return;
					}
					break;
				}
				IL_131:
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_147:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000902 RID: 2306 RVA: 0x0004E8F8 File Offset: 0x0004CAF8
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04001348 RID: 4936
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04001349 RID: 4937
			private readonly Func<TSource, int, CancellationToken, UniTask<bool>> predicate;

			// Token: 0x0400134A RID: 4938
			private readonly CancellationToken cancellationToken;

			// Token: 0x0400134B RID: 4939
			private int state = -1;

			// Token: 0x0400134C RID: 4940
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x0400134D RID: 4941
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x0400134E RID: 4942
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x0400134F RID: 4943
			private Action moveNextAction;

			// Token: 0x04001350 RID: 4944
			private int index;
		}
	}
}
