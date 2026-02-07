using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000034 RID: 52
	internal sealed class GroupByAwaitWithCancellation<TSource, TKey, TElement> : IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>>
	{
		// Token: 0x0600028D RID: 653 RVA: 0x00009763 File Offset: 0x00007963
		public GroupByAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.elementSelector = elementSelector;
			this.comparer = comparer;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00009788 File Offset: 0x00007988
		public IUniTaskAsyncEnumerator<IGrouping<TKey, TElement>> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupByAwaitWithCancellation<TSource, TKey, TElement>._GroupByAwaitWithCancellation(this.source, this.keySelector, this.elementSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000D3 RID: 211
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000D4 RID: 212
		private readonly Func<TSource, CancellationToken, UniTask<TKey>> keySelector;

		// Token: 0x040000D5 RID: 213
		private readonly Func<TSource, CancellationToken, UniTask<TElement>> elementSelector;

		// Token: 0x040000D6 RID: 214
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000110 RID: 272
		private sealed class _GroupByAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<IGrouping<TKey, TElement>>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005FC RID: 1532 RVA: 0x00028BE8 File Offset: 0x00026DE8
			public _GroupByAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.elementSelector = elementSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060005FD RID: 1533 RVA: 0x00028C15 File Offset: 0x00026E15
			// (set) Token: 0x060005FE RID: 1534 RVA: 0x00028C1D File Offset: 0x00026E1D
			public IGrouping<TKey, TElement> Current { get; private set; }

			// Token: 0x060005FF RID: 1535 RVA: 0x00028C28 File Offset: 0x00026E28
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

			// Token: 0x06000600 RID: 1536 RVA: 0x00028C7C File Offset: 0x00026E7C
			private UniTaskVoid CreateLookup()
			{
				GroupByAwaitWithCancellation<TSource, TKey, TElement>._GroupByAwaitWithCancellation.<CreateLookup>d__12 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupByAwaitWithCancellation<TSource, TKey, TElement>._GroupByAwaitWithCancellation.<CreateLookup>d__12>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x00028CC0 File Offset: 0x00026EC0
			private void SourceMoveNext()
			{
				try
				{
					if (this.groupEnumerator.MoveNext())
					{
						this.Current = this.groupEnumerator.Current;
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

			// Token: 0x06000602 RID: 1538 RVA: 0x00028D2C File Offset: 0x00026F2C
			public UniTask DisposeAsync()
			{
				if (this.groupEnumerator != null)
				{
					this.groupEnumerator.Dispose();
				}
				return default(UniTask);
			}

			// Token: 0x0400096E RID: 2414
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x0400096F RID: 2415
			private readonly Func<TSource, CancellationToken, UniTask<TKey>> keySelector;

			// Token: 0x04000970 RID: 2416
			private readonly Func<TSource, CancellationToken, UniTask<TElement>> elementSelector;

			// Token: 0x04000971 RID: 2417
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x04000972 RID: 2418
			private CancellationToken cancellationToken;

			// Token: 0x04000973 RID: 2419
			private IEnumerator<IGrouping<TKey, TElement>> groupEnumerator;
		}
	}
}
