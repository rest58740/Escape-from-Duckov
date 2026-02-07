using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x02000245 RID: 581
	public static class ListPool<T>
	{
		// Token: 0x06000DB6 RID: 3510 RVA: 0x0005680C File Offset: 0x00054A0C
		public static List<T> Claim()
		{
			List<List<T>> obj = ListPool<T>.pool;
			List<T> result;
			lock (obj)
			{
				if (ListPool<T>.pool.Count > 0)
				{
					List<T> list = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
					ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
					ListPool<T>.inPool.Remove(list);
					result = list;
				}
				else
				{
					result = new List<T>();
				}
			}
			return result;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00056898 File Offset: 0x00054A98
		private static int FindCandidate(List<List<T>> pool, int capacity)
		{
			List<T> list = null;
			int result = -1;
			int num = 0;
			while (num < pool.Count && num < 8)
			{
				List<T> list2 = pool[pool.Count - 1 - num];
				if ((list == null || list2.Capacity > list.Capacity) && list2.Capacity < capacity * 16)
				{
					list = list2;
					result = pool.Count - 1 - num;
					if (list.Capacity >= capacity)
					{
						return result;
					}
				}
				num++;
			}
			return result;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00056908 File Offset: 0x00054B08
		public static List<T> Claim(int capacity)
		{
			List<List<T>> obj = ListPool<T>.pool;
			List<T> result;
			lock (obj)
			{
				List<List<T>> list = ListPool<T>.pool;
				int num = ListPool<T>.FindCandidate(ListPool<T>.pool, capacity);
				if (capacity > 5000)
				{
					int num2 = ListPool<T>.FindCandidate(ListPool<T>.largePool, capacity);
					if (num2 != -1)
					{
						list = ListPool<T>.largePool;
						num = num2;
					}
				}
				if (num == -1)
				{
					result = new List<T>(capacity);
				}
				else
				{
					List<T> list2 = list[num];
					ListPool<T>.inPool.Remove(list2);
					list[num] = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
					result = list2;
				}
			}
			return result;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000569C4 File Offset: 0x00054BC4
		public static void Warmup(int count, int size)
		{
			List<List<T>> obj = ListPool<T>.pool;
			lock (obj)
			{
				List<T>[] array = new List<T>[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = ListPool<T>.Claim(size);
				}
				for (int j = 0; j < count; j++)
				{
					ListPool<T>.Release(array[j]);
				}
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00056A34 File Offset: 0x00054C34
		public static void Release(ref List<T> list)
		{
			ListPool<T>.Release(list);
			list = null;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00056A40 File Offset: 0x00054C40
		public static void Release(List<T> list)
		{
			list.ClearFast<T>();
			List<List<T>> obj = ListPool<T>.pool;
			lock (obj)
			{
				if (list.Capacity > 5000)
				{
					ListPool<T>.largePool.Add(list);
					if (ListPool<T>.largePool.Count > 8)
					{
						ListPool<T>.largePool.RemoveAt(0);
					}
				}
				else
				{
					ListPool<T>.pool.Add(list);
				}
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00056ABC File Offset: 0x00054CBC
		public static void Clear()
		{
			List<List<T>> obj = ListPool<T>.pool;
			lock (obj)
			{
				ListPool<T>.pool.Clear();
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00056B00 File Offset: 0x00054D00
		public static int GetSize()
		{
			return ListPool<T>.pool.Count;
		}

		// Token: 0x04000A8C RID: 2700
		private static readonly List<List<T>> pool = new List<List<T>>();

		// Token: 0x04000A8D RID: 2701
		private static readonly List<List<T>> largePool = new List<List<T>>();

		// Token: 0x04000A8E RID: 2702
		private static readonly HashSet<List<T>> inPool = new HashSet<List<T>>();

		// Token: 0x04000A8F RID: 2703
		private const int MaxCapacitySearchLength = 8;

		// Token: 0x04000A90 RID: 2704
		private const int LargeThreshold = 5000;

		// Token: 0x04000A91 RID: 2705
		private const int MaxLargePoolSize = 8;
	}
}
