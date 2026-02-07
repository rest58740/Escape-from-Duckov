using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Pathfinding.Collections
{
	// Token: 0x0200025E RID: 606
	public struct SlabAllocator<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000E79 RID: 3705 RVA: 0x00059F2D File Offset: 0x0005812D
		internal static int SizeIndexToElements(int sizeIndex)
		{
			return 1 << sizeIndex;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00059F35 File Offset: 0x00058135
		internal static int ElementsToSizeIndex(int nElements)
		{
			if (nElements < 0)
			{
				throw new Exception("SlabAllocator cannot allocate less than 1 element");
			}
			if (nElements == 0)
			{
				return 0;
			}
			int num = CollectionHelper.Log2Ceil(nElements);
			if (num > 12)
			{
				throw new Exception("SlabAllocator cannot allocate more than MaxAllocationSize elements.");
			}
			return num;
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x000185BF File Offset: 0x000167BF
		public bool IsDebugAllocator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00059F61 File Offset: 0x00058161
		public bool IsCreated
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x00059F70 File Offset: 0x00058170
		public unsafe int ByteSize
		{
			get
			{
				return this.data->mem.Length;
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00059F82 File Offset: 0x00058182
		public unsafe SlabAllocator(int initialCapacityBytes, AllocatorManager.AllocatorHandle allocator)
		{
			this.data = AllocatorManager.Allocate<SlabAllocator<T>.AllocatorData>(allocator, 1);
			this.data->mem = new UnsafeList<byte>(initialCapacityBytes, allocator, NativeArrayOptions.UninitializedMemory);
			this.Clear();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00059FAC File Offset: 0x000581AC
		public unsafe void Clear()
		{
			this.CheckDisposed();
			this.data->mem.Clear();
			for (int i = 0; i < 13; i++)
			{
				*(ref this.data->freeHeads.FixedElementField + (IntPtr)i * 4) = -1;
			}
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00059FF4 File Offset: 0x000581F4
		public unsafe UnsafeSpan<T> GetSpan(int allocatedIndex)
		{
			this.CheckDisposed();
			if (allocatedIndex == -1)
			{
				return new UnsafeSpan<T>(null, 0);
			}
			byte* ptr = this.data->mem.Ptr + allocatedIndex;
			SlabAllocator<T>.Header* ptr2 = (SlabAllocator<T>.Header*)(ptr - sizeof(SlabAllocator<T>.Header));
			uint length = ptr2->length & 1073741823U;
			return new UnsafeSpan<T>((void*)ptr, (int)length);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0005A044 File Offset: 0x00058244
		public unsafe void Realloc(ref int allocatedIndex, int nElements)
		{
			this.CheckDisposed();
			if (allocatedIndex == -1)
			{
				allocatedIndex = this.Allocate(nElements);
				return;
			}
			SlabAllocator<T>.Header* ptr = (SlabAllocator<T>.Header*)(this.data->mem.Ptr + allocatedIndex - sizeof(SlabAllocator<T>.Header));
			uint num = ptr->length & 1073741823U;
			int num2 = SlabAllocator<T>.ElementsToSizeIndex((int)num);
			int num3 = SlabAllocator<T>.ElementsToSizeIndex(nElements);
			if (num2 == num3)
			{
				ptr->length = (uint)(nElements | 1073741824 | int.MinValue);
				return;
			}
			int num4 = this.Allocate(nElements);
			UnsafeSpan<T> span = this.GetSpan(allocatedIndex);
			UnsafeSpan<T> span2 = this.GetSpan(num4);
			span.Slice(0, math.min((int)num, nElements)).CopyTo(span2);
			this.Free(allocatedIndex);
			allocatedIndex = num4;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0005A0F4 File Offset: 0x000582F4
		public unsafe int Allocate(List<T> values)
		{
			int num = this.Allocate(values.Count);
			UnsafeSpan<T> span = this.GetSpan(num);
			for (int i = 0; i < span.Length; i++)
			{
				*span[i] = values[i];
			}
			return num;
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0005A140 File Offset: 0x00058340
		public int Allocate(NativeList<T> values)
		{
			int num = this.Allocate(values.Length);
			this.GetSpan(num).CopyFrom(values.AsArray());
			return num;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0005A170 File Offset: 0x00058370
		public unsafe int Allocate(int nElements)
		{
			this.CheckDisposed();
			if (nElements == 0)
			{
				return -1;
			}
			int num = SlabAllocator<T>.ElementsToSizeIndex(nElements);
			int num2 = *(ref this.data->freeHeads.FixedElementField + (IntPtr)num * 4);
			if (num2 != -1)
			{
				byte* ptr = this.data->mem.Ptr;
				*(ref this.data->freeHeads.FixedElementField + (IntPtr)num * 4) = ((SlabAllocator<T>.NextBlock*)(ptr + num2))->next;
				*(SlabAllocator<T>.Header*)(ptr + num2 - sizeof(SlabAllocator<T>.Header)) = new SlabAllocator<T>.Header
				{
					length = (uint)(nElements | int.MinValue | 1073741824)
				};
				return num2;
			}
			int length = this.data->mem.Length;
			int num3 = length + sizeof(SlabAllocator<T>.Header) + SlabAllocator<T>.SizeIndexToElements(num) * sizeof(T);
			if (Hint.Unlikely(num3 > this.data->mem.Capacity))
			{
				this.data->mem.SetCapacity(math.max(this.data->mem.Capacity * 2, num3));
			}
			this.data->mem.m_length = num3;
			*(SlabAllocator<T>.Header*)(this.data->mem.Ptr + length) = new SlabAllocator<T>.Header
			{
				length = (uint)(nElements | int.MinValue | 1073741824)
			};
			return length + sizeof(SlabAllocator<T>.Header);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0005A2C4 File Offset: 0x000584C4
		public unsafe void Free(int allocatedIndex)
		{
			this.CheckDisposed();
			if (allocatedIndex == -1)
			{
				return;
			}
			byte* ptr = this.data->mem.Ptr;
			SlabAllocator<T>.Header* ptr2 = (SlabAllocator<T>.Header*)(ptr + allocatedIndex - sizeof(SlabAllocator<T>.Header));
			int num = SlabAllocator<T>.ElementsToSizeIndex((int)(ptr2->length & 1073741823U));
			*(SlabAllocator<T>.NextBlock*)(ptr + allocatedIndex) = new SlabAllocator<T>.NextBlock
			{
				next = *(ref this.data->freeHeads.FixedElementField + (IntPtr)num * 4)
			};
			*(ref this.data->freeHeads.FixedElementField + (IntPtr)num * 4) = allocatedIndex;
			ptr2->length &= 1073741823U;
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0005A35C File Offset: 0x0005855C
		public unsafe void CopyTo(SlabAllocator<T> other)
		{
			this.CheckDisposed();
			other.CheckDisposed();
			other.data->mem.CopyFrom(this.data->mem);
			for (int i = 0; i < 13; i++)
			{
				*(ref other.data->freeHeads.FixedElementField + (IntPtr)i * 4) = *(ref this.data->freeHeads.FixedElementField + (IntPtr)i * 4);
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x000035CE File Offset: 0x000017CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckDisposed()
		{
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0005A3CC File Offset: 0x000585CC
		public unsafe void Dispose()
		{
			if (this.data == null)
			{
				return;
			}
			AllocatorManager.AllocatorHandle allocator = this.data->mem.Allocator;
			this.data->mem.Dispose();
			AllocatorManager.Free<SlabAllocator<T>.AllocatorData>(allocator, this.data, 1);
			this.data = null;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0005A418 File Offset: 0x00058618
		public SlabAllocator<T>.List GetList(int allocatedIndex)
		{
			return new SlabAllocator<T>.List(this, allocatedIndex);
		}

		// Token: 0x04000AF5 RID: 2805
		public const int InvalidAllocation = -2;

		// Token: 0x04000AF6 RID: 2806
		public const int ZeroLengthArray = -1;

		// Token: 0x04000AF7 RID: 2807
		public const int MaxAllocationSizeIndex = 12;

		// Token: 0x04000AF8 RID: 2808
		public const int MaxAllocationSize = 4096;

		// Token: 0x04000AF9 RID: 2809
		private const uint UsedBit = 2147483648U;

		// Token: 0x04000AFA RID: 2810
		private const uint AllocatedBit = 1073741824U;

		// Token: 0x04000AFB RID: 2811
		private const uint LengthMask = 1073741823U;

		// Token: 0x04000AFC RID: 2812
		[NativeDisableUnsafePtrRestriction]
		private unsafe SlabAllocator<T>.AllocatorData* data;

		// Token: 0x0200025F RID: 607
		private struct AllocatorData
		{
			// Token: 0x04000AFD RID: 2813
			public UnsafeList<byte> mem;

			// Token: 0x04000AFE RID: 2814
			[FixedBuffer(typeof(int), 13)]
			public SlabAllocator<T>.AllocatorData.<freeHeads>e__FixedBuffer freeHeads;

			// Token: 0x02000260 RID: 608
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 52)]
			public struct <freeHeads>e__FixedBuffer
			{
				// Token: 0x04000AFF RID: 2815
				public int FixedElementField;
			}
		}

		// Token: 0x02000261 RID: 609
		private struct Header
		{
			// Token: 0x04000B00 RID: 2816
			public uint length;
		}

		// Token: 0x02000262 RID: 610
		private struct NextBlock
		{
			// Token: 0x04000B01 RID: 2817
			public int next;
		}

		// Token: 0x02000263 RID: 611
		public ref struct List
		{
			// Token: 0x06000E8A RID: 3722 RVA: 0x0005A426 File Offset: 0x00058626
			public List(SlabAllocator<T> allocator, int allocationIndex)
			{
				this.span = allocator.GetSpan(allocationIndex);
				this.allocator = allocator;
				this.allocationIndex = allocationIndex;
			}

			// Token: 0x06000E8B RID: 3723 RVA: 0x0005A444 File Offset: 0x00058644
			public unsafe void Add(T value)
			{
				this.allocator.Realloc(ref this.allocationIndex, this.span.Length + 1);
				this.span = this.allocator.GetSpan(this.allocationIndex);
				*this.span[this.span.Length - 1] = value;
			}

			// Token: 0x06000E8C RID: 3724 RVA: 0x0005A4A4 File Offset: 0x000586A4
			public void RemoveAt(int index)
			{
				this.span.Slice(index + 1).CopyTo(this.span.Slice(index, this.span.Length - index - 1));
				this.allocator.Realloc(ref this.allocationIndex, this.span.Length - 1);
				this.span = this.allocator.GetSpan(this.allocationIndex);
			}

			// Token: 0x06000E8D RID: 3725 RVA: 0x0005A517 File Offset: 0x00058717
			public void Clear()
			{
				this.allocator.Realloc(ref this.allocationIndex, 0);
				this.span = this.allocator.GetSpan(this.allocationIndex);
			}

			// Token: 0x17000201 RID: 513
			// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0005A542 File Offset: 0x00058742
			public int Length
			{
				get
				{
					return this.span.Length;
				}
			}

			// Token: 0x17000202 RID: 514
			public T this[int index]
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.span[index];
				}
			}

			// Token: 0x04000B02 RID: 2818
			public UnsafeSpan<T> span;

			// Token: 0x04000B03 RID: 2819
			private SlabAllocator<T> allocator;

			// Token: 0x04000B04 RID: 2820
			public int allocationIndex;
		}
	}
}
