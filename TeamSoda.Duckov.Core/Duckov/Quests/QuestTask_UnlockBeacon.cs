using System;
using Duckvo.Beacons;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000352 RID: 850
	public class QuestTask_UnlockBeacon : Task
	{
		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x0006AA4B File Offset: 0x00068C4B
		// (set) Token: 0x06001D6E RID: 7534 RVA: 0x0006AA5D File Offset: 0x00068C5D
		[LocalizationKey("Default")]
		private string DescriptionKey
		{
			get
			{
				return "Task_Beacon_" + this.beaconID;
			}
			set
			{
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x0006AA5F File Offset: 0x00068C5F
		public override string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.DescriptionKey);
			}
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x0006AA6C File Offset: 0x00068C6C
		public override object GenerateSaveData()
		{
			return 0;
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x0006AA74 File Offset: 0x00068C74
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x0006AA76 File Offset: 0x00068C76
		protected override bool CheckFinished()
		{
			return BeaconManager.GetBeaconUnlocked(this.beaconID, this.beaconIndex);
		}

		// Token: 0x04001482 RID: 5250
		[SerializeField]
		private string beaconID;

		// Token: 0x04001483 RID: 5251
		[SerializeField]
		private int beaconIndex;
	}
}
