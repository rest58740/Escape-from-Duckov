using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003BC RID: 956
	public class TagsDisplay : MonoBehaviour
	{
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x00077C9C File Offset: 0x00075E9C
		private PrefabPool<TagsDisplayEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<TagsDisplayEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x00077CD5 File Offset: 0x00075ED5
		private void Awake()
		{
			this.entryTemplate.gameObject.SetActive(false);
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x00077CE8 File Offset: 0x00075EE8
		public void Setup(Item item)
		{
			this.EntryPool.ReleaseAll();
			if (item == null)
			{
				return;
			}
			foreach (Tag tag in item.Tags)
			{
				if (!(tag == null) && tag.Show)
				{
					this.EntryPool.Get(null).Setup(tag);
				}
			}
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x00077D68 File Offset: 0x00075F68
		internal void Clear()
		{
			this.EntryPool.ReleaseAll();
		}

		// Token: 0x04001737 RID: 5943
		[SerializeField]
		private TagsDisplayEntry entryTemplate;

		// Token: 0x04001738 RID: 5944
		private PrefabPool<TagsDisplayEntry> _entryPool;
	}
}
