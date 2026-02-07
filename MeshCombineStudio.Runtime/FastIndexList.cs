using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200002A RID: 42
	public class FastIndexList<T> : FastList<T>, IFastIndexList where T : IFastIndex
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x0000821F File Offset: 0x0000641F
		public FastIndexList()
		{
			this.items = new T[4];
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00008233 File Offset: 0x00006433
		public FastIndexList(int capacity)
		{
			this.items = new T[capacity];
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00008248 File Offset: 0x00006448
		public new void Clear()
		{
			for (int i = 0; i < this._count; i++)
			{
				this.items[i].ListIndex = -1;
				this.items[i].List = null;
				this.items[i] = default(T);
			}
			this.Count = (this._count = 0);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000082C0 File Offset: 0x000064C0
		public void SetItem(int index, T item)
		{
			if (item.List != null)
			{
				Debug.LogError("Is already in another list!");
				return;
			}
			if (index >= this.items.Length)
			{
				base.SetCapacity(index * 2);
			}
			else if (index >= this._count)
			{
				this._count = (this.Count = index + 1);
			}
			this.items[index] = item;
			item.ListIndex = index;
			item.List = this;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00008344 File Offset: 0x00006544
		public new int Add(T item)
		{
			IFastIndexList list = item.List;
			if (list == this)
			{
				Debug.LogError("Item is already in this list");
				return item.ListIndex;
			}
			if (list != null)
			{
				Debug.LogError("Is already in another list!");
				return -1;
			}
			if (item.ListIndex != -1)
			{
				Debug.Log("Item already added");
				return -1;
			}
			if (this._count == this.items.Length)
			{
				base.DoubleCapacity();
			}
			this.items[this._count] = item;
			int count = this._count;
			this._count = count + 1;
			item.ListIndex = count;
			item.List = this;
			this.Count = this._count;
			return this._count - 1;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00008410 File Offset: 0x00006610
		public new void AddRange(T[] newItems)
		{
			int num = this._count + newItems.Length;
			if (num >= this.items.Length)
			{
				base.SetCapacity(num * 2);
			}
			for (int i = 0; i < newItems.Length; i++)
			{
				if (newItems[i].List != null)
				{
					Debug.LogError("Is already in another list!");
				}
				else if (newItems[i].ListIndex != -1)
				{
					Debug.Log("Item already added");
				}
				else
				{
					this.items[this._count] = newItems[i];
					int num2 = i;
					int count = this._count;
					this._count = count + 1;
					newItems[num2].ListIndex = count;
					newItems[i].List = this;
				}
			}
			this.Count = this._count;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000084F4 File Offset: 0x000066F4
		public new bool RemoveAt(int index)
		{
			if (index >= this._count)
			{
				Debug.LogError("Index " + index.ToString() + " is out of range. List count is " + this._count.ToString());
				return false;
			}
			T t = this.items[index];
			if (t.ListIndex == -1)
			{
				Debug.Log("Item already removed");
				return false;
			}
			T[] items = this.items;
			int num = index;
			T[] items2 = this.items;
			int num2 = this._count - 1;
			this._count = num2;
			items[num] = items2[num2];
			this.items[index].ListIndex = index;
			this.items[this._count] = default(T);
			t.ListIndex = -1;
			t.List = null;
			this.Count = this._count;
			return true;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000085E0 File Offset: 0x000067E0
		public override T Dequeue()
		{
			if (this._count == 0)
			{
				return default(T);
			}
			T[] items = this.items;
			int num = this._count - 1;
			this._count = num;
			T result = items[num];
			this.items[this._count] = default(T);
			result.ListIndex = -1;
			result.List = null;
			this.Count = this._count;
			return result;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00008660 File Offset: 0x00006860
		public bool Remove(IFastIndex item)
		{
			if (item == null || item.List != this)
			{
				return false;
			}
			int listIndex = item.ListIndex;
			if (listIndex == -1)
			{
				Debug.Log("Item already removed");
				return false;
			}
			T[] items = this.items;
			int num = listIndex;
			T[] items2 = this.items;
			int num2 = this._count - 1;
			this._count = num2;
			items[num] = items2[num2];
			this.items[listIndex].ListIndex = listIndex;
			this.items[this._count] = default(T);
			item.ListIndex = -1;
			item.List = null;
			this.Count = this._count;
			return true;
		}
	}
}
