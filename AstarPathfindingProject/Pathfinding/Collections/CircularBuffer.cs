using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Pooling;
using Unity.Profiling;

namespace Pathfinding.Collections
{
	// Token: 0x02000254 RID: 596
	public struct CircularBuffer<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<!0>
	{
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00058B02 File Offset: 0x00056D02
		public readonly int Length
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.length;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x00058B0A File Offset: 0x00056D0A
		public readonly int AbsoluteStartIndex
		{
			get
			{
				return this.head;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00058B12 File Offset: 0x00056D12
		public readonly int AbsoluteEndIndex
		{
			get
			{
				return this.head + this.length - 1;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x00058B23 File Offset: 0x00056D23
		public readonly ref T First
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref this.data[this.head & this.data.Length - 1];
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00058B41 File Offset: 0x00056D41
		public readonly ref T Last
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref this.data[this.head + this.length - 1 & this.data.Length - 1];
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x00058B02 File Offset: 0x00056D02
		int IReadOnlyCollection<!0>.Count
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.length;
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00058B68 File Offset: 0x00056D68
		public CircularBuffer(int initialCapacity)
		{
			this.data = ArrayPool<T>.Claim(initialCapacity);
			this.head = 0;
			this.length = 0;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00058B84 File Offset: 0x00056D84
		public CircularBuffer(T[] backingArray)
		{
			this.data = backingArray;
			this.head = 0;
			this.length = 0;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00058B9B File Offset: 0x00056D9B
		public void Clear()
		{
			this.length = 0;
			this.head = 0;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00058BAC File Offset: 0x00056DAC
		public void AddRange(List<T> items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				this.PushEnd(items[i]);
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00058BD8 File Offset: 0x00056DD8
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushStart(T item)
		{
			if (this.data == null || this.length >= this.data.Length)
			{
				this.Grow();
			}
			this.length++;
			this.head--;
			this[0] = item;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00058C27 File Offset: 0x00056E27
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushEnd(T item)
		{
			if (this.data == null || this.length >= this.data.Length)
			{
				this.Grow();
			}
			this.length++;
			this[this.length - 1] = item;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00058C64 File Offset: 0x00056E64
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Push(bool toStart, T item)
		{
			if (toStart)
			{
				this.PushStart(item);
				return;
			}
			this.PushEnd(item);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00058C78 File Offset: 0x00056E78
		[IgnoredByDeepProfiler]
		public T PopStart()
		{
			if (this.length == 0)
			{
				throw new InvalidOperationException();
			}
			T result = this[0];
			this.head++;
			this.length--;
			return result;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00058CAB File Offset: 0x00056EAB
		[IgnoredByDeepProfiler]
		public T PopEnd()
		{
			if (this.length == 0)
			{
				throw new InvalidOperationException();
			}
			T result = this[this.length - 1];
			this.length--;
			return result;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00058CD7 File Offset: 0x00056ED7
		[IgnoredByDeepProfiler]
		public T Pop(bool fromStart)
		{
			if (fromStart)
			{
				return this.PopStart();
			}
			return this.PopEnd();
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00058CE9 File Offset: 0x00056EE9
		public readonly T GetBoundaryValue(bool start)
		{
			return this.GetAbsolute(start ? this.AbsoluteStartIndex : this.AbsoluteEndIndex);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00058D02 File Offset: 0x00056F02
		public void InsertAbsolute(int index, T item)
		{
			this.SpliceUninitializedAbsolute(index, 0, 1);
			this.data[index & this.data.Length - 1] = item;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00058D25 File Offset: 0x00056F25
		public void Splice(int startIndex, int toRemove, List<T> toInsert)
		{
			this.SpliceAbsolute(startIndex + this.head, toRemove, toInsert);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00058D38 File Offset: 0x00056F38
		public void SpliceAbsolute(int startIndex, int toRemove, List<T> toInsert)
		{
			if (toInsert == null)
			{
				this.SpliceUninitializedAbsolute(startIndex, toRemove, 0);
				return;
			}
			this.SpliceUninitializedAbsolute(startIndex, toRemove, toInsert.Count);
			for (int i = 0; i < toInsert.Count; i++)
			{
				this.data[startIndex + i & this.data.Length - 1] = toInsert[i];
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00058D91 File Offset: 0x00056F91
		public void SpliceUninitialized(int startIndex, int toRemove, int toInsert)
		{
			this.SpliceUninitializedAbsolute(startIndex + this.head, toRemove, toInsert);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00058DA4 File Offset: 0x00056FA4
		public void SpliceUninitializedAbsolute(int startIndex, int toRemove, int toInsert)
		{
			int num = toInsert - toRemove;
			while (this.length + num > this.data.Length)
			{
				this.Grow();
			}
			this.MoveAbsolute(startIndex + toRemove, this.AbsoluteEndIndex, num);
			this.length += num;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00058DF0 File Offset: 0x00056FF0
		private void MoveAbsolute(int startIndex, int endIndex, int deltaIndex)
		{
			if (deltaIndex > 0)
			{
				for (int i = endIndex; i >= startIndex; i--)
				{
					this.data[i + deltaIndex & this.data.Length - 1] = this.data[i & this.data.Length - 1];
				}
				return;
			}
			if (deltaIndex < 0)
			{
				for (int j = startIndex; j <= endIndex; j++)
				{
					this.data[j + deltaIndex & this.data.Length - 1] = this.data[j & this.data.Length - 1];
				}
			}
		}

		// Token: 0x170001E8 RID: 488
		public T this[int index]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.data[index + this.head & this.data.Length - 1];
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.data[index + this.head & this.data.Length - 1] = value;
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00058EBF File Offset: 0x000570BF
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly T GetAbsolute(int index)
		{
			return this.data[index & this.data.Length - 1];
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00058ED8 File Offset: 0x000570D8
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly void SetAbsolute(int index, T value)
		{
			this.data[index & this.data.Length - 1] = value;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00058EF4 File Offset: 0x000570F4
		private void Grow()
		{
			T[] array = ArrayPool<T>.Claim(Math.Max(4, (this.data != null) ? (this.data.Length * 2) : 0));
			if (this.data != null)
			{
				int num = this.data.Length - (this.head & this.data.Length - 1);
				Array.Copy(this.data, this.head & this.data.Length - 1, array, this.head & array.Length - 1, num);
				int num2 = this.length - num;
				if (num2 > 0)
				{
					Array.Copy(this.data, 0, array, this.head + num & array.Length - 1, num2);
				}
				Array.Fill<T>(this.data, default(T));
				ArrayPool<T>.Release(ref this.data, false);
			}
			this.data = array;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00058FC4 File Offset: 0x000571C4
		public void Pool()
		{
			Array.Fill<T>(this.data, default(T));
			ArrayPool<T>.Release(ref this.data, false);
			this.length = 0;
			this.head = 0;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00058FFF File Offset: 0x000571FF
		public IEnumerator<T> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.length; i = num + 1)
			{
				yield return this[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00059013 File Offset: 0x00057213
		IEnumerator IEnumerable.GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.length; i = num + 1)
			{
				yield return this[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00059028 File Offset: 0x00057228
		public CircularBuffer<T> Clone()
		{
			return new CircularBuffer<T>
			{
				data = ((this.data != null) ? ((T[])this.data.Clone()) : null),
				length = this.length,
				head = this.head
			};
		}

		// Token: 0x04000AD0 RID: 2768
		internal T[] data;

		// Token: 0x04000AD1 RID: 2769
		internal int head;

		// Token: 0x04000AD2 RID: 2770
		private int length;
	}
}
