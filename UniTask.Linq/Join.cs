using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200003A RID: 58
	internal sealed class Join<TOuter, TInner, TKey, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000299 RID: 665 RVA: 0x00009955 File Offset: 0x00007B55
		public Join(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.outer = outer;
			this.inner = inner;
			this.outerKeySelector = outerKeySelector;
			this.innerKeySelector = innerKeySelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000998A File Offset: 0x00007B8A
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Join<TOuter, TInner, TKey, TResult>._Join(this.outer, this.inner, this.outerKeySelector, this.innerKeySelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000F1 RID: 241
		private readonly IUniTaskAsyncEnumerable<TOuter> outer;

		// Token: 0x040000F2 RID: 242
		private readonly IUniTaskAsyncEnumerable<TInner> inner;

		// Token: 0x040000F3 RID: 243
		private readonly Func<TOuter, TKey> outerKeySelector;

		// Token: 0x040000F4 RID: 244
		private readonly Func<TInner, TKey> innerKeySelector;

		// Token: 0x040000F5 RID: 245
		private readonly Func<TOuter, TInner, TResult> resultSelector;

		// Token: 0x040000F6 RID: 246
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000116 RID: 278
		private sealed class _Join : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000630 RID: 1584 RVA: 0x000298E9 File Offset: 0x00027AE9
			public _Join(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.outer = outer;
				this.inner = inner;
				this.outerKeySelector = outerKeySelector;
				this.innerKeySelector = innerKeySelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x06000631 RID: 1585 RVA: 0x00029926 File Offset: 0x00027B26
			// (set) Token: 0x06000632 RID: 1586 RVA: 0x0002992E File Offset: 0x00027B2E
			public TResult Current { get; private set; }

			// Token: 0x06000633 RID: 1587 RVA: 0x00029938 File Offset: 0x00027B38
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

			// Token: 0x06000634 RID: 1588 RVA: 0x0002998C File Offset: 0x00027B8C
			private UniTaskVoid CreateInnerHashSet()
			{
				Join<TOuter, TInner, TKey, TResult>._Join.<CreateInnerHashSet>d__20 <CreateInnerHashSet>d__;
				<CreateInnerHashSet>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateInnerHashSet>d__.<>4__this = this;
				<CreateInnerHashSet>d__.<>1__state = -1;
				<CreateInnerHashSet>d__.<>t__builder.Start<Join<TOuter, TInner, TKey, TResult>._Join.<CreateInnerHashSet>d__20>(ref <CreateInnerHashSet>d__);
				return <CreateInnerHashSet>d__.<>t__builder.Task;
			}

			// Token: 0x06000635 RID: 1589 RVA: 0x000299D0 File Offset: 0x00027BD0
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
							goto IL_92;
						}
						this.continueNext = true;
						Join<TOuter, TInner, TKey, TResult>._Join.MoveNextCore(this);
						if (!this.continueNext)
						{
							goto IL_A3;
						}
						this.continueNext = false;
					}
					this.Current = this.resultSelector(this.currentOuterValue, this.valueEnumerator.Current);
					goto IL_B6;
					IL_92:
					this.awaiter.SourceOnCompleted(Join<TOuter, TInner, TKey, TResult>._Join.MoveNextCoreDelegate, this);
					IL_A3:;
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
				return;
				IL_B6:
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000636 RID: 1590 RVA: 0x00029AB0 File Offset: 0x00027CB0
			private static void MoveNextCore(object state)
			{
				Join<TOuter, TInner, TKey, TResult>._Join join = (Join<TOuter, TInner, TKey, TResult>._Join)state;
				bool flag;
				if (!join.TryGetResult<bool>(join.awaiter, ref flag))
				{
					join.continueNext = false;
					return;
				}
				if (!flag)
				{
					join.continueNext = false;
					join.completionSource.TrySetResult(false);
					return;
				}
				join.currentOuterValue = join.enumerator.Current;
				TKey key = join.outerKeySelector(join.currentOuterValue);
				join.valueEnumerator = join.lookup[key].GetEnumerator();
				if (join.continueNext)
				{
					return;
				}
				join.SourceMoveNext();
			}

			// Token: 0x06000637 RID: 1591 RVA: 0x00029B40 File Offset: 0x00027D40
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

			// Token: 0x040009B2 RID: 2482
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(Join<TOuter, TInner, TKey, TResult>._Join.MoveNextCore);

			// Token: 0x040009B3 RID: 2483
			private readonly IUniTaskAsyncEnumerable<TOuter> outer;

			// Token: 0x040009B4 RID: 2484
			private readonly IUniTaskAsyncEnumerable<TInner> inner;

			// Token: 0x040009B5 RID: 2485
			private readonly Func<TOuter, TKey> outerKeySelector;

			// Token: 0x040009B6 RID: 2486
			private readonly Func<TInner, TKey> innerKeySelector;

			// Token: 0x040009B7 RID: 2487
			private readonly Func<TOuter, TInner, TResult> resultSelector;

			// Token: 0x040009B8 RID: 2488
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x040009B9 RID: 2489
			private CancellationToken cancellationToken;

			// Token: 0x040009BA RID: 2490
			private ILookup<TKey, TInner> lookup;

			// Token: 0x040009BB RID: 2491
			private IUniTaskAsyncEnumerator<TOuter> enumerator;

			// Token: 0x040009BC RID: 2492
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040009BD RID: 2493
			private TOuter currentOuterValue;

			// Token: 0x040009BE RID: 2494
			private IEnumerator<TInner> valueEnumerator;

			// Token: 0x040009BF RID: 2495
			private bool continueNext;
		}
	}
}
