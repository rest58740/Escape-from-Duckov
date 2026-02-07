using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200000C RID: 12
	public static class ObjectPool<T> where T : class, new()
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000033A3 File Offset: 0x000015A3
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x000033B0 File Offset: 0x000015B0
		public static int Count
		{
			get
			{
				return ObjectPool<T>.Items.Count;
			}
			set
			{
				int num = ObjectPool<T>.Items.Count;
				if (num < value)
				{
					if (ObjectPool<T>.Items.Capacity < value)
					{
						ObjectPool<T>.Items.Capacity = Mathf.NextPowerOfTwo(value);
					}
					do
					{
						ObjectPool<T>.Items.Add(Activator.CreateInstance<T>());
						num++;
					}
					while (num < value);
					return;
				}
				if (num > value)
				{
					ObjectPool<T>.Items.RemoveRange(value, num - value);
				}
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00003413 File Offset: 0x00001613
		public static void IncreaseCountTo(int count)
		{
			if (ObjectPool<T>.Count < count)
			{
				ObjectPool<T>.Count = count;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00003423 File Offset: 0x00001623
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x0000342F File Offset: 0x0000162F
		public static int Capacity
		{
			get
			{
				return ObjectPool<T>.Items.Capacity;
			}
			set
			{
				if (ObjectPool<T>.Items.Count > value)
				{
					ObjectPool<T>.Items.RemoveRange(value, ObjectPool<T>.Items.Count - value);
				}
				ObjectPool<T>.Items.Capacity = value;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00003460 File Offset: 0x00001660
		public static void IncreaseCapacityTo(int capacity)
		{
			if (ObjectPool<T>.Capacity < capacity)
			{
				ObjectPool<T>.Capacity = capacity;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00003470 File Offset: 0x00001670
		public static T Acquire()
		{
			int num = ObjectPool<T>.Items.Count;
			if (num == 0)
			{
				return Activator.CreateInstance<T>();
			}
			num--;
			T result = ObjectPool<T>.Items[num];
			ObjectPool<T>.Items.RemoveAt(num);
			return result;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000034AB File Offset: 0x000016AB
		public static void Release(T item)
		{
			ObjectPool<T>.Items.Add(item);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000034B8 File Offset: 0x000016B8
		public static string GetDetails()
		{
			return typeof(T).Name + string.Format(" ({0} = {1}", "Count", ObjectPool<T>.Items.Count) + string.Format(", {0} = {1}", "Capacity", ObjectPool<T>.Items.Capacity) + ")";
		}

		// Token: 0x04000011 RID: 17
		private static readonly List<T> Items = new List<T>();

		// Token: 0x0200007D RID: 125
		public readonly struct Disposable : IDisposable
		{
			// Token: 0x060005BB RID: 1467 RVA: 0x0000F3B4 File Offset: 0x0000D5B4
			public Disposable(out T item, Action<T> onRelease = null)
			{
				this.Item = (item = ObjectPool<T>.Acquire());
				this.OnRelease = onRelease;
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x0000F3DC File Offset: 0x0000D5DC
			void IDisposable.Dispose()
			{
				Action<T> onRelease = this.OnRelease;
				if (onRelease != null)
				{
					onRelease(this.Item);
				}
				ObjectPool<T>.Release(this.Item);
			}

			// Token: 0x0400010D RID: 269
			public readonly T Item;

			// Token: 0x0400010E RID: 270
			public readonly Action<T> OnRelease;
		}
	}
}
