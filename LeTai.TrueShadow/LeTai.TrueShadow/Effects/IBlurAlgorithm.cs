using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeTai.Effects
{
	// Token: 0x02000029 RID: 41
	public interface IBlurAlgorithm
	{
		// Token: 0x0600012F RID: 303
		void Configure(BlurConfig config);

		// Token: 0x06000130 RID: 304
		void Blur(CommandBuffer cmd, RenderTargetIdentifier src, Rect srcCropRegion, RenderTexture target);
	}
}
