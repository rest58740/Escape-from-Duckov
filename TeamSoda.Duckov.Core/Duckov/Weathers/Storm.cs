using System;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x02000251 RID: 593
	[Serializable]
	public class Storm
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x00047D45 File Offset: 0x00045F45
		[TimeSpan]
		private long Period
		{
			get
			{
				return this.sleepTime + this.stage1Time + this.stage2Time;
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00047D5C File Offset: 0x00045F5C
		public int GetStormLevel(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			if (num < this.sleepTime)
			{
				return 0;
			}
			if (num < this.sleepTime + this.stage1Time)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00047DA0 File Offset: 0x00045FA0
		public TimeSpan GetStormETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			if (num < this.sleepTime)
			{
				return TimeSpan.FromTicks(this.sleepTime - num);
			}
			return TimeSpan.Zero;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00047DE0 File Offset: 0x00045FE0
		public TimeSpan GetStormIOverETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			return TimeSpan.FromTicks(this.sleepTime + this.stage1Time - num);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00047E18 File Offset: 0x00046018
		public TimeSpan GetStormIIOverETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			return TimeSpan.FromTicks(this.Period - num);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00047E48 File Offset: 0x00046048
		public float GetSleepPercent(TimeSpan dayAndTime)
		{
			return (float)((dayAndTime.Ticks + this.offset) % this.Period) / (float)this.sleepTime;
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00047E68 File Offset: 0x00046068
		public float GetStormRemainPercent(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period - this.sleepTime;
			return 1f - (float)num / ((float)this.stage1Time + (float)this.stage2Time);
		}

		// Token: 0x04000E62 RID: 3682
		[SerializeField]
		[TimeSpan]
		private long offset;

		// Token: 0x04000E63 RID: 3683
		[SerializeField]
		[TimeSpan]
		private long sleepTime;

		// Token: 0x04000E64 RID: 3684
		[SerializeField]
		[TimeSpan]
		private long stage1Time;

		// Token: 0x04000E65 RID: 3685
		[SerializeField]
		[TimeSpan]
		private long stage2Time;
	}
}
