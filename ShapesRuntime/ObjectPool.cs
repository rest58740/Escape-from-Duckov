using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x0200005D RID: 93
	internal static class ObjectPool<T> where T : new()
	{
		// Token: 0x06000C94 RID: 3220 RVA: 0x00019357 File Offset: 0x00017557
		public static T Alloc()
		{
			if (ObjectPool<T>.pool.Count != 0)
			{
				return ObjectPool<T>.pool.Pop();
			}
			return Activator.CreateInstance<T>();
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00019375 File Offset: 0x00017575
		public static void Free(T obj)
		{
			ObjectPool<T>.pool.Push(obj);
		}

		// Token: 0x04000205 RID: 517
		private static readonly Stack<T> pool = new Stack<T>();
	}
}
