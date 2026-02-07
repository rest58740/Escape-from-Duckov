using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000D1 RID: 209
	[Serializable]
	public sealed class ImmutableList<T> : IImmutableList<T>, IImmutableList, IList, ICollection, IEnumerable, IList<T>, ICollection<T>, IEnumerable<!0>
	{
		// Token: 0x060005E2 RID: 1506 RVA: 0x0002A0E1 File Offset: 0x000282E1
		public ImmutableList(IList<T> innerList)
		{
			if (innerList == null)
			{
				throw new ArgumentNullException("innerList");
			}
			this.innerList = innerList;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0002A0FE File Offset: 0x000282FE
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0002A10B File Offset: 0x0002830B
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0000EE6B File Offset: 0x0000D06B
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x000275A0 File Offset: 0x000257A0
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x000275A0 File Offset: 0x000257A0
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x000275A0 File Offset: 0x000257A0
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0002A10E File Offset: 0x0002830E
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x0002A086 File Offset: 0x00028286
		object IList.Item
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new NotSupportedException("Immutable Lists cannot be edited.");
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0002A11C File Offset: 0x0002831C
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x0002A086 File Offset: 0x00028286
		T IList<!0>.Item
		{
			get
			{
				return this.innerList[index];
			}
			set
			{
				throw new NotSupportedException("Immutable Lists cannot be edited.");
			}
		}

		// Token: 0x17000074 RID: 116
		public T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0002A12A File Offset: 0x0002832A
		public bool Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002A138 File Offset: 0x00028338
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0002A147 File Offset: 0x00028347
		public IEnumerator<T> GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0002A154 File Offset: 0x00028354
		void ICollection.CopyTo(Array array, int index)
		{
			this.innerList.CopyTo((T[])array, index);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0002A086 File Offset: 0x00028286
		void ICollection<!0>.Add(T item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0002A086 File Offset: 0x00028286
		void ICollection<!0>.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0002A086 File Offset: 0x00028286
		bool ICollection<!0>.Remove(T item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002A168 File Offset: 0x00028368
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0002A086 File Offset: 0x00028286
		int IList.Add(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0002A170 File Offset: 0x00028370
		bool IList.Contains(object value)
		{
			return this.innerList.Contains((T)((object)value));
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0002A183 File Offset: 0x00028383
		int IList.IndexOf(object value)
		{
			return this.innerList.IndexOf((T)((object)value));
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0002A086 File Offset: 0x00028286
		void IList<!0>.Insert(int index, T item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0002A196 File Offset: 0x00028396
		public int IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0002A086 File Offset: 0x00028286
		void IList<!0>.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x04000220 RID: 544
		[SerializeField]
		private IList<T> innerList;
	}
}
