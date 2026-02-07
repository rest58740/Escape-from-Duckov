using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x0200005E RID: 94
	internal static class ListPool<T>
	{
		// Token: 0x06000C97 RID: 3223 RVA: 0x0001938E File Offset: 0x0001758E
		public static List<T> Alloc()
		{
			if (ListPool<T>.pool.Count != 0)
			{
				return ListPool<T>.pool.Pop();
			}
			return new List<T>();
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x000193AC File Offset: 0x000175AC
		public static void Free(List<T> list)
		{
			list.Clear();
			ListPool<T>.pool.Push(list);
		}

		// Token: 0x04000206 RID: 518
		private static readonly Stack<List<T>> pool = new Stack<List<T>>();
	}
}
