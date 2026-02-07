using System;
using System.Diagnostics;

namespace Pathfinding.Jobs
{
	// Token: 0x0200018B RID: 395
	public struct TimeSlice
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0003DE9B File Offset: 0x0003C09B
		public bool isInfinite
		{
			get
			{
				return this.endTick == long.MaxValue;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x0003DEAE File Offset: 0x0003C0AE
		public bool expired
		{
			get
			{
				return Stopwatch.GetTimestamp() > this.endTick;
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0003DEC0 File Offset: 0x0003C0C0
		public static TimeSlice MillisFromNow(float millis)
		{
			return new TimeSlice
			{
				endTick = Stopwatch.GetTimestamp() + (long)(millis * 10000f)
			};
		}

		// Token: 0x04000774 RID: 1908
		public long endTick;

		// Token: 0x04000775 RID: 1909
		public static readonly TimeSlice Infinite = new TimeSlice
		{
			endTick = long.MaxValue
		};
	}
}
