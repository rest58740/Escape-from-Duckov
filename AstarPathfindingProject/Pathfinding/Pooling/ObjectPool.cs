using System;

namespace Pathfinding.Pooling
{
	// Token: 0x02000247 RID: 583
	public static class ObjectPool<T> where T : class, IAstarPooledObject, new()
	{
		// Token: 0x06000DC0 RID: 3520 RVA: 0x00056B2C File Offset: 0x00054D2C
		public static T Claim()
		{
			return ObjectPoolSimple<T>.Claim();
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00056B33 File Offset: 0x00054D33
		public static void Release(ref T obj)
		{
			obj.OnEnterPool();
			ObjectPoolSimple<T>.Release(ref obj);
		}
	}
}
