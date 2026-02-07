using System;

namespace VLB
{
	// Token: 0x02000023 RID: 35
	[Flags]
	public enum DirtyProps
	{
		// Token: 0x040000A5 RID: 165
		None = 0,
		// Token: 0x040000A6 RID: 166
		Intensity = 2,
		// Token: 0x040000A7 RID: 167
		HDRPExposureWeight = 4,
		// Token: 0x040000A8 RID: 168
		ColorMode = 8,
		// Token: 0x040000A9 RID: 169
		Color = 16,
		// Token: 0x040000AA RID: 170
		BlendingMode = 32,
		// Token: 0x040000AB RID: 171
		Cone = 64,
		// Token: 0x040000AC RID: 172
		SideSoftness = 128,
		// Token: 0x040000AD RID: 173
		Attenuation = 256,
		// Token: 0x040000AE RID: 174
		Dimensions = 512,
		// Token: 0x040000AF RID: 175
		RaymarchingQuality = 1024,
		// Token: 0x040000B0 RID: 176
		Jittering = 2048,
		// Token: 0x040000B1 RID: 177
		NoiseMode = 4096,
		// Token: 0x040000B2 RID: 178
		NoiseIntensity = 8192,
		// Token: 0x040000B3 RID: 179
		NoiseVelocityAndScale = 16384,
		// Token: 0x040000B4 RID: 180
		CookieProps = 32768,
		// Token: 0x040000B5 RID: 181
		ShadowProps = 65536,
		// Token: 0x040000B6 RID: 182
		AllWithoutMaterialChange = 125142,
		// Token: 0x040000B7 RID: 183
		OnlyMaterialChangeOnly = 5928,
		// Token: 0x040000B8 RID: 184
		All = 131070
	}
}
