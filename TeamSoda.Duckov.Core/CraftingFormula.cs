using System;
using Duckov.Economy;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001AD RID: 429
[Serializable]
public struct CraftingFormula
{
	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00036F01 File Offset: 0x00035101
	public bool IDValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.id);
		}
	}

	// Token: 0x04000B41 RID: 2881
	public string id;

	// Token: 0x04000B42 RID: 2882
	public CraftingFormula.ItemEntry result;

	// Token: 0x04000B43 RID: 2883
	public string[] tags;

	// Token: 0x04000B44 RID: 2884
	[SerializeField]
	public Cost cost;

	// Token: 0x04000B45 RID: 2885
	public bool unlockByDefault;

	// Token: 0x04000B46 RID: 2886
	public bool lockInDemo;

	// Token: 0x04000B47 RID: 2887
	public string requirePerk;

	// Token: 0x04000B48 RID: 2888
	public bool hideInIndex;

	// Token: 0x020004E0 RID: 1248
	[Serializable]
	public struct ItemEntry
	{
		// Token: 0x04001D84 RID: 7556
		[ItemTypeID]
		public int id;

		// Token: 0x04001D85 RID: 7557
		public int amount;
	}
}
