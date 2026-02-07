using System;
using Duckov.Buildings;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000369 RID: 873
	public class QuestTask_ConstructBuilding : Task
	{
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001E68 RID: 7784 RVA: 0x0006D382 File Offset: 0x0006B582
		[LocalizationKey("Default")]
		private string descriptionFormatKey
		{
			get
			{
				return "Task_ConstructBuilding";
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x0006D389 File Offset: 0x0006B589
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x0006D396 File Offset: 0x0006B596
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					BuildingName = Building.GetDisplayName(this.buildingID)
				});
			}
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0006D3B3 File Offset: 0x0006B5B3
		public override object GenerateSaveData()
		{
			return null;
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0006D3B6 File Offset: 0x0006B5B6
		protected override bool CheckFinished()
		{
			return BuildingManager.Any(this.buildingID, false);
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0006D3C4 File Offset: 0x0006B5C4
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x0400151E RID: 5406
		[SerializeField]
		private string buildingID;
	}
}
