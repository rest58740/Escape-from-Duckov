using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200004B RID: 75
	internal class OrderedAsyncEnumerableAwaitWithCancellation<TElement, TKey> : OrderedAsyncEnumerable<TElement>
	{
		// Token: 0x0600031E RID: 798 RVA: 0x0000BE4D File Offset: 0x0000A04D
		public OrderedAsyncEnumerableAwaitWithCancellation(IUniTaskAsyncEnumerable<TElement> source, Func<TElement, CancellationToken, UniTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, OrderedAsyncEnumerable<TElement> parent) : base(source)
		{
			this.keySelector = keySelector;
			this.comparer = comparer;
			this.descending = descending;
			this.parent = parent;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000BE74 File Offset: 0x0000A074
		internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken)
		{
			AsyncEnumerableSorter<TElement> asyncEnumerableSorter = new AsyncSelectorWithCancellationEnumerableSorter<TElement, TKey>(this.keySelector, this.comparer, this.descending, next, cancellationToken);
			if (this.parent != null)
			{
				asyncEnumerableSorter = this.parent.GetAsyncEnumerableSorter(asyncEnumerableSorter, cancellationToken);
			}
			return asyncEnumerableSorter;
		}

		// Token: 0x0400011F RID: 287
		private readonly Func<TElement, CancellationToken, UniTask<TKey>> keySelector;

		// Token: 0x04000120 RID: 288
		private readonly IComparer<TKey> comparer;

		// Token: 0x04000121 RID: 289
		private readonly bool descending;

		// Token: 0x04000122 RID: 290
		private readonly OrderedAsyncEnumerable<TElement> parent;
	}
}
