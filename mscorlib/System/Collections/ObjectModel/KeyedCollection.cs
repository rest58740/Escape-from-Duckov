using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Collections.ObjectModel
{
	// Token: 0x02000A82 RID: 2690
	[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public abstract class KeyedCollection<TKey, TItem> : Collection<TItem>
	{
		// Token: 0x06006040 RID: 24640 RVA: 0x001428E9 File Offset: 0x00140AE9
		protected KeyedCollection() : this(null, 0)
		{
		}

		// Token: 0x06006041 RID: 24641 RVA: 0x001428F3 File Offset: 0x00140AF3
		protected KeyedCollection(IEqualityComparer<TKey> comparer) : this(comparer, 0)
		{
		}

		// Token: 0x06006042 RID: 24642 RVA: 0x00142900 File Offset: 0x00140B00
		protected KeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold) : base(new List<TItem>())
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			if (dictionaryCreationThreshold == -1)
			{
				dictionaryCreationThreshold = int.MaxValue;
			}
			if (dictionaryCreationThreshold < -1)
			{
				throw new ArgumentOutOfRangeException("dictionaryCreationThreshold", "The specified threshold for creating dictionary is out of range.");
			}
			this.comparer = comparer;
			this.threshold = dictionaryCreationThreshold;
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x06006043 RID: 24643 RVA: 0x0014294F File Offset: 0x00140B4F
		private new List<TItem> Items
		{
			get
			{
				return (List<TItem>)base.Items;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x06006044 RID: 24644 RVA: 0x0014295C File Offset: 0x00140B5C
		public IEqualityComparer<TKey> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x170010F9 RID: 4345
		public TItem this[TKey key]
		{
			get
			{
				TItem result;
				if (this.TryGetValue(key, out result))
				{
					return result;
				}
				throw new KeyNotFoundException(SR.Format("The given key '{0}' was not present in the dictionary.", key.ToString()));
			}
		}

		// Token: 0x06006046 RID: 24646 RVA: 0x0014299C File Offset: 0x00140B9C
		public bool Contains(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this.dict != null)
			{
				return this.dict.ContainsKey(key);
			}
			foreach (TItem item in this.Items)
			{
				if (this.comparer.Equals(this.GetKeyForItem(item), key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x00142A2C File Offset: 0x00140C2C
		public bool TryGetValue(TKey key, out TItem item)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this.dict != null)
			{
				return this.dict.TryGetValue(key, out item);
			}
			foreach (TItem titem in this.Items)
			{
				TKey keyForItem = this.GetKeyForItem(titem);
				if (keyForItem != null && this.comparer.Equals(key, keyForItem))
				{
					item = titem;
					return true;
				}
			}
			item = default(TItem);
			return false;
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x00142AD8 File Offset: 0x00140CD8
		private bool ContainsItem(TItem item)
		{
			TKey keyForItem;
			if (this.dict == null || (keyForItem = this.GetKeyForItem(item)) == null)
			{
				return this.Items.Contains(item);
			}
			TItem x;
			return this.dict.TryGetValue(keyForItem, out x) && EqualityComparer<TItem>.Default.Equals(x, item);
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x00142B28 File Offset: 0x00140D28
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this.dict != null)
			{
				TItem item;
				return this.dict.TryGetValue(key, out item) && base.Remove(item);
			}
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (this.comparer.Equals(this.GetKeyForItem(this.Items[i]), key))
				{
					this.RemoveItem(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x0600604A RID: 24650 RVA: 0x00142BAA File Offset: 0x00140DAA
		protected IDictionary<TKey, TItem> Dictionary
		{
			get
			{
				return this.dict;
			}
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x00142BB4 File Offset: 0x00140DB4
		protected void ChangeItemKey(TItem item, TKey newKey)
		{
			if (!this.ContainsItem(item))
			{
				throw new ArgumentException("The specified item does not exist in this KeyedCollection.");
			}
			TKey keyForItem = this.GetKeyForItem(item);
			if (!this.comparer.Equals(keyForItem, newKey))
			{
				if (newKey != null)
				{
					this.AddKey(newKey, item);
				}
				if (keyForItem != null)
				{
					this.RemoveKey(keyForItem);
				}
			}
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x00142C0B File Offset: 0x00140E0B
		protected override void ClearItems()
		{
			base.ClearItems();
			if (this.dict != null)
			{
				this.dict.Clear();
			}
			this.keyCount = 0;
		}

		// Token: 0x0600604D RID: 24653
		protected abstract TKey GetKeyForItem(TItem item);

		// Token: 0x0600604E RID: 24654 RVA: 0x00142C30 File Offset: 0x00140E30
		protected override void InsertItem(int index, TItem item)
		{
			TKey keyForItem = this.GetKeyForItem(item);
			if (keyForItem != null)
			{
				this.AddKey(keyForItem, item);
			}
			base.InsertItem(index, item);
		}

		// Token: 0x0600604F RID: 24655 RVA: 0x00142C60 File Offset: 0x00140E60
		protected override void RemoveItem(int index)
		{
			TKey keyForItem = this.GetKeyForItem(this.Items[index]);
			if (keyForItem != null)
			{
				this.RemoveKey(keyForItem);
			}
			base.RemoveItem(index);
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x00142C98 File Offset: 0x00140E98
		protected override void SetItem(int index, TItem item)
		{
			TKey keyForItem = this.GetKeyForItem(item);
			TKey keyForItem2 = this.GetKeyForItem(this.Items[index]);
			if (this.comparer.Equals(keyForItem2, keyForItem))
			{
				if (keyForItem != null && this.dict != null)
				{
					this.dict[keyForItem] = item;
				}
			}
			else
			{
				if (keyForItem != null)
				{
					this.AddKey(keyForItem, item);
				}
				if (keyForItem2 != null)
				{
					this.RemoveKey(keyForItem2);
				}
			}
			base.SetItem(index, item);
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x00142D18 File Offset: 0x00140F18
		private void AddKey(TKey key, TItem item)
		{
			if (this.dict != null)
			{
				this.dict.Add(key, item);
				return;
			}
			if (this.keyCount == this.threshold)
			{
				this.CreateDictionary();
				this.dict.Add(key, item);
				return;
			}
			if (this.Contains(key))
			{
				throw new ArgumentException(SR.Format("An item with the same key has already been added. Key: {0}", key));
			}
			this.keyCount++;
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x00142D8C File Offset: 0x00140F8C
		private void CreateDictionary()
		{
			this.dict = new Dictionary<TKey, TItem>(this.comparer);
			foreach (TItem titem in this.Items)
			{
				TKey keyForItem = this.GetKeyForItem(titem);
				if (keyForItem != null)
				{
					this.dict.Add(keyForItem, titem);
				}
			}
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x00142E08 File Offset: 0x00141008
		private void RemoveKey(TKey key)
		{
			if (this.dict != null)
			{
				this.dict.Remove(key);
				return;
			}
			this.keyCount--;
		}

		// Token: 0x040039AC RID: 14764
		private const int defaultThreshold = 0;

		// Token: 0x040039AD RID: 14765
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x040039AE RID: 14766
		private Dictionary<TKey, TItem> dict;

		// Token: 0x040039AF RID: 14767
		private int keyCount;

		// Token: 0x040039B0 RID: 14768
		private readonly int threshold;
	}
}
