using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002FD RID: 765
	[Serializable]
	public struct CropData
	{
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x0005B382 File Offset: 0x00059582
		public ProductRanking Ranking
		{
			get
			{
				if (this.score < 33)
				{
					return ProductRanking.Poor;
				}
				if (this.score < 66)
				{
					return ProductRanking.Normal;
				}
				return ProductRanking.Good;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x0005B39D File Offset: 0x0005959D
		public TimeSpan GrowTime
		{
			get
			{
				return TimeSpan.FromTicks(this.growTicks);
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x0005B3AA File Offset: 0x000595AA
		// (set) Token: 0x060018C6 RID: 6342 RVA: 0x0005B3B7 File Offset: 0x000595B7
		public DateTime LastUpdateDateTime
		{
			get
			{
				return DateTime.FromBinary(this.lastUpdateDateTimeRaw);
			}
			set
			{
				this.lastUpdateDateTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x04001218 RID: 4632
		public string gardenID;

		// Token: 0x04001219 RID: 4633
		public Vector2Int coord;

		// Token: 0x0400121A RID: 4634
		public string cropID;

		// Token: 0x0400121B RID: 4635
		public int score;

		// Token: 0x0400121C RID: 4636
		public bool watered;

		// Token: 0x0400121D RID: 4637
		[TimeSpan]
		public long growTicks;

		// Token: 0x0400121E RID: 4638
		[DateTime]
		public long lastUpdateDateTimeRaw;
	}
}
