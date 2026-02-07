using System;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class InventoryFilterProvider : MonoBehaviour
{
	// Token: 0x04000C8D RID: 3213
	public InventoryFilterProvider.FilterEntry[] entries;

	// Token: 0x020004FE RID: 1278
	[Serializable]
	public struct FilterEntry
	{
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002864 RID: 10340 RVA: 0x000936B2 File Offset: 0x000918B2
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.name);
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000936C0 File Offset: 0x000918C0
		private bool FilterFunction(Item item)
		{
			if (item == null)
			{
				return false;
			}
			if (this.requireTags.Length == 0)
			{
				return true;
			}
			foreach (Tag tag in this.requireTags)
			{
				if (!(tag == null) && item.Tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x00093716 File Offset: 0x00091916
		public Func<Item, bool> GetFunction()
		{
			if (this.requireTags.Length == 0)
			{
				return null;
			}
			return new Func<Item, bool>(this.FilterFunction);
		}

		// Token: 0x04001DF9 RID: 7673
		[LocalizationKey("Default")]
		public string name;

		// Token: 0x04001DFA RID: 7674
		public Sprite icon;

		// Token: 0x04001DFB RID: 7675
		public Tag[] requireTags;
	}
}
