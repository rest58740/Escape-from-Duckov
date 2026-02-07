using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000089 RID: 137
	internal sealed class WhereAwaitWithCancellation<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x0000E005 File Offset: 0x0000C205
		public WhereAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000E01B File Offset: 0x0000C21B
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new WhereAwaitWithCancellation<TSource>._WhereAwaitWithCancellation(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000199 RID: 409
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400019A RID: 410
		private readonly Func<TSource, CancellationToken, UniTask<bool>> predicate;

		// Token: 0x020001FD RID: 509
		private sealed class _WhereAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008F7 RID: 2295 RVA: 0x0004E4D9 File Offset: 0x0004C6D9
			public _WhereAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken)
			{
				this.source = source;
				this.predicate = predicate;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700005A RID: 90
			// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0004E50F File Offset: 0x0004C70F
			// (set) Token: 0x060008F9 RID: 2297 RVA: 0x0004E517 File Offset: 0x0004C717
			public TSource Current { get; private set; }

			// Token: 0x060008FA RID: 2298 RVA: 0x0004E520 File Offset: 0x0004C720
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

			// Token: 0x060008FB RID: 2299 RVA: 0x0004E564 File Offset: 0x0004C764
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
							goto IL_EB;
						default:
							goto IL_120;
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
						this.awaiter2 = this.predicate(this.Current, this.cancellationToken).GetAwaiter();
						if (!this.awaiter2.IsCompleted)
						{
							this.state = 2;
							this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_EB:
						if (this.awaiter2.GetResult())
						{
							goto IL_136;
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
				IL_120:
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_136:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060008FC RID: 2300 RVA: 0x0004E6D8 File Offset: 0x0004C8D8
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x0400133F RID: 4927
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04001340 RID: 4928
			private readonly Func<TSource, CancellationToken, UniTask<bool>> predicate;

			// Token: 0x04001341 RID: 4929
			private readonly CancellationToken cancellationToken;

			// Token: 0x04001342 RID: 4930
			private int state = -1;

			// Token: 0x04001343 RID: 4931
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04001344 RID: 4932
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04001345 RID: 4933
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x04001346 RID: 4934
			private Action moveNextAction;
		}
	}
}
