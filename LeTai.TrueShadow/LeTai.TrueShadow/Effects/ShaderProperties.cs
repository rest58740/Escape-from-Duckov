using System;
using UnityEngine;

namespace LeTai.Effects
{
	// Token: 0x0200002C RID: 44
	public static class ShaderProperties
	{
		// Token: 0x06000146 RID: 326 RVA: 0x00006BFA File Offset: 0x00004DFA
		public static void Init()
		{
			if (ShaderProperties.isInitialized)
			{
				return;
			}
			ShaderProperties.blurRadius = Shader.PropertyToID("_Radius");
			ShaderProperties.blurTextureCropRegion = Shader.PropertyToID("_CropRegion");
			ShaderProperties.isInitialized = true;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006C28 File Offset: 0x00004E28
		public static void Init(int stackDepth)
		{
			ShaderProperties.intermediateRT = new int[stackDepth * 2 - 1];
			for (int i = 0; i < ShaderProperties.intermediateRT.Length; i++)
			{
				ShaderProperties.intermediateRT[i] = Shader.PropertyToID(string.Format("TI_intermediate_rt_{0}", i));
			}
			ShaderProperties.Init();
		}

		// Token: 0x040000BE RID: 190
		private static bool isInitialized;

		// Token: 0x040000BF RID: 191
		public static int[] intermediateRT;

		// Token: 0x040000C0 RID: 192
		public static int blurRadius;

		// Token: 0x040000C1 RID: 193
		public static int blurTextureCropRegion;
	}
}
