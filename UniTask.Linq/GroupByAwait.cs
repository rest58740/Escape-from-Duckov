using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000032 RID: 50
	internal sealed class GroupByAwait<TSource, TKey, TElement> : IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>>
	{
		// Token: 0x06000289 RID: 649 RVA: 0x000096CB File Offset: 0x000078CB
		public GroupByAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.elementSelector = elementSelector;
			this.comparer = comparer;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000096F0 File Offset: 0x000078F0
		public IUniTaskAsyncEnumerator<IGrouping<TKey, TElement>> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupByAwait<TSource, TKey, TElement>._GroupByAwait(this.source, this.keySelector, this.elementSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000CA RID: 202
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000CB RID: 203
		private readonly Func<TSource, UniTask<TKey>> keySelector;

		// Token: 0x040000CC RID: 204
		private readonly Func<TSource, UniTask<TElement>> elementSelector;

		// Token: 0x040000CD RID: 205
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x0200010E RID: 270
		private sealed class _GroupByAwait : MoveNextSource, IUniTaskAsyncEnumerator<IGrouping<TKey, TElement>>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005EC RID: 1516 RVA: 0x00028889 File Offset: 0x00026A89
			public _GroupByAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.elementSelector = elementSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060005ED RID: 1517 RVA: 0x000288B6 File Offset: 0x00026AB6
			// (set) Token: 0x060005EE RID: 1518 RVA: 0x000288BE File Offset: 0x00026ABE
			public IGrouping<TKey, TElement> Current { get; private set; }

			// Token: 0x060005EF RID: 1519 RVA: 0x000288C8 File Offset: 0x00026AC8
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

			// Token: 0x060005F0 RID: 1520 RVA: 0x0002891C File Offset: 0x00026B1C
			private UniTaskVoid CreateLookup()
			{
				GroupByAwait<TSource, TKey, TElement>._GroupByAwait.<CreateLookup>d__12 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupByAwait<TSource, TKey, TElement>._GroupByAwait.<CreateLookup>d__12>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x00028960 File Offset: 0x00026B60
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

			// Token: 0x060005F2 RID: 1522 RVA: 0x000289CC File Offset: 0x00026BCC
			public UniTask DisposeAsync()
			{
				if (this.groupEnumerator != null)
				{
					this.groupEnumerator.Dispose();
				}
				return default(UniTask);
			}

			// Token: 0x0400095D RID: 2397
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x0400095E RID: 2398
			private readonly Func<TSource, UniTask<TKey>> keySelector;

			// Token: 0x0400095F RID: 2399
			private readonly Func<TSource, UniTask<TElement>> elementSelector;

			// Token: 0x04000960 RID: 2400
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x04000961 RID: 2401
			private CancellationToken cancellationToken;

			// Token: 0x04000962 RID: 2402
			private IEnumerator<IGrouping<TKey, TElement>> groupEnumerator;
		}
	}
}
