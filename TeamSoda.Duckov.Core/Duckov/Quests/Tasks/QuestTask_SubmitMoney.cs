using System;
using Duckov.Economy;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200036D RID: 877
	public class QuestTask_SubmitMoney : Task
	{
		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x0006DBBE File Offset: 0x0006BDBE
		public string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.decriptionFormatKey);
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x0006DBCB File Offset: 0x0006BDCB
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					this.money
				});
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x0006DBE3 File Offset: 0x0006BDE3
		public override bool Interactable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x0006DBE6 File Offset: 0x0006BDE6
		public override bool PossibleValidInteraction
		{
			get
			{
				return this.CheckMoneyEnough();
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x0006DBEE File Offset: 0x0006BDEE
		public override string InteractText
		{
			get
			{
				return LocalizationManager.ToPlainText(this.interactTextKey);
			}
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0006DBFC File Offset: 0x0006BDFC
		public override void Interact()
		{
			Cost cost = new Cost((long)this.money);
			if (cost.Pay(true, true))
			{
				this.submitted = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0006DC2F File Offset: 0x0006BE2F
		private bool CheckMoneyEnough()
		{
			return EconomyManager.Money >= (long)this.money;
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x0006DC42 File Offset: 0x0006BE42
		public override object GenerateSaveData()
		{
			return this.submitted;
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0006DC50 File Offset: 0x0006BE50
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.submitted = flag;
			}
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0006DC73 File Offset: 0x0006BE73
		protected override bool CheckFinished()
		{
			return this.submitted;
		}

		// Token: 0x04001531 RID: 5425
		[SerializeField]
		private int money;

		// Token: 0x04001532 RID: 5426
		[SerializeField]
		[LocalizationKey("Default")]
		private string decriptionFormatKey = "QuestTask_SubmitMoney";

		// Token: 0x04001533 RID: 5427
		[SerializeField]
		[LocalizationKey("Default")]
		private string interactTextKey = "QuestTask_SubmitMoney_Interact";

		// Token: 0x04001534 RID: 5428
		private bool submitted;
	}
}
