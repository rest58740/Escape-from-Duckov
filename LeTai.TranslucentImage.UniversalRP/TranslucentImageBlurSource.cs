using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;

namespace LeTai.Asset.TranslucentImage.UniversalRP
{
	// Token: 0x02000006 RID: 6
	[MovedFrom("LeTai.Asset.TranslucentImage.LWRP")]
	public class TranslucentImageBlurSource : ScriptableRendererFeature
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002334 File Offset: 0x00000534
		public void RegisterSource(TranslucentImageSource source)
		{
			this.blurSourceCache[source.GetComponent<Camera>()] = source;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002348 File Offset: 0x00000548
		public override void Create()
		{
			this.blurAlgorithm = new ScalableBlur();
			this.urpRendererInternal = new URPRendererInternal();
			RenderPassEvent renderPassEvent = (this.renderOrder == TranslucentImageBlurSource.RenderOrder.BeforePostProcessing) ? RenderPassEvent.BeforeRenderingPostProcessing : RenderPassEvent.AfterRenderingPostProcessing;
			this.pass = new TranslucentImageBlurRenderPass(this.urpRendererInternal)
			{
				renderPassEvent = renderPassEvent
			};
			this.blurSourceCache.Clear();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023A4 File Offset: 0x000005A4
		private void SetupSRP(ScriptableRenderer renderer)
		{
			this.urpRendererInternal.CacheRenderer(renderer);
			if (renderer is UniversalRenderer)
			{
				this.rendererType = RendererType.Universal;
			}
			else
			{
				this.rendererType = RendererType.Renderer2D;
			}
			this.pass.SetupSRP(new TranslucentImageBlurRenderPass.SRPassData
			{
				canvasDisappearWorkaround = this.canvasDisappearWorkaround
			});
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023F8 File Offset: 0x000005F8
		public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
		{
			CameraData cameraData = renderingData.cameraData;
			if (this.GetBlurSource(cameraData.camera) == null)
			{
				return;
			}
			this.SetupSRP(renderer);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002428 File Offset: 0x00000628
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			CameraData cameraData = renderingData.cameraData;
			Camera camera = renderingData.cameraData.camera;
			TranslucentImageSource blurSource = this.GetBlurSource(camera);
			if (blurSource == null)
			{
				return;
			}
			if (blurSource.BlurConfig == null)
			{
				return;
			}
			if (cameraData.cameraType != CameraType.Game)
			{
				return;
			}
			blurSource.CamRectOverride = Rect.zero;
			if (cameraData.renderType == CameraRenderType.Overlay)
			{
				Camera baseCamera = this.GetBaseCamera(camera);
				if (baseCamera)
				{
					blurSource.CamRectOverride = baseCamera.rect;
				}
			}
			this.blurAlgorithm.Init(blurSource.BlurConfig, false);
			this.pass.Setup(new TranslucentImageBlurRenderPass.PassData
			{
				blurAlgorithm = this.blurAlgorithm,
				blurSource = blurSource,
				camPixelSize = Vector2Int.RoundToInt(this.GetPixelSize(cameraData).size),
				shouldUpdateBlur = blurSource.ShouldUpdateBlur(),
				isPreviewing = blurSource.Preview
			});
			renderer.EnqueuePass(this.pass);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002521 File Offset: 0x00000721
		private TranslucentImageSource GetBlurSource(Camera camera)
		{
			if (!this.blurSourceCache.ContainsKey(camera))
			{
				this.blurSourceCache.Add(camera, camera.GetComponent<TranslucentImageSource>());
			}
			return this.blurSourceCache[camera];
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002550 File Offset: 0x00000750
		private Camera GetBaseCamera(Camera camera)
		{
			if (!this.baseCameraCache.ContainsKey(camera))
			{
				Camera value = null;
				foreach (UniversalAdditionalCameraData universalAdditionalCameraData in Shims.FindObjectsOfType<UniversalAdditionalCameraData>(false))
				{
					if (universalAdditionalCameraData.renderType == CameraRenderType.Base && universalAdditionalCameraData.cameraStack != null && universalAdditionalCameraData.cameraStack.Contains(camera))
					{
						value = universalAdditionalCameraData.GetComponent<Camera>();
					}
				}
				this.baseCameraCache.Add(camera, value);
			}
			return this.baseCameraCache[camera];
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000025C4 File Offset: 0x000007C4
		public Rect GetPixelSize(CameraData cameraData)
		{
			if (cameraData.renderType == CameraRenderType.Base)
			{
				return cameraData.camera.pixelRect;
			}
			if (this.cameraDataPixelRectField == null)
			{
				Debug.LogError("CameraData.pixelRect does not exists in this version of URP. Please report a bug.");
			}
			return (Rect)this.cameraDataPixelRectField.GetValue(cameraData);
		}

		// Token: 0x0400000C RID: 12
		public TranslucentImageBlurSource.RenderOrder renderOrder;

		// Token: 0x0400000D RID: 13
		public bool canvasDisappearWorkaround;

		// Token: 0x0400000E RID: 14
		internal RendererType rendererType;

		// Token: 0x0400000F RID: 15
		private readonly Dictionary<Camera, TranslucentImageSource> blurSourceCache = new Dictionary<Camera, TranslucentImageSource>();

		// Token: 0x04000010 RID: 16
		private readonly Dictionary<Camera, Camera> baseCameraCache = new Dictionary<Camera, Camera>();

		// Token: 0x04000011 RID: 17
		private URPRendererInternal urpRendererInternal;

		// Token: 0x04000012 RID: 18
		private TranslucentImageBlurRenderPass pass;

		// Token: 0x04000013 RID: 19
		private IBlurAlgorithm blurAlgorithm;

		// Token: 0x04000014 RID: 20
		private readonly FieldInfo cameraDataPixelRectField = typeof(CameraData).GetField("pixelRect", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0200000D RID: 13
		public enum RenderOrder
		{
			// Token: 0x04000028 RID: 40
			AfterPostProcessing,
			// Token: 0x04000029 RID: 41
			BeforePostProcessing
		}
	}
}
