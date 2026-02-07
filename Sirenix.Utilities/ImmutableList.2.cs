using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public sealed class ImmutableList<T> : IImmutableList<T>, IImmutableList, IList, ICollection, IEnumerable, IList<T>, ICollection<T>, IEnumerable<!0>
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000B984 File Offset: 0x00009B84
		public ImmutableList(IList<T> innerList)
		{
			if (innerList == null)
			{
				throw new ArgumentNullException("innerList");
			}
			this.innerList = innerList;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000B9A1 File Offset: 0x00009BA1
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000B9AE File Offset: 0x00009BAE
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000B9B1 File Offset: 0x00009BB1
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00008CC8 File Offset: 0x00006EC8
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00008CC8 File Offset: 0x00006EC8
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00008CC8 File Offset: 0x00006EC8
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000B9B4 File Offset: 0x00009BB4
		// (set) Token: 0x06000190 RID: 400 RVA: 0x0000B929 File Offset: 0x00009B29
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000B9C2 File Offset: 0x00009BC2
		// (set) Token: 0x06000192 RID: 402 RVA: 0x0000B929 File Offset: 0x00009B29
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

		// Token: 0x1700001F RID: 31
		public T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000B9D0 File Offset: 0x00009BD0
		public bool Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000B9DE File Offset: 0x00009BDE
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000B9ED File Offset: 0x00009BED
		public IEnumerator<T> GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000B9FA File Offset: 0x00009BFA
		void ICollection.CopyTo(Array array, int index)
		{
			this.innerList.CopyTo((T[])array, index);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B929 File Offset: 0x00009B29
		void ICollection<!0>.Add(T item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B929 File Offset: 0x00009B29
		void ICollection<!0>.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000B929 File Offset: 0x00009B29
		bool ICollection<!0>.Remove(T item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000BA0E File Offset: 0x00009C0E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B929 File Offset: 0x00009B29
		int IList.Add(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Clear()
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000BA16 File Offset: 0x00009C16
		bool IList.Contains(object value)
		{
			return this.innerList.Contains((T)((object)value));
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000BA29 File Offset: 0x00009C29
		int IList.IndexOf(object value)
		{
			return this.innerList.IndexOf((T)((object)value));
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList<!0>.Insert(int index, T item)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000BA3C File Offset: 0x00009C3C
		public int IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000B929 File Offset: 0x00009B29
		void IList<!0>.RemoveAt(int index)
		{
			throw new NotSupportedException("Immutable Lists cannot be edited.");
		}

		// Token: 0x04000057 RID: 87
		[SerializeField]
		private IList<T> innerList;
	}
}
