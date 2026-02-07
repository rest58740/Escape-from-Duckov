using System;
using Duckov.Crops;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200025C RID: 604
	public class GardenAutoWater : PerkBehaviour, IGardenAutoWaterProvider
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x00048CC6 File Offset: 0x00046EC6
		public override string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionKey);
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00048CD3 File Offset: 0x00046ED3
		protected override void OnUnlocked()
		{
			Garden.Register(this);
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00048CDB File Offset: 0x00046EDB
		protected override void OnOnDestroy()
		{
			Garden.Unregister(this);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00048CE3 File Offset: 0x00046EE3
		public bool TakeEffect(string gardenID)
		{
			return gardenID == this.gardenID;
		}

		// Token: 0x04000E93 RID: 3731
		[SerializeField]
		[LocalizationKey("Default")]
		private string descriptionKey = "PerkBehaviour_GardenAutoWater";

		// Token: 0x04000E94 RID: 3732
		[SerializeField]
		private string gardenID = "Default";
	}
}
