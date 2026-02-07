using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x0200000C RID: 12
	public static class BlurExecutor
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000030F4 File Offset: 0x000012F4
		static BlurExecutor()
		{
			for (int i = 0; i < BlurExecutor.TEMP_RT.Length; i++)
			{
				BlurExecutor.TEMP_RT[i] = Shader.PropertyToID(string.Format("TI_intermediate_rt_{0}", i));
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000313C File Offset: 0x0000133C
		public static void ExecuteBlurWithTempTextures(CommandBuffer cmd, ref BlurExecutor.BlurExecutionData data)
		{
			int scratchesCount = data.blurAlgorithm.GetScratchesCount();
			RenderTextureDescriptor descriptor = data.blurSource.BlurredScreen.descriptor;
			descriptor.msaaSamples = 1;
			descriptor.useMipMap = false;
			descriptor.depthBufferBits = 0;
			for (int i = 0; i < scratchesCount; i++)
			{
				data.blurAlgorithm.GetScratchDescriptor(i, ref descriptor);
				cmd.GetTemporaryRT(BlurExecutor.TEMP_RT[i], descriptor, FilterMode.Bilinear);
				data.blurAlgorithm.SetScratch(i, BlurExecutor.TEMP_RT[i]);
			}
			BlurExecutor.ExecuteBlur(cmd, ref data);
			for (int j = 0; j < scratchesCount; j++)
			{
				cmd.ReleaseTemporaryRT(BlurExecutor.TEMP_RT[j]);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000031E0 File Offset: 0x000013E0
		public static void ExecuteBlur(CommandBuffer cmd, ref BlurExecutor.BlurExecutionData data)
		{
			TranslucentImageSource blurSource = data.blurSource;
			RenderTexture blurredScreen = blurSource.BlurredScreen;
			Rect blurRegion = blurSource.BlurRegion;
			data.blurAlgorithm.Blur(cmd, data.sourceTex, blurRegion, blurSource.BackgroundFill, blurredScreen);
		}

		// Token: 0x04000035 RID: 53
		private static readonly int[] TEMP_RT = new int[14];

		// Token: 0x02000012 RID: 18
		public readonly struct BlurExecutionData
		{
			// Token: 0x06000058 RID: 88 RVA: 0x00003494 File Offset: 0x00001694
			public BlurExecutionData(RenderTargetIdentifier sourceTex, TranslucentImageSource blurSource, IBlurAlgorithm blurAlgorithm)
			{
				this.sourceTex = sourceTex;
				this.blurSource = blurSource;
				this.blurAlgorithm = blurAlgorithm;
			}

			// Token: 0x04000044 RID: 68
			public readonly RenderTargetIdentifier sourceTex;

			// Token: 0x04000045 RID: 69
			public readonly TranslucentImageSource blurSource;

			// Token: 0x04000046 RID: 70
			public readonly IBlurAlgorithm blurAlgorithm;
		}
	}
}
