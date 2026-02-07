using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000046 RID: 70
	internal class AsyncSelectorEnumerableSorter<TElement, TKey> : AsyncEnumerableSorter<TElement>
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000BB8A File Offset: 0x00009D8A
		internal AsyncSelectorEnumerableSorter(Func<TElement, UniTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement> next)
		{
			this.keySelector = keySelector;
			this.comparer = comparer;
			this.descending = descending;
			this.next = next;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000BBB0 File Offset: 0x00009DB0
		internal override UniTask ComputeKeysAsync(TElement[] elements, int count)
		{
			AsyncSelectorEnumerableSorter<TElement, TKey>.<ComputeKeysAsync>d__6 <ComputeKeysAsync>d__;
			<ComputeKeysAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ComputeKeysAsync>d__.<>4__this = this;
			<ComputeKeysAsync>d__.elements = elements;
			<ComputeKeysAsync>d__.count = count;
			<ComputeKeysAsync>d__.<>1__state = -1;
			<ComputeKeysAsync>d__.<>t__builder.Start<AsyncSelectorEnumerableSorter<TElement, TKey>.<ComputeKeysAsync>d__6>(ref <ComputeKeysAsync>d__);
			return <ComputeKeysAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000BC04 File Offset: 0x00009E04
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

		// Token: 0x0400010B RID: 267
		private readonly Func<TElement, UniTask<TKey>> keySelector;

		// Token: 0x0400010C RID: 268
		private readonly IComparer<TKey> comparer;

		// Token: 0x0400010D RID: 269
		private readonly bool descending;

		// Token: 0x0400010E RID: 270
		private readonly AsyncEnumerableSorter<TElement> next;

		// Token: 0x0400010F RID: 271
		private TKey[] keys;
	}
}
