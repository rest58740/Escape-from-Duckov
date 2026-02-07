using System;
using System.Collections;
using System.Collections.Generic;

namespace LeTai.TrueShadow
{
	// Token: 0x0200001A RID: 26
	internal class IndexedSet<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x06000106 RID: 262 RVA: 0x000064E3 File Offset: 0x000046E3
		public void Add(T item)
		{
			this.dict.Add(item, this.list.Count);
			this.list.Add(item);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006508 File Offset: 0x00004708
		public bool AddUnique(T item)
		{
			if (this.dict.ContainsKey(item))
			{
				return false;
			}
			this.dict.Add(item, this.list.Count);
			this.list.Add(item);
			return true;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006540 File Offset: 0x00004740
		public bool Remove(T item)
		{
			int index;
			if (!this.dict.TryGetValue(item, out index))
			{
				return false;
			}
			this.RemoveAt(index);
			return true;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006568 File Offset: 0x00004768
		public void Remove(Predicate<T> match)
		{
			int i = 0;
			while (i < this.list.Count)
			{
				T t = this.list[i];
				if (match(t))
				{
					this.Remove(t);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000065AB File Offset: 0x000047AB
		public void Clear()
		{
			this.list.Clear();
			this.dict.Clear();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000065C3 File Offset: 0x000047C3
		public bool Contains(T item)
		{
			return this.dict.ContainsKey(item);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000065D1 File Offset: 0x000047D1
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000065E0 File Offset: 0x000047E0
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000065ED File Offset: 0x000047ED
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000065F0 File Offset: 0x000047F0
		public int IndexOf(T item)
		{
			int result;
			if (this.dict.TryGetValue(item, out result))
			{
				return result;
			}
			return -1;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006610 File Offset: 0x00004810
		public void Insert(int index, T item)
		{
			throw new NotSupportedException("Random Insertion is semantically invalid, since this structure does not guarantee ordering.");
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000661C File Offset: 0x0000481C
		public void RemoveAt(int index)
		{
			T key = this.list[index];
			this.dict.Remove(key);
			if (index == this.list.Count - 1)
			{
				this.list.RemoveAt(index);
				return;
			}
			int index2 = this.list.Count - 1;
			T t = this.list[index2];
			this.list[index] = t;
			this.dict[t] = index;
			this.list.RemoveAt(index2);
		}

		// Token: 0x17000032 RID: 50
		public T this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				T key = this.list[index];
				this.dict.Remove(key);
				this.list[index] = value;
				this.dict.Add(key, index);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000066F4 File Offset: 0x000048F4
		public void Sort(Comparison<T> sortLayoutFunction)
		{
			this.list.Sort(sortLayoutFunction);
			for (int i = 0; i < this.list.Count; i++)
			{
				T key = this.list[i];
				this.dict[key] = i;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000673D File Offset: 0x0000493D
		public IEnumerator<T> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000674F File Offset: 0x0000494F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040000AF RID: 175
		private readonly List<T> list = new List<T>();

		// Token: 0x040000B0 RID: 176
		private readonly Dictionary<T, int> dict = new Dictionary<T, int>();
	}
}
