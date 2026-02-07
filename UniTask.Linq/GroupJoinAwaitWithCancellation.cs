using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000038 RID: 56
	internal sealed class GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000295 RID: 661 RVA: 0x000098BD File Offset: 0x00007ABD
		public GroupJoinAwaitWithCancellation(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector, Func<TOuter, IEnumerable<TInner>, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.outer = outer;
			this.inner = inner;
			this.outerKeySelector = outerKeySelector;
			this.innerKeySelector = innerKeySelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000098F2 File Offset: 0x00007AF2
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation(this.outer, this.inner, this.outerKeySelector, this.innerKeySelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000E8 RID: 232
		private readonly IUniTaskAsyncEnumerable<TOuter> outer;

		// Token: 0x040000E9 RID: 233
		private readonly IUniTaskAsyncEnumerable<TInner> inner;

		// Token: 0x040000EA RID: 234
		private readonly Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector;

		// Token: 0x040000EB RID: 235
		private readonly Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector;

		// Token: 0x040000EC RID: 236
		private readonly Func<TOuter, IEnumerable<TInner>, CancellationToken, UniTask<TResult>> resultSelector;

		// Token: 0x040000ED RID: 237
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000114 RID: 276
		private sealed class _GroupJoinAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000620 RID: 1568 RVA: 0x00029497 File Offset: 0x00027697
			public _GroupJoinAwaitWithCancellation(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector, Func<TOuter, IEnumerable<TInner>, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.outer = outer;
				this.inner = inner;
				this.outerKeySelector = outerKeySelector;
				this.innerKeySelector = innerKeySelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x06000621 RID: 1569 RVA: 0x000294D4 File Offset: 0x000276D4
			// (set) Token: 0x06000622 RID: 1570 RVA: 0x000294DC File Offset: 0x000276DC
			public TResult Current { get; private set; }

			// Token: 0x06000623 RID: 1571 RVA: 0x000294E8 File Offset: 0x000276E8
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (this.lookup == null)
				{
					this.CreateLookup().Forget();
				}
				else
				{
					this.SourceMoveNext();
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000624 RID: 1572 RVA: 0x0002953C File Offset: 0x0002773C
			private UniTaskVoid CreateLookup()
			{
				GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.<CreateLookup>d__22 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.<CreateLookup>d__22>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x06000625 RID: 1573 RVA: 0x00029580 File Offset: 0x00027780
			private void SourceMoveNext()
			{
				try
				{
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (this.awaiter.IsCompleted)
					{
						GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.MoveNextCore(this);
					}
					else
					{
						this.awaiter.SourceOnCompleted(GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.MoveNextCoreDelegate, this);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x06000626 RID: 1574 RVA: 0x000295F0 File Offset: 0x000277F0
			private static void MoveNextCore(object state)
			{
				GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation groupJoinAwaitWithCancellation = (GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation)state;
				bool flag;
				if (groupJoinAwaitWithCancellation.TryGetResult<bool>(groupJoinAwaitWithCancellation.awaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							groupJoinAwaitWithCancellation.outerValue = groupJoinAwaitWithCancellation.enumerator.Current;
							groupJoinAwaitWithCancellation.outerKeyAwaiter = groupJoinAwaitWithCancellation.outerKeySelector(groupJoinAwaitWithCancellation.outerValue, groupJoinAwaitWithCancellation.cancellationToken).GetAwaiter();
							if (groupJoinAwaitWithCancellation.outerKeyAwaiter.IsCompleted)
							{
								GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.OuterKeySelectCore(groupJoinAwaitWithCancellation);
							}
							else
							{
								groupJoinAwaitWithCancellation.outerKeyAwaiter.SourceOnCompleted(GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.OuterKeySelectCoreDelegate, groupJoinAwaitWithCancellation);
							}
							return;
						}
						catch (Exception ex)
						{
							groupJoinAwaitWithCancellation.completionSource.TrySetException(ex);
							return;
						}
					}
					groupJoinAwaitWithCancellation.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x06000627 RID: 1575 RVA: 0x000296A4 File Offset: 0x000278A4
			private static void OuterKeySelectCore(object state)
			{
				GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation groupJoinAwaitWithCancellation = (GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation)state;
				TKey key;
				if (groupJoinAwaitWithCancellation.TryGetResult<TKey>(groupJoinAwaitWithCancellation.outerKeyAwaiter, ref key))
				{
					try
					{
						IEnumerable<TInner> arg = groupJoinAwaitWithCancellation.lookup[key];
						groupJoinAwaitWithCancellation.resultAwaiter = groupJoinAwaitWithCancellation.resultSelector(groupJoinAwaitWithCancellation.outerValue, arg, groupJoinAwaitWithCancellation.cancellationToken).GetAwaiter();
						if (groupJoinAwaitWithCancellation.resultAwaiter.IsCompleted)
						{
							GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.ResultSelectCore(groupJoinAwaitWithCancellation);
						}
						else
						{
							groupJoinAwaitWithCancellation.resultAwaiter.SourceOnCompleted(GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.ResultSelectCoreDelegate, groupJoinAwaitWithCancellation);
						}
					}
					catch (Exception ex)
					{
						groupJoinAwaitWithCancellation.completionSource.TrySetException(ex);
					}
				}
			}

			// Token: 0x06000628 RID: 1576 RVA: 0x00029748 File Offset: 0x00027948
			private static void ResultSelectCore(object state)
			{
				GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation groupJoinAwaitWithCancellation = (GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation)state;
				TResult value;
				if (groupJoinAwaitWithCancellation.TryGetResult<TResult>(groupJoinAwaitWithCancellation.resultAwaiter, ref value))
				{
					groupJoinAwaitWithCancellation.Current = value;
					groupJoinAwaitWithCancellation.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x06000629 RID: 1577 RVA: 0x00029780 File Offset: 0x00027980
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x0400099C RID: 2460
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.MoveNextCore);

			// Token: 0x0400099D RID: 2461
			private static readonly Action<object> ResultSelectCoreDelegate = new Action<object>(GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.ResultSelectCore);

			// Token: 0x0400099E RID: 2462
			private static readonly Action<object> OuterKeySelectCoreDelegate = new Action<object>(GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>._GroupJoinAwaitWithCancellation.OuterKeySelectCore);

			// Token: 0x0400099F RID: 2463
			private readonly IUniTaskAsyncEnumerable<TOuter> outer;

			// Token: 0x040009A0 RID: 2464
			private readonly IUniTaskAsyncEnumerable<TInner> inner;

			// Token: 0x040009A1 RID: 2465
			private readonly Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector;

			// Token: 0x040009A2 RID: 2466
			private readonly Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector;

			// Token: 0x040009A3 RID: 2467
			private readonly Func<TOuter, IEnumerable<TInner>, CancellationToken, UniTask<TResult>> resultSelector;

			// Token: 0x040009A4 RID: 2468
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x040009A5 RID: 2469
			private CancellationToken cancellationToken;

			// Token: 0x040009A6 RID: 2470
			private ILookup<TKey, TInner> lookup;

			// Token: 0x040009A7 RID: 2471
			private IUniTaskAsyncEnumerator<TOuter> enumerator;

			// Token: 0x040009A8 RID: 2472
			private TOuter outerValue;

			// Token: 0x040009A9 RID: 2473
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040009AA RID: 2474
			private UniTask<TKey>.Awaiter outerKeyAwaiter;

			// Token: 0x040009AB RID: 2475
			private UniTask<TResult>.Awaiter resultAwaiter;
		}
	}
}
