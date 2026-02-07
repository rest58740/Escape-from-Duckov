using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000028 RID: 40
	internal sealed class DistinctUntilChangedAwait<TSource, TKey> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600026F RID: 623 RVA: 0x0000916E File Offset: 0x0000736E
		public DistinctUntilChangedAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.comparer = comparer;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000918B File Offset: 0x0000738B
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new DistinctUntilChangedAwait<TSource, TKey>._DistinctUntilChangedAwait(this.source, this.keySelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000B3 RID: 179
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000B4 RID: 180
		private readonly Func<TSource, UniTask<TKey>> keySelector;

		// Token: 0x040000B5 RID: 181
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x020000FC RID: 252
		private sealed class _DistinctUntilChangedAwait : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005A9 RID: 1449 RVA: 0x0002626D File Offset: 0x0002446D
			public _DistinctUntilChangedAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x060005AA RID: 1450 RVA: 0x000262AB File Offset: 0x000244AB
			// (set) Token: 0x060005AB RID: 1451 RVA: 0x000262B3 File Offset: 0x000244B3
			public TSource Current { get; private set; }

			// Token: 0x060005AC RID: 1452 RVA: 0x000262BC File Offset: 0x000244BC
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

			// Token: 0x060005AD RID: 1453 RVA: 0x00026300 File Offset: 0x00024500
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
							goto IL_19E;
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
							goto IL_15D;
						default:
							goto IL_19E;
						}
						if (this.awaiter.GetResult())
						{
							this.Current = this.enumerator.Current;
							goto IL_1D0;
						}
						break;
						IL_F4:
						if (!this.awaiter.GetResult())
						{
							break;
						}
						this.enumeratorCurrent = this.enumerator.Current;
						this.awaiter2 = this.keySelector(this.enumeratorCurrent).GetAwaiter();
						if (!this.awaiter2.IsCompleted)
						{
							this.state = 2;
							this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_15D:
						TKey result = this.awaiter2.GetResult();
						if (!this.comparer.Equals(this.prev, result))
						{
							this.prev = result;
							this.Current = this.enumeratorCurrent;
							goto IL_1D0;
						}
						this.state = 0;
						continue;
						IL_19E:;
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
				IL_1D0:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060005AE RID: 1454 RVA: 0x00026510 File Offset: 0x00024710
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x040008AA RID: 2218
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040008AB RID: 2219
			private readonly Func<TSource, UniTask<TKey>> keySelector;

			// Token: 0x040008AC RID: 2220
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x040008AD RID: 2221
			private readonly CancellationToken cancellationToken;

			// Token: 0x040008AE RID: 2222
			private int state = -1;

			// Token: 0x040008AF RID: 2223
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040008B0 RID: 2224
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040008B1 RID: 2225
			private UniTask<TKey>.Awaiter awaiter2;

			// Token: 0x040008B2 RID: 2226
			private Action moveNextAction;

			// Token: 0x040008B3 RID: 2227
			private TSource enumeratorCurrent;

			// Token: 0x040008B4 RID: 2228
			private TKey prev;
		}
	}
}
