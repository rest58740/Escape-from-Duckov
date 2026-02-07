using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;

namespace Pathfinding.Collections
{
	// Token: 0x02000265 RID: 613
	public readonly struct UnsafeSpan<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0005A585 File Offset: 0x00058785
		public int Length
		{
			get
			{
				return (int)this.length;
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0005A58D File Offset: 0x0005878D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe UnsafeSpan(void* ptr, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (length > 0 && ptr == null)
			{
				throw new ArgumentNullException();
			}
			this.ptr = (T*)ptr;
			this.length = (uint)length;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0005A5B6 File Offset: 0x000587B6
		public unsafe UnsafeSpan(T[] data, out ulong gcHandle)
		{
			this.ptr = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(data, out gcHandle);
			this.length = (uint)data.Length;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0005A5CE File Offset: 0x000587CE
		public unsafe UnsafeSpan(T[,] data, out ulong gcHandle)
		{
			this.ptr = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(data, out gcHandle);
			this.length = (uint)data.Length;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0005A5E9 File Offset: 0x000587E9
		public unsafe UnsafeSpan(Allocator allocator, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (length > 0)
			{
				this.ptr = (T*)UnsafeUtility.MallocTracked((long)length * (long)UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator, 1);
			}
			else
			{
				this.ptr = null;
			}
			this.length = (uint)length;
		}

		// Token: 0x17000204 RID: 516
		public unsafe T this[int index]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= (int)this.length)
				{
					throw new IndexOutOfRangeException();
				}
				Hint.Assume(this.ptr != null);
				return ref this.ptr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x17000205 RID: 517
		public unsafe T this[uint index]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= this.length)
				{
					throw new IndexOutOfRangeException();
				}
				Hint.Assume(this.ptr != null);
				Hint.Assume(this.ptr + (ulong)index * (ulong)((long)sizeof(T)) / (ulong)sizeof(T) != null);
				return ref this.ptr[(ulong)index * (ulong)((long)sizeof(T)) / (ulong)sizeof(T)];
			}
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0005A6B6 File Offset: 0x000588B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe UnsafeSpan<U> Reinterpret<[IsUnmanaged] U>() where U : struct, ValueType
		{
			if (sizeof(T) != sizeof(U))
			{
				throw new InvalidOperationException("Cannot reinterpret span because the size of the types do not match");
			}
			return new UnsafeSpan<U>((void*)this.ptr, (int)this.length);
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0005A6E2 File Offset: 0x000588E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe UnsafeSpan<U> Reinterpret<[IsUnmanaged] U>(int expectedOriginalTypeSize) where U : struct, ValueType
		{
			return new UnsafeSpan<U>((void*)this.ptr, (int)(this.length * (uint)sizeof(T) / (uint)sizeof(U)));
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0005A703 File Offset: 0x00058903
		public unsafe UnsafeSpan<T> Slice(int start, int length)
		{
			if (start < 0 || length < 0 || (long)(start + length) > (long)((ulong)this.length))
			{
				throw new ArgumentOutOfRangeException();
			}
			return new UnsafeSpan<T>((void*)(this.ptr + (IntPtr)start * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), length);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0005A736 File Offset: 0x00058936
		public UnsafeSpan<T> Slice(int start)
		{
			return this.Slice(start, (int)(this.length - (uint)start));
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0005A748 File Offset: 0x00058948
		public unsafe void Move(int startIndex, int toIndex, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (startIndex < 0 || (long)(startIndex + count) > (long)((ulong)this.length))
			{
				throw new ArgumentOutOfRangeException();
			}
			if (toIndex < 0 || (long)(toIndex + count) > (long)((ulong)this.length))
			{
				throw new ArgumentOutOfRangeException();
			}
			if (count == 0)
			{
				return;
			}
			UnsafeUtility.MemMove((void*)(this.ptr + (IntPtr)toIndex * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)(this.ptr + (IntPtr)startIndex * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)sizeof(T) * (long)count);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0005A7C0 File Offset: 0x000589C0
		public static void RemoveAt(ref UnsafeSpan<T> span, int index)
		{
			if (index < 0 || (long)index >= (long)((ulong)span.length))
			{
				throw new ArgumentOutOfRangeException();
			}
			span.Move(index + 1, index, (int)(span.length - (uint)index - 1U));
			span = span.Slice(0, (int)(span.length - 1U));
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0005A800 File Offset: 0x00058A00
		public unsafe void CopyTo(UnsafeSpan<T> other)
		{
			if (other.length < this.length)
			{
				throw new ArgumentException();
			}
			if (this.length > 0U)
			{
				UnsafeUtility.MemCpy((void*)other.ptr, (void*)this.ptr, (long)sizeof(T) * (long)((ulong)this.length));
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0005A840 File Offset: 0x00058A40
		public unsafe void CopyTo(List<T> buffer)
		{
			if (buffer.Capacity < buffer.Count + this.Length)
			{
				buffer.Capacity = buffer.Count + this.Length;
			}
			for (int i = 0; i < this.Length; i++)
			{
				buffer.Add(*this[i]);
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0005A898 File Offset: 0x00058A98
		public UnsafeSpan<T> Clone(Allocator allocator)
		{
			UnsafeSpan<T> unsafeSpan = new UnsafeSpan<T>(allocator, (int)this.length);
			this.CopyTo(unsafeSpan);
			return unsafeSpan;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0005A8BC File Offset: 0x00058ABC
		public unsafe T[] ToArray()
		{
			T[] array = new T[this.length];
			if (this.length > 0U)
			{
				T[] array2;
				T* destination;
				if ((array2 = array) == null || array2.Length == 0)
				{
					destination = null;
				}
				else
				{
					destination = &array2[0];
				}
				UnsafeUtility.MemCpy((void*)destination, (void*)this.ptr, (long)sizeof(T) * (long)((ulong)this.length));
				array2 = null;
			}
			return array;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0005A914 File Offset: 0x00058B14
		public unsafe NativeArray<T> MoveToNativeArray(Allocator allocator)
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.ptr, this.Length, allocator);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0005A928 File Offset: 0x00058B28
		public unsafe void Free(Allocator allocator)
		{
			if (this.length > 0U)
			{
				UnsafeUtility.FreeTracked((void*)this.ptr, allocator);
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0005A940 File Offset: 0x00058B40
		public UnsafeSpan<T> Reallocate(Allocator allocator, int newSize)
		{
			UnsafeSpan<T> unsafeSpan = new UnsafeSpan<T>(allocator, newSize);
			this.Slice(0, Math.Min(newSize, this.Length)).CopyTo(unsafeSpan);
			this.Free(allocator);
			return unsafeSpan;
		}

		// Token: 0x04000B05 RID: 2821
		[NativeDisableUnsafePtrRestriction]
		internal unsafe readonly T* ptr;

		// Token: 0x04000B06 RID: 2822
		internal readonly uint length;
	}
}
