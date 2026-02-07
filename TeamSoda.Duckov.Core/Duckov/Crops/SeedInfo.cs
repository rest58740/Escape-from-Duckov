using System;
using Duckov.Utilities;
using ItemStatsSystem;

namespace Duckov.Crops
{
	// Token: 0x020002FA RID: 762
	[Serializable]
	public struct SeedInfo
	{
		// Token: 0x060018BF RID: 6335 RVA: 0x0005B2C2 File Offset: 0x000594C2
		public string GetRandomCropID()
		{
			return this.cropIDs.GetRandom(0f);
		}

		// Token: 0x0400120A RID: 4618
		[ItemTypeID]
		public int itemTypeID;

		// Token: 0x0400120B RID: 4619
		public RandomContainer<string> cropIDs;
	}
}
