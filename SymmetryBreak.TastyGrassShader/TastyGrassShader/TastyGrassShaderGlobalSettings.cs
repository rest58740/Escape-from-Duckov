using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x0200000E RID: 14
	public class TastyGrassShaderGlobalSettings : ScriptableRendererFeature
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002192 File Offset: 0x00000392
		public override void Create()
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002194 File Offset: 0x00000394
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			TastyGrassShaderGlobalSettings.LastActiveInstance = this;
			TastyGrassShaderGlobalSettings.GlobalDensityScale = this.densityScale;
			TastyGrassShaderGlobalSettings.GlobalLodScale = this.lodScale;
			TastyGrassShaderGlobalSettings.GlobalLodFalloffExponent = this.lodFalloffExponent;
			TastyGrassShaderGlobalSettings.GlobalMaxBakesPerFrame = this.maxBakesPerFrame;
			TastyGrassShaderGlobalSettings.CustomRenderingMaterial = this.customRenderingMaterial;
			TastyGrassShaderGlobalSettings.NoAlphaToCoverage = this.noAlphaToCoverage;
		}

		// Token: 0x04000018 RID: 24
		public static TastyGrassShaderGlobalSettings LastActiveInstance;

		// Token: 0x04000019 RID: 25
		[Header("Visual")]
		[Tooltip("Optional custom material for rendering. If None/Null the default internal material will be used. Helpful, when using custom lighting models or other assets/effects that affect the global rendering are used. See the TgsAmplify")]
		public Material customRenderingMaterial;

		// Token: 0x0400001A RID: 26
		[Tooltip("Fixes alpha issues with XR by disabling alpha to coverage and using simple alpha clipping instead. Note that this prevents MSAA from working with the grass. May only work with the default TGS shader/customRenderingMaterial is set to null.")]
		public bool noAlphaToCoverage;

		// Token: 0x0400001B RID: 27
		[Header("Performance & Quality")]
		[Tooltip("The maximum amount of instances that are baked per frame.")]
		[Min(1f)]
		public int maxBakesPerFrame = 32;

		// Token: 0x0400001C RID: 28
		[Tooltip("Global multiplication for the amount value.")]
		[Range(0.001f, 2f)]
		public float densityScale = 1f;

		// Token: 0x0400001D RID: 29
		[Tooltip("The exponent for the internal LOD factor. Higher values will reduce the amount of blades visible at distance. This can be used to improve performance.")]
		[Range(0.5f, 10f)]
		public float lodFalloffExponent = 2.5f;

		// Token: 0x0400001E RID: 30
		[Tooltip("Global multiplication for of the LOD.")]
		[Range(0.001f, 4f)]
		public float lodScale = 1f;

		// Token: 0x0400001F RID: 31
		public static float GlobalDensityScale = 1f;

		// Token: 0x04000020 RID: 32
		public static float GlobalLodScale = 1f;

		// Token: 0x04000021 RID: 33
		public static float GlobalLodFalloffExponent = 2.5f;

		// Token: 0x04000022 RID: 34
		public static int GlobalMaxBakesPerFrame = 32;

		// Token: 0x04000023 RID: 35
		public static bool NoAlphaToCoverage;

		// Token: 0x04000024 RID: 36
		public static Material CustomRenderingMaterial;
	}
}
