using System;

namespace VLB
{
	// Token: 0x0200001F RID: 31
	[Flags]
	public enum DynamicOcclusionUpdateRate
	{
		// Token: 0x0400008F RID: 143
		Never = 1,
		// Token: 0x04000090 RID: 144
		OnEnable = 2,
		// Token: 0x04000091 RID: 145
		OnBeamMove = 4,
		// Token: 0x04000092 RID: 146
		EveryXFrames = 8,
		// Token: 0x04000093 RID: 147
		OnBeamMoveAndEveryXFrames = 12
	}
}
