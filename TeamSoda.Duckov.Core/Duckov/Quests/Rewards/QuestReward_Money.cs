using System;
using Duckov.Economy;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x02000373 RID: 883
	public class QuestReward_Money : Reward
	{
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x0006E707 File Offset: 0x0006C907
		public int Amount
		{
			get
			{
				return this.amount;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001F0B RID: 7947 RVA: 0x0006E70F File Offset: 0x0006C90F
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x0006E717 File Offset: 0x0006C917
		[SerializeField]
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_Money";
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001F0D RID: 7949 RVA: 0x0006E71E File Offset: 0x0006C91E
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x0006E72B File Offset: 0x0006C92B
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x0006E72E File Offset: 0x0006C92E
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					this.amount
				});
			}
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x0006E746 File Offset: 0x0006C946
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x0006E753 File Offset: 0x0006C953
		public override void OnClaim()
		{
			if (this.Claimed)
			{
				return;
			}
			if (!EconomyManager.Add((long)this.amount))
			{
				return;
			}
			this.claimed = true;
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0006E774 File Offset: 0x0006C974
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x0400154B RID: 5451
		[Min(0f)]
		[SerializeField]
		private int amount;

		// Token: 0x0400154C RID: 5452
		[SerializeField]
		private bool claimed;
	}
}
