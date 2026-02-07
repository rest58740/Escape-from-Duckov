using System;
using System.Collections;
using System.Collections.Generic;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000D2 RID: 210
	[Serializable]
	public sealed class ImmutableList<TList, TElement> : IImmutableList<TElement>, IImmutableList, IList, ICollection, IEnumerable, IList<TElement>, ICollection<TElement>, IEnumerable<TElement> where TList : IList<TElement>
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x0002A1A4 File Offset: 0x000283A4
		public ImmutableList(TList innerList)
		{
			if (innerList == null)
			{
				throw new ArgumentNullException("innerList");
			}
			this.innerList = innerList;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0002A1C6 File Offset: 0x000283C6
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x0002A10B File Offset: 0x0002830B
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0000EE6B File Offset: 0x0000D06B
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x000275A0 File Offset: 0x000257A0
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000275A0 File Offset: 0x000257A0
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x000275A0 File Offset: 0x000257A0
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0002A1D9 File Offset: 0x000283D9
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x0002A086 File Offset: 0x00028286
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0002A1E7 File Offset: 0x000283E7
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x0002A086 File Offset: 0x00028286
		TElement IList<!1>.Item
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

		// Token: 0x1700007D RID: 125
		public TElement this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0002A1FB File Offset: 0x000283FB
		public bool Contains(TElement item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0002A20F File Offset: 0x0002840F
		public void CopyTo(TElement[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0002A224 File Offset: 0x00028424
		public IEnumerator<TElement> GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0002A237 File Offset: 0x00028437
		void ICollection.CopyTo(Array array, int index)
		{
			this.innerList.CopyTo((TElement[])array, index);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0002A086 File Offset: 0x00028286
		void ICollection<!1>.Add(TElement item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0002A086 File Offset: 0x00028286
		void ICollection<!1>.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0002A086 File Offset: 0x00028286
		bool ICollection<!1>.Remove(TElement item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0002A251 File Offset: 0x00028451
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0002A086 File Offset: 0x00028286
		int IList.Add(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0002A259 File Offset: 0x00028459
		bool IList.Contains(object value)
		{
			return this.innerList.Contains((TElement)((object)value));
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0002A272 File Offset: 0x00028472
		int IList.IndexOf(object value)
		{
			return this.innerList.IndexOf((TElement)((object)value));
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0002A086 File Offset: 0x00028286
		void IList<!1>.Insert(int index, TElement item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0002A28B File Offset: 0x0002848B
		public int IndexOf(TElement item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0002A086 File Offset: 0x00028286
		void IList<!1>.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x04000221 RID: 545
		private TList innerList;
	}
}
