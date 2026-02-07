using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000047 RID: 71
	internal class AsyncSelectorWithCancellationEnumerableSorter<TElement, TKey> : AsyncEnumerableSorter<TElement>
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000BC5E File Offset: 0x00009E5E
		internal AsyncSelectorWithCancellationEnumerableSorter(Func<TElement, CancellationToken, UniTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken)
		{
			this.keySelector = keySelector;
			this.comparer = comparer;
			this.descending = descending;
			this.next = next;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000BC8C File Offset: 0x00009E8C
		internal override UniTask ComputeKeysAsync(TElement[] elements, int count)
		{
			AsyncSelectorWithCancellationEnumerableSorter<TElement, TKey>.<ComputeKeysAsync>d__7 <ComputeKeysAsync>d__;
			<ComputeKeysAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ComputeKeysAsync>d__.<>4__this = this;
			<ComputeKeysAsync>d__.elements = elements;
			<ComputeKeysAsync>d__.count = count;
			<ComputeKeysAsync>d__.<>1__state = -1;
			<ComputeKeysAsync>d__.<>t__builder.Start<AsyncSelectorWithCancellationEnumerableSorter<TElement, TKey>.<ComputeKeysAsync>d__7>(ref <ComputeKeysAsync>d__);
			return <ComputeKeysAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		internal override int CompareKeys(int index1, int index2)
		{
			int num = this.comparer.Compare(this.keys[index1], this.keys[index2]);
			if (num == 0)
			{
				if (this.next == null)
				{
					return index1 - index2;
				}
				return this.next.CompareKeys(index1, index2);
			}
			else
			{
				if (!this.descending)
				{
					return num;
				}
				return -num;
			}
		}

		// Token: 0x04000110 RID: 272
		private readonly Func<TElement, CancellationToken, UniTask<TKey>> keySelector;

		// Token: 0x04000111 RID: 273
		private readonly IComparer<TKey> comparer;

		// Token: 0x04000112 RID: 274
		private readonly bool descending;

		// Token: 0x04000113 RID: 275
		private readonly AsyncEnumerableSorter<TElement> next;

		// Token: 0x04000114 RID: 276
		private CancellationToken cancellationToken;

		// Token: 0x04000115 RID: 277
		private TKey[] keys;
	}
}
