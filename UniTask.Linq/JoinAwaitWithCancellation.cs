using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200003C RID: 60
	internal sealed class JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600029D RID: 669 RVA: 0x00009A17 File Offset: 0x00007C17
		public JoinAwaitWithCancellation(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector, Func<TOuter, TInner, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.outer = outer;
			this.inner = inner;
			this.outerKeySelector = outerKeySelector;
			this.innerKeySelector = innerKeySelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00009A4C File Offset: 0x00007C4C
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation(this.outer, this.inner, this.outerKeySelector, this.innerKeySelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000FD RID: 253
		private readonly IUniTaskAsyncEnumerable<TOuter> outer;

		// Token: 0x040000FE RID: 254
		private readonly IUniTaskAsyncEnumerable<TInner> inner;

		// Token: 0x040000FF RID: 255
		private readonly Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector;

		// Token: 0x04000100 RID: 256
		private readonly Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector;

		// Token: 0x04000101 RID: 257
		private readonly Func<TOuter, TInner, CancellationToken, UniTask<TResult>> resultSelector;

		// Token: 0x04000102 RID: 258
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000118 RID: 280
		private sealed class _JoinAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000644 RID: 1604 RVA: 0x00029F16 File Offset: 0x00028116
			public _JoinAwaitWithCancellation(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector, Func<TOuter, TInner, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.outer = outer;
				this.inner = inner;
				this.outerKeySelector = outerKeySelector;
				this.innerKeySelector = innerKeySelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x06000645 RID: 1605 RVA: 0x00029F53 File Offset: 0x00028153
			// (set) Token: 0x06000646 RID: 1606 RVA: 0x00029F5B File Offset: 0x0002815B
			public TResult Current { get; private set; }

			// Token: 0x06000647 RID: 1607 RVA: 0x00029F64 File Offset: 0x00028164
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (this.lookup == null)
				{
					this.CreateInnerHashSet().Forget();
				}
				else
				{
					this.SourceMoveNext();
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000648 RID: 1608 RVA: 0x00029FB8 File Offset: 0x000281B8
			private UniTaskVoid CreateInnerHashSet()
			{
				JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.<CreateInnerHashSet>d__24 <CreateInnerHashSet>d__;
				<CreateInnerHashSet>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateInnerHashSet>d__.<>4__this = this;
				<CreateInnerHashSet>d__.<>1__state = -1;
				<CreateInnerHashSet>d__.<>t__builder.Start<JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.<CreateInnerHashSet>d__24>(ref <CreateInnerHashSet>d__);
				return <CreateInnerHashSet>d__.<>t__builder.Task;
			}

			// Token: 0x06000649 RID: 1609 RVA: 0x00029FFC File Offset: 0x000281FC
			private void SourceMoveNext()
			{
				try
				{
					for (;;)
					{
						if (this.valueEnumerator != null)
						{
							if (this.valueEnumerator.MoveNext())
							{
								break;
							}
							this.valueEnumerator.Dispose();
							this.valueEnumerator = null;
						}
						this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
						if (!this.awaiter.IsCompleted)
						{
							goto IL_C6;
						}
						this.continueNext = true;
						JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.MoveNextCore(this);
						if (!this.continueNext)
						{
							goto IL_D7;
						}
						this.continueNext = false;
					}
					this.resultAwaiter = this.resultSelector(this.currentOuterValue, this.valueEnumerator.Current, this.cancellationToken).GetAwaiter();
					if (this.resultAwaiter.IsCompleted)
					{
						JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.ResultSelectCore(this);
					}
					else
					{
						this.resultAwaiter.SourceOnCompleted(JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.ResultSelectCoreDelegate, this);
					}
					return;
					IL_C6:
					this.awaiter.SourceOnCompleted(JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.MoveNextCoreDelegate, this);
					IL_D7:;
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x0600064A RID: 1610 RVA: 0x0002A104 File Offset: 0x00028304
			private static void MoveNextCore(object state)
			{
				JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation joinAwaitWithCancellation = (JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation)state;
				bool flag;
				if (!joinAwaitWithCancellation.TryGetResult<bool>(joinAwaitWithCancellation.awaiter, ref flag))
				{
					joinAwaitWithCancellation.continueNext = false;
					return;
				}
				if (!flag)
				{
					joinAwaitWithCancellation.continueNext = false;
					joinAwaitWithCancellation.completionSource.TrySetResult(false);
					return;
				}
				joinAwaitWithCancellation.currentOuterValue = joinAwaitWithCancellation.enumerator.Current;
				joinAwaitWithCancellation.outerKeyAwaiter = joinAwaitWithCancellation.outerKeySelector(joinAwaitWithCancellation.currentOuterValue, joinAwaitWithCancellation.cancellationToken).GetAwaiter();
				if (joinAwaitWithCancellation.outerKeyAwaiter.IsCompleted)
				{
					JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.OuterSelectCore(joinAwaitWithCancellation);
					return;
				}
				joinAwaitWithCancellation.continueNext = false;
				joinAwaitWithCancellation.outerKeyAwaiter.SourceOnCompleted(JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.OuterSelectCoreDelegate, joinAwaitWithCancellation);
			}

			// Token: 0x0600064B RID: 1611 RVA: 0x0002A1AC File Offset: 0x000283AC
			private static void OuterSelectCore(object state)
			{
				JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation joinAwaitWithCancellation = (JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation)state;
				TKey key;
				if (!joinAwaitWithCancellation.TryGetResult<TKey>(joinAwaitWithCancellation.outerKeyAwaiter, ref key))
				{
					joinAwaitWithCancellation.continueNext = false;
					return;
				}
				joinAwaitWithCancellation.valueEnumerator = joinAwaitWithCancellation.lookup[key].GetEnumerator();
				if (joinAwaitWithCancellation.continueNext)
				{
					return;
				}
				joinAwaitWithCancellation.SourceMoveNext();
			}

			// Token: 0x0600064C RID: 1612 RVA: 0x0002A200 File Offset: 0x00028400
			private static void ResultSelectCore(object state)
			{
				JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation joinAwaitWithCancellation = (JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation)state;
				TResult value;
				if (joinAwaitWithCancellation.TryGetResult<TResult>(joinAwaitWithCancellation.resultAwaiter, ref value))
				{
					joinAwaitWithCancellation.Current = value;
					joinAwaitWithCancellation.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x0600064D RID: 1613 RVA: 0x0002A238 File Offset: 0x00028438
			public UniTask DisposeAsync()
			{
				if (this.valueEnumerator != null)
				{
					this.valueEnumerator.Dispose();
				}
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x040009D4 RID: 2516
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.MoveNextCore);

			// Token: 0x040009D5 RID: 2517
			private static readonly Action<object> OuterSelectCoreDelegate = new Action<object>(JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.OuterSelectCore);

			// Token: 0x040009D6 RID: 2518
			private static readonly Action<object> ResultSelectCoreDelegate = new Action<object>(JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._JoinAwaitWithCancellation.ResultSelectCore);

			// Token: 0x040009D7 RID: 2519
			private readonly IUniTaskAsyncEnumerable<TOuter> outer;

			// Token: 0x040009D8 RID: 2520
			private readonly IUniTaskAsyncEnumerable<TInner> inner;

			// Token: 0x040009D9 RID: 2521
			private readonly Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector;

			// Token: 0x040009DA RID: 2522
			private readonly Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector;

			// Token: 0x040009DB RID: 2523
			private readonly Func<TOuter, TInner, CancellationToken, UniTask<TResult>> resultSelector;

			// Token: 0x040009DC RID: 2524
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x040009DD RID: 2525
			private CancellationToken cancellationToken;

			// Token: 0x040009DE RID: 2526
			private ILookup<TKey, TInner> lookup;

			// Token: 0x040009DF RID: 2527
			private IUniTaskAsyncEnumerator<TOuter> enumerator;

			// Token: 0x040009E0 RID: 2528
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040009E1 RID: 2529
			private TOuter currentOuterValue;

			// Token: 0x040009E2 RID: 2530
			private IEnumerator<TInner> valueEnumerator;

			// Token: 0x040009E3 RID: 2531
			private UniTask<TResult>.Awaiter resultAwaiter;

			// Token: 0x040009E4 RID: 2532
			private UniTask<TKey>.Awaiter outerKeyAwaiter;

			// Token: 0x040009E5 RID: 2533
			private bool continueNext;
		}
	}
}
