using System;

namespace VLB
{
	// Token: 0x02000021 RID: 33
	[Flags]
	public enum ShadowUpdateRate
	{
		// Token: 0x04000099 RID: 153
		Never = 1,
		// Token: 0x0400009A RID: 154
		OnEnable = 2,
		// Token: 0x0400009B RID: 155
		OnBeamMove = 4,
		// Token: 0x0400009C RID: 156
		EveryXFrames = 8,
		// Token: 0x0400009D RID: 157
		OnBeamMoveAndEveryXFrames = 12
	}
}
