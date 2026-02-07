using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000034 RID: 52
	public static class Noise3D
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00006D4E File Offset: 0x00004F4E
		public static bool isSupported
		{
			get
			{
				if (!Noise3D.ms_IsSupportedChecked)
				{
					Noise3D.ms_IsSupported = (SystemInfo.graphicsShaderLevel >= 35);
					if (!Noise3D.ms_IsSupported)
					{
						Debug.LogWarning(Noise3D.isNotSupportedString);
					}
					Noise3D.ms_IsSupportedChecked = true;
				}
				return Noise3D.ms_IsSupported;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00006D84 File Offset: 0x00004F84
		public static bool isProperlyLoaded
		{
			get
			{
				return Noise3D.ms_NoiseTexture != null;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00006D91 File Offset: 0x00004F91
		public static string isNotSupportedString
		{
			get
			{
				return string.Format("3D Noise requires higher shader capabilities (Shader Model 3.5 / OpenGL ES 3.0), which are not available on the current platform: graphicsShaderLevel (current/required) = {0} / {1}", SystemInfo.graphicsShaderLevel, 35);
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006DAE File Offset: 0x00004FAE
		[RuntimeInitializeOnLoadMethod]
		private static void OnStartUp()
		{
			Noise3D.LoadIfNeeded();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public static void LoadIfNeeded()
		{
			if (!Noise3D.isSupported)
			{
				return;
			}
			if (Noise3D.ms_NoiseTexture == null)
			{
				Noise3D.ms_NoiseTexture = Config.Instance.noiseTexture3D;
				Shader.SetGlobalTexture(ShaderProperties.GlobalNoiseTex3D, Noise3D.ms_NoiseTexture);
				Shader.SetGlobalFloat(ShaderProperties.GlobalNoiseCustomTime, -1f);
			}
		}

		// Token: 0x04000119 RID: 281
		private static bool ms_IsSupportedChecked;

		// Token: 0x0400011A RID: 282
		private static bool ms_IsSupported;

		// Token: 0x0400011B RID: 283
		private static Texture3D ms_NoiseTexture;

		// Token: 0x0400011C RID: 284
		private const int kMinShaderLevel = 35;
	}
}
