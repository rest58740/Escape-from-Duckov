using System;
using Duckov.MiniGames.SnakeForces;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200035A RID: 858
	public class QuestTask_Snake_Score : Task
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001DAF RID: 7599 RVA: 0x0006B511 File Offset: 0x00069711
		// (set) Token: 0x06001DB0 RID: 7600 RVA: 0x0006B518 File Offset: 0x00069718
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_Snake_Score";
			}
			set
			{
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001DB1 RID: 7601 RVA: 0x0006B51A File Offset: 0x0006971A
		public override string Description
		{
			get
			{
				return StringExtensions.Format(LocalizationManager.ToPlainText(this.descriptionKey), new
				{
					score = this.targetScore
				});
			}
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0006B537 File Offset: 0x00069737
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x0006B544 File Offset: 0x00069744
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x0006B546 File Offset: 0x00069746
		protected override bool CheckFinished()
		{
			return SnakeForce.HighScore >= this.targetScore;
		}

		// Token: 0x040014A1 RID: 5281
		[SerializeField]
		private int targetScore;

		// Token: 0x040014A2 RID: 5282
		private bool finished;
	}
}
