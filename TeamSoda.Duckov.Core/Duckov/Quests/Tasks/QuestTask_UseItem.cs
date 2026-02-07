using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000370 RID: 880
	public class QuestTask_UseItem : Task
	{
		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x0006E01A File Offset: 0x0006C21A
		private ItemMetaData CachedMeta
		{
			get
			{
				if (this._cachedMeta == null)
				{
					this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.itemTypeID));
				}
				return this._cachedMeta.Value;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001ED7 RID: 7895 RVA: 0x0006E04A File Offset: 0x0006C24A
		private string descriptionFormatKey
		{
			get
			{
				return "Task_UseItem";
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001ED8 RID: 7896 RVA: 0x0006E051 File Offset: 0x0006C251
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x0006E060 File Offset: 0x0006C260
		private string ItemDisplayName
		{
			get
			{
				return this.CachedMeta.DisplayName;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001EDA RID: 7898 RVA: 0x0006E07B File Offset: 0x0006C27B
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					this.ItemDisplayName,
					this.amount,
					this.requireAmount
				});
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001EDB RID: 7899 RVA: 0x0006E09F File Offset: 0x0006C29F
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0006E0AC File Offset: 0x0006C2AC
		private void OnEnable()
		{
			Item.onUseStatic += this.OnItemUsed;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0006E0D0 File Offset: 0x0006C2D0
		private void OnDisable()
		{
			Item.onUseStatic -= this.OnItemUsed;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x0006E0F4 File Offset: 0x0006C2F4
		private void OnLevelInitialized()
		{
			if (this.resetOnLevelInitialized)
			{
				this.amount = 0;
			}
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0006E105 File Offset: 0x0006C305
		private void OnItemUsed(Item item, object user)
		{
			if (!LevelManager.Instance)
			{
				return;
			}
			if (user as CharacterMainControl == LevelManager.Instance.MainCharacter && item.TypeID == this.itemTypeID)
			{
				this.AddCount();
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0006E13F File Offset: 0x0006C33F
		private void AddCount()
		{
			if (this.amount < this.requireAmount)
			{
				this.amount++;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0006E163 File Offset: 0x0006C363
		public override object GenerateSaveData()
		{
			return this.amount;
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0006E170 File Offset: 0x0006C370
		protected override bool CheckFinished()
		{
			return this.amount >= this.requireAmount;
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0006E184 File Offset: 0x0006C384
		public override void SetupSaveData(object data)
		{
			if (data is int)
			{
				int num = (int)data;
				this.amount = num;
			}
		}

		// Token: 0x0400153E RID: 5438
		[SerializeField]
		private int requireAmount = 1;

		// Token: 0x0400153F RID: 5439
		[ItemTypeID]
		[SerializeField]
		private int itemTypeID;

		// Token: 0x04001540 RID: 5440
		[SerializeField]
		private bool resetOnLevelInitialized;

		// Token: 0x04001541 RID: 5441
		[SerializeField]
		private int amount;

		// Token: 0x04001542 RID: 5442
		private ItemMetaData? _cachedMeta;
	}
}
