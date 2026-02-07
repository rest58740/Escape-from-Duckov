using System;

namespace VLB
{
	// Token: 0x0200001C RID: 28
	public enum RenderQueue
	{
		// Token: 0x04000081 RID: 129
		Custom,
		// Token: 0x04000082 RID: 130
		Background = 1000,
		// Token: 0x04000083 RID: 131
		Geometry = 2000,
		// Token: 0x04000084 RID: 132
		AlphaTest = 2450,
		// Token: 0x04000085 RID: 133
		GeometryLast = 2500,
		// Token: 0x04000086 RID: 134
		Transparent = 3000,
		// Token: 0x04000087 RID: 135
		Overlay = 4000
	}
}
