using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;

namespace Pathfinding.Collections
{
	// Token: 0x0200025B RID: 603
	public struct NativeCircularBuffer<[IsUnmanaged] T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : struct, ValueType
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x000597E9 File Offset: 0x000579E9
		public readonly int Length
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.length;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x000597F1 File Offset: 0x000579F1
		public readonly int AbsoluteStartIndex
		{
			get
			{
				return this.head;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x000597F9 File Offset: 0x000579F9
		public readonly int AbsoluteEndIndex
		{
			get
			{
				return this.head + this.length - 1;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0005980A File Offset: 0x00057A0A
		public unsafe readonly ref T First
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref this.data[(IntPtr)(this.head & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x00059828 File Offset: 0x00057A28
		public unsafe readonly ref T Last
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref this.data[(IntPtr)(this.head + this.length - 1 & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0005984F File Offset: 0x00057A4F
		int IReadOnlyCollection<!0>.Count
		{
			get
			{
				return this.Length;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x00059857 File Offset: 0x00057A57
		public readonly bool IsCreated
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00059866 File Offset: 0x00057A66
		public NativeCircularBuffer(AllocatorManager.AllocatorHandle allocator)
		{
			this.data = null;
			this.Allocator = allocator;
			this.capacityMask = -1;
			this.head = 0;
			this.length = 0;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0005988C File Offset: 0x00057A8C
		public NativeCircularBuffer(int initialCapacity, AllocatorManager.AllocatorHandle allocator)
		{
			initialCapacity = math.ceilpow2(initialCapacity);
			this.data = AllocatorManager.Allocate<T>(allocator, initialCapacity);
			this.capacityMask = initialCapacity - 1;
			this.Allocator = allocator;
			this.head = 0;
			this.length = 0;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000598C1 File Offset: 0x00057AC1
		public NativeCircularBuffer(CircularBuffer<T> buffer, out ulong gcHandle)
		{
			this = new NativeCircularBuffer<T>(buffer.data, buffer.head, buffer.Length, out gcHandle);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000598DD File Offset: 0x00057ADD
		public unsafe NativeCircularBuffer(T[] data, int head, int length, out ulong gcHandle)
		{
			this.data = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(data, out gcHandle);
			this.capacityMask = data.Length - 1;
			this.head = head;
			this.length = length;
			this.Allocator = Unity.Collections.Allocator.None;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00059912 File Offset: 0x00057B12
		public void Clear()
		{
			this.length = 0;
			this.head = 0;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00059924 File Offset: 0x00057B24
		public void AddRange(List<T> items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				this.PushEnd(items[i]);
			}
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0005994F File Offset: 0x00057B4F
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushStart(T item)
		{
			if (this.length > this.capacityMask)
			{
				this.Grow();
			}
			this.length++;
			this.head--;
			this[0] = item;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00059989 File Offset: 0x00057B89
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushEnd(T item)
		{
			if (this.length > this.capacityMask)
			{
				this.Grow();
			}
			this.length++;
			this[this.length - 1] = item;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x000599BC File Offset: 0x00057BBC
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

		// Token: 0x06000E5B RID: 3675 RVA: 0x000599D0 File Offset: 0x00057BD0
		[IgnoredByDeepProfiler]
		public T PopStart()
		{
			T result = this[0];
			this.head++;
			this.length--;
			return result;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000599F5 File Offset: 0x00057BF5
		[IgnoredByDeepProfiler]
		public T PopEnd()
		{
			T result = this[this.length - 1];
			this.length--;
			return result;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00059A13 File Offset: 0x00057C13
		public T Pop(bool fromStart)
		{
			if (fromStart)
			{
				return this.PopStart();
			}
			return this.PopEnd();
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00059A25 File Offset: 0x00057C25
		public readonly T GetBoundaryValue(bool start)
		{
			if (!start)
			{
				return this.GetAbsolute(this.AbsoluteEndIndex);
			}
			return this.GetAbsolute(this.AbsoluteStartIndex);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00059A43 File Offset: 0x00057C43
		public void TrimTo(int length)
		{
			this.length = math.min(this.length, length);
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00059A57 File Offset: 0x00057C57
		public void Splice(int startIndex, int toRemove, List<T> toInsert)
		{
			this.SpliceAbsolute(startIndex + this.head, toRemove, toInsert);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00059A6C File Offset: 0x00057C6C
		public unsafe void SpliceAbsolute(int startIndex, int toRemove, List<T> toInsert)
		{
			this.SpliceUninitializedAbsolute(startIndex, toRemove, toInsert.Count);
			for (int i = 0; i < toInsert.Count; i++)
			{
				this.data[(IntPtr)(startIndex + i & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = toInsert[i];
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00059ABD File Offset: 0x00057CBD
		public void SpliceUninitialized(int startIndex, int toRemove, int toInsert)
		{
			this.SpliceUninitializedAbsolute(startIndex + this.head, toRemove, toInsert);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00059AD0 File Offset: 0x00057CD0
		public void SpliceUninitializedAbsolute(int startIndex, int toRemove, int toInsert)
		{
			int num = toInsert - toRemove;
			while (this.length + num > this.capacityMask + 1)
			{
				this.Grow();
			}
			this.MoveAbsolute(startIndex + toRemove, this.AbsoluteEndIndex, num);
			this.length += num;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00059B1C File Offset: 0x00057D1C
		private unsafe void MoveAbsolute(int startIndex, int endIndex, int deltaIndex)
		{
			if (deltaIndex > 0)
			{
				for (int i = endIndex; i >= startIndex; i--)
				{
					this.data[(IntPtr)(i + deltaIndex & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = this.data[(IntPtr)(i & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
				return;
			}
			if (deltaIndex < 0)
			{
				for (int j = startIndex; j <= endIndex; j++)
				{
					this.data[(IntPtr)(j + deltaIndex & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = this.data[(IntPtr)(j & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
			}
		}

		// Token: 0x170001F9 RID: 505
		public unsafe T this[int index]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.data[(IntPtr)(index + this.head & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.data[(IntPtr)(index + this.head & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
			}
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00059C09 File Offset: 0x00057E09
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly T GetAbsolute(int index)
		{
			return this.data[(IntPtr)(index & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00059C28 File Offset: 0x00057E28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe void Grow()
		{
			int num = this.capacityMask + 1;
			int num2 = math.max(4, num * 2);
			T* ptr = AllocatorManager.Allocate<T>(this.Allocator, num2);
			if (this.data != null)
			{
				int num3 = num - (this.head & this.capacityMask);
				UnsafeUtility.MemCpy((void*)(ptr + (IntPtr)(this.head & num2 - 1) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)(this.data + (IntPtr)(this.head & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)(num3 * sizeof(T)));
				int num4 = this.length - num3;
				if (num4 > 0)
				{
					UnsafeUtility.MemCpy((void*)(ptr + (IntPtr)(this.head + num3 & num2 - 1) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)this.data, (long)(num4 * sizeof(T)));
				}
				AllocatorManager.Free<T>(this.Allocator, this.data, 1);
			}
			this.capacityMask = num2 - 1;
			this.data = ptr;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00059D0C File Offset: 0x00057F0C
		public void Dispose()
		{
			this.capacityMask = -1;
			this.length = 0;
			this.head = 0;
			AllocatorManager.Free<T>(this.Allocator, this.data, 1);
			this.data = null;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00059D3D File Offset: 0x00057F3D
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

		// Token: 0x06000E6B RID: 3691 RVA: 0x00059D51 File Offset: 0x00057F51
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

		// Token: 0x06000E6C RID: 3692 RVA: 0x00059D68 File Offset: 0x00057F68
		public unsafe NativeCircularBuffer<T> Clone()
		{
			if (!this.IsCreated)
			{
				return default(NativeCircularBuffer<T>);
			}
			T* destination = AllocatorManager.Allocate<T>(this.Allocator, this.capacityMask + 1);
			UnsafeUtility.MemCpy((void*)destination, (void*)this.data, (long)(this.length * sizeof(T)));
			return new NativeCircularBuffer<T>
			{
				data = destination,
				head = this.head,
				length = this.length,
				capacityMask = this.capacityMask,
				Allocator = this.Allocator
			};
		}

		// Token: 0x04000AE8 RID: 2792
		[NativeDisableUnsafePtrRestriction]
		internal unsafe T* data;

		// Token: 0x04000AE9 RID: 2793
		internal int head;

		// Token: 0x04000AEA RID: 2794
		private int length;

		// Token: 0x04000AEB RID: 2795
		private int capacityMask;

		// Token: 0x04000AEC RID: 2796
		public AllocatorManager.AllocatorHandle Allocator;
	}
}
