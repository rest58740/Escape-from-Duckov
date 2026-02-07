using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000048 RID: 72
	internal abstract class OrderedAsyncEnumerable<TElement> : IUniTaskOrderedAsyncEnumerable<TElement>, IUniTaskAsyncEnumerable<TElement>
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000BD3A File Offset: 0x00009F3A
		public OrderedAsyncEnumerable(IUniTaskAsyncEnumerable<TElement> source)
		{
			this.source = source;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000BD49 File Offset: 0x00009F49
		public IUniTaskOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
		{
			return new OrderedAsyncEnumerable<TElement, TKey>(this.source, keySelector, comparer, descending, this);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000BD5A File Offset: 0x00009F5A
		public IUniTaskOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, UniTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending)
		{
			return new OrderedAsyncEnumerableAwait<TElement, TKey>(this.source, keySelector, comparer, descending, this);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000BD6B File Offset: 0x00009F6B
		public IUniTaskOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, CancellationToken, UniTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending)
		{
			return new OrderedAsyncEnumerableAwaitWithCancellation<TElement, TKey>(this.source, keySelector, comparer, descending, this);
		}

		// Token: 0x06000318 RID: 792
		internal abstract AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken);

		// Token: 0x06000319 RID: 793 RVA: 0x0000BD7C File Offset: 0x00009F7C
		public IUniTaskAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new OrderedAsyncEnumerable<TElement>._OrderedAsyncEnumerator(this, cancellationToken);
		}

		// Token: 0x04000116 RID: 278
		protected readonly IUniTaskAsyncEnumerable<TElement> source;

		// Token: 0x02000181 RID: 385
		private class _OrderedAsyncEnumerator : MoveNextSource, IUniTaskAsyncEnumerator<TElement>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000728 RID: 1832 RVA: 0x0003E502 File Offset: 0x0003C702
			public _OrderedAsyncEnumerator(OrderedAsyncEnumerable<TElement> parent, CancellationToken cancellationToken)
			{
				this.parent = parent;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x06000729 RID: 1833 RVA: 0x0003E518 File Offset: 0x0003C718
			// (set) Token: 0x0600072A RID: 1834 RVA: 0x0003E520 File Offset: 0x0003C720
			public TElement Current { get; private set; }

			// Token: 0x0600072B RID: 1835 RVA: 0x0003E52C File Offset: 0x0003C72C
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.map == null)
				{
					this.completionSource.Reset();
					this.CreateSortSource().Forget();
					return new UniTask<bool>(this, this.completionSource.Version);
				}
				if (this.index < this.buffer.Length)
				{
					TElement[] array = this.buffer;
					int[] array2 = this.map;
					int num = this.index;
					this.index = num + 1;
					this.Current = array[array2[num]];
					return CompletedTasks.True;
				}
				return CompletedTasks.False;
			}

			// Token: 0x0600072C RID: 1836 RVA: 0x0003E5BC File Offset: 0x0003C7BC
			private UniTaskVoid CreateSortSource()
			{
				OrderedAsyncEnumerable<TElement>._OrderedAsyncEnumerator.<CreateSortSource>d__11 <CreateSortSource>d__;
				<CreateSortSource>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateSortSource>d__.<>4__this = this;
				<CreateSortSource>d__.<>1__state = -1;
				<CreateSortSource>d__.<>t__builder.Start<OrderedAsyncEnumerable<TElement>._OrderedAsyncEnumerator.<CreateSortSource>d__11>(ref <CreateSortSource>d__);
				return <CreateSortSource>d__.<>t__builder.Task;
			}

			// Token: 0x0600072D RID: 1837 RVA: 0x0003E600 File Offset: 0x0003C800
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x04000E8D RID: 3725
			protected readonly OrderedAsyncEnumerable<TElement> parent;

			// Token: 0x04000E8E RID: 3726
			private CancellationToken cancellationToken;

			// Token: 0x04000E8F RID: 3727
			private TElement[] buffer;

			// Token: 0x04000E90 RID: 3728
			private int[] map;

			// Token: 0x04000E91 RID: 3729
			private int index;
		}
	}
}
