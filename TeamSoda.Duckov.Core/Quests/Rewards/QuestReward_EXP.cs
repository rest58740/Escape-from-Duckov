using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x02000372 RID: 882
	public class QuestReward_EXP : Reward
	{
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x0006E670 File Offset: 0x0006C870
		public int Amount
		{
			get
			{
				return this.amount;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x0006E678 File Offset: 0x0006C878
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0006E680 File Offset: 0x0006C880
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_Exp";
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x0006E687 File Offset: 0x0006C887
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0006E694 File Offset: 0x0006C894
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

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x0006E6AC File Offset: 0x0006C8AC
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x0006E6AF File Offset: 0x0006C8AF
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0006E6BC File Offset: 0x0006C8BC
		public override void OnClaim()
		{
			if (this.Claimed)
			{
				return;
			}
			if (!EXPManager.AddExp(this.amount))
			{
				return;
			}
			this.claimed = true;
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0006E6DC File Offset: 0x0006C8DC
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x04001549 RID: 5449
		[SerializeField]
		private int amount;

		// Token: 0x0400154A RID: 5450
		[SerializeField]
		private bool claimed;
	}
}
