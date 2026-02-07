using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200003B RID: 59
	internal sealed class JoinAwait<TOuter, TInner, TKey, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600029B RID: 667 RVA: 0x000099B6 File Offset: 0x00007BB6
		public JoinAwait(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, UniTask<TKey>> outerKeySelector, Func<TInner, UniTask<TKey>> innerKeySelector, Func<TOuter, TInner, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.outer = outer;
			this.inner = inner;
			this.outerKeySelector = outerKeySelector;
			this.innerKeySelector = innerKeySelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000099EB File Offset: 0x00007BEB
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait(this.outer, this.inner, this.outerKeySelector, this.innerKeySelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000F7 RID: 247
		private readonly IUniTaskAsyncEnumerable<TOuter> outer;

		// Token: 0x040000F8 RID: 248
		private readonly IUniTaskAsyncEnumerable<TInner> inner;

		// Token: 0x040000F9 RID: 249
		private readonly Func<TOuter, UniTask<TKey>> outerKeySelector;

		// Token: 0x040000FA RID: 250
		private readonly Func<TInner, UniTask<TKey>> innerKeySelector;

		// Token: 0x040000FB RID: 251
		private readonly Func<TOuter, TInner, UniTask<TResult>> resultSelector;

		// Token: 0x040000FC RID: 252
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000117 RID: 279
		private sealed class _JoinAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000639 RID: 1593 RVA: 0x00029B90 File Offset: 0x00027D90
			public _JoinAwait(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, UniTask<TKey>> outerKeySelector, Func<TInner, UniTask<TKey>> innerKeySelector, Func<TOuter, TInner, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.outer = outer;
				this.inner = inner;
				this.outerKeySelector = outerKeySelector;
				this.innerKeySelector = innerKeySelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x0600063A RID: 1594 RVA: 0x00029BCD File Offset: 0x00027DCD
			// (set) Token: 0x0600063B RID: 1595 RVA: 0x00029BD5 File Offset: 0x00027DD5
			public TResult Current { get; private set; }

			// Token: 0x0600063C RID: 1596 RVA: 0x00029BE0 File Offset: 0x00027DE0
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

			// Token: 0x0600063D RID: 1597 RVA: 0x00029C34 File Offset: 0x00027E34
			private UniTaskVoid CreateInnerHashSet()
			{
				JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.<CreateInnerHashSet>d__24 <CreateInnerHashSet>d__;
				<CreateInnerHashSet>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateInnerHashSet>d__.<>4__this = this;
				<CreateInnerHashSet>d__.<>1__state = -1;
				<CreateInnerHashSet>d__.<>t__builder.Start<JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.<CreateInnerHashSet>d__24>(ref <CreateInnerHashSet>d__);
				return <CreateInnerHashSet>d__.<>t__builder.Task;
			}

			// Token: 0x0600063E RID: 1598 RVA: 0x00029C78 File Offset: 0x00027E78
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
							goto IL_C0;
						}
						this.continueNext = true;
						JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.MoveNextCore(this);
						if (!this.continueNext)
						{
							goto IL_D1;
						}
						this.continueNext = false;
					}
					this.resultAwaiter = this.resultSelector(this.currentOuterValue, this.valueEnumerator.Current).GetAwaiter();
					if (this.resultAwaiter.IsCompleted)
					{
						JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.ResultSelectCore(this);
					}
					else
					{
						this.resultAwaiter.SourceOnCompleted(JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.ResultSelectCoreDelegate, this);
					}
					return;
					IL_C0:
					this.awaiter.SourceOnCompleted(JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.MoveNextCoreDelegate, this);
					IL_D1:;
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x00029D78 File Offset: 0x00027F78
			private static void MoveNextCore(object state)
			{
				JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait joinAwait = (JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait)state;
				bool flag;
				if (!joinAwait.TryGetResult<bool>(joinAwait.awaiter, ref flag))
				{
					joinAwait.continueNext = false;
					return;
				}
				if (!flag)
				{
					joinAwait.continueNext = false;
					joinAwait.completionSource.TrySetResult(false);
					return;
				}
				joinAwait.currentOuterValue = joinAwait.enumerator.Current;
				joinAwait.outerKeyAwaiter = joinAwait.outerKeySelector(joinAwait.currentOuterValue).GetAwaiter();
				if (joinAwait.outerKeyAwaiter.IsCompleted)
				{
					JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.OuterSelectCore(joinAwait);
					return;
				}
				joinAwait.continueNext = false;
				joinAwait.outerKeyAwaiter.SourceOnCompleted(JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.OuterSelectCoreDelegate, joinAwait);
			}

			// Token: 0x06000640 RID: 1600 RVA: 0x00029E18 File Offset: 0x00028018
			private static void OuterSelectCore(object state)
			{
				JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait joinAwait = (JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait)state;
				TKey key;
				if (!joinAwait.TryGetResult<TKey>(joinAwait.outerKeyAwaiter, ref key))
				{
					joinAwait.continueNext = false;
					return;
				}
				joinAwait.valueEnumerator = joinAwait.lookup[key].GetEnumerator();
				if (joinAwait.continueNext)
				{
					return;
				}
				joinAwait.SourceMoveNext();
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x00029E6C File Offset: 0x0002806C
			private static void ResultSelectCore(object state)
			{
				JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait joinAwait = (JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait)state;
				TResult value;
				if (joinAwait.TryGetResult<TResult>(joinAwait.resultAwaiter, ref value))
				{
					joinAwait.Current = value;
					joinAwait.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x06000642 RID: 1602 RVA: 0x00029EA4 File Offset: 0x000280A4
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

			// Token: 0x040009C1 RID: 2497
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.MoveNextCore);

			// Token: 0x040009C2 RID: 2498
			private static readonly Action<object> OuterSelectCoreDelegate = new Action<object>(JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.OuterSelectCore);

			// Token: 0x040009C3 RID: 2499
			private static readonly Action<object> ResultSelectCoreDelegate = new Action<object>(JoinAwait<TOuter, TInner, TKey, TResult>._JoinAwait.ResultSelectCore);

			// Token: 0x040009C4 RID: 2500
			private readonly IUniTaskAsyncEnumerable<TOuter> outer;

			// Token: 0x040009C5 RID: 2501
			private readonly IUniTaskAsyncEnumerable<TInner> inner;

			// Token: 0x040009C6 RID: 2502
			private readonly Func<TOuter, UniTask<TKey>> outerKeySelector;

			// Token: 0x040009C7 RID: 2503
			private readonly Func<TInner, UniTask<TKey>> innerKeySelector;

			// Token: 0x040009C8 RID: 2504
			private readonly Func<TOuter, TInner, UniTask<TResult>> resultSelector;

			// Token: 0x040009C9 RID: 2505
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x040009CA RID: 2506
			private CancellationToken cancellationToken;

			// Token: 0x040009CB RID: 2507
			private ILookup<TKey, TInner> lookup;

			// Token: 0x040009CC RID: 2508
			private IUniTaskAsyncEnumerator<TOuter> enumerator;

			// Token: 0x040009CD RID: 2509
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040009CE RID: 2510
			private TOuter currentOuterValue;

			// Token: 0x040009CF RID: 2511
			private IEnumerator<TInner> valueEnumerator;

			// Token: 0x040009D0 RID: 2512
			private UniTask<TResult>.Awaiter resultAwaiter;

			// Token: 0x040009D1 RID: 2513
			private UniTask<TKey>.Awaiter outerKeyAwaiter;

			// Token: 0x040009D2 RID: 2514
			private bool continueNext;
		}
	}
}
