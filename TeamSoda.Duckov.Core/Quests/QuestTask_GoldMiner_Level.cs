using System;
using Duckov.MiniGames.GoldMiner;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000359 RID: 857
	public class QuestTask_GoldMiner_Level : Task
	{
		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0006B4C0 File Offset: 0x000696C0
		// (set) Token: 0x06001DA9 RID: 7593 RVA: 0x0006B4C7 File Offset: 0x000696C7
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_GoldMiner_Level";
			}
			set
			{
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001DAA RID: 7594 RVA: 0x0006B4C9 File Offset: 0x000696C9
		public override string Description
		{
			get
			{
				return StringExtensions.Format(LocalizationManager.ToPlainText(this.descriptionKey), new
				{
					level = this.targetLevel
				});
			}
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0006B4E6 File Offset: 0x000696E6
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0006B4F3 File Offset: 0x000696F3
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x0006B4F5 File Offset: 0x000696F5
		protected override bool CheckFinished()
		{
			return GoldMiner.HighLevel + 1 >= this.targetLevel;
		}

		// Token: 0x0400149F RID: 5279
		[SerializeField]
		private int targetLevel;

		// Token: 0x040014A0 RID: 5280
		private bool finished;
	}
}
