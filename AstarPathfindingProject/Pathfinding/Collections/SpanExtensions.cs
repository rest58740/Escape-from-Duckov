using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Pathfinding.Collections
{
	// Token: 0x02000266 RID: 614
	public static class SpanExtensions
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x0005A97A File Offset: 0x00058B7A
		public unsafe static void FillZeros<[IsUnmanaged] T>(this UnsafeSpan<T> span) where T : struct, ValueType
		{
			if (span.length > 0U)
			{
				UnsafeUtility.MemSet((void*)span.ptr, 0, (long)sizeof(T) * (long)((ulong)span.length));
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0005A9A0 File Offset: 0x00058BA0
		public unsafe static void Fill<[IsUnmanaged] T>(this UnsafeSpan<T> span, T value) where T : struct, ValueType
		{
			if (span.length > 0U)
			{
				if ((long)sizeof(T) * (long)((ulong)span.length) > 2147483647L)
				{
					throw new ArgumentException("Span is too large to fill");
				}
				UnsafeUtility.MemCpyReplicate((void*)span.ptr, (void*)(&value), sizeof(T), (int)span.length);
			}
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0005A9F4 File Offset: 0x00058BF4
		public static void CopyFrom<[IsUnmanaged] T>(this UnsafeSpan<T> span, NativeArray<T> array) where T : struct, ValueType
		{
			array.AsUnsafeReadOnlySpan<T>().CopyTo(span);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0005AA10 File Offset: 0x00058C10
		public static void CopyFrom<[IsUnmanaged] T>(this UnsafeSpan<T> span, UnsafeSpan<T> other) where T : struct, ValueType
		{
			other.CopyTo(span);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0005AA1C File Offset: 0x00058C1C
		public unsafe static void CopyFrom<[IsUnmanaged] T>(this UnsafeSpan<T> span, T[] array) where T : struct, ValueType
		{
			if (array.Length > span.Length)
			{
				throw new InvalidOperationException();
			}
			if (array.Length == 0)
			{
				return;
			}
			ulong gcHandle;
			void* source = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out gcHandle);
			UnsafeUtility.MemCpy((void*)span.ptr, source, (long)sizeof(T) * (long)array.Length);
			UnsafeUtility.ReleaseGCObject(gcHandle);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0005AA68 File Offset: 0x00058C68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this UnsafeAppendBuffer buffer) where T : struct, ValueType
		{
			int num = buffer.Length / UnsafeUtility.SizeOf<T>();
			if (num * UnsafeUtility.SizeOf<T>() != buffer.Length)
			{
				throw new ArgumentException("Buffer length is not a multiple of the element size");
			}
			return new UnsafeSpan<T>((void*)buffer.Ptr, num);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0005AAA8 File Offset: 0x00058CA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType
		{
			return new UnsafeSpan<T>((void*)list.GetUnsafePtr<T>(), list.Length);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0005AABC File Offset: 0x00058CBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this NativeArray<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(arr.GetUnsafePtr<T>(), arr.Length);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0005AAD0 File Offset: 0x00058CD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnsafeSpan<T> AsUnsafeSpanNoChecks<[IsUnmanaged] T>(this NativeArray<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(arr), arr.Length);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0005AAE4 File Offset: 0x00058CE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnsafeSpan<T> AsUnsafeReadOnlySpan<[IsUnmanaged] T>(this NativeArray<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(arr.GetUnsafeReadOnlyPtr<T>(), arr.Length);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0005AAF8 File Offset: 0x00058CF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this UnsafeList<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>((void*)arr.Ptr, arr.Length);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0005AB0C File Offset: 0x00058D0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this NativeSlice<T> slice) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(slice.GetUnsafePtr<T>(), slice.Length);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0005AB20 File Offset: 0x00058D20
		public static bool Contains<[IsUnmanaged] T>(this UnsafeSpan<T> span, T value) where T : struct, ValueType, IEquatable<T>
		{
			return span.IndexOf(value) != -1;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0005AB2F File Offset: 0x00058D2F
		public unsafe static int IndexOf<[IsUnmanaged] T>(this UnsafeSpan<T> span, T value) where T : struct, ValueType, IEquatable<T>
		{
			return new ReadOnlySpan<T>((void*)span.ptr, (int)span.length).IndexOf(value);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0005AB48 File Offset: 0x00058D48
		public static void Sort<[IsUnmanaged] T>(this UnsafeSpan<T> span) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.Sort<T>(span.ptr, span.Length);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0005AB5C File Offset: 0x00058D5C
		public static void Sort<[IsUnmanaged] T, U>(this UnsafeSpan<T> span, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			NativeSortExtension.Sort<T, U>(span.ptr, span.Length, comp);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0005AB74 File Offset: 0x00058D74
		public static void InsertRange<[IsUnmanaged] T>(this NativeList<T> list, int index, int count) where T : struct, ValueType
		{
			list.ResizeUninitialized(list.Length + count);
			list.AsUnsafeSpan<T>().Move(index, index + count, list.Length - (index + count));
		}
	}
}
