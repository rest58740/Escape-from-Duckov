using System;
using Duckov.MiniGames.BubblePoppers;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000358 RID: 856
	public class QuestTask_BubblePopper_Level : Task
	{
		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x0006B471 File Offset: 0x00069671
		// (set) Token: 0x06001DA2 RID: 7586 RVA: 0x0006B478 File Offset: 0x00069678
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_BubblePopper_Level";
			}
			set
			{
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x0006B47A File Offset: 0x0006967A
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

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0006B497 File Offset: 0x00069697
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0006B4A4 File Offset: 0x000696A4
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x0006B4A6 File Offset: 0x000696A6
		protected override bool CheckFinished()
		{
			return BubblePopper.HighLevel >= this.targetLevel;
		}

		// Token: 0x0400149D RID: 5277
		[SerializeField]
		private int targetLevel;

		// Token: 0x0400149E RID: 5278
		private bool finished;
	}
}
