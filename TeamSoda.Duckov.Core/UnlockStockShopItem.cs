using System;
using Duckov.Economy;
using Duckov.PerkTrees;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class UnlockStockShopItem : PerkBehaviour
{
	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0003C1B5 File Offset: 0x0003A3B5
	private string DescriptionFormat
	{
		get
		{
			return LocalizationManager.ToPlainText("PerkBehaviour_UnlockStockShopItem");
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0003C1C1 File Offset: 0x0003A3C1
	public override string Description
	{
		get
		{
			return StringExtensions.Format(this.DescriptionFormat, new
			{
				this.ItemDisplayName
			});
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06000ECC RID: 3788 RVA: 0x0003C1DC File Offset: 0x0003A3DC
	private string ItemDisplayName
	{
		get
		{
			return ItemAssetsCollection.GetMetaData(this.itemTypeID).DisplayName;
		}
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0003C1FC File Offset: 0x0003A3FC
	private void Start()
	{
		if (base.Master.Unlocked && !EconomyManager.IsUnlocked(this.itemTypeID))
		{
			EconomyManager.Unlock(this.itemTypeID, false, false);
		}
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x0003C225 File Offset: 0x0003A425
	protected override void OnUnlocked()
	{
		base.OnUnlocked();
		EconomyManager.Unlock(this.itemTypeID, false, true);
	}

	// Token: 0x04000C47 RID: 3143
	[ItemTypeID]
	[SerializeField]
	private int itemTypeID;
}
