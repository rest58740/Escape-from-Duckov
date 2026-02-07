using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000107 RID: 263
	internal static class ArrayPoolUtil
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x0000D2B3 File Offset: 0x0000B4B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void EnsureCapacity<T>(ref T[] array, int index, ArrayPool<T> pool)
		{
			if (array.Length <= index)
			{
				ArrayPoolUtil.EnsureCapacityCore<T>(ref array, index, pool);
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void EnsureCapacityCore<T>(ref T[] array, int index, ArrayPool<T> pool)
		{
			if (array.Length <= index)
			{
				int num = array.Length * 2;
				T[] array2 = pool.Rent((index < num) ? num : (index * 2));
				Array.Copy(array, 0, array2, 0, array.Length);
				pool.Return(array, !RuntimeHelpersAbstraction.IsWellKnownNoReferenceContainsType<T>());
				array = array2;
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0000D314 File Offset: 0x0000B514
		public static ArrayPoolUtil.RentArray<T> Materialize<T>(IEnumerable<T> source)
		{
			T[] array = source as T[];
			if (array != null)
			{
				return new ArrayPoolUtil.RentArray<T>(array, array.Length, null);
			}
			int num = 32;
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				if (collection.Count == 0)
				{
					return new ArrayPoolUtil.RentArray<T>(Array.Empty<T>(), 0, null);
				}
				num = collection.Count;
				ArrayPool<T> shared = ArrayPool<T>.Shared;
				T[] array2 = shared.Rent(num);
				collection.CopyTo(array2, 0);
				return new ArrayPoolUtil.RentArray<T>(array2, collection.Count, shared);
			}
			else
			{
				IReadOnlyCollection<T> readOnlyCollection = source as IReadOnlyCollection<T>;
				if (readOnlyCollection != null)
				{
					num = readOnlyCollection.Count;
				}
				if (num == 0)
				{
					return new ArrayPoolUtil.RentArray<T>(Array.Empty<T>(), 0, null);
				}
				ArrayPool<T> shared2 = ArrayPool<T>.Shared;
				int num2 = 0;
				T[] array3 = shared2.Rent(num);
				foreach (T t in source)
				{
					ArrayPoolUtil.EnsureCapacity<T>(ref array3, num2, shared2);
					array3[num2++] = t;
				}
				return new ArrayPoolUtil.RentArray<T>(array3, num2, shared2);
			}
		}

		// Token: 0x02000209 RID: 521
		public struct RentArray<T> : IDisposable
		{
			// Token: 0x06000BC5 RID: 3013 RVA: 0x0002A98F File Offset: 0x00028B8F
			public RentArray(T[] array, int length, ArrayPool<T> pool)
			{
				this.Array = array;
				this.Length = length;
				this.pool = pool;
			}

			// Token: 0x06000BC6 RID: 3014 RVA: 0x0002A9A6 File Offset: 0x00028BA6
			public void Dispose()
			{
				this.DisposeManually(!RuntimeHelpersAbstraction.IsWellKnownNoReferenceContainsType<T>());
			}

			// Token: 0x06000BC7 RID: 3015 RVA: 0x0002A9B6 File Offset: 0x00028BB6
			public void DisposeManually(bool clearArray)
			{
				if (this.pool != null)
				{
					if (clearArray)
					{
						System.Array.Clear(this.Array, 0, this.Length);
					}
					this.pool.Return(this.Array, false);
					this.pool = null;
				}
			}

			// Token: 0x04000543 RID: 1347
			public readonly T[] Array;

			// Token: 0x04000544 RID: 1348
			public readonly int Length;

			// Token: 0x04000545 RID: 1349
			private ArrayPool<T> pool;
		}
	}
}
