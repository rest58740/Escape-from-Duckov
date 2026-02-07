using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x02000244 RID: 580
	public static class ListExtensions
	{
		// Token: 0x06000DB4 RID: 3508 RVA: 0x000567B0 File Offset: 0x000549B0
		public static T[] ToArrayFromPool<T>(this List<T> list)
		{
			T[] array = ArrayPool<T>.ClaimWithExactLength(list.Count);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = list[i];
			}
			return array;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x000567E6 File Offset: 0x000549E6
		public static void ClearFast<T>(this List<T> list)
		{
			if (list.Count * 2 < list.Capacity)
			{
				list.RemoveRange(0, list.Count);
				return;
			}
			list.Clear();
		}
	}
}
