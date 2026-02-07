using System;
using Duckov.Scenes;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200036A RID: 874
	public class QuestTask_Evacuate : Task
	{
		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x0006D3CE File Offset: 0x0006B5CE
		private SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x0006D3DC File Offset: 0x0006B5DC
		private SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x0006D3FB File Offset: 0x0006B5FB
		private string descriptionFormatKey
		{
			get
			{
				return "Task_Evacuate";
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x0006D402 File Offset: 0x0006B602
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001E73 RID: 7795 RVA: 0x0006D410 File Offset: 0x0006B610
		private string TargetDisplayName
		{
			get
			{
				if (this.RequireScene != null && this.RequireScene.UnsafeReason == null)
				{
					return this.RequireSceneInfo.DisplayName;
				}
				if (base.Master.RequireScene != null && base.Master.RequireScene.UnsafeReason == null)
				{
					return base.Master.RequireSceneInfo.DisplayName;
				}
				return LocalizationManager.ToPlainText("Scene_Any");
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x0006D478 File Offset: 0x0006B678
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					this.TargetDisplayName
				});
			}
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0006D490 File Offset: 0x0006B690
		private void OnEnable()
		{
			LevelManager.OnEvacuated += this.OnEvacuated;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0006D4A3 File Offset: 0x0006B6A3
		private void OnDisable()
		{
			LevelManager.OnEvacuated -= this.OnEvacuated;
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0006D4B8 File Offset: 0x0006B6B8
		private void OnEvacuated(EvacuationInfo info)
		{
			if (this.finished)
			{
				return;
			}
			if (this.RequireScene == null || this.RequireScene.UnsafeReason == 1)
			{
				if (base.Master.SceneRequirementSatisfied)
				{
					this.finished = true;
					base.ReportStatusChanged();
					return;
				}
			}
			else if (this.RequireScene.UnsafeReason == null && this.RequireScene.LoadedScene.isLoaded)
			{
				this.finished = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0006D52E File Offset: 0x0006B72E
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0006D53B File Offset: 0x0006B73B
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0006D544 File Offset: 0x0006B744
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x0400151F RID: 5407
		[SerializeField]
		[SceneID]
		private string requireSceneID;

		// Token: 0x04001520 RID: 5408
		[SerializeField]
		private bool finished;
	}
}
