using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000035 RID: 53
	internal sealed class GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600028F RID: 655 RVA: 0x000097A8 File Offset: 0x000079A8
		public GroupByAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, Func<TKey, IEnumerable<TElement>, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.elementSelector = elementSelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000097D5 File Offset: 0x000079D5
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>._GroupByAwaitWithCancellation(this.source, this.keySelector, this.elementSelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000D7 RID: 215
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000D8 RID: 216
		private readonly Func<TSource, CancellationToken, UniTask<TKey>> keySelector;

		// Token: 0x040000D9 RID: 217
		private readonly Func<TSource, CancellationToken, UniTask<TElement>> elementSelector;

		// Token: 0x040000DA RID: 218
		private readonly Func<TKey, IEnumerable<TElement>, CancellationToken, UniTask<TResult>> resultSelector;

		// Token: 0x040000DB RID: 219
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000111 RID: 273
		private sealed class _GroupByAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000603 RID: 1539 RVA: 0x00028D55 File Offset: 0x00026F55
			public _GroupByAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, Func<TKey, IEnumerable<TElement>, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.elementSelector = elementSelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x06000604 RID: 1540 RVA: 0x00028D8A File Offset: 0x00026F8A
			// (set) Token: 0x06000605 RID: 1541 RVA: 0x00028D92 File Offset: 0x00026F92
			public TResult Current { get; private set; }

			// Token: 0x06000606 RID: 1542 RVA: 0x00028D9C File Offset: 0x00026F9C
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (this.groupEnumerator == null)
				{
					this.CreateLookup().Forget();
				}
				else
				{
					this.SourceMoveNext();
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x00028DF0 File Offset: 0x00026FF0
			private UniTaskVoid CreateLookup()
			{
				GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>._GroupByAwaitWithCancellation.<CreateLookup>d__15 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>._GroupByAwaitWithCancellation.<CreateLookup>d__15>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x06000608 RID: 1544 RVA: 0x00028E34 File Offset: 0x00027034
			private void SourceMoveNext()
			{
				try
				{
					if (this.groupEnumerator.MoveNext())
					{
						IGrouping<TKey, TElement> grouping = this.groupEnumerator.Current;
						this.awaiter = this.resultSelector(grouping.Key, grouping, this.cancellationToken).GetAwaiter();
						if (this.awaiter.IsCompleted)
						{
							GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>._GroupByAwaitWithCancellation.ResultSelectCore(this);
						}
						else
						{
							this.awaiter.SourceOnCompleted(GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>._GroupByAwaitWithCancellation.ResultSelectCoreDelegate, this);
						}
					}
					else
					{
						this.completionSource.TrySetResult(false);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x06000609 RID: 1545 RVA: 0x00028ED8 File Offset: 0x000270D8
			private static void ResultSelectCore(object state)
			{
				GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>._GroupByAwaitWithCancellation groupByAwaitWithCancellation = (GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>._GroupByAwaitWithCancellation)state;
				TResult value;
				if (groupByAwaitWithCancellation.TryGetResult<TResult>(groupByAwaitWithCancellation.awaiter, ref value))
				{
					groupByAwaitWithCancellation.Current = value;
					groupByAwaitWithCancellation.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x0600060A RID: 1546 RVA: 0x00028F10 File Offset: 0x00027110
			public UniTask DisposeAsync()
			{
				if (this.groupEnumerator != null)
				{
					this.groupEnumerator.Dispose();
				}
				return default(UniTask);
			}

			// Token: 0x04000975 RID: 2421
			private static readonly Action<object> ResultSelectCoreDelegate = new Action<object>(GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>._GroupByAwaitWithCancellation.ResultSelectCore);

			// Token: 0x04000976 RID: 2422
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000977 RID: 2423
			private readonly Func<TSource, CancellationToken, UniTask<TKey>> keySelector;

			// Token: 0x04000978 RID: 2424
			private readonly Func<TSource, CancellationToken, UniTask<TElement>> elementSelector;

			// Token: 0x04000979 RID: 2425
			private readonly Func<TKey, IEnumerable<TElement>, CancellationToken, UniTask<TResult>> resultSelector;

			// Token: 0x0400097A RID: 2426
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x0400097B RID: 2427
			private CancellationToken cancellationToken;

			// Token: 0x0400097C RID: 2428
			private IEnumerator<IGrouping<TKey, TElement>> groupEnumerator;

			// Token: 0x0400097D RID: 2429
			private UniTask<TResult>.Awaiter awaiter;
		}
	}
}
