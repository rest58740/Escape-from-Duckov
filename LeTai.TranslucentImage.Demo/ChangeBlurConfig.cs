using System;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x02000004 RID: 4
	[RequireComponent(typeof(TranslucentImageSource))]
	public class ChangeBlurConfig : MonoBehaviour
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002179 File Offset: 0x00000379
		private void Awake()
		{
			this.source = base.GetComponent<TranslucentImageSource>();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002187 File Offset: 0x00000387
		public void ChangeBlurStrength(float value)
		{
			((ScalableBlurConfig)this.source.BlurConfig).Strength = value;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000219F File Offset: 0x0000039F
		public void SetUpdateRate(float value)
		{
			this.source.MaxUpdateRate = value;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021AD File Offset: 0x000003AD
		public float GetUpdateRate()
		{
			return this.source.MaxUpdateRate;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021BA File Offset: 0x000003BA
		public void ChangeBlurSize(float value)
		{
			((ScalableBlurConfig)this.source.BlurConfig).Radius = value;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021D2 File Offset: 0x000003D2
		public void ChangeIteration(float value)
		{
			((ScalableBlurConfig)this.source.BlurConfig).Iteration = Mathf.RoundToInt(value);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021EF File Offset: 0x000003EF
		public void ChangeDownsample(float value)
		{
			this.source.Downsample = Mathf.RoundToInt(value);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002204 File Offset: 0x00000404
		public void ChangeVibrancy(float value)
		{
			for (int i = 0; i < this.translucentImages.Length; i++)
			{
				this.translucentImages[i].vibrancy = value;
			}
		}

		// Token: 0x04000006 RID: 6
		private TranslucentImageSource source;

		// Token: 0x04000007 RID: 7
		public TranslucentImage[] translucentImages;
	}
}
