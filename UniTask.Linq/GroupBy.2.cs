using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000031 RID: 49
	internal sealed class GroupBy<TSource, TKey, TElement, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000287 RID: 647 RVA: 0x00009678 File Offset: 0x00007878
		public GroupBy(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.elementSelector = elementSelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x000096A5 File Offset: 0x000078A5
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupBy<TSource, TKey, TElement, TResult>._GroupBy(this.source, this.keySelector, this.elementSelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000C5 RID: 197
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000C6 RID: 198
		private readonly Func<TSource, TKey> keySelector;

		// Token: 0x040000C7 RID: 199
		private readonly Func<TSource, TElement> elementSelector;

		// Token: 0x040000C8 RID: 200
		private readonly Func<TKey, IEnumerable<TElement>, TResult> resultSelector;

		// Token: 0x040000C9 RID: 201
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x0200010D RID: 269
		private sealed class _GroupBy : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005E5 RID: 1509 RVA: 0x00028705 File Offset: 0x00026905
			public _GroupBy(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.elementSelector = elementSelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0002873A File Offset: 0x0002693A
			// (set) Token: 0x060005E7 RID: 1511 RVA: 0x00028742 File Offset: 0x00026942
			public TResult Current { get; private set; }

			// Token: 0x060005E8 RID: 1512 RVA: 0x0002874C File Offset: 0x0002694C
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

			// Token: 0x060005E9 RID: 1513 RVA: 0x000287A0 File Offset: 0x000269A0
			private UniTaskVoid CreateLookup()
			{
				GroupBy<TSource, TKey, TElement, TResult>._GroupBy.<CreateLookup>d__13 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupBy<TSource, TKey, TElement, TResult>._GroupBy.<CreateLookup>d__13>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x060005EA RID: 1514 RVA: 0x000287E4 File Offset: 0x000269E4
			private void SourceMoveNext()
			{
				try
				{
					if (this.groupEnumerator.MoveNext())
					{
						IGrouping<TKey, TElement> grouping = this.groupEnumerator.Current;
						this.Current = this.resultSelector(grouping.Key, grouping);
						this.completionSource.TrySetResult(true);
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

			// Token: 0x060005EB RID: 1515 RVA: 0x00028860 File Offset: 0x00026A60
			public UniTask DisposeAsync()
			{
				if (this.groupEnumerator != null)
				{
					this.groupEnumerator.Dispose();
				}
				return default(UniTask);
			}

			// Token: 0x04000955 RID: 2389
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000956 RID: 2390
			private readonly Func<TSource, TKey> keySelector;

			// Token: 0x04000957 RID: 2391
			private readonly Func<TSource, TElement> elementSelector;

			// Token: 0x04000958 RID: 2392
			private readonly Func<TKey, IEnumerable<TElement>, TResult> resultSelector;

			// Token: 0x04000959 RID: 2393
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x0400095A RID: 2394
			private CancellationToken cancellationToken;

			// Token: 0x0400095B RID: 2395
			private IEnumerator<IGrouping<TKey, TElement>> groupEnumerator;
		}
	}
}
