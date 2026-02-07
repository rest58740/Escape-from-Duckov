using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005A4 RID: 1444
	[ComVisible(true)]
	public abstract class BaseChannelObjectWithProperties : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06003814 RID: 14356 RVA: 0x000C948D File Offset: 0x000C768D
		protected BaseChannelObjectWithProperties()
		{
			this.table = new Hashtable();
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06003815 RID: 14357 RVA: 0x000C94A0 File Offset: 0x000C76A0
		public virtual int Count
		{
			[SecuritySafeCritical]
			get
			{
				return this.table.Count;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06003816 RID: 14358 RVA: 0x000040F7 File Offset: 0x000022F7
		public virtual bool IsFixedSize
		{
			[SecuritySafeCritical]
			get
			{
				return true;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06003817 RID: 14359 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsReadOnly
		{
			[SecuritySafeCritical]
			get
			{
				return false;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06003818 RID: 14360 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsSynchronized
		{
			[SecuritySafeCritical]
			get
			{
				return false;
			}
		}

		// Token: 0x170007E9 RID: 2025
		public virtual object this[object key]
		{
			[SecuritySafeCritical]
			get
			{
				throw new NotImplementedException();
			}
			[SecuritySafeCritical]
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600381B RID: 14363 RVA: 0x000C94AD File Offset: 0x000C76AD
		public virtual ICollection Keys
		{
			[SecuritySafeCritical]
			get
			{
				return this.table.Keys;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x0000270D File Offset: 0x0000090D
		public virtual IDictionary Properties
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x0600381D RID: 14365 RVA: 0x0000270D File Offset: 0x0000090D
		public virtual object SyncRoot
		{
			[SecuritySafeCritical]
			get
			{
				return this;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x0600381E RID: 14366 RVA: 0x000C94BA File Offset: 0x000C76BA
		public virtual ICollection Values
		{
			[SecuritySafeCritical]
			get
			{
				return this.table.Values;
			}
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x000472CC File Offset: 0x000454CC
		[SecuritySafeCritical]
		public virtual void Add(object key, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x000472CC File Offset: 0x000454CC
		[SecuritySafeCritical]
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x000C94C7 File Offset: 0x000C76C7
		[SecuritySafeCritical]
		public virtual bool Contains(object key)
		{
			return this.table.Contains(key);
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000472CC File Offset: 0x000454CC
		[SecuritySafeCritical]
		public virtual void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000C94D5 File Offset: 0x000C76D5
		[SecuritySafeCritical]
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return this.table.GetEnumerator();
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x000C94D5 File Offset: 0x000C76D5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.table.GetEnumerator();
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000472CC File Offset: 0x000454CC
		[SecuritySafeCritical]
		public virtual void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040025CD RID: 9677
		private Hashtable table;
	}
}
