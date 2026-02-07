using System;
using Duckov.Economy;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x02000374 RID: 884
	public class QuestReward_UnlockStockItem : Reward
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0006E79F File Offset: 0x0006C99F
		public int UnlockItem
		{
			get
			{
				return this.unlockItem;
			}
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0006E7A7 File Offset: 0x0006C9A7
		private ItemMetaData GetItemMeta()
		{
			return ItemAssetsCollection.GetMetaData(this.unlockItem);
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001F16 RID: 7958 RVA: 0x0006E7B4 File Offset: 0x0006C9B4
		public override Sprite Icon
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.unlockItem).icon;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x0006E7C6 File Offset: 0x0006C9C6
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_UnlockStockItem";
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001F18 RID: 7960 RVA: 0x0006E7CD File Offset: 0x0006C9CD
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001F19 RID: 7961 RVA: 0x0006E7DC File Offset: 0x0006C9DC
		private string ItemDisplayName
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.unlockItem).DisplayName;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001F1A RID: 7962 RVA: 0x0006E7FC File Offset: 0x0006C9FC
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

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x0006E814 File Offset: 0x0006CA14
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001F1C RID: 7964 RVA: 0x0006E81C File Offset: 0x0006CA1C
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0006E81F File Offset: 0x0006CA1F
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0006E82C File Offset: 0x0006CA2C
		public override void OnClaim()
		{
			EconomyManager.Unlock(this.unlockItem, true, true);
			this.claimed = true;
			base.ReportStatusChanged();
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0006E848 File Offset: 0x0006CA48
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0006E86B File Offset: 0x0006CA6B
		public override void NotifyReload(Quest questInstance)
		{
			if (questInstance.Complete)
			{
				EconomyManager.Unlock(this.unlockItem, true, true);
			}
		}

		// Token: 0x0400154D RID: 5453
		[SerializeField]
		[ItemTypeID]
		private int unlockItem;

		// Token: 0x0400154E RID: 5454
		private bool claimed;
	}
}
