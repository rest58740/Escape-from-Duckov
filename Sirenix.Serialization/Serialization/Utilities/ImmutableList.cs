using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000D0 RID: 208
	[Serializable]
	internal sealed class ImmutableList : IImmutableList<object>, IImmutableList, IList, ICollection, IEnumerable, IList<object>, ICollection<object>, IEnumerable<object>
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x0002A034 File Offset: 0x00028234
		public ImmutableList(IList innerList)
		{
			if (innerList == null)
			{
				throw new ArgumentNullException("innerList");
			}
			this.innerList = innerList;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0002A051 File Offset: 0x00028251
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x000275A0 File Offset: 0x000257A0
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x000275A0 File Offset: 0x000257A0
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0002A05E File Offset: 0x0002825E
		public bool IsSynchronized
		{
			get
			{
				return this.innerList.IsSynchronized;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0002A06B File Offset: 0x0002826B
		public object SyncRoot
		{
			get
			{
				return this.innerList.SyncRoot;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0002A078 File Offset: 0x00028278
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x0002A086 File Offset: 0x00028286
		object IList.Item
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0002A078 File Offset: 0x00028278
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x0002A086 File Offset: 0x00028286
		object IList<object>.Item
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

		// Token: 0x1700006B RID: 107
		public object this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0002A092 File Offset: 0x00028292
		public bool Contains(object value)
		{
			return this.innerList.Contains(value);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0002A0A0 File Offset: 0x000282A0
		public void CopyTo(object[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0002A0A0 File Offset: 0x000282A0
		public void CopyTo(Array array, int index)
		{
			this.innerList.CopyTo(array, index);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0002A0AF File Offset: 0x000282AF
		public IEnumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0002A0BC File Offset: 0x000282BC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0002A0C4 File Offset: 0x000282C4
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			foreach (object obj in this.innerList)
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0002A086 File Offset: 0x00028286
		int IList.Add(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0002A086 File Offset: 0x00028286
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0002A0D3 File Offset: 0x000282D3
		public int IndexOf(object value)
		{
			return this.innerList.IndexOf(value);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0002A086 File Offset: 0x00028286
		void IList<object>.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0002A086 File Offset: 0x00028286
		void IList<object>.Insert(int index, object item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0002A086 File Offset: 0x00028286
		void ICollection<object>.Add(object item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0002A086 File Offset: 0x00028286
		void ICollection<object>.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002A086 File Offset: 0x00028286
		bool ICollection<object>.Remove(object item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0400021F RID: 543
		[SerializeField]
		private IList innerList;
	}
}
