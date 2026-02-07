using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000049 RID: 73
	internal class OrderedAsyncEnumerable<TElement, TKey> : OrderedAsyncEnumerable<TElement>
	{
		// Token: 0x0600031A RID: 794 RVA: 0x0000BD85 File Offset: 0x00009F85
		public OrderedAsyncEnumerable(IUniTaskAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending, OrderedAsyncEnumerable<TElement> parent) : base(source)
		{
			this.keySelector = keySelector;
			this.comparer = comparer;
			this.descending = descending;
			this.parent = parent;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000BDAC File Offset: 0x00009FAC
		internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken)
		{
			AsyncEnumerableSorter<TElement> asyncEnumerableSorter = new SyncSelectorAsyncEnumerableSorter<TElement, TKey>(this.keySelector, this.comparer, this.descending, next);
			if (this.parent != null)
			{
				asyncEnumerableSorter = this.parent.GetAsyncEnumerableSorter(asyncEnumerableSorter, cancellationToken);
			}
			return asyncEnumerableSorter;
		}

		// Token: 0x04000117 RID: 279
		private readonly Func<TElement, TKey> keySelector;

		// Token: 0x04000118 RID: 280
		private readonly IComparer<TKey> comparer;

		// Token: 0x04000119 RID: 281
		private readonly bool descending;

		// Token: 0x0400011A RID: 282
		private readonly OrderedAsyncEnumerable<TElement> parent;
	}
}
