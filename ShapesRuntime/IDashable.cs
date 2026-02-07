using System;

namespace Shapes
{
	// Token: 0x02000045 RID: 69
	public interface IDashable
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000C3E RID: 3134
		// (set) Token: 0x06000C3F RID: 3135
		bool MatchDashSpacingToSize { get; set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000C40 RID: 3136
		// (set) Token: 0x06000C41 RID: 3137
		bool Dashed { get; set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000C42 RID: 3138
		// (set) Token: 0x06000C43 RID: 3139
		float DashSize { get; set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000C44 RID: 3140
		// (set) Token: 0x06000C45 RID: 3141
		float DashSpacing { get; set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000C46 RID: 3142
		// (set) Token: 0x06000C47 RID: 3143
		float DashOffset { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000C48 RID: 3144
		// (set) Token: 0x06000C49 RID: 3145
		DashSpace DashSpace { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000C4A RID: 3146
		// (set) Token: 0x06000C4B RID: 3147
		DashSnapping DashSnap { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000C4C RID: 3148
		// (set) Token: 0x06000C4D RID: 3149
		DashType DashType { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000C4E RID: 3150
		// (set) Token: 0x06000C4F RID: 3151
		float DashShapeModifier { get; set; }
	}
}
