using System;

namespace Pathfinding
{
	// Token: 0x02000141 RID: 321
	[Obsolete("Has been renamed to DynamicObstacle")]
	public interface DynamicGridObstacle
	{
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060009BD RID: 2493
		// (set) Token: 0x060009BE RID: 2494
		bool enabled { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060009BF RID: 2495
		// (set) Token: 0x060009C0 RID: 2496
		float updateError { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060009C1 RID: 2497
		// (set) Token: 0x060009C2 RID: 2498
		float checkTime { get; set; }

		// Token: 0x060009C3 RID: 2499
		void DoUpdateGraphs();
	}
}
