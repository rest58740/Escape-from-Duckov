using System;

namespace System.Collections
{
	// Token: 0x02000A25 RID: 2597
	[Serializable]
	public abstract class DictionaryBase : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06005BF7 RID: 23543 RVA: 0x001358A9 File Offset: 0x00133AA9
		protected Hashtable InnerHashtable
		{
			get
			{
				if (this._hashtable == null)
				{
					this._hashtable = new Hashtable();
				}
				return this._hashtable;
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x0000270D File Offset: 0x0000090D
		protected IDictionary Dictionary
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x001358C4 File Offset: 0x00133AC4
		public int Count
		{
			get
			{
				if (this._hashtable != null)
				{
					return this._hashtable.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06005BFA RID: 23546 RVA: 0x001358DB File Offset: 0x00133ADB
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.InnerHashtable.IsReadOnly;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06005BFB RID: 23547 RVA: 0x001358E8 File Offset: 0x00133AE8
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.InnerHashtable.IsFixedSize;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x06005BFC RID: 23548 RVA: 0x001358F5 File Offset: 0x00133AF5
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerHashtable.IsSynchronized;
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x06005BFD RID: 23549 RVA: 0x00135902 File Offset: 0x00133B02
		ICollection IDictionary.Keys
		{
			get
			{
				return this.InnerHashtable.Keys;
			}
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06005BFE RID: 23550 RVA: 0x0013590F File Offset: 0x00133B0F
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerHashtable.SyncRoot;
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06005BFF RID: 23551 RVA: 0x0013591C File Offset: 0x00133B1C
		ICollection IDictionary.Values
		{
			get
			{
				return this.InnerHashtable.Values;
			}
		}

		// Token: 0x06005C00 RID: 23552 RVA: 0x00135929 File Offset: 0x00133B29
		public void CopyTo(Array array, int index)
		{
			this.InnerHashtable.CopyTo(array, index);
		}

		// Token: 0x17000FF6 RID: 4086
		object IDictionary.this[object key]
		{
			get
			{
				object obj = this.InnerHashtable[key];
				this.OnGet(key, obj);
				return obj;
			}
			set
			{
				this.OnValidate(key, value);
				bool flag = true;
				object obj = this.InnerHashtable[key];
				if (obj == null)
				{
					flag = this.InnerHashtable.Contains(key);
				}
				this.OnSet(key, obj, value);
				this.InnerHashtable[key] = value;
				try
				{
					this.OnSetComplete(key, obj, value);
				}
				catch
				{
					if (flag)
					{
						this.InnerHashtable[key] = obj;
					}
					else
					{
						this.InnerHashtable.Remove(key);
					}
					throw;
				}
			}
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x001359E4 File Offset: 0x00133BE4
		bool IDictionary.Contains(object key)
		{
			return this.InnerHashtable.Contains(key);
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x001359F4 File Offset: 0x00133BF4
		void IDictionary.Add(object key, object value)
		{
			this.OnValidate(key, value);
			this.OnInsert(key, value);
			this.InnerHashtable.Add(key, value);
			try
			{
				this.OnInsertComplete(key, value);
			}
			catch
			{
				this.InnerHashtable.Remove(key);
				throw;
			}
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x00135A48 File Offset: 0x00133C48
		public void Clear()
		{
			this.OnClear();
			this.InnerHashtable.Clear();
			this.OnClearComplete();
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x00135A64 File Offset: 0x00133C64
		void IDictionary.Remove(object key)
		{
			if (this.InnerHashtable.Contains(key))
			{
				object value = this.InnerHashtable[key];
				this.OnValidate(key, value);
				this.OnRemove(key, value);
				this.InnerHashtable.Remove(key);
				try
				{
					this.OnRemoveComplete(key, value);
				}
				catch
				{
					this.InnerHashtable.Add(key, value);
					throw;
				}
			}
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x00135AD4 File Offset: 0x00133CD4
		public IDictionaryEnumerator GetEnumerator()
		{
			return this.InnerHashtable.GetEnumerator();
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x00135AD4 File Offset: 0x00133CD4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.InnerHashtable.GetEnumerator();
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x0008869B File Offset: 0x0008689B
		protected virtual object OnGet(object key, object currentValue)
		{
			return currentValue;
		}

		// Token: 0x06005C0A RID: 23562 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnSet(object key, object oldValue, object newValue)
		{
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnInsert(object key, object value)
		{
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnClear()
		{
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnRemove(object key, object value)
		{
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnValidate(object key, object value)
		{
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnSetComplete(object key, object oldValue, object newValue)
		{
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnInsertComplete(object key, object value)
		{
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnRemoveComplete(object key, object value)
		{
		}

		// Token: 0x04003887 RID: 14471
		private Hashtable _hashtable;
	}
}
