using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x02000008 RID: 8
	public class ScalableBlur : IBlurAlgorithm
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020DB File Offset: 0x000002DB
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002115 File Offset: 0x00000315
		private Material Material
		{
			get
			{
				if (this.material == null)
				{
					this.Material = new Material(Shader.Find(this.isBirp ? "Hidden/EfficientBlur" : "Hidden/EfficientBlur_UniversalRP"));
				}
				return this.material;
			}
			set
			{
				this.material = value;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000211E File Offset: 0x0000031E
		public void Init(BlurConfig config, bool isBirp)
		{
			this.isBirp = isBirp;
			this.config = (ScalableBlurConfig)config;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002134 File Offset: 0x00000334
		public void Blur(CommandBuffer cmd, RenderTargetIdentifier src, Rect srcCropRegion, BackgroundFill backgroundFill, RenderTexture target)
		{
			float radius = this.ScaleWithResolution(this.config.Radius, (float)target.width * srcCropRegion.width, (float)target.height * srcCropRegion.height);
			this.ConfigMaterial(radius, srcCropRegion.ToMinMaxVector(), backgroundFill);
			int num = Mathf.Clamp(this.config.Iteration * 2 - 1, 1, this.scratches.Length * 2 - 1);
			if (num > 1)
			{
				cmd.BlitCustom(src, this.scratches[0], this.Material, 1, this.isBirp);
			}
			int max = Mathf.Min(this.config.Iteration - 1, this.scratches.Length - 1);
			for (int i = 1; i < num; i++)
			{
				int num2 = ScalableBlur.SimplePingPong(i - 1, max);
				int num3 = ScalableBlur.SimplePingPong(i, max);
				cmd.BlitCustom(this.scratches[num2], this.scratches[num3], this.Material, 0, this.isBirp);
			}
			cmd.BlitCustom((num > 1) ? this.scratches[0] : src, target, this.Material, (num > 1) ? 0 : 1, this.isBirp);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002266 File Offset: 0x00000466
		public int GetScratchesCount()
		{
			return Mathf.Min(this.config.Iteration, this.scratches.Length);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002280 File Offset: 0x00000480
		public void GetScratchDescriptor(int index, ref RenderTextureDescriptor descriptor)
		{
			if (index == 0)
			{
				int num = (this.config.Iteration > 0) ? 1 : 0;
				descriptor.width >>= num;
				descriptor.height >>= num;
			}
			else
			{
				descriptor.width >>= 1;
				descriptor.height >>= 1;
			}
			if (descriptor.width <= 0)
			{
				descriptor.width = 1;
			}
			if (descriptor.height <= 0)
			{
				descriptor.height = 1;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002303 File Offset: 0x00000503
		public void SetScratch(int index, RenderTargetIdentifier value)
		{
			this.scratches[index] = value;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002314 File Offset: 0x00000514
		protected void ConfigMaterial(float radius, Vector4 cropRegion, BackgroundFill backgroundFill)
		{
			BackgroundFillMode mode = backgroundFill.mode;
			if (mode != BackgroundFillMode.None)
			{
				if (mode == BackgroundFillMode.Color)
				{
					this.Material.EnableKeyword("BACKGROUND_FILL_COLOR");
					this.Material.DisableKeyword("BACKGROUND_FILL_NONE");
					this.Material.SetColor(ShaderId.BACKGROUND_COLOR, backgroundFill.color);
				}
			}
			else
			{
				this.Material.EnableKeyword("BACKGROUND_FILL_NONE");
				this.Material.DisableKeyword("BACKGROUND_FILL_COLOR");
			}
			this.Material.SetFloat(ShaderId.RADIUS, radius);
			this.Material.SetVector(ShaderId.CROP_REGION, cropRegion);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023AC File Offset: 0x000005AC
		private float ScaleWithResolution(float baseRadius, float width, float height)
		{
			float num = Mathf.Min(width, height) / 1080f;
			num = Mathf.Clamp(num, 0.5f, 2f);
			return baseRadius * num;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023DB File Offset: 0x000005DB
		public static int SimplePingPong(int t, int max)
		{
			if (t > max)
			{
				return 2 * max - t;
			}
			return t;
		}

		// Token: 0x04000008 RID: 8
		private const int BLUR_PASS = 0;

		// Token: 0x04000009 RID: 9
		private const int CROP_BLUR_PASS = 1;

		// Token: 0x0400000A RID: 10
		private readonly RenderTargetIdentifier[] scratches = new RenderTargetIdentifier[14];

		// Token: 0x0400000B RID: 11
		private bool isBirp;

		// Token: 0x0400000C RID: 12
		private Material material;

		// Token: 0x0400000D RID: 13
		private ScalableBlurConfig config;
	}
}
