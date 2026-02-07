using System;

namespace VLB
{
	// Token: 0x02000024 RID: 36
	[Flags]
	public enum BeamProps
	{
		// Token: 0x040000BA RID: 186
		Transform = 1,
		// Token: 0x040000BB RID: 187
		Color = 2,
		// Token: 0x040000BC RID: 188
		BlendingMode = 4,
		// Token: 0x040000BD RID: 189
		Intensity = 8,
		// Token: 0x040000BE RID: 190
		SideSoftness = 16,
		// Token: 0x040000BF RID: 191
		SpotShape = 32,
		// Token: 0x040000C0 RID: 192
		FallOffAttenuation = 64,
		// Token: 0x040000C1 RID: 193
		Noise3D = 128,
		// Token: 0x040000C2 RID: 194
		SDConeGeometry = 256,
		// Token: 0x040000C3 RID: 195
		SDSoftIntersectBlendingDist = 512,
		// Token: 0x040000C4 RID: 196
		Props2D = 1024
	}
}
