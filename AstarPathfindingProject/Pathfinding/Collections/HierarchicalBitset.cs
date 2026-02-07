using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Pathfinding.Collections
{
	// Token: 0x02000257 RID: 599
	[BurstCompile]
	public struct HierarchicalBitset
	{
		// Token: 0x06000E25 RID: 3621 RVA: 0x000591B0 File Offset: 0x000573B0
		public HierarchicalBitset(int size, Allocator allocator)
		{
			this.allocator = allocator;
			this.l1 = new UnsafeSpan<ulong>(allocator, size + 64 - 1 >> 6);
			this.l2 = new UnsafeSpan<ulong>(allocator, size + 4095 >> 6 >> 6);
			this.l3 = new UnsafeSpan<ulong>(allocator, size + 262143 >> 6 >> 6 >> 6);
			this.l1.FillZeros<ulong>();
			this.l2.FillZeros<ulong>();
			this.l3.FillZeros<ulong>();
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00059229 File Offset: 0x00057429
		public bool IsCreated
		{
			get
			{
				return this.Capacity > 0;
			}
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00059234 File Offset: 0x00057434
		public void Dispose()
		{
			this.l1.Free(this.allocator);
			this.l2.Free(this.allocator);
			this.l3.Free(this.allocator);
			this = default(HierarchicalBitset);
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x00059270 File Offset: 0x00057470
		// (set) Token: 0x06000E29 RID: 3625 RVA: 0x00059280 File Offset: 0x00057480
		public int Capacity
		{
			get
			{
				return this.l1.Length << 6;
			}
			set
			{
				if (value < this.Capacity)
				{
					throw new ArgumentException("Shrinking the bitset is not supported");
				}
				if (value == this.Capacity)
				{
					return;
				}
				HierarchicalBitset hierarchicalBitset = new HierarchicalBitset(value, this.allocator);
				this.l1.CopyTo(hierarchicalBitset.l1);
				this.l2.CopyTo(hierarchicalBitset.l2);
				this.l3.CopyTo(hierarchicalBitset.l3);
				this.Dispose();
				this = hierarchicalBitset;
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x000592FC File Offset: 0x000574FC
		public unsafe int Count()
		{
			int num = 0;
			for (int i = 0; i < this.l1.Length; i++)
			{
				num += math.countbits(*this.l1[i]);
			}
			return num;
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x00059338 File Offset: 0x00057538
		public unsafe bool IsEmpty
		{
			get
			{
				for (int i = 0; i < this.l3.Length; i++)
				{
					if (*this.l3[i] != 0UL)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0005936D File Offset: 0x0005756D
		public void Clear()
		{
			this.l1.FillZeros<ulong>();
			this.l2.FillZeros<ulong>();
			this.l3.FillZeros<ulong>();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00059390 File Offset: 0x00057590
		public void GetIndices(NativeList<int> result)
		{
			NativeArray<int> arr = new NativeArray<int>(256, Allocator.Temp, NativeArrayOptions.ClearMemory);
			HierarchicalBitset.Iterator iterator = this.GetIterator(arr.AsUnsafeSpan<int>());
			while (iterator.MoveNext())
			{
				UnsafeSpan<int> unsafeSpan = iterator.Current;
				for (int i = 0; i < unsafeSpan.Length; i++)
				{
					result.Add(unsafeSpan[i]);
				}
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000593EC File Offset: 0x000575EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool SetAtomic(ref UnsafeSpan<ulong> span, int index)
		{
			int index2 = index >> 6;
			ulong num = *span[index2];
			if ((num & 1UL << index) != 0UL)
			{
				return true;
			}
			for (;;)
			{
				ulong num2 = (ulong)Interlocked.CompareExchange(UnsafeUtility.As<ulong, long>(span[index2]), (long)(num | 1UL << index), (long)num);
				if (num2 == num)
				{
					break;
				}
				num = num2;
			}
			return false;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00059438 File Offset: 0x00057638
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool ResetAtomic(ref UnsafeSpan<ulong> span, int index)
		{
			int index2 = index >> 6;
			ulong num = *span[index2];
			if ((num & 1UL << index) == 0UL)
			{
				return true;
			}
			for (;;)
			{
				ulong num2 = (ulong)Interlocked.CompareExchange(UnsafeUtility.As<ulong, long>(span[index2]), (long)(num & ~(1UL << index)), (long)num);
				if (num2 == num)
				{
					break;
				}
				num = num2;
			}
			return false;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00059485 File Offset: 0x00057685
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool Get(int index)
		{
			return (*this.l1[index >> 6] & 1UL << index) > 0UL;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x000594A2 File Offset: 0x000576A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(int index)
		{
			if (HierarchicalBitset.SetAtomic(ref this.l1, index))
			{
				return;
			}
			HierarchicalBitset.SetAtomic(ref this.l2, index >> 6);
			HierarchicalBitset.SetAtomic(ref this.l3, index >> 12);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000594D4 File Offset: 0x000576D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Reset(int index)
		{
			if (HierarchicalBitset.ResetAtomic(ref this.l1, index))
			{
				return;
			}
			if (*this.l1[index >> 6] == 0UL)
			{
				HierarchicalBitset.ResetAtomic(ref this.l2, index >> 6);
			}
			if (*this.l2[index >> 12] == 0UL)
			{
				HierarchicalBitset.ResetAtomic(ref this.l3, index >> 12);
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00059532 File Offset: 0x00057732
		public HierarchicalBitset.Iterator GetIterator(UnsafeSpan<int> scratchBuffer)
		{
			return new HierarchicalBitset.Iterator(this, scratchBuffer);
		}

		// Token: 0x04000ADB RID: 2779
		private UnsafeSpan<ulong> l1;

		// Token: 0x04000ADC RID: 2780
		private UnsafeSpan<ulong> l2;

		// Token: 0x04000ADD RID: 2781
		private UnsafeSpan<ulong> l3;

		// Token: 0x04000ADE RID: 2782
		private Allocator allocator;

		// Token: 0x04000ADF RID: 2783
		private const int Log64 = 6;

		// Token: 0x02000258 RID: 600
		[BurstCompile]
		public struct Iterator : IEnumerator<UnsafeSpan<int>>, IEnumerator, IDisposable, IEnumerable<UnsafeSpan<int>>, IEnumerable
		{
			// Token: 0x170001F0 RID: 496
			// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00059540 File Offset: 0x00057740
			public UnsafeSpan<int> Current
			{
				get
				{
					return this.result.Slice(0, this.resultCount);
				}
			}

			// Token: 0x170001F1 RID: 497
			// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0002298A File Offset: 0x00020B8A
			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x06000E36 RID: 3638 RVA: 0x0002298A File Offset: 0x00020B8A
			public void Reset()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000E37 RID: 3639 RVA: 0x000035CE File Offset: 0x000017CE
			public void Dispose()
			{
			}

			// Token: 0x06000E38 RID: 3640 RVA: 0x00059554 File Offset: 0x00057754
			public IEnumerator<UnsafeSpan<int>> GetEnumerator()
			{
				return this;
			}

			// Token: 0x06000E39 RID: 3641 RVA: 0x0002298A File Offset: 0x00020B8A
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000E3A RID: 3642 RVA: 0x00059561 File Offset: 0x00057761
			private static int l2index(int l3index, int l3bitIndex)
			{
				return (l3index << 6) + l3bitIndex;
			}

			// Token: 0x06000E3B RID: 3643 RVA: 0x00059561 File Offset: 0x00057761
			private static int l1index(int l2index, int l2bitIndex)
			{
				return (l2index << 6) + l2bitIndex;
			}

			// Token: 0x06000E3C RID: 3644 RVA: 0x00059568 File Offset: 0x00057768
			public Iterator(HierarchicalBitset bitSet, UnsafeSpan<int> result)
			{
				this.bitSet = bitSet;
				this.result = result;
				this.resultCount = 0;
				this.l3index = 0;
				this.l3bitIndex = 0;
				this.l2bitIndex = 0;
				if (result.Length < 128)
				{
					throw new ArgumentException("Result array must be at least 128 elements long");
				}
			}

			// Token: 0x06000E3D RID: 3645 RVA: 0x000595B8 File Offset: 0x000577B8
			public bool MoveNext()
			{
				return HierarchicalBitset.Iterator.MoveNextBurst(ref this);
			}

			// Token: 0x06000E3E RID: 3646 RVA: 0x000595C0 File Offset: 0x000577C0
			[BurstCompile]
			public static bool MoveNextBurst(ref HierarchicalBitset.Iterator iter)
			{
				return HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.Invoke(ref iter);
			}

			// Token: 0x06000E3F RID: 3647 RVA: 0x000595C8 File Offset: 0x000577C8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private unsafe bool MoveNextInternal()
			{
				uint num = 0U;
				int num2 = this.l3index;
				int num3 = this.l3bitIndex;
				int num4 = this.l2bitIndex;
				while ((long)num2 < (long)((ulong)this.bitSet.l3.length))
				{
					ulong num5 = *this.bitSet.l3[num2] & ulong.MaxValue << num3;
					if (num5 != 0UL)
					{
						while (num5 != 0UL)
						{
							num3 = math.tzcnt(num5);
							int num6 = HierarchicalBitset.Iterator.l2index(num2, num3);
							for (ulong num7 = *this.bitSet.l2[num6] & ulong.MaxValue << num4; num7 != 0UL; num7 &= num7 - 1UL)
							{
								num4 = math.tzcnt(num7);
								if ((ulong)(num + 64U) > (ulong)((long)this.result.Length))
								{
									this.resultCount = (int)num;
									this.l3index = num2;
									this.l3bitIndex = num3;
									this.l2bitIndex = num4;
									return true;
								}
								int num8 = HierarchicalBitset.Iterator.l1index(num6, num4);
								ulong num9 = *this.bitSet.l1[num8];
								int num10 = num8 << 6;
								while (num9 != 0UL)
								{
									int num11 = math.tzcnt(num9);
									num9 &= num9 - 1UL;
									int num12 = num10 + num11;
									Hint.Assume(num < (uint)this.result.Length);
									*this.result[num++] = num12;
								}
							}
							num5 &= num5 - 1UL;
							num4 = 0;
						}
						num4 = 0;
						num3 = 0;
					}
					num2++;
				}
				this.resultCount = (int)num;
				this.l3index = num2;
				this.l3bitIndex = num3;
				this.l2bitIndex = num4;
				return num > 0U;
			}

			// Token: 0x06000E40 RID: 3648 RVA: 0x00059754 File Offset: 0x00057954
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool MoveNextBurst$BurstManaged(ref HierarchicalBitset.Iterator iter)
			{
				return iter.MoveNextInternal();
			}

			// Token: 0x04000AE0 RID: 2784
			private HierarchicalBitset bitSet;

			// Token: 0x04000AE1 RID: 2785
			private UnsafeSpan<int> result;

			// Token: 0x04000AE2 RID: 2786
			private int resultCount;

			// Token: 0x04000AE3 RID: 2787
			private int l3index;

			// Token: 0x04000AE4 RID: 2788
			private int l3bitIndex;

			// Token: 0x04000AE5 RID: 2789
			private int l2bitIndex;

			// Token: 0x02000259 RID: 601
			// (Invoke) Token: 0x06000E42 RID: 3650
			internal delegate bool MoveNextBurst_00000D4C$PostfixBurstDelegate(ref HierarchicalBitset.Iterator iter);

			// Token: 0x0200025A RID: 602
			internal static class MoveNextBurst_00000D4C$BurstDirectCall
			{
				// Token: 0x06000E45 RID: 3653 RVA: 0x0005975C File Offset: 0x0005795C
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.Pointer == 0)
					{
						HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.DeferredCompilation, methodof(HierarchicalBitset.Iterator.MoveNextBurst$BurstManaged(HierarchicalBitset.Iterator*)).MethodHandle, typeof(HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.Pointer;
				}

				// Token: 0x06000E46 RID: 3654 RVA: 0x00059788 File Offset: 0x00057988
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x06000E47 RID: 3655 RVA: 0x000597A0 File Offset: 0x000579A0
				public unsafe static void Constructor()
				{
					HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(HierarchicalBitset.Iterator.MoveNextBurst(HierarchicalBitset.Iterator*)).MethodHandle);
				}

				// Token: 0x06000E48 RID: 3656 RVA: 0x000035CE File Offset: 0x000017CE
				public static void Initialize()
				{
				}

				// Token: 0x06000E49 RID: 3657 RVA: 0x000597B1 File Offset: 0x000579B1
				// Note: this type is marked as 'beforefieldinit'.
				static MoveNextBurst_00000D4C$BurstDirectCall()
				{
					HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.Constructor();
				}

				// Token: 0x06000E4A RID: 3658 RVA: 0x000597B8 File Offset: 0x000579B8
				public static bool Invoke(ref HierarchicalBitset.Iterator iter)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Boolean(Pathfinding.Collections.HierarchicalBitset/Iterator&), ref iter, functionPointer);
						}
					}
					return HierarchicalBitset.Iterator.MoveNextBurst$BurstManaged(ref iter);
				}

				// Token: 0x04000AE6 RID: 2790
				private static IntPtr Pointer;

				// Token: 0x04000AE7 RID: 2791
				private static IntPtr DeferredCompilation;
			}
		}
	}
}
