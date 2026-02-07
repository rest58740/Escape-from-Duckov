using System;
using System.Collections;
using System.Collections.Generic;

namespace ItemStatsSystem.Items
{
	// Token: 0x0200002A RID: 42
	public class SlotCollection : ItemComponent, ICollection<Slot>, IEnumerable<Slot>, IEnumerable
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000226 RID: 550 RVA: 0x000087CA File Offset: 0x000069CA
		private Dictionary<int, Slot> slotsDictionary
		{
			get
			{
				if (this._cachedSlotsDictionary == null)
				{
					this.BuildDictionary();
				}
				return this._cachedSlotsDictionary;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000087E0 File Offset: 0x000069E0
		public int Count
		{
			get
			{
				if (this.list != null)
				{
					return this.list.Count;
				}
				return 0;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000087F7 File Offset: 0x000069F7
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000087FA File Offset: 0x000069FA
		public Slot GetSlotByIndex(int index)
		{
			return this.list[index];
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00008808 File Offset: 0x00006A08
		public Slot GetSlot(int hash)
		{
			Slot result;
			if (this.slotsDictionary.TryGetValue(hash, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00008828 File Offset: 0x00006A28
		public Slot GetSlot(string key)
		{
			int hashCode = key.GetHashCode();
			Slot slot = this.GetSlot(hashCode);
			if (slot == null)
			{
				slot = this.list.Find((Slot e) => e.Key == key);
			}
			return slot;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00008874 File Offset: 0x00006A74
		private void BuildDictionary()
		{
			if (this._cachedSlotsDictionary == null)
			{
				this._cachedSlotsDictionary = new Dictionary<int, Slot>();
			}
			this._cachedSlotsDictionary.Clear();
			foreach (Slot slot in this.list)
			{
				int hashCode = slot.Key.GetHashCode();
				this._cachedSlotsDictionary[hashCode] = slot;
			}
		}

		// Token: 0x1700009B RID: 155
		public Slot this[string key]
		{
			get
			{
				return this.GetSlot(key);
			}
		}

		// Token: 0x1700009C RID: 156
		public Slot this[int index]
		{
			get
			{
				return this.GetSlotByIndex(index);
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000890C File Offset: 0x00006B0C
		internal override void OnInitialize()
		{
			foreach (Slot slot in this.list)
			{
				slot.Initialize(this);
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00008960 File Offset: 0x00006B60
		public IEnumerator<Slot> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008972 File Offset: 0x00006B72
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00008984 File Offset: 0x00006B84
		public void Add(Slot item)
		{
			this.list.Add(item);
			item.Initialize(this);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00008999 File Offset: 0x00006B99
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000089A6 File Offset: 0x00006BA6
		public bool Contains(Slot item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000089B4 File Offset: 0x00006BB4
		public void CopyTo(Slot[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000089C3 File Offset: 0x00006BC3
		public bool Remove(Slot item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x040000C9 RID: 201
		public Action<Slot> OnSlotContentChanged;

		// Token: 0x040000CA RID: 202
		public List<Slot> list = new List<Slot>();

		// Token: 0x040000CB RID: 203
		private Dictionary<int, Slot> _cachedSlotsDictionary;
	}
}
