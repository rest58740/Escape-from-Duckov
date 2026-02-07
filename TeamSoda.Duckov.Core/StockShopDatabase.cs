using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000159 RID: 345
[CreateAssetMenu(menuName = "Duckov/Stock Shop Database")]
public class StockShopDatabase : ScriptableObject
{
	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0002FDF0 File Offset: 0x0002DFF0
	public static StockShopDatabase Instance
	{
		get
		{
			return GameplayDataSettings.StockshopDatabase;
		}
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0002FDF8 File Offset: 0x0002DFF8
	public StockShopDatabase.MerchantProfile GetMerchantProfile(string merchantID)
	{
		return this.merchantProfiles.Find((StockShopDatabase.MerchantProfile e) => e.merchantID == merchantID);
	}

	// Token: 0x04000988 RID: 2440
	public List<StockShopDatabase.MerchantProfile> merchantProfiles;

	// Token: 0x020004C8 RID: 1224
	[Serializable]
	public class MerchantProfile
	{
		// Token: 0x04001D20 RID: 7456
		public string merchantID;

		// Token: 0x04001D21 RID: 7457
		public List<StockShopDatabase.ItemEntry> entries = new List<StockShopDatabase.ItemEntry>();
	}

	// Token: 0x020004C9 RID: 1225
	[Serializable]
	public class ItemEntry
	{
		// Token: 0x04001D22 RID: 7458
		[ItemTypeID]
		public int typeID;

		// Token: 0x04001D23 RID: 7459
		public int maxStock;

		// Token: 0x04001D24 RID: 7460
		public bool forceUnlock;

		// Token: 0x04001D25 RID: 7461
		public float priceFactor;

		// Token: 0x04001D26 RID: 7462
		public float possibility;

		// Token: 0x04001D27 RID: 7463
		public bool lockInDemo;
	}
}
