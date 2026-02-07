using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000029 RID: 41
	internal sealed class DistinctUntilChangedAwaitWithCancellation<TSource, TKey> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000271 RID: 625 RVA: 0x000091A5 File Offset: 0x000073A5
		public DistinctUntilChangedAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.comparer = comparer;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000091C2 File Offset: 0x000073C2
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new DistinctUntilChangedAwaitWithCancellation<TSource, TKey>._DistinctUntilChangedAwaitWithCancellation(this.source, this.keySelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000B6 RID: 182
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000B7 RID: 183
		private readonly Func<TSource, CancellationToken, UniTask<TKey>> keySelector;

		// Token: 0x040000B8 RID: 184
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x020000FD RID: 253
		private sealed class _DistinctUntilChangedAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005AF RID: 1455 RVA: 0x0002651D File Offset: 0x0002471D
			public _DistinctUntilChangedAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0002655B File Offset: 0x0002475B
			// (set) Token: 0x060005B1 RID: 1457 RVA: 0x00026563 File Offset: 0x00024763
			public TSource Current { get; private set; }

			// Token: 0x060005B2 RID: 1458 RVA: 0x0002656C File Offset: 0x0002476C
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

			// Token: 0x060005B3 RID: 1459 RVA: 0x000265B0 File Offset: 0x000247B0
			private void MoveNext()
			{
				for (;;)
				{
					try
					{
						switch (this.state)
						{
						case -3:
							break;
						case -2:
							goto IL_1A4;
						case -1:
							this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
							this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
							if (!this.awaiter.IsCompleted)
							{
								this.state = -3;
								this.awaiter.UnsafeOnCompleted(this.moveNextAction);
								return;
							}
							break;
						case 0:
							this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
							if (!this.awaiter.IsCompleted)
							{
								this.state = 1;
								this.awaiter.UnsafeOnCompleted(this.moveNextAction);
								return;
							}
							goto IL_F4;
						case 1:
							goto IL_F4;
						case 2:
							goto IL_163;
						default:
							goto IL_1A4;
						}
						if (this.awaiter.GetResult())
						{
							this.Current = this.enumerator.Current;
							goto IL_1D6;
						}
						break;
						IL_F4:
						if (!this.awaiter.GetResult())
						{
							break;
						}
						this.enumeratorCurrent = this.enumerator.Current;
						this.awaiter2 = this.keySelector(this.enumeratorCurrent, this.cancellationToken).GetAwaiter();
						if (!this.awaiter2.IsCompleted)
						{
							this.state = 2;
							this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_163:
						TKey result = this.awaiter2.GetResult();
						if (!this.comparer.Equals(this.prev, result))
						{
							this.prev = result;
							this.Current = this.enumeratorCurrent;
							goto IL_1D6;
						}
						this.state = 0;
						continue;
						IL_1A4:;
					}
					catch (Exception ex)
					{
						this.state = -2;
						this.completionSource.TrySetException(ex);
						return;
					}
					break;
				}
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_1D6:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060005B4 RID: 1460 RVA: 0x000267C4 File Offset: 0x000249C4
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x040008B6 RID: 2230
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040008B7 RID: 2231
			private readonly Func<TSource, CancellationToken, UniTask<TKey>> keySelector;

			// Token: 0x040008B8 RID: 2232
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x040008B9 RID: 2233
			private readonly CancellationToken cancellationToken;

			// Token: 0x040008BA RID: 2234
			private int state = -1;

			// Token: 0x040008BB RID: 2235
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040008BC RID: 2236
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040008BD RID: 2237
			private UniTask<TKey>.Awaiter awaiter2;

			// Token: 0x040008BE RID: 2238
			private Action moveNextAction;

			// Token: 0x040008BF RID: 2239
			private TSource enumeratorCurrent;

			// Token: 0x040008C0 RID: 2240
			private TKey prev;
		}
	}
}
