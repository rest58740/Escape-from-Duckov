using System;

namespace VLB
{
	// Token: 0x0200003E RID: 62
	public static class ShaderKeywords
	{
		// Token: 0x0400017B RID: 379
		public const string AlphaAsBlack = "VLB_ALPHA_AS_BLACK";

		// Token: 0x0400017C RID: 380
		public const string ColorGradientMatrixLow = "VLB_COLOR_GRADIENT_MATRIX_LOW";

		// Token: 0x0400017D RID: 381
		public const string ColorGradientMatrixHigh = "VLB_COLOR_GRADIENT_MATRIX_HIGH";

		// Token: 0x0400017E RID: 382
		public const string Noise3D = "VLB_NOISE_3D";

		// Token: 0x020000C1 RID: 193
		public static class SD
		{
			// Token: 0x040003DE RID: 990
			public const string DepthBlend = "VLB_DEPTH_BLEND";

			// Token: 0x040003DF RID: 991
			public const string OcclusionClippingPlane = "VLB_OCCLUSION_CLIPPING_PLANE";

			// Token: 0x040003E0 RID: 992
			public const string OcclusionDepthTexture = "VLB_OCCLUSION_DEPTH_TEXTURE";

			// Token: 0x040003E1 RID: 993
			public const string MeshSkewing = "VLB_MESH_SKEWING";

			// Token: 0x040003E2 RID: 994
			public const string ShaderAccuracyHigh = "VLB_SHADER_ACCURACY_HIGH";
		}

		// Token: 0x020000C2 RID: 194
		public static class HD
		{
			// Token: 0x060004F3 RID: 1267 RVA: 0x00013E6D File Offset: 0x0001206D
			public static string GetRaymarchingQuality(int id)
			{
				return "VLB_RAYMARCHING_QUALITY_" + id.ToString();
			}

			// Token: 0x040003E3 RID: 995
			public const string AttenuationLinear = "VLB_ATTENUATION_LINEAR";

			// Token: 0x040003E4 RID: 996
			public const string AttenuationQuad = "VLB_ATTENUATION_QUAD";

			// Token: 0x040003E5 RID: 997
			public const string Shadow = "VLB_SHADOW";

			// Token: 0x040003E6 RID: 998
			public const string CookieSingleChannel = "VLB_COOKIE_1CHANNEL";

			// Token: 0x040003E7 RID: 999
			public const string CookieRGBA = "VLB_COOKIE_RGBA";

			// Token: 0x040003E8 RID: 1000
			public const string RaymarchingStepCount = "VLB_RAYMARCHING_STEP_COUNT";
		}
	}
}
