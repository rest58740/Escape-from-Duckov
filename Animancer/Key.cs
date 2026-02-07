using System;
using System.Collections;
using System.Collections.Generic;

namespace Animancer
{
	// Token: 0x02000009 RID: 9
	public class Key : Key.IListItem
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x000031AB File Offset: 0x000013AB
		public static int IndexOf(Key key)
		{
			return key._Index;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000031B3 File Offset: 0x000013B3
		public static bool IsInList(Key key)
		{
			return key._Index != -1;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000031C1 File Offset: 0x000013C1
		Key Key.IListItem.Key
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0400000B RID: 11
		public const int NotInList = -1;

		// Token: 0x0400000C RID: 12
		private int _Index = -1;

		// Token: 0x0200007A RID: 122
		public interface IListItem
		{
			// Token: 0x17000170 RID: 368
			// (get) Token: 0x0600059D RID: 1437
			Key Key { get; }
		}

		// Token: 0x0200007B RID: 123
		public class KeyedList<T> : IList<T>, ICollection<T>, IEnumerable<!0>, IEnumerable, ICollection where T : class, Key.IListItem
		{
			// Token: 0x0600059E RID: 1438 RVA: 0x0000EEC7 File Offset: 0x0000D0C7
			public KeyedList()
			{
				this.Items = new List<T>();
			}

			// Token: 0x0600059F RID: 1439 RVA: 0x0000EEDA File Offset: 0x0000D0DA
			public KeyedList(int capacity)
			{
				this.Items = new List<T>(capacity);
			}

			// Token: 0x17000171 RID: 369
			// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0000EEEE File Offset: 0x0000D0EE
			public int Count
			{
				get
				{
					return this.Items.Count;
				}
			}

			// Token: 0x17000172 RID: 370
			// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0000EEFB File Offset: 0x0000D0FB
			// (set) Token: 0x060005A2 RID: 1442 RVA: 0x0000EF08 File Offset: 0x0000D108
			public int Capacity
			{
				get
				{
					return this.Items.Capacity;
				}
				set
				{
					this.Items.Capacity = value;
				}
			}

			// Token: 0x17000173 RID: 371
			public T this[int index]
			{
				get
				{
					return this.Items[index];
				}
				set
				{
					Key key = value.Key;
					if (key._Index != -1)
					{
						throw new ArgumentException("Each item can only be used in one KeyedList at a time.");
					}
					this.Items[index].Key._Index = -1;
					key._Index = index;
					this.Items[index] = value;
				}
			}

			// Token: 0x060005A5 RID: 1445 RVA: 0x0000EF80 File Offset: 0x0000D180
			public bool Contains(T item)
			{
				if (item == null)
				{
					return false;
				}
				int index = item.Key._Index;
				return index < this.Items.Count && this.Items[index] == item;
			}

			// Token: 0x060005A6 RID: 1446 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
			public int IndexOf(T item)
			{
				if (item == null)
				{
					return -1;
				}
				int index = item.Key._Index;
				if (index < this.Items.Count && this.Items[index] == item)
				{
					return index;
				}
				return -1;
			}

			// Token: 0x060005A7 RID: 1447 RVA: 0x0000F026 File Offset: 0x0000D226
			public void Add(T item)
			{
				Key key = item.Key;
				if (key._Index != -1)
				{
					throw new ArgumentException("Each item can only be used in one KeyedList at a time.");
				}
				key._Index = this.Items.Count;
				this.Items.Add(item);
			}

			// Token: 0x060005A8 RID: 1448 RVA: 0x0000F063 File Offset: 0x0000D263
			public void AddNew(T item)
			{
				if (!this.Contains(item))
				{
					this.Add(item);
				}
			}

			// Token: 0x060005A9 RID: 1449 RVA: 0x0000F078 File Offset: 0x0000D278
			public void Insert(int index, T item)
			{
				for (int i = index; i < this.Items.Count; i++)
				{
					this.Items[i].Key._Index++;
				}
				item.Key._Index = index;
				this.Items.Insert(index, item);
			}

			// Token: 0x060005AA RID: 1450 RVA: 0x0000F0DC File Offset: 0x0000D2DC
			public void RemoveAt(int index)
			{
				for (int i = index + 1; i < this.Items.Count; i++)
				{
					this.Items[i].Key._Index--;
				}
				this.Items[index].Key._Index = -1;
				this.Items.RemoveAt(index);
			}

			// Token: 0x060005AB RID: 1451 RVA: 0x0000F14C File Offset: 0x0000D34C
			public void RemoveAtSwap(int index)
			{
				this.Items[index].Key._Index = -1;
				int num = this.Items.Count - 1;
				if (num > index)
				{
					T t = this.Items[num];
					t.Key._Index = index;
					this.Items[index] = t;
				}
				this.Items.RemoveAt(num);
			}

			// Token: 0x060005AC RID: 1452 RVA: 0x0000F1C0 File Offset: 0x0000D3C0
			public bool Remove(T item)
			{
				int index = item.Key._Index;
				if (index == -1)
				{
					return false;
				}
				if (this.Items[index] != item)
				{
					throw new ArgumentException("The specified item does not exist in this KeyedList.", "item");
				}
				this.RemoveAt(index);
				return true;
			}

			// Token: 0x060005AD RID: 1453 RVA: 0x0000F218 File Offset: 0x0000D418
			public bool RemoveSwap(T item)
			{
				int index = item.Key._Index;
				if (index == -1)
				{
					return false;
				}
				if (this.Items[index] != item)
				{
					throw new ArgumentException("The specified item does not exist in this KeyedList.", "item");
				}
				this.RemoveAtSwap(index);
				return true;
			}

			// Token: 0x060005AE RID: 1454 RVA: 0x0000F270 File Offset: 0x0000D470
			public void Clear()
			{
				for (int i = this.Items.Count - 1; i >= 0; i--)
				{
					this.Items[i].Key._Index = -1;
				}
				this.Items.Clear();
			}

			// Token: 0x060005AF RID: 1455 RVA: 0x0000F2BC File Offset: 0x0000D4BC
			public void CopyTo(T[] array, int index)
			{
				this.Items.CopyTo(array, index);
			}

			// Token: 0x060005B0 RID: 1456 RVA: 0x0000F2CB File Offset: 0x0000D4CB
			void ICollection.CopyTo(Array array, int index)
			{
				((ICollection)this.Items).CopyTo(array, index);
			}

			// Token: 0x17000174 RID: 372
			// (get) Token: 0x060005B1 RID: 1457 RVA: 0x0000F2DA File Offset: 0x0000D4DA
			bool ICollection<!0>.IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060005B2 RID: 1458 RVA: 0x0000F2DD File Offset: 0x0000D4DD
			public List<T>.Enumerator GetEnumerator()
			{
				return this.Items.GetEnumerator();
			}

			// Token: 0x060005B3 RID: 1459 RVA: 0x0000F2EA File Offset: 0x0000D4EA
			IEnumerator<T> IEnumerable<!0>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060005B4 RID: 1460 RVA: 0x0000F2F7 File Offset: 0x0000D4F7
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x17000175 RID: 373
			// (get) Token: 0x060005B5 RID: 1461 RVA: 0x0000F304 File Offset: 0x0000D504
			bool ICollection.IsSynchronized
			{
				get
				{
					return ((ICollection)this.Items).IsSynchronized;
				}
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0000F311 File Offset: 0x0000D511
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.Items).SyncRoot;
				}
			}

			// Token: 0x0400010A RID: 266
			private const string SingleUse = "Each item can only be used in one KeyedList at a time.";

			// Token: 0x0400010B RID: 267
			private const string NotFound = "The specified item does not exist in this KeyedList.";

			// Token: 0x0400010C RID: 268
			private readonly List<T> Items;
		}
	}
}
