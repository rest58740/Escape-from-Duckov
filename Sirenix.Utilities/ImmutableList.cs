using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000025 RID: 37
	[Serializable]
	public sealed class ImmutableList : IImmutableList<object>, IImmutableList, IList, ICollection, IEnumerable, IList<object>, ICollection<object>, IEnumerable<object>
	{
		// Token: 0x0600016C RID: 364 RVA: 0x0000B8D7 File Offset: 0x00009AD7
		public ImmutableList(IList innerList)
		{
			if (innerList == null)
			{
				throw new ArgumentNullException("innerList");
			}
			this.innerList = innerList;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000B8F4 File Offset: 0x00009AF4
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00008CC8 File Offset: 0x00006EC8
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00008CC8 File Offset: 0x00006EC8
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000B901 File Offset: 0x00009B01
		public bool IsSynchronized
		{
			get
			{
				return this.innerList.IsSynchronized;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000171 RID: 369 RVA: 0x0000B90E File Offset: 0x00009B0E
		public object SyncRoot
		{
			get
			{
				return this.innerList.SyncRoot;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000B91B File Offset: 0x00009B1B
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000B929 File Offset: 0x00009B29
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000B91B File Offset: 0x00009B1B
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0000B929 File Offset: 0x00009B29
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

		// Token: 0x17000016 RID: 22
		public object this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000B935 File Offset: 0x00009B35
		public bool Contains(object value)
		{
			return this.innerList.Contains(value);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000B943 File Offset: 0x00009B43
		public void CopyTo(object[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000B943 File Offset: 0x00009B43
		public void CopyTo(Array array, int index)
		{
			this.innerList.CopyTo(array, index);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000B952 File Offset: 0x00009B52
		public IEnumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000B95F File Offset: 0x00009B5F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000B967 File Offset: 0x00009B67
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

		// Token: 0x0600017D RID: 381 RVA: 0x0000B929 File Offset: 0x00009B29
		int IList.Add(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000B976 File Offset: 0x00009B76
		public int IndexOf(object value)
		{
			return this.innerList.IndexOf(value);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList<object>.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList<object>.Insert(int index, object item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000B929 File Offset: 0x00009B29
		void ICollection<object>.Add(object item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000B929 File Offset: 0x00009B29
		void ICollection<object>.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000B929 File Offset: 0x00009B29
		bool ICollection<object>.Remove(object item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x04000056 RID: 86
		[SerializeField]
		private IList innerList;
	}
}
