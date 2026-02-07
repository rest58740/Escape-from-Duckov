using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000037 RID: 55
	internal sealed class GroupJoinAwait<TOuter, TInner, TKey, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000293 RID: 659 RVA: 0x0000985C File Offset: 0x00007A5C
		public GroupJoinAwait(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, UniTask<TKey>> outerKeySelector, Func<TInner, UniTask<TKey>> innerKeySelector, Func<TOuter, IEnumerable<TInner>, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.outer = outer;
			this.inner = inner;
			this.outerKeySelector = outerKeySelector;
			this.innerKeySelector = innerKeySelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00009891 File Offset: 0x00007A91
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait(this.outer, this.inner, this.outerKeySelector, this.innerKeySelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000E2 RID: 226
		private readonly IUniTaskAsyncEnumerable<TOuter> outer;

		// Token: 0x040000E3 RID: 227
		private readonly IUniTaskAsyncEnumerable<TInner> inner;

		// Token: 0x040000E4 RID: 228
		private readonly Func<TOuter, UniTask<TKey>> outerKeySelector;

		// Token: 0x040000E5 RID: 229
		private readonly Func<TInner, UniTask<TKey>> innerKeySelector;

		// Token: 0x040000E6 RID: 230
		private readonly Func<TOuter, IEnumerable<TInner>, UniTask<TResult>> resultSelector;

		// Token: 0x040000E7 RID: 231
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000113 RID: 275
		private sealed class _GroupJoinAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000615 RID: 1557 RVA: 0x00029161 File Offset: 0x00027361
			public _GroupJoinAwait(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, UniTask<TKey>> outerKeySelector, Func<TInner, UniTask<TKey>> innerKeySelector, Func<TOuter, IEnumerable<TInner>, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.outer = outer;
				this.inner = inner;
				this.outerKeySelector = outerKeySelector;
				this.innerKeySelector = innerKeySelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x06000616 RID: 1558 RVA: 0x0002919E File Offset: 0x0002739E
			// (set) Token: 0x06000617 RID: 1559 RVA: 0x000291A6 File Offset: 0x000273A6
			public TResult Current { get; private set; }

			// Token: 0x06000618 RID: 1560 RVA: 0x000291B0 File Offset: 0x000273B0
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

			// Token: 0x06000619 RID: 1561 RVA: 0x00029204 File Offset: 0x00027404
			private UniTaskVoid CreateLookup()
			{
				GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.<CreateLookup>d__22 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.<CreateLookup>d__22>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x0600061A RID: 1562 RVA: 0x00029248 File Offset: 0x00027448
			private void SourceMoveNext()
			{
				try
				{
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (this.awaiter.IsCompleted)
					{
						GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.MoveNextCore(this);
					}
					else
					{
						this.awaiter.SourceOnCompleted(GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.MoveNextCoreDelegate, this);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x0600061B RID: 1563 RVA: 0x000292B8 File Offset: 0x000274B8
			private static void MoveNextCore(object state)
			{
				GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait groupJoinAwait = (GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait)state;
				bool flag;
				if (groupJoinAwait.TryGetResult<bool>(groupJoinAwait.awaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							groupJoinAwait.outerValue = groupJoinAwait.enumerator.Current;
							groupJoinAwait.outerKeyAwaiter = groupJoinAwait.outerKeySelector(groupJoinAwait.outerValue).GetAwaiter();
							if (groupJoinAwait.outerKeyAwaiter.IsCompleted)
							{
								GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.OuterKeySelectCore(groupJoinAwait);
							}
							else
							{
								groupJoinAwait.outerKeyAwaiter.SourceOnCompleted(GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.OuterKeySelectCoreDelegate, groupJoinAwait);
							}
							return;
						}
						catch (Exception ex)
						{
							groupJoinAwait.completionSource.TrySetException(ex);
							return;
						}
					}
					groupJoinAwait.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x0600061C RID: 1564 RVA: 0x00029364 File Offset: 0x00027564
			private static void OuterKeySelectCore(object state)
			{
				GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait groupJoinAwait = (GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait)state;
				TKey key;
				if (groupJoinAwait.TryGetResult<TKey>(groupJoinAwait.outerKeyAwaiter, ref key))
				{
					try
					{
						IEnumerable<TInner> arg = groupJoinAwait.lookup[key];
						groupJoinAwait.resultAwaiter = groupJoinAwait.resultSelector(groupJoinAwait.outerValue, arg).GetAwaiter();
						if (groupJoinAwait.resultAwaiter.IsCompleted)
						{
							GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.ResultSelectCore(groupJoinAwait);
						}
						else
						{
							groupJoinAwait.resultAwaiter.SourceOnCompleted(GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.ResultSelectCoreDelegate, groupJoinAwait);
						}
					}
					catch (Exception ex)
					{
						groupJoinAwait.completionSource.TrySetException(ex);
					}
				}
			}

			// Token: 0x0600061D RID: 1565 RVA: 0x00029400 File Offset: 0x00027600
			private static void ResultSelectCore(object state)
			{
				GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait groupJoinAwait = (GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait)state;
				TResult value;
				if (groupJoinAwait.TryGetResult<TResult>(groupJoinAwait.resultAwaiter, ref value))
				{
					groupJoinAwait.Current = value;
					groupJoinAwait.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x0600061E RID: 1566 RVA: 0x00029438 File Offset: 0x00027638
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x0400098B RID: 2443
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.MoveNextCore);

			// Token: 0x0400098C RID: 2444
			private static readonly Action<object> ResultSelectCoreDelegate = new Action<object>(GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.ResultSelectCore);

			// Token: 0x0400098D RID: 2445
			private static readonly Action<object> OuterKeySelectCoreDelegate = new Action<object>(GroupJoinAwait<TOuter, TInner, TKey, TResult>._GroupJoinAwait.OuterKeySelectCore);

			// Token: 0x0400098E RID: 2446
			private readonly IUniTaskAsyncEnumerable<TOuter> outer;

			// Token: 0x0400098F RID: 2447
			private readonly IUniTaskAsyncEnumerable<TInner> inner;

			// Token: 0x04000990 RID: 2448
			private readonly Func<TOuter, UniTask<TKey>> outerKeySelector;

			// Token: 0x04000991 RID: 2449
			private readonly Func<TInner, UniTask<TKey>> innerKeySelector;

			// Token: 0x04000992 RID: 2450
			private readonly Func<TOuter, IEnumerable<TInner>, UniTask<TResult>> resultSelector;

			// Token: 0x04000993 RID: 2451
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x04000994 RID: 2452
			private CancellationToken cancellationToken;

			// Token: 0x04000995 RID: 2453
			private ILookup<TKey, TInner> lookup;

			// Token: 0x04000996 RID: 2454
			private IUniTaskAsyncEnumerator<TOuter> enumerator;

			// Token: 0x04000997 RID: 2455
			private TOuter outerValue;

			// Token: 0x04000998 RID: 2456
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000999 RID: 2457
			private UniTask<TKey>.Awaiter outerKeyAwaiter;

			// Token: 0x0400099A RID: 2458
			private UniTask<TResult>.Awaiter resultAwaiter;
		}
	}
}
