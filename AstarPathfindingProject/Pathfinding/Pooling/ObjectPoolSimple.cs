using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x02000248 RID: 584
	public static class ObjectPoolSimple<T> where T : class, new()
	{
		// Token: 0x06000DC2 RID: 3522 RVA: 0x00056B48 File Offset: 0x00054D48
		public static T Claim()
		{
			List<T> obj = ObjectPoolSimple<T>.pool;
			T result;
			lock (obj)
			{
				if (ObjectPoolSimple<T>.pool.Count > 0)
				{
					T t = ObjectPoolSimple<T>.pool[ObjectPoolSimple<T>.pool.Count - 1];
					ObjectPoolSimple<T>.pool.RemoveAt(ObjectPoolSimple<T>.pool.Count - 1);
					ObjectPoolSimple<T>.inPool.Remove(t);
					result = t;
				}
				else
				{
					result = Activator.CreateInstance<T>();
				}
			}
			return result;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00056BD4 File Offset: 0x00054DD4
		public static void Release(ref T obj)
		{
			List<T> obj2 = ObjectPoolSimple<T>.pool;
			lock (obj2)
			{
				ObjectPoolSimple<T>.pool.Add(obj);
			}
			obj = default(T);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00056C24 File Offset: 0x00054E24
		public static void Clear()
		{
			List<T> obj = ObjectPoolSimple<T>.pool;
			lock (obj)
			{
				ObjectPoolSimple<T>.pool.Clear();
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00056C68 File Offset: 0x00054E68
		public static int GetSize()
		{
			return ObjectPoolSimple<T>.pool.Count;
		}

		// Token: 0x04000A92 RID: 2706
		private static List<T> pool = new List<T>();

		// Token: 0x04000A93 RID: 2707
		private static readonly HashSet<T> inPool = new HashSet<T>();
	}
}
