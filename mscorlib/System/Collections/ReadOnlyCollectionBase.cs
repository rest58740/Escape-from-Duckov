using System;

namespace System.Collections
{
	// Token: 0x02000A2B RID: 2603
	[Serializable]
	public abstract class ReadOnlyCollectionBase : ICollection, IEnumerable
	{
		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06005C3E RID: 23614 RVA: 0x0013662B File Offset: 0x0013482B
		protected ArrayList InnerList
		{
			get
			{
				if (this._list == null)
				{
					this._list = new ArrayList();
				}
				return this._list;
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06005C3F RID: 23615 RVA: 0x00136646 File Offset: 0x00134846
		public virtual int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x00136653 File Offset: 0x00134853
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06005C41 RID: 23617 RVA: 0x00136660 File Offset: 0x00134860
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		// Token: 0x06005C42 RID: 23618 RVA: 0x000BCA7D File Offset: 0x000BAC7D
		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		// Token: 0x06005C43 RID: 23619 RVA: 0x0013666D File Offset: 0x0013486D
		public virtual IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x0400389A RID: 14490
		private ArrayList _list;
	}
}
