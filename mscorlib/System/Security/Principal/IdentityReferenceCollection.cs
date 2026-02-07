using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004E5 RID: 1253
	[ComVisible(false)]
	public class IdentityReferenceCollection : IEnumerable, ICollection<IdentityReference>, IEnumerable<IdentityReference>
	{
		// Token: 0x060031FB RID: 12795 RVA: 0x000B7BFC File Offset: 0x000B5DFC
		public IdentityReferenceCollection()
		{
			this._list = new ArrayList();
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x000B7C0F File Offset: 0x000B5E0F
		public IdentityReferenceCollection(int capacity)
		{
			this._list = new ArrayList(capacity);
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x000B7C23 File Offset: 0x000B5E23
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060031FE RID: 12798 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006A9 RID: 1705
		public IdentityReference this[int index]
		{
			get
			{
				if (index >= this._list.Count)
				{
					return null;
				}
				return (IdentityReference)this._list[index];
			}
			set
			{
				this._list[index] = value;
			}
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x000B7C62 File Offset: 0x000B5E62
		public void Add(IdentityReference identity)
		{
			this._list.Add(identity);
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x000B7C71 File Offset: 0x000B5E71
		public void Clear()
		{
			this._list.Clear();
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x000B7C80 File Offset: 0x000B5E80
		public bool Contains(IdentityReference identity)
		{
			using (IEnumerator enumerator = this._list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((IdentityReference)enumerator.Current).Equals(identity))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x000479FC File Offset: 0x00045BFC
		public void CopyTo(IdentityReference[] array, int offset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x000479FC File Offset: 0x00045BFC
		public IEnumerator<IdentityReference> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x000479FC File Offset: 0x00045BFC
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x000B7CE0 File Offset: 0x000B5EE0
		public bool Remove(IdentityReference identity)
		{
			foreach (object obj in this._list)
			{
				IdentityReference identityReference = (IdentityReference)obj;
				if (identityReference.Equals(identity))
				{
					this._list.Remove(identityReference);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000479FC File Offset: 0x00045BFC
		public IdentityReferenceCollection Translate(Type targetType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000479FC File Offset: 0x00045BFC
		public IdentityReferenceCollection Translate(Type targetType, bool forceSuccess)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040022C0 RID: 8896
		private ArrayList _list;
	}
}
