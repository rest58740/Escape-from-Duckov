using System;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000368 RID: 872
	public class QuestTask_CheckSaveData : Task
	{
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001E5E RID: 7774 RVA: 0x0006D329 File Offset: 0x0006B529
		public string SaveDataKey
		{
			get
			{
				return this.saveDataKey;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x0006D331 File Offset: 0x0006B531
		// (set) Token: 0x06001E60 RID: 7776 RVA: 0x0006D33E File Offset: 0x0006B53E
		private bool SaveDataTrue
		{
			get
			{
				return SavesSystem.Load<bool>(this.saveDataKey);
			}
			set
			{
				SavesSystem.Save<bool>(this.saveDataKey, value);
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0006D34C File Offset: 0x0006B54C
		public override string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.description);
			}
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x0006D359 File Offset: 0x0006B559
		protected override void OnInit()
		{
			base.OnInit();
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0006D361 File Offset: 0x0006B561
		private void OnDisable()
		{
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0006D363 File Offset: 0x0006B563
		protected override bool CheckFinished()
		{
			return this.SaveDataTrue;
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x0006D36B File Offset: 0x0006B56B
		public override object GenerateSaveData()
		{
			return this.SaveDataTrue;
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0006D378 File Offset: 0x0006B578
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x0400151C RID: 5404
		[SerializeField]
		private string saveDataKey;

		// Token: 0x0400151D RID: 5405
		[SerializeField]
		[LocalizationKey("Quests")]
		private string description;
	}
}
