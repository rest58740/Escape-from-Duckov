using System;

namespace EPOOutline
{
	// Token: 0x02000012 RID: 18
	[Flags]
	public enum OutlinableDrawingMode
	{
		// Token: 0x04000047 RID: 71
		Normal = 1,
		// Token: 0x04000048 RID: 72
		ZOnly = 2,
		// Token: 0x04000049 RID: 73
		GenericMask = 4,
		// Token: 0x0400004A RID: 74
		Obstacle = 8,
		// Token: 0x0400004B RID: 75
		Mask = 16
	}
}
