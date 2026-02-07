using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200004A RID: 74
	internal class OrderedAsyncEnumerableAwait<TElement, TKey> : OrderedAsyncEnumerable<TElement>
	{
		// Token: 0x0600031C RID: 796 RVA: 0x0000BDE9 File Offset: 0x00009FE9
		public OrderedAsyncEnumerableAwait(IUniTaskAsyncEnumerable<TElement> source, Func<TElement, UniTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, OrderedAsyncEnumerable<TElement> parent) : base(source)
		{
			this.keySelector = keySelector;
			this.comparer = comparer;
			this.descending = descending;
			this.parent = parent;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000BE10 File Offset: 0x0000A010
		internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken)
		{
			AsyncEnumerableSorter<TElement> asyncEnumerableSorter = new AsyncSelectorEnumerableSorter<TElement, TKey>(this.keySelector, this.comparer, this.descending, next);
			if (this.parent != null)
			{
				asyncEnumerableSorter = this.parent.GetAsyncEnumerableSorter(asyncEnumerableSorter, cancellationToken);
			}
			return asyncEnumerableSorter;
		}

		// Token: 0x0400011B RID: 283
		private readonly Func<TElement, UniTask<TKey>> keySelector;

		// Token: 0x0400011C RID: 284
		private readonly IComparer<TKey> comparer;

		// Token: 0x0400011D RID: 285
		private readonly bool descending;

		// Token: 0x0400011E RID: 286
		private readonly OrderedAsyncEnumerable<TElement> parent;
	}
}
