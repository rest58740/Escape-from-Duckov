using System;
using Duckov.Scenes;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200036C RID: 876
	public class QuestTask_ReachLocation : Task
	{
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x0006D9B6 File Offset: 0x0006BBB6
		public string descriptionFormatkey
		{
			get
			{
				return "Task_ReachLocation";
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x0006D9BD File Offset: 0x0006BBBD
		public string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatkey);
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x0006D9CA File Offset: 0x0006BBCA
		public string TargetLocationDisplayName
		{
			get
			{
				return this.location.GetDisplayName();
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x0006D9D7 File Offset: 0x0006BBD7
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					this.TargetLocationDisplayName
				});
			}
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0006D9EF File Offset: 0x0006BBEF
		private void OnEnable()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0006DA13 File Offset: 0x0006BC13
		private void Start()
		{
			this.CacheLocation();
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0006DA1B File Offset: 0x0006BC1B
		private void OnDisable()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0006DA3F File Offset: 0x0006BC3F
		protected override void OnInit()
		{
			base.OnInit();
			if (!base.IsFinished())
			{
				this.SetMapElementVisable(true);
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x0006DA56 File Offset: 0x0006BC56
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.CacheLocation();
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0006DA5E File Offset: 0x0006BC5E
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Reach location task caching";
			this.CacheLocation();
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0006DA70 File Offset: 0x0006BC70
		private void CacheLocation()
		{
			this.target = this.location.GetLocationTransform();
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0006DA84 File Offset: 0x0006BC84
		private void Update()
		{
			if (this.finished)
			{
				return;
			}
			if (this.target == null)
			{
				return;
			}
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return;
			}
			if ((main.transform.position - this.target.position).magnitude <= this.radius)
			{
				this.finished = true;
				this.SetMapElementVisable(false);
			}
			base.ReportStatusChanged();
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0006DAF8 File Offset: 0x0006BCF8
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0006DB05 File Offset: 0x0006BD05
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0006DB10 File Offset: 0x0006BD10
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x0006DB34 File Offset: 0x0006BD34
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.locations.Clear();
				this.mapElement.locations.Add(this.location);
				this.mapElement.range = this.radius;
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x0400152C RID: 5420
		[SerializeField]
		private MultiSceneLocation location;

		// Token: 0x0400152D RID: 5421
		[SerializeField]
		private float radius = 1f;

		// Token: 0x0400152E RID: 5422
		[SerializeField]
		private bool finished;

		// Token: 0x0400152F RID: 5423
		[SerializeField]
		private Transform target;

		// Token: 0x04001530 RID: 5424
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
