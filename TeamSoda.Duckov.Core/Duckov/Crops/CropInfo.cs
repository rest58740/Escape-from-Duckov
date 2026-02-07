using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002FB RID: 763
	[Serializable]
	public struct CropInfo
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x0005B2D4 File Offset: 0x000594D4
		public string DisplayName
		{
			get
			{
				if (this._normalMetaData == null)
				{
					this._normalMetaData = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.resultNormal));
				}
				return this._normalMetaData.Value.DisplayName;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x0005B317 File Offset: 0x00059517
		public TimeSpan GrowTime
		{
			get
			{
				return TimeSpan.FromTicks(this.totalGrowTicks);
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x0005B324 File Offset: 0x00059524
		public int GetProduct(ProductRanking ranking)
		{
			int num = 0;
			switch (ranking)
			{
			case ProductRanking.Poor:
				num = this.resultPoor;
				break;
			case ProductRanking.Normal:
				num = this.resultNormal;
				break;
			case ProductRanking.Good:
				num = this.resultGood;
				break;
			}
			if (num == 0)
			{
				if (this.resultNormal != 0)
				{
					return this.resultNormal;
				}
				if (this.resultPoor != 0)
				{
					return this.resultPoor;
				}
			}
			return num;
		}

		// Token: 0x0400120C RID: 4620
		public string id;

		// Token: 0x0400120D RID: 4621
		public GameObject displayPrefab;

		// Token: 0x0400120E RID: 4622
		[ItemTypeID]
		public int resultPoor;

		// Token: 0x0400120F RID: 4623
		[ItemTypeID]
		public int resultNormal;

		// Token: 0x04001210 RID: 4624
		[ItemTypeID]
		public int resultGood;

		// Token: 0x04001211 RID: 4625
		private ItemMetaData? _normalMetaData;

		// Token: 0x04001212 RID: 4626
		public int resultAmount;

		// Token: 0x04001213 RID: 4627
		[TimeSpan]
		public long totalGrowTicks;
	}
}
