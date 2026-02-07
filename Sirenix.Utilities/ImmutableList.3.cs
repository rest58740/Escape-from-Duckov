using System;
using System.Collections;
using System.Collections.Generic;

namespace Sirenix.Utilities
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public sealed class ImmutableList<TList, TElement> : IImmutableList<TElement>, IImmutableList, IList, ICollection, IEnumerable, IList<TElement>, ICollection<TElement>, IEnumerable<TElement> where TList : IList<TElement>
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x0000BA4A File Offset: 0x00009C4A
		public ImmutableList(TList innerList)
		{
			if (innerList == null)
			{
				throw new ArgumentNullException("innerList");
			}
			this.innerList = innerList;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000BA6C File Offset: 0x00009C6C
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000B9AE File Offset: 0x00009BAE
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000B9B1 File Offset: 0x00009BB1
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00008CC8 File Offset: 0x00006EC8
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00008CC8 File Offset: 0x00006EC8
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00008CC8 File Offset: 0x00006EC8
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000BA7F File Offset: 0x00009C7F
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000B929 File Offset: 0x00009B29
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000BA8D File Offset: 0x00009C8D
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000B929 File Offset: 0x00009B29
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

		// Token: 0x17000028 RID: 40
		public TElement this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000BAA1 File Offset: 0x00009CA1
		public bool Contains(TElement item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000BAB5 File Offset: 0x00009CB5
		public void CopyTo(TElement[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000BACA File Offset: 0x00009CCA
		public IEnumerator<TElement> GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000BADD File Offset: 0x00009CDD
		void ICollection.CopyTo(Array array, int index)
		{
			this.innerList.CopyTo((TElement[])array, index);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000B929 File Offset: 0x00009B29
		void ICollection<!1>.Add(TElement item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B929 File Offset: 0x00009B29
		void ICollection<!1>.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B929 File Offset: 0x00009B29
		bool ICollection<!1>.Remove(TElement item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000BAF7 File Offset: 0x00009CF7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B929 File Offset: 0x00009B29
		int IList.Add(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000BAFF File Offset: 0x00009CFF
		bool IList.Contains(object value)
		{
			return this.innerList.Contains((TElement)((object)value));
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000BB18 File Offset: 0x00009D18
		int IList.IndexOf(object value)
		{
			return this.innerList.IndexOf((TElement)((object)value));
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList<!1>.Insert(int index, TElement item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000BB31 File Offset: 0x00009D31
		public int IndexOf(TElement item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList<!1>.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x04000058 RID: 88
		private TList innerList;
	}
}
