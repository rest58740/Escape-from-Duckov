using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200002D RID: 45
	[Serializable]
	public class FastList<T> : FastListBase<T>
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x0000875C File Offset: 0x0000695C
		public FastList()
		{
			this.items = new T[4];
			this.arraySize = 4;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00008778 File Offset: 0x00006978
		public FastList(bool reserve, int reserved)
		{
			int num = Mathf.Max(reserved, 4);
			this.items = new T[num];
			this.arraySize = num;
			this._count = reserved;
			this.Count = reserved;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000087B6 File Offset: 0x000069B6
		public FastList(int capacity)
		{
			if (capacity < 1)
			{
				capacity = 1;
			}
			this.items = new T[capacity];
			this.arraySize = capacity;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000087D8 File Offset: 0x000069D8
		public FastList(FastList<T> list)
		{
			if (list == null)
			{
				this.items = new T[4];
				this.arraySize = 4;
				return;
			}
			this.items = new T[list.Count];
			Array.Copy(list.items, this.items, list.Count);
			this.arraySize = this.items.Length;
			this.Count = (this._count = this.items.Length);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00008850 File Offset: 0x00006A50
		public FastList(T[] items)
		{
			this.items = items;
			this._count = (this.Count = (this.arraySize = items.Length));
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00008888 File Offset: 0x00006A88
		protected void SetCapacity(int capacity)
		{
			this.arraySize = capacity;
			T[] array = new T[this.arraySize];
			if (this._count > 0)
			{
				Array.Copy(this.items, array, this._count);
			}
			this.items = array;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000088CC File Offset: 0x00006ACC
		public void SetCount(int count)
		{
			if (count > this.arraySize)
			{
				this.SetCapacity(count);
			}
			this._count = count;
			this.Count = count;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000088FC File Offset: 0x00006AFC
		public void EnsureCount(int count)
		{
			if (count <= this._count)
			{
				return;
			}
			if (count > this.arraySize)
			{
				this.SetCapacity(count);
			}
			this._count = count;
			this.Count = count;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00008934 File Offset: 0x00006B34
		public virtual void SetArray(T[] items)
		{
			this.items = items;
			this._count = (this.Count = (this.arraySize = items.Length));
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00008963 File Offset: 0x00006B63
		public int AddUnique(T item)
		{
			if (!this.Contains(item))
			{
				return this.Add(item);
			}
			return -1;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00008977 File Offset: 0x00006B77
		public bool Contains(T item)
		{
			return Array.IndexOf<T>(this.items, item, 0, this._count) != -1;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00008992 File Offset: 0x00006B92
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this.items, item, 0, this._count);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000089A8 File Offset: 0x00006BA8
		public T GetIndex(T item)
		{
			int num = Array.IndexOf<T>(this.items, item, 0, this._count);
			if (num == -1)
			{
				return default(T);
			}
			return this.items[num];
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000089E4 File Offset: 0x00006BE4
		public virtual int Add(T item)
		{
			if (this._count == this.arraySize)
			{
				base.DoubleCapacity();
			}
			this.items[this._count] = item;
			int count = this._count + 1;
			this._count = count;
			this.Count = count;
			return this._count - 1;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00008A38 File Offset: 0x00006C38
		public virtual int AddThreadSafe(T item)
		{
			int num;
			lock (this)
			{
				if (this._count == this.arraySize)
				{
					base.DoubleCapacity();
				}
				this.items[this._count] = item;
				num = this._count + 1;
				this._count = num;
				this.Count = num;
				num = this._count - 1;
			}
			return num;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00008AB4 File Offset: 0x00006CB4
		public virtual void Add(T item, T item2)
		{
			if (this._count + 1 >= this.arraySize)
			{
				base.DoubleCapacity();
			}
			T[] items = this.items;
			int count = this._count;
			this._count = count + 1;
			items[count] = item;
			T[] items2 = this.items;
			count = this._count;
			this._count = count + 1;
			items2[count] = item2;
			this.Count = this._count;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00008B20 File Offset: 0x00006D20
		public virtual void Add(T item, T item2, T item3)
		{
			if (this._count + 2 >= this.arraySize)
			{
				base.DoubleCapacity();
			}
			T[] items = this.items;
			int count = this._count;
			this._count = count + 1;
			items[count] = item;
			T[] items2 = this.items;
			count = this._count;
			this._count = count + 1;
			items2[count] = item2;
			T[] items3 = this.items;
			count = this._count;
			this._count = count + 1;
			items3[count] = item3;
			this.Count = this._count;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00008BA8 File Offset: 0x00006DA8
		public virtual void Add(T item, T item2, T item3, T item4)
		{
			if (this._count + 3 >= this.arraySize)
			{
				base.DoubleCapacity();
			}
			T[] items = this.items;
			int count = this._count;
			this._count = count + 1;
			items[count] = item;
			T[] items2 = this.items;
			count = this._count;
			this._count = count + 1;
			items2[count] = item2;
			T[] items3 = this.items;
			count = this._count;
			this._count = count + 1;
			items3[count] = item3;
			T[] items4 = this.items;
			count = this._count;
			this._count = count + 1;
			items4[count] = item4;
			this.Count = this._count;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008C4C File Offset: 0x00006E4C
		public virtual void Add(T item, T item2, T item3, T item4, T item5)
		{
			if (this._count + 4 >= this.arraySize)
			{
				base.DoubleCapacity();
			}
			T[] items = this.items;
			int count = this._count;
			this._count = count + 1;
			items[count] = item;
			T[] items2 = this.items;
			count = this._count;
			this._count = count + 1;
			items2[count] = item2;
			T[] items3 = this.items;
			count = this._count;
			this._count = count + 1;
			items3[count] = item3;
			T[] items4 = this.items;
			count = this._count;
			this._count = count + 1;
			items4[count] = item4;
			T[] items5 = this.items;
			count = this._count;
			this._count = count + 1;
			items5[count] = item5;
			this.Count = this._count;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008D10 File Offset: 0x00006F10
		public virtual void Insert(int index, T item)
		{
			if (index > this._count)
			{
				Debug.LogError("Index " + index.ToString() + " is out of range " + this._count.ToString());
			}
			if (this._count == this.arraySize)
			{
				base.DoubleCapacity();
			}
			if (index < this._count)
			{
				Array.Copy(this.items, index, this.items, index + 1, this._count - index);
			}
			this.items[index] = item;
			int count = this._count + 1;
			this._count = count;
			this.Count = count;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008DAC File Offset: 0x00006FAC
		public virtual void AddRange(T[] arrayItems)
		{
			if (arrayItems == null)
			{
				return;
			}
			int num = arrayItems.Length;
			int num2 = this._count + num;
			if (num2 >= this.arraySize)
			{
				this.SetCapacity(num2 * 2);
			}
			Array.Copy(arrayItems, 0, this.items, this._count, num);
			this.Count = (this._count = num2);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00008E00 File Offset: 0x00007000
		public virtual void AddRange(T[] arrayItems, int startIndex, int length)
		{
			int num = this._count + length;
			if (num >= this.arraySize)
			{
				this.SetCapacity(num * 2);
			}
			Array.Copy(arrayItems, startIndex, this.items, this._count, length);
			this.Count = (this._count = num);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008E4C File Offset: 0x0000704C
		public virtual void AddRange(FastList<T> list)
		{
			if (list.Count == 0)
			{
				return;
			}
			int num = this._count + list.Count;
			if (num >= this.arraySize)
			{
				this.SetCapacity(num * 2);
			}
			Array.Copy(list.items, 0, this.items, this._count, list.Count);
			this.Count = (this._count = num);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008EB0 File Offset: 0x000070B0
		public virtual int GrabListThreadSafe(FastList<T> threadList, bool fastClear = false)
		{
			int result;
			lock (threadList)
			{
				int count = this._count;
				this.AddRange(threadList);
				if (fastClear)
				{
					threadList.FastClear();
				}
				else
				{
					threadList.Clear();
				}
				result = count;
			}
			return result;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008F08 File Offset: 0x00007108
		public virtual void ChangeRange(int startIndex, T[] arrayItems)
		{
			for (int i = 0; i < arrayItems.Length; i++)
			{
				this.items[startIndex + i] = arrayItems[i];
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00008F38 File Offset: 0x00007138
		public virtual bool Remove(T item, bool weakReference = false)
		{
			int num = Array.IndexOf<T>(this.items, item, 0, this._count);
			if (num >= 0)
			{
				T[] items = this.items;
				int num2 = num;
				T[] items2 = this.items;
				int num3 = this._count - 1;
				this._count = num3;
				items[num2] = items2[num3];
				this.items[this._count] = default(T);
				this.Count = this._count;
				return true;
			}
			return false;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00008FB0 File Offset: 0x000071B0
		public virtual void RemoveAt(int index)
		{
			if (index >= this._count)
			{
				Debug.LogError("Index " + index.ToString() + " is out of range. List count is " + this._count.ToString());
				return;
			}
			T[] items = this.items;
			int num = index;
			T[] items2 = this.items;
			int num2 = this._count - 1;
			this._count = num2;
			items[num] = items2[num2];
			this.items[this._count] = default(T);
			this.Count = this._count;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000903C File Offset: 0x0000723C
		public virtual void RemoveLast()
		{
			if (this._count == 0)
			{
				return;
			}
			this._count--;
			this.items[this._count] = default(T);
			this.Count = this._count;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00009088 File Offset: 0x00007288
		public virtual void RemoveRange(int index, int length)
		{
			if (this._count - index < length)
			{
				Debug.LogError("Invalid length!");
			}
			if (length > 0)
			{
				this._count -= length;
				if (index < this._count)
				{
					Array.Copy(this.items, index + length, this.items, index, this._count - index);
				}
				Array.Clear(this.items, this._count, length);
				this.Count = this._count;
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00009100 File Offset: 0x00007300
		public virtual T Dequeue()
		{
			if (this._count == 0)
			{
				Debug.LogError("List is empty!");
				return default(T);
			}
			T[] items = this.items;
			int num = this._count - 1;
			this._count = num;
			T result = items[num];
			this.items[this._count] = default(T);
			this.Count = this._count;
			return result;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000916C File Offset: 0x0000736C
		public virtual T Dequeue(int index)
		{
			T result = this.items[index];
			T[] items = this.items;
			T[] items2 = this.items;
			int num = this._count - 1;
			this._count = num;
			items[index] = items2[num];
			this.items[this._count] = default(T);
			this.Count = this._count;
			return result;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000091D4 File Offset: 0x000073D4
		public virtual void Clear()
		{
			Array.Clear(this.items, 0, this._count);
			this.Count = (this._count = 0);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00009204 File Offset: 0x00007404
		public virtual void ClearThreadSafe()
		{
			lock (this)
			{
				Array.Clear(this.items, 0, this._count);
				this.Count = (this._count = 0);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000925C File Offset: 0x0000745C
		public virtual void ClearRange(int startIndex)
		{
			Array.Clear(this.items, startIndex, this._count - startIndex);
			this._count = startIndex;
			this.Count = startIndex;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00009290 File Offset: 0x00007490
		public virtual void FastClear()
		{
			this.Count = (this._count = 0);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000092B0 File Offset: 0x000074B0
		public virtual void FastClear(int newCount)
		{
			if (newCount < this.Count)
			{
				this._count = newCount;
				this.Count = newCount;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000092D8 File Offset: 0x000074D8
		public virtual T[] ToArray()
		{
			T[] array = new T[this._count];
			Array.Copy(this.items, 0, array, 0, this._count);
			return array;
		}
	}
}
