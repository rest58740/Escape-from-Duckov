using System;
using System.Threading;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000C2 RID: 194
	public sealed class Cache<T> : ICache, IDisposable where T : class, new()
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00027114 File Offset: 0x00025314
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x0002711B File Offset: 0x0002531B
		public static int MaxCacheSize
		{
			get
			{
				return Cache<T>.maxCacheSize;
			}
			set
			{
				Cache<T>.maxCacheSize = Math.Max(1, value);
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00027129 File Offset: 0x00025329
		private Cache()
		{
			this.Value = Activator.CreateInstance<T>();
			this.isFree = false;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00027143 File Offset: 0x00025343
		public bool IsFree
		{
			get
			{
				return this.isFree;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0002714B File Offset: 0x0002534B
		object ICache.Value
		{
			get
			{
				return this.Value;
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00027158 File Offset: 0x00025358
		public static Cache<T> Claim()
		{
			Cache<T> cache = null;
			while (Interlocked.CompareExchange(ref Cache<T>.THREAD_LOCK_TOKEN, 1, 0) != 0)
			{
			}
			object[] freeValues = Cache<T>.FreeValues;
			int num = freeValues.Length;
			for (int i = 0; i < num; i++)
			{
				cache = (Cache<T>)freeValues[i];
				if (cache != null)
				{
					freeValues[i] = null;
					cache.isFree = false;
					break;
				}
			}
			Cache<T>.THREAD_LOCK_TOKEN = 0;
			if (cache == null)
			{
				cache = new Cache<T>();
			}
			if (Cache<T>.IsNotificationReceiver)
			{
				(cache.Value as ICacheNotificationReceiver).OnClaimed();
			}
			return cache;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x000271D4 File Offset: 0x000253D4
		public static void Release(Cache<T> cache)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (cache.isFree)
			{
				return;
			}
			if (Cache<T>.IsNotificationReceiver)
			{
				(cache.Value as ICacheNotificationReceiver).OnFreed();
			}
			while (Interlocked.CompareExchange(ref Cache<T>.THREAD_LOCK_TOKEN, 1, 0) != 0)
			{
			}
			if (cache.isFree)
			{
				Cache<T>.THREAD_LOCK_TOKEN = 0;
				return;
			}
			cache.isFree = true;
			object[] freeValues = Cache<T>.FreeValues;
			int num = freeValues.Length;
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (freeValues[i] == null)
				{
					freeValues[i] = cache;
					flag = true;
					break;
				}
			}
			if (!flag && num < Cache<T>.MaxCacheSize)
			{
				object[] array = new object[num * 2];
				for (int j = 0; j < num; j++)
				{
					array[j] = freeValues[j];
				}
				array[num] = cache;
				Cache<T>.FreeValues = array;
			}
			Cache<T>.THREAD_LOCK_TOKEN = 0;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x000272A4 File Offset: 0x000254A4
		public static implicit operator T(Cache<T> cache)
		{
			if (cache == null)
			{
				return default(T);
			}
			return cache.Value;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x000272C4 File Offset: 0x000254C4
		public void Release()
		{
			Cache<T>.Release(this);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000272C4 File Offset: 0x000254C4
		void IDisposable.Dispose()
		{
			Cache<T>.Release(this);
		}

		// Token: 0x04000207 RID: 519
		private static readonly bool IsNotificationReceiver = typeof(ICacheNotificationReceiver).IsAssignableFrom(typeof(T));

		// Token: 0x04000208 RID: 520
		private static object[] FreeValues = new object[4];

		// Token: 0x04000209 RID: 521
		private bool isFree;

		// Token: 0x0400020A RID: 522
		private static volatile int THREAD_LOCK_TOKEN = 0;

		// Token: 0x0400020B RID: 523
		private static int maxCacheSize = 5;

		// Token: 0x0400020C RID: 524
		public T Value;
	}
}
