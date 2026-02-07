using System;
using System.Runtime.CompilerServices;
using Pathfinding.Graphs.Grid;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Jobs
{
	// Token: 0x0200016F RID: 367
	internal static class NativeArrayExtensions
	{
		// Token: 0x06000ABE RID: 2750 RVA: 0x0003CE34 File Offset: 0x0003B034
		public static JobMemSet<T> MemSet<[IsUnmanaged] T>(this NativeArray<T> self, T value) where T : struct, ValueType
		{
			return new JobMemSet<T>
			{
				data = self,
				value = value
			};
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0003CE5C File Offset: 0x0003B05C
		public static JobAND BitwiseAndWith(this NativeArray<bool> self, NativeArray<bool> other)
		{
			return new JobAND
			{
				result = self,
				data = other
			};
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0003CE84 File Offset: 0x0003B084
		public static JobCopy<T> CopyToJob<T>(this NativeArray<T> from, NativeArray<T> to) where T : struct
		{
			return new JobCopy<T>
			{
				from = from,
				to = to
			};
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0003CEAC File Offset: 0x0003B0AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SliceActionJob<T> WithSlice<T>(this T action, Slice3D slice) where T : struct, GridIterationUtilities.ISliceAction
		{
			return new SliceActionJob<T>
			{
				action = action,
				slice = slice
			};
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0003CED4 File Offset: 0x0003B0D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IndexActionJob<T> WithLength<T>(this T action, int length) where T : struct, GridIterationUtilities.ISliceAction
		{
			return new IndexActionJob<T>
			{
				action = action,
				length = length
			};
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0003CEFC File Offset: 0x0003B0FC
		public static JobRotate3DArray<T> Rotate3D<[IsUnmanaged] T>(this NativeArray<T> arr, int3 size, int dx, int dz) where T : struct, ValueType
		{
			return new JobRotate3DArray<T>
			{
				arr = arr,
				size = size,
				dx = dx,
				dz = dz
			};
		}
	}
}
