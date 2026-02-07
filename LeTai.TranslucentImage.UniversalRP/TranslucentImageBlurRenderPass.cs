using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;

namespace LeTai.Asset.TranslucentImage.UniversalRP
{
	// Token: 0x02000004 RID: 4
	[MovedFrom("LeTai.Asset.TranslucentImage.LWRP")]
	public class TranslucentImageBlurRenderPass : ScriptableRenderPass
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020BE File Offset: 0x000002BE
		public Material PreviewMaterial
		{
			get
			{
				if (!this.previewMaterial)
				{
					this.previewMaterial = CoreUtils.CreateEngineMaterial("Hidden/FillCrop_UniversalRP");
				}
				return this.previewMaterial;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E3 File Offset: 0x000002E3
		internal TranslucentImageBlurRenderPass(URPRendererInternal urpRendererInternal)
		{
			this.urpRendererInternal = urpRendererInternal;
			this.RenderGraphInit();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020F8 File Offset: 0x000002F8
		~TranslucentImageBlurRenderPass()
		{
			CoreUtils.Destroy(this.previewMaterial);
			this.RenderGraphDispose();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002130 File Offset: 0x00000330
		internal void SetupSRP(TranslucentImageBlurRenderPass.SRPassData srPassData)
		{
			this.currentSRPassData = srPassData;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002139 File Offset: 0x00000339
		internal void Setup(TranslucentImageBlurRenderPass.PassData passData)
		{
			this.currentPassData = passData;
			base.ConfigureInput(ScriptableRenderPassInput.Color);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000214C File Offset: 0x0000034C
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get("Translucent Image Source");
			RenderTargetIdentifier backBuffer = this.urpRendererInternal.GetBackBuffer();
			TranslucentImageSource blurSource = this.currentPassData.blurSource;
			bool flag = this.currentSRPassData.canvasDisappearWorkaround && renderingData.cameraData.resolveFinalTarget;
			if (this.currentPassData.shouldUpdateBlur)
			{
				blurSource.ReallocateBlurTexIfNeeded(this.currentPassData.camPixelSize);
				BlurExecutor.BlurExecutionData blurExecutionData;
				blurExecutionData..ctor(backBuffer, blurSource, this.currentPassData.blurAlgorithm);
				BlurExecutor.ExecuteBlurWithTempTextures(commandBuffer, ref blurExecutionData);
				if (flag)
				{
					CoreUtils.SetRenderTarget(commandBuffer, BuiltinRenderTextureType.CameraTarget, ClearFlag.None, 0, CubemapFace.Unknown, -1);
				}
			}
			if (this.currentPassData.isPreviewing)
			{
				RenderTargetIdentifier previewTarget = flag ? BuiltinRenderTextureType.CameraTarget : backBuffer;
				TranslucentImageBlurRenderPass.PreviewExecutionData previewExecutionData = new TranslucentImageBlurRenderPass.PreviewExecutionData(blurSource, previewTarget, this.PreviewMaterial);
				TranslucentImageBlurRenderPass.ExecutePreview(commandBuffer, ref previewExecutionData);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002228 File Offset: 0x00000428
		public static void ExecutePreview(CommandBuffer cmd, ref TranslucentImageBlurRenderPass.PreviewExecutionData data)
		{
			TranslucentImageSource blurSource = data.blurSource;
			data.previewMaterial.SetVector(ShaderId.CROP_REGION, Extensions.ToMinMaxVector(blurSource.BlurRegion));
			Extensions.BlitCustom(cmd, blurSource.BlurredScreen, data.previewTarget, data.previewMaterial, 0, false);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002276 File Offset: 0x00000476
		private void RenderGraphInit()
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002278 File Offset: 0x00000478
		private void RenderGraphDispose()
		{
		}

		// Token: 0x04000004 RID: 4
		private const string PROFILER_TAG = "Translucent Image Source";

		// Token: 0x04000005 RID: 5
		private readonly URPRendererInternal urpRendererInternal;

		// Token: 0x04000006 RID: 6
		private TranslucentImageBlurRenderPass.PassData currentPassData;

		// Token: 0x04000007 RID: 7
		private TranslucentImageBlurRenderPass.SRPassData currentSRPassData;

		// Token: 0x04000008 RID: 8
		private Material previewMaterial;

		// Token: 0x02000009 RID: 9
		internal struct PassData
		{
			// Token: 0x0400001C RID: 28
			public TranslucentImageSource blurSource;

			// Token: 0x0400001D RID: 29
			public IBlurAlgorithm blurAlgorithm;

			// Token: 0x0400001E RID: 30
			public Vector2Int camPixelSize;

			// Token: 0x0400001F RID: 31
			public bool shouldUpdateBlur;

			// Token: 0x04000020 RID: 32
			public bool isPreviewing;
		}

		// Token: 0x0200000A RID: 10
		internal struct SRPassData
		{
			// Token: 0x04000021 RID: 33
			public bool canvasDisappearWorkaround;
		}

		// Token: 0x0200000B RID: 11
		public readonly struct PreviewExecutionData
		{
			// Token: 0x0600001A RID: 26 RVA: 0x0000264D File Offset: 0x0000084D
			public PreviewExecutionData(TranslucentImageSource blurSource, RenderTargetIdentifier previewTarget, Material previewMaterial)
			{
				this.blurSource = blurSource;
				this.previewTarget = previewTarget;
				this.previewMaterial = previewMaterial;
			}

			// Token: 0x04000022 RID: 34
			public readonly TranslucentImageSource blurSource;

			// Token: 0x04000023 RID: 35
			public readonly RenderTargetIdentifier previewTarget;

			// Token: 0x04000024 RID: 36
			public readonly Material previewMaterial;
		}
	}
}
