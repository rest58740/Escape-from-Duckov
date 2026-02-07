using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000447 RID: 1095
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntryCollection : ICollection, IEnumerable
	{
		// Token: 0x06002C6A RID: 11370 RVA: 0x0009FA9D File Offset: 0x0009DC9D
		internal KeyContainerPermissionAccessEntryCollection()
		{
			this._list = new ArrayList();
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x0009FAB0 File Offset: 0x0009DCB0
		internal KeyContainerPermissionAccessEntryCollection(KeyContainerPermissionAccessEntry[] entries)
		{
			if (entries != null)
			{
				foreach (KeyContainerPermissionAccessEntry accessEntry in entries)
				{
					this.Add(accessEntry);
				}
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002C6C RID: 11372 RVA: 0x0009FAE2 File Offset: 0x0009DCE2
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A2 RID: 1442
		public KeyContainerPermissionAccessEntry this[int index]
		{
			get
			{
				return (KeyContainerPermissionAccessEntry)this._list[index];
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x0000270D File Offset: 0x0000090D
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x0009FB02 File Offset: 0x0009DD02
		public int Add(KeyContainerPermissionAccessEntry accessEntry)
		{
			return this._list.Add(accessEntry);
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x0009FB10 File Offset: 0x0009DD10
		public void Clear()
		{
			this._list.Clear();
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x0009FB1D File Offset: 0x0009DD1D
		public void CopyTo(KeyContainerPermissionAccessEntry[] array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x0009FB1D File Offset: 0x0009DD1D
		void ICollection.CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x0009FB2C File Offset: 0x0009DD2C
		public KeyContainerPermissionAccessEntryEnumerator GetEnumerator()
		{
			return new KeyContainerPermissionAccessEntryEnumerator(this._list);
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x0009FB2C File Offset: 0x0009DD2C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new KeyContainerPermissionAccessEntryEnumerator(this._list);
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x0009FB3C File Offset: 0x0009DD3C
		public int IndexOf(KeyContainerPermissionAccessEntry accessEntry)
		{
			if (accessEntry == null)
			{
				throw new ArgumentNullException("accessEntry");
			}
			for (int i = 0; i < this._list.Count; i++)
			{
				if (accessEntry.Equals(this._list[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x0009FB84 File Offset: 0x0009DD84
		public void Remove(KeyContainerPermissionAccessEntry accessEntry)
		{
			if (accessEntry == null)
			{
				throw new ArgumentNullException("accessEntry");
			}
			for (int i = 0; i < this._list.Count; i++)
			{
				if (accessEntry.Equals(this._list[i]))
				{
					this._list.RemoveAt(i);
				}
			}
		}

		// Token: 0x04002053 RID: 8275
		private ArrayList _list;
	}
}
