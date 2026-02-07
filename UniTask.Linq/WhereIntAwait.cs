using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000088 RID: 136
	internal sealed class WhereIntAwait<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x0000DFDB File Offset: 0x0000C1DB
		public WhereIntAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000DFF1 File Offset: 0x0000C1F1
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new WhereIntAwait<TSource>._WhereAwait(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000197 RID: 407
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000198 RID: 408
		private readonly Func<TSource, int, UniTask<bool>> predicate;

		// Token: 0x020001FC RID: 508
		private sealed class _WhereAwait : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008F1 RID: 2289 RVA: 0x0004E2C1 File Offset: 0x0004C4C1
			public _WhereAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate, CancellationToken cancellationToken)
			{
				this.source = source;
				this.predicate = predicate;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0004E2F7 File Offset: 0x0004C4F7
			// (set) Token: 0x060008F3 RID: 2291 RVA: 0x0004E2FF File Offset: 0x0004C4FF
			public TSource Current { get; private set; }

			// Token: 0x060008F4 RID: 2292 RVA: 0x0004E308 File Offset: 0x0004C508
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

			// Token: 0x060008F5 RID: 2293 RVA: 0x0004E34C File Offset: 0x0004C54C
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
							goto IL_F6;
						default:
							goto IL_12B;
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
						Func<TSource, int, UniTask<bool>> func = this.predicate;
						TSource arg = this.Current;
						int num = this.index;
						this.index = checked(num + 1);
						this.awaiter2 = func(arg, num).GetAwaiter();
						if (!this.awaiter2.IsCompleted)
						{
							this.state = 2;
							this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_F6:
						if (this.awaiter2.GetResult())
						{
							goto IL_141;
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
				IL_12B:
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_141:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060008F6 RID: 2294 RVA: 0x0004E4CC File Offset: 0x0004C6CC
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04001335 RID: 4917
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04001336 RID: 4918
			private readonly Func<TSource, int, UniTask<bool>> predicate;

			// Token: 0x04001337 RID: 4919
			private readonly CancellationToken cancellationToken;

			// Token: 0x04001338 RID: 4920
			private int state = -1;

			// Token: 0x04001339 RID: 4921
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x0400133A RID: 4922
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x0400133B RID: 4923
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x0400133C RID: 4924
			private Action moveNextAction;

			// Token: 0x0400133D RID: 4925
			private int index;
		}
	}
}
