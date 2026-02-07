using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200036E RID: 878
	public class QuestTask_TaskEvent : Task
	{
		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x0006DC99 File Offset: 0x0006BE99
		public string EventKey
		{
			get
			{
				return this.eventKey;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x0006DCA1 File Offset: 0x0006BEA1
		public override string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.description);
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0006DCAE File Offset: 0x0006BEAE
		private void OnTaskEvent(string _key)
		{
			if (_key == this.eventKey)
			{
				this.finished = true;
				this.SetMapElementVisable(false);
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0006DCD2 File Offset: 0x0006BED2
		protected override void OnInit()
		{
			base.OnInit();
			TaskEvent.OnTaskEvent += this.OnTaskEvent;
			this.SetMapElementVisable(!base.IsFinished());
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0006DCFA File Offset: 0x0006BEFA
		private void OnDisable()
		{
			TaskEvent.OnTaskEvent -= this.OnTaskEvent;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0006DD0D File Offset: 0x0006BF0D
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0006DD15 File Offset: 0x0006BF15
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0006DD24 File Offset: 0x0006BF24
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0006DD48 File Offset: 0x0006BF48
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (!this.mapElement.enabled)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x04001535 RID: 5429
		[SerializeField]
		private string eventKey;

		// Token: 0x04001536 RID: 5430
		[SerializeField]
		[LocalizationKey("Quests")]
		private string description;

		// Token: 0x04001537 RID: 5431
		private bool finished;

		// Token: 0x04001538 RID: 5432
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
