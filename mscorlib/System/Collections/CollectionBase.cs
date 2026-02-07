using System;

namespace System.Collections
{
	// Token: 0x02000A24 RID: 2596
	[Serializable]
	public abstract class CollectionBase : IList, ICollection, IEnumerable
	{
		// Token: 0x06005BD8 RID: 23512 RVA: 0x0013554D File Offset: 0x0013374D
		protected CollectionBase()
		{
			this._list = new ArrayList();
		}

		// Token: 0x06005BD9 RID: 23513 RVA: 0x00135560 File Offset: 0x00133760
		protected CollectionBase(int capacity)
		{
			this._list = new ArrayList(capacity);
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06005BDA RID: 23514 RVA: 0x00135574 File Offset: 0x00133774
		protected ArrayList InnerList
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06005BDB RID: 23515 RVA: 0x0000270D File Offset: 0x0000090D
		protected IList List
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06005BDC RID: 23516 RVA: 0x0013557C File Offset: 0x0013377C
		// (set) Token: 0x06005BDD RID: 23517 RVA: 0x00135589 File Offset: 0x00133789
		public int Capacity
		{
			get
			{
				return this.InnerList.Capacity;
			}
			set
			{
				this.InnerList.Capacity = value;
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06005BDE RID: 23518 RVA: 0x00135597 File Offset: 0x00133797
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x06005BDF RID: 23519 RVA: 0x001355A4 File Offset: 0x001337A4
		public void Clear()
		{
			this.OnClear();
			this.InnerList.Clear();
			this.OnClearComplete();
		}

		// Token: 0x06005BE0 RID: 23520 RVA: 0x001355C0 File Offset: 0x001337C0
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			object value = this.InnerList[index];
			this.OnValidate(value);
			this.OnRemove(index, value);
			this.InnerList.RemoveAt(index);
			try
			{
				this.OnRemoveComplete(index, value);
			}
			catch
			{
				this.InnerList.Insert(index, value);
				throw;
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06005BE1 RID: 23521 RVA: 0x0013563C File Offset: 0x0013383C
		bool IList.IsReadOnly
		{
			get
			{
				return this.InnerList.IsReadOnly;
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06005BE2 RID: 23522 RVA: 0x00135649 File Offset: 0x00133849
		bool IList.IsFixedSize
		{
			get
			{
				return this.InnerList.IsFixedSize;
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06005BE3 RID: 23523 RVA: 0x00135656 File Offset: 0x00133856
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06005BE4 RID: 23524 RVA: 0x00135663 File Offset: 0x00133863
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		// Token: 0x06005BE5 RID: 23525 RVA: 0x00135670 File Offset: 0x00133870
		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		// Token: 0x17000FEC RID: 4076
		object IList.this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				return this.InnerList[index];
			}
			set
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				this.OnValidate(value);
				object obj = this.InnerList[index];
				this.OnSet(index, obj, value);
				this.InnerList[index] = value;
				try
				{
					this.OnSetComplete(index, obj, value);
				}
				catch
				{
					this.InnerList[index] = obj;
					throw;
				}
			}
		}

		// Token: 0x06005BE8 RID: 23528 RVA: 0x0013572C File Offset: 0x0013392C
		bool IList.Contains(object value)
		{
			return this.InnerList.Contains(value);
		}

		// Token: 0x06005BE9 RID: 23529 RVA: 0x0013573C File Offset: 0x0013393C
		int IList.Add(object value)
		{
			this.OnValidate(value);
			this.OnInsert(this.InnerList.Count, value);
			int num = this.InnerList.Add(value);
			try
			{
				this.OnInsertComplete(num, value);
			}
			catch
			{
				this.InnerList.RemoveAt(num);
				throw;
			}
			return num;
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x0013579C File Offset: 0x0013399C
		void IList.Remove(object value)
		{
			this.OnValidate(value);
			int num = this.InnerList.IndexOf(value);
			if (num < 0)
			{
				throw new ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");
			}
			this.OnRemove(num, value);
			this.InnerList.RemoveAt(num);
			try
			{
				this.OnRemoveComplete(num, value);
			}
			catch
			{
				this.InnerList.Insert(num, value);
				throw;
			}
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x0013580C File Offset: 0x00133A0C
		int IList.IndexOf(object value)
		{
			return this.InnerList.IndexOf(value);
		}

		// Token: 0x06005BEC RID: 23532 RVA: 0x0013581C File Offset: 0x00133A1C
		void IList.Insert(int index, object value)
		{
			if (index < 0 || index > this.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			this.OnValidate(value);
			this.OnInsert(index, value);
			this.InnerList.Insert(index, value);
			try
			{
				this.OnInsertComplete(index, value);
			}
			catch
			{
				this.InnerList.RemoveAt(index);
				throw;
			}
		}

		// Token: 0x06005BED RID: 23533 RVA: 0x0013588C File Offset: 0x00133A8C
		public IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x06005BEE RID: 23534 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnSet(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x06005BEF RID: 23535 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnInsert(int index, object value)
		{
		}

		// Token: 0x06005BF0 RID: 23536 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnClear()
		{
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnRemove(int index, object value)
		{
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x00135899 File Offset: 0x00133A99
		protected virtual void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x06005BF3 RID: 23539 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnSetComplete(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x06005BF4 RID: 23540 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnInsertComplete(int index, object value)
		{
		}

		// Token: 0x06005BF5 RID: 23541 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x06005BF6 RID: 23542 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnRemoveComplete(int index, object value)
		{
		}

		// Token: 0x04003886 RID: 14470
		private ArrayList _list;
	}
}
