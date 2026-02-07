using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x02000007 RID: 7
	public interface IBlurAlgorithm
	{
		// Token: 0x06000005 RID: 5
		void Init(BlurConfig config, bool isBirp);

		// Token: 0x06000006 RID: 6
		void Blur(CommandBuffer cmd, RenderTargetIdentifier src, Rect srcCropRegion, BackgroundFill backgroundFill, RenderTexture target);

		// Token: 0x06000007 RID: 7
		int GetScratchesCount();

		// Token: 0x06000008 RID: 8
		void GetScratchDescriptor(int index, ref RenderTextureDescriptor descriptor);

		// Token: 0x06000009 RID: 9
		void SetScratch(int index, RenderTargetIdentifier value);
	}
}
