using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x02000058 RID: 88
	[BurstCompile]
	public struct BinaryHeap
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000F30B File Offset: 0x0000D50B
		public bool isEmpty
		{
			get
			{
				return this.numberOfItems <= 0;
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000F319 File Offset: 0x0000D519
		private static int RoundUpToNextMultipleMod1(int v)
		{
			return v + (4 - (v - 1) % 4) % 4;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000F326 File Offset: 0x0000D526
		public BinaryHeap(int capacity)
		{
			capacity = BinaryHeap.RoundUpToNextMultipleMod1(capacity);
			this.heap = new UnsafeSpan<BinaryHeap.HeapNode>(Allocator.Persistent, capacity);
			this.numberOfItems = 0;
			this.insertionOrder = 0U;
			this.tieBreaking = BinaryHeap.TieBreaking.HScore;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000F352 File Offset: 0x0000D552
		public void Dispose()
		{
			AllocatorManager.Free<BinaryHeap.HeapNode>(Allocator.Persistent, this.heap.ptr, this.heap.Length);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000F378 File Offset: 0x0000D578
		public void Clear(UnsafeSpan<PathNode> pathNodes)
		{
			for (int i = 0; i < this.numberOfItems; i++)
			{
				pathNodes[this.heap[i].pathNodeIndex].heapIndex = ushort.MaxValue;
			}
			this.numberOfItems = 0;
			this.insertionOrder = 0U;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000F3C6 File Offset: 0x0000D5C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetPathNodeIndex(int heapIndex)
		{
			return this.heap[heapIndex].pathNodeIndex;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000F3D9 File Offset: 0x0000D5D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetH(int heapIndex)
		{
			if (this.tieBreaking != BinaryHeap.TieBreaking.HScore)
			{
				return 0U;
			}
			return this.heap[heapIndex].TieBreaker;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000F3F6 File Offset: 0x0000D5F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetF(int heapIndex)
		{
			return this.heap[heapIndex].F;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000F40C File Offset: 0x0000D60C
		public void SetH(int heapIndex, uint h)
		{
			ref BinaryHeap.HeapNode ptr = ref this.heap[heapIndex];
			uint num = ptr.F - ptr.TieBreaker;
			ptr.TieBreaker = h;
			ptr.F = num + h;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000F444 File Offset: 0x0000D644
		private static void Expand(ref UnsafeSpan<BinaryHeap.HeapNode> heap)
		{
			int num = math.max(heap.Length + 4, math.min(65533, (int)math.round((float)heap.Length * 2f)));
			num = BinaryHeap.RoundUpToNextMultipleMod1(num);
			if (num > 65534)
			{
				throw new Exception("Binary Heap Size really large (>65534). A heap size this large is probably the cause of pathfinding running in an infinite loop. ");
			}
			UnsafeSpan<BinaryHeap.HeapNode> unsafeSpan = new UnsafeSpan<BinaryHeap.HeapNode>(Allocator.Persistent, num);
			unsafeSpan.CopyFrom(heap);
			AllocatorManager.Free<BinaryHeap.HeapNode>(Allocator.Persistent, heap.ptr, heap.Length);
			heap = unsafeSpan;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000F4CC File Offset: 0x0000D6CC
		public void Add(UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h)
		{
			uint num = this.insertionOrder;
			this.insertionOrder = num + 1U;
			BinaryHeap.Add(ref this, ref nodes, pathNodeIndex, g, h, num, this.tieBreaking);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		[BurstCompile]
		private static void Add(ref BinaryHeap binaryHeap, ref UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h, uint insertionOrder, BinaryHeap.TieBreaking tieBreaking)
		{
			BinaryHeap.Add_000002FF$BurstDirectCall.Invoke(ref binaryHeap, ref nodes, pathNodeIndex, g, h, insertionOrder, tieBreaking);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000F510 File Offset: 0x0000D710
		private unsafe static void DecreaseKey(UnsafeSpan<BinaryHeap.HeapNode> heap, UnsafeSpan<PathNode> nodes, BinaryHeap.HeapNode node, ushort index)
		{
			uint num;
			uint num2;
			for (num = (uint)index; num != 0U; num = num2)
			{
				num2 = (num - 1U) / 4U;
				Hint.Assume(num2 < heap.length);
				Hint.Assume(num < heap.length);
				if (node.sortKey >= heap[num2].sortKey)
				{
					break;
				}
				*heap[num] = *heap[num2];
				nodes[heap[num].pathNodeIndex].heapIndex = (ushort)num;
			}
			Hint.Assume(num < heap.length);
			*heap[num] = node;
			nodes[node.pathNodeIndex].heapIndex = (ushort)num;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000F5C4 File Offset: 0x0000D7C4
		public uint Remove(UnsafeSpan<PathNode> nodes, out uint g, out uint h)
		{
			uint num;
			uint num2;
			uint result = BinaryHeap.Remove(ref nodes, ref this, out num, out num2);
			h = ((this.tieBreaking == BinaryHeap.TieBreaking.HScore) ? num : 0U);
			g = num2 - h;
			return result;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000F5F1 File Offset: 0x0000D7F1
		[BurstCompile]
		private static uint Remove(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap, [NoAlias] out uint removedTieBreaker, [NoAlias] out uint removedF)
		{
			return BinaryHeap.Remove_00000302$BurstDirectCall.Invoke(ref nodes, ref binaryHeap, out removedTieBreaker, out removedF);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000F5FC File Offset: 0x0000D7FC
		[Conditional("VALIDATE_BINARY_HEAP")]
		private static void Validate(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap)
		{
			for (int i = 1; i < binaryHeap.numberOfItems; i++)
			{
				int index = (i - 1) / 4;
				if (binaryHeap.heap[index].F > binaryHeap.heap[i].F)
				{
					throw new Exception(string.Concat(new string[]
					{
						"Invalid state at ",
						i.ToString(),
						":",
						index.ToString(),
						" ( ",
						binaryHeap.heap[index].F.ToString(),
						" > ",
						binaryHeap.heap[i].F.ToString(),
						" ) "
					}));
				}
				if (binaryHeap.heap[index].sortKey > binaryHeap.heap[i].sortKey)
				{
					throw new Exception(string.Concat(new string[]
					{
						"Invalid state at ",
						i.ToString(),
						":",
						index.ToString(),
						" ( ",
						binaryHeap.heap[index].F.ToString(),
						" > ",
						binaryHeap.heap[i].F.ToString(),
						" ) "
					}));
				}
				if ((int)nodes[binaryHeap.heap[i].pathNodeIndex].heapIndex != i)
				{
					throw new Exception("Invalid heap index");
				}
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		public unsafe void Rebuild(UnsafeSpan<PathNode> nodes)
		{
			for (int i = 2; i < this.numberOfItems; i++)
			{
				int num = i;
				BinaryHeap.HeapNode heapNode = *this.heap[i];
				uint f = heapNode.F;
				while (num != 1)
				{
					int num2 = num / 4;
					if (f >= this.heap[num2].F)
					{
						break;
					}
					*this.heap[num] = *this.heap[num2];
					nodes[this.heap[num].pathNodeIndex].heapIndex = (ushort)num;
					*this.heap[num2] = heapNode;
					nodes[this.heap[num2].pathNodeIndex].heapIndex = (ushort)num2;
					num = num2;
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000F890 File Offset: 0x0000DA90
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void Add$BurstManaged(ref BinaryHeap binaryHeap, ref UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h, uint insertionOrder, BinaryHeap.TieBreaking tieBreaking)
		{
			ref int ptr = ref binaryHeap.numberOfItems;
			ref UnsafeSpan<BinaryHeap.HeapNode> ptr2 = ref binaryHeap.heap;
			uint f = g + h;
			uint tieBreaker = (tieBreaking == BinaryHeap.TieBreaking.HScore) ? h : (insertionOrder << 2);
			ref PathNode ptr3 = ref nodes[pathNodeIndex];
			if (ptr3.heapIndex != 65535)
			{
				BinaryHeap.HeapNode node = new BinaryHeap.HeapNode(pathNodeIndex, tieBreaker, f);
				BinaryHeap.DecreaseKey(ptr2, nodes, node, ptr3.heapIndex);
				return;
			}
			if (ptr == ptr2.Length)
			{
				BinaryHeap.Expand(ref ptr2);
			}
			BinaryHeap.DecreaseKey(ptr2, nodes, new BinaryHeap.HeapNode(pathNodeIndex, tieBreaker, f), (ushort)ptr);
			ptr++;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000F92C File Offset: 0x0000DB2C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static uint Remove$BurstManaged(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap, [NoAlias] out uint removedTieBreaker, [NoAlias] out uint removedF)
		{
			ref int ptr = ref binaryHeap.numberOfItems;
			UnsafeSpan<BinaryHeap.HeapNode> unsafeSpan = binaryHeap.heap;
			if (ptr == 0)
			{
				throw new InvalidOperationException("Removing item from empty heap");
			}
			Hint.Assume(0UL < (ulong)unsafeSpan.length);
			uint pathNodeIndex = unsafeSpan[0].pathNodeIndex;
			nodes[pathNodeIndex].heapIndex = ushort.MaxValue;
			removedTieBreaker = unsafeSpan[0].TieBreaker;
			removedF = unsafeSpan[0].F;
			ptr--;
			if (ptr == 0)
			{
				return pathNodeIndex;
			}
			Hint.Assume(ptr < (int)unsafeSpan.length);
			BinaryHeap.HeapNode heapNode = *unsafeSpan[ptr];
			uint num = 0U;
			ulong num2 = heapNode.sortKey & 18446744073709551612UL;
			for (;;)
			{
				uint num3 = num;
				uint num4 = num3 * 4U + 1U;
				if ((ulong)num4 >= (ulong)((long)ptr))
				{
					break;
				}
				Hint.Assume(num4 < unsafeSpan.length);
				ulong num5 = (unsafeSpan[num4].sortKey & 18446744073709551612UL) | 0UL;
				Hint.Assume(num4 + 1U < unsafeSpan.length);
				ulong y = (unsafeSpan[num4 + 1U].sortKey & 18446744073709551612UL) | 1UL;
				Hint.Assume(num4 + 2U < unsafeSpan.length);
				ulong y2 = (unsafeSpan[num4 + 2U].sortKey & 18446744073709551612UL) | 2UL;
				Hint.Assume(num4 + 3U < unsafeSpan.length);
				ulong y3 = (unsafeSpan[num4 + 3U].sortKey & 18446744073709551612UL) | 3UL;
				ulong num6 = num5;
				if ((ulong)(num4 + 1U) < (ulong)((long)ptr))
				{
					num6 = math.min(num6, y);
				}
				if ((ulong)(num4 + 2U) < (ulong)((long)ptr))
				{
					num6 = math.min(num6, y2);
				}
				if ((ulong)(num4 + 3U) < (ulong)((long)ptr))
				{
					num6 = math.min(num6, y3);
				}
				if ((num6 & 18446744073709551612UL) >= num2)
				{
					break;
				}
				num = num4 + (uint)(num6 & 3UL);
				Hint.Assume(num3 < unsafeSpan.length);
				Hint.Assume(num < unsafeSpan.length);
				*unsafeSpan[num3] = *unsafeSpan[num];
				Hint.Assume(num < unsafeSpan.length);
				nodes[unsafeSpan[num].pathNodeIndex].heapIndex = (ushort)num3;
			}
			Hint.Assume(num < unsafeSpan.length);
			*unsafeSpan[num] = heapNode;
			nodes[heapNode.pathNodeIndex].heapIndex = (ushort)num;
			return pathNodeIndex;
		}

		// Token: 0x040001EF RID: 495
		private UnsafeSpan<BinaryHeap.HeapNode> heap;

		// Token: 0x040001F0 RID: 496
		public int numberOfItems;

		// Token: 0x040001F1 RID: 497
		private uint insertionOrder;

		// Token: 0x040001F2 RID: 498
		public BinaryHeap.TieBreaking tieBreaking;

		// Token: 0x040001F3 RID: 499
		public const float GrowthFactor = 2f;

		// Token: 0x040001F4 RID: 500
		private const int D = 4;

		// Token: 0x040001F5 RID: 501
		public const ushort NotInHeap = 65535;

		// Token: 0x02000059 RID: 89
		public enum TieBreaking : byte
		{
			// Token: 0x040001F7 RID: 503
			HScore,
			// Token: 0x040001F8 RID: 504
			InsertionOrder
		}

		// Token: 0x0200005A RID: 90
		private struct HeapNode
		{
			// Token: 0x06000328 RID: 808 RVA: 0x0000FB88 File Offset: 0x0000DD88
			public HeapNode(uint pathNodeIndex, uint tieBreaker, uint f)
			{
				this.pathNodeIndex = pathNodeIndex;
				this.sortKey = ((ulong)f << 32 | (ulong)tieBreaker);
			}

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x06000329 RID: 809 RVA: 0x0000FB9F File Offset: 0x0000DD9F
			// (set) Token: 0x0600032A RID: 810 RVA: 0x0000FBAB File Offset: 0x0000DDAB
			public uint F
			{
				get
				{
					return (uint)(this.sortKey >> 32);
				}
				set
				{
					this.sortKey = ((this.sortKey & (ulong)-1) | (ulong)value << 32);
				}
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x0600032B RID: 811 RVA: 0x0000FBC2 File Offset: 0x0000DDC2
			// (set) Token: 0x0600032C RID: 812 RVA: 0x0000FBCB File Offset: 0x0000DDCB
			public uint TieBreaker
			{
				get
				{
					return (uint)this.sortKey;
				}
				set
				{
					this.sortKey = ((this.sortKey & 18446744069414584320UL) | (ulong)value);
				}
			}

			// Token: 0x040001F9 RID: 505
			public uint pathNodeIndex;

			// Token: 0x040001FA RID: 506
			public ulong sortKey;
		}

		// Token: 0x0200005B RID: 91
		// (Invoke) Token: 0x0600032E RID: 814
		internal delegate void Add_000002FF$PostfixBurstDelegate(ref BinaryHeap binaryHeap, ref UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h, uint insertionOrder, BinaryHeap.TieBreaking tieBreaking);

		// Token: 0x0200005C RID: 92
		internal static class Add_000002FF$BurstDirectCall
		{
			// Token: 0x06000331 RID: 817 RVA: 0x0000FBE6 File Offset: 0x0000DDE6
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (BinaryHeap.Add_000002FF$BurstDirectCall.Pointer == 0)
				{
					BinaryHeap.Add_000002FF$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BinaryHeap.Add_000002FF$BurstDirectCall.DeferredCompilation, methodof(BinaryHeap.Add$BurstManaged(BinaryHeap*, UnsafeSpan<PathNode>*, uint, uint, uint, uint, BinaryHeap.TieBreaking)).MethodHandle, typeof(BinaryHeap.Add_000002FF$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = BinaryHeap.Add_000002FF$BurstDirectCall.Pointer;
			}

			// Token: 0x06000332 RID: 818 RVA: 0x0000FC14 File Offset: 0x0000DE14
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				BinaryHeap.Add_000002FF$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000333 RID: 819 RVA: 0x0000FC2C File Offset: 0x0000DE2C
			public unsafe static void Constructor()
			{
				BinaryHeap.Add_000002FF$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BinaryHeap.Add(BinaryHeap*, UnsafeSpan<PathNode>*, uint, uint, uint, uint, BinaryHeap.TieBreaking)).MethodHandle);
			}

			// Token: 0x06000334 RID: 820 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000335 RID: 821 RVA: 0x0000FC3D File Offset: 0x0000DE3D
			// Note: this type is marked as 'beforefieldinit'.
			static Add_000002FF$BurstDirectCall()
			{
				BinaryHeap.Add_000002FF$BurstDirectCall.Constructor();
			}

			// Token: 0x06000336 RID: 822 RVA: 0x0000FC44 File Offset: 0x0000DE44
			public static void Invoke(ref BinaryHeap binaryHeap, ref UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h, uint insertionOrder, BinaryHeap.TieBreaking tieBreaking)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = BinaryHeap.Add_000002FF$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.BinaryHeap&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.PathNode>&,System.UInt32,System.UInt32,System.UInt32,System.UInt32,Pathfinding.BinaryHeap/TieBreaking), ref binaryHeap, ref nodes, pathNodeIndex, g, h, insertionOrder, tieBreaking, functionPointer);
						return;
					}
				}
				BinaryHeap.Add$BurstManaged(ref binaryHeap, ref nodes, pathNodeIndex, g, h, insertionOrder, tieBreaking);
			}

			// Token: 0x040001FB RID: 507
			private static IntPtr Pointer;

			// Token: 0x040001FC RID: 508
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200005D RID: 93
		// (Invoke) Token: 0x06000338 RID: 824
		internal delegate uint Remove_00000302$PostfixBurstDelegate(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap, [NoAlias] out uint removedTieBreaker, [NoAlias] out uint removedF);

		// Token: 0x0200005E RID: 94
		internal static class Remove_00000302$BurstDirectCall
		{
			// Token: 0x0600033B RID: 827 RVA: 0x0000FC87 File Offset: 0x0000DE87
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (BinaryHeap.Remove_00000302$BurstDirectCall.Pointer == 0)
				{
					BinaryHeap.Remove_00000302$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BinaryHeap.Remove_00000302$BurstDirectCall.DeferredCompilation, methodof(BinaryHeap.Remove$BurstManaged(UnsafeSpan<PathNode>*, BinaryHeap*, uint*, uint*)).MethodHandle, typeof(BinaryHeap.Remove_00000302$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = BinaryHeap.Remove_00000302$BurstDirectCall.Pointer;
			}

			// Token: 0x0600033C RID: 828 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				BinaryHeap.Remove_00000302$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x0600033D RID: 829 RVA: 0x0000FCCC File Offset: 0x0000DECC
			public unsafe static void Constructor()
			{
				BinaryHeap.Remove_00000302$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BinaryHeap.Remove(UnsafeSpan<PathNode>*, BinaryHeap*, uint*, uint*)).MethodHandle);
			}

			// Token: 0x0600033E RID: 830 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x0600033F RID: 831 RVA: 0x0000FCDD File Offset: 0x0000DEDD
			// Note: this type is marked as 'beforefieldinit'.
			static Remove_00000302$BurstDirectCall()
			{
				BinaryHeap.Remove_00000302$BurstDirectCall.Constructor();
			}

			// Token: 0x06000340 RID: 832 RVA: 0x0000FCE4 File Offset: 0x0000DEE4
			public static uint Invoke(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap, [NoAlias] out uint removedTieBreaker, [NoAlias] out uint removedF)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = BinaryHeap.Remove_00000302$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.UInt32(Pathfinding.Collections.UnsafeSpan`1<Pathfinding.PathNode>&,Pathfinding.BinaryHeap&,System.UInt32&,System.UInt32&), ref nodes, ref binaryHeap, ref removedTieBreaker, ref removedF, functionPointer);
					}
				}
				return BinaryHeap.Remove$BurstManaged(ref nodes, ref binaryHeap, out removedTieBreaker, out removedF);
			}

			// Token: 0x040001FD RID: 509
			private static IntPtr Pointer;

			// Token: 0x040001FE RID: 510
			private static IntPtr DeferredCompilation;
		}
	}
}
