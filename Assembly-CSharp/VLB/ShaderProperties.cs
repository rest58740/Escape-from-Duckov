using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200003F RID: 63
	public static class ShaderProperties
	{
		// Token: 0x0400017F RID: 383
		public static readonly int ConeRadius = Shader.PropertyToID("_ConeRadius");

		// Token: 0x04000180 RID: 384
		public static readonly int ConeGeomProps = Shader.PropertyToID("_ConeGeomProps");

		// Token: 0x04000181 RID: 385
		public static readonly int ColorFlat = Shader.PropertyToID("_ColorFlat");

		// Token: 0x04000182 RID: 386
		public static readonly int DistanceFallOff = Shader.PropertyToID("_DistanceFallOff");

		// Token: 0x04000183 RID: 387
		public static readonly int NoiseVelocityAndScale = Shader.PropertyToID("_NoiseVelocityAndScale");

		// Token: 0x04000184 RID: 388
		public static readonly int NoiseParam = Shader.PropertyToID("_NoiseParam");

		// Token: 0x04000185 RID: 389
		public static readonly int ColorGradientMatrix = Shader.PropertyToID("_ColorGradientMatrix");

		// Token: 0x04000186 RID: 390
		public static readonly int LocalToWorldMatrix = Shader.PropertyToID("_LocalToWorldMatrix");

		// Token: 0x04000187 RID: 391
		public static readonly int WorldToLocalMatrix = Shader.PropertyToID("_WorldToLocalMatrix");

		// Token: 0x04000188 RID: 392
		public static readonly int BlendSrcFactor = Shader.PropertyToID("_BlendSrcFactor");

		// Token: 0x04000189 RID: 393
		public static readonly int BlendDstFactor = Shader.PropertyToID("_BlendDstFactor");

		// Token: 0x0400018A RID: 394
		public static readonly int ZTest = Shader.PropertyToID("_ZTest");

		// Token: 0x0400018B RID: 395
		public static readonly int ParticlesTintColor = Shader.PropertyToID("_TintColor");

		// Token: 0x0400018C RID: 396
		public static readonly int HDRPExposureWeight = Shader.PropertyToID("_HDRPExposureWeight");

		// Token: 0x0400018D RID: 397
		public static readonly int GlobalUsesReversedZBuffer = Shader.PropertyToID("_VLB_UsesReversedZBuffer");

		// Token: 0x0400018E RID: 398
		public static readonly int GlobalNoiseTex3D = Shader.PropertyToID("_VLB_NoiseTex3D");

		// Token: 0x0400018F RID: 399
		public static readonly int GlobalNoiseCustomTime = Shader.PropertyToID("_VLB_NoiseCustomTime");

		// Token: 0x04000190 RID: 400
		public static readonly int GlobalDitheringFactor = Shader.PropertyToID("_VLB_DitheringFactor");

		// Token: 0x04000191 RID: 401
		public static readonly int GlobalDitheringNoiseTex = Shader.PropertyToID("_VLB_DitheringNoiseTex");

		// Token: 0x020000C3 RID: 195
		public static class SD
		{
			// Token: 0x040003E9 RID: 1001
			public static readonly int FadeOutFactor = Shader.PropertyToID("_FadeOutFactor");

			// Token: 0x040003EA RID: 1002
			public static readonly int ConeSlopeCosSin = Shader.PropertyToID("_ConeSlopeCosSin");

			// Token: 0x040003EB RID: 1003
			public static readonly int AlphaInside = Shader.PropertyToID("_AlphaInside");

			// Token: 0x040003EC RID: 1004
			public static readonly int AlphaOutside = Shader.PropertyToID("_AlphaOutside");

			// Token: 0x040003ED RID: 1005
			public static readonly int AttenuationLerpLinearQuad = Shader.PropertyToID("_AttenuationLerpLinearQuad");

			// Token: 0x040003EE RID: 1006
			public static readonly int DistanceCamClipping = Shader.PropertyToID("_DistanceCamClipping");

			// Token: 0x040003EF RID: 1007
			public static readonly int FresnelPow = Shader.PropertyToID("_FresnelPow");

			// Token: 0x040003F0 RID: 1008
			public static readonly int GlareBehind = Shader.PropertyToID("_GlareBehind");

			// Token: 0x040003F1 RID: 1009
			public static readonly int GlareFrontal = Shader.PropertyToID("_GlareFrontal");

			// Token: 0x040003F2 RID: 1010
			public static readonly int DrawCap = Shader.PropertyToID("_DrawCap");

			// Token: 0x040003F3 RID: 1011
			public static readonly int DepthBlendDistance = Shader.PropertyToID("_DepthBlendDistance");

			// Token: 0x040003F4 RID: 1012
			public static readonly int CameraParams = Shader.PropertyToID("_CameraParams");

			// Token: 0x040003F5 RID: 1013
			public static readonly int DynamicOcclusionClippingPlaneWS = Shader.PropertyToID("_DynamicOcclusionClippingPlaneWS");

			// Token: 0x040003F6 RID: 1014
			public static readonly int DynamicOcclusionClippingPlaneProps = Shader.PropertyToID("_DynamicOcclusionClippingPlaneProps");

			// Token: 0x040003F7 RID: 1015
			public static readonly int DynamicOcclusionDepthTexture = Shader.PropertyToID("_DynamicOcclusionDepthTexture");

			// Token: 0x040003F8 RID: 1016
			public static readonly int DynamicOcclusionDepthProps = Shader.PropertyToID("_DynamicOcclusionDepthProps");

			// Token: 0x040003F9 RID: 1017
			public static readonly int LocalForwardDirection = Shader.PropertyToID("_LocalForwardDirection");

			// Token: 0x040003FA RID: 1018
			public static readonly int TiltVector = Shader.PropertyToID("_TiltVector");

			// Token: 0x040003FB RID: 1019
			public static readonly int AdditionalClippingPlaneWS = Shader.PropertyToID("_AdditionalClippingPlaneWS");
		}

		// Token: 0x020000C4 RID: 196
		public static class HD
		{
			// Token: 0x040003FC RID: 1020
			public static readonly int Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x040003FD RID: 1021
			public static readonly int SideSoftness = Shader.PropertyToID("_SideSoftness");

			// Token: 0x040003FE RID: 1022
			public static readonly int CameraForwardOS = Shader.PropertyToID("_CameraForwardOS");

			// Token: 0x040003FF RID: 1023
			public static readonly int CameraForwardWS = Shader.PropertyToID("_CameraForwardWS");

			// Token: 0x04000400 RID: 1024
			public static readonly int TransformScale = Shader.PropertyToID("_TransformScale");

			// Token: 0x04000401 RID: 1025
			public static readonly int ShadowDepthTexture = Shader.PropertyToID("_ShadowDepthTexture");

			// Token: 0x04000402 RID: 1026
			public static readonly int ShadowProps = Shader.PropertyToID("_ShadowProps");

			// Token: 0x04000403 RID: 1027
			public static readonly int Jittering = Shader.PropertyToID("_Jittering");

			// Token: 0x04000404 RID: 1028
			public static readonly int CookieTexture = Shader.PropertyToID("_CookieTexture");

			// Token: 0x04000405 RID: 1029
			public static readonly int CookieProperties = Shader.PropertyToID("_CookieProperties");

			// Token: 0x04000406 RID: 1030
			public static readonly int CookiePosAndScale = Shader.PropertyToID("_CookiePosAndScale");

			// Token: 0x04000407 RID: 1031
			public static readonly int GlobalCameraBlendingDistance = Shader.PropertyToID("_VLB_CameraBlendingDistance");

			// Token: 0x04000408 RID: 1032
			public static readonly int GlobalJitteringNoiseTex = Shader.PropertyToID("_VLB_JitteringNoiseTex");
		}
	}
}
