using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace EPOOutline
{
	// Token: 0x02000015 RID: 21
	[ExecuteAlways]
	[RequireComponent(typeof(Camera))]
	public class Outliner : MonoBehaviour
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00005394 File Offset: 0x00003594
		private OutlineParameters Parameters
		{
			get
			{
				OutlineParameters result;
				if ((result = this.parameters) == null)
				{
					result = (this.parameters = new OutlineParameters(new BasicCommandBufferWrapper(new CommandBuffer())));
				}
				return result;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000053C3 File Offset: 0x000035C3
		private CameraEvent Event
		{
			get
			{
				if (this.stage != RenderStage.BeforeTransparents)
				{
					return CameraEvent.BeforeImageEffects;
				}
				return CameraEvent.AfterForwardOpaque;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000053D2 File Offset: 0x000035D2
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000053DA File Offset: 0x000035DA
		public int PrimarySizeReference
		{
			get
			{
				return this.primarySizeReference;
			}
			set
			{
				this.primarySizeReference = ((value < 10) ? 50 : value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000053EC File Offset: 0x000035EC
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000053F4 File Offset: 0x000035F4
		public BufferSizeMode PrimaryBufferSizeMode
		{
			get
			{
				return this.primaryBufferSizeMode;
			}
			set
			{
				this.primaryBufferSizeMode = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000053FD File Offset: 0x000035FD
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00005405 File Offset: 0x00003605
		public OutlineRenderingStrategy RenderingStrategy
		{
			get
			{
				return this.renderingStrategy;
			}
			set
			{
				this.renderingStrategy = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000540E File Offset: 0x0000360E
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00005416 File Offset: 0x00003616
		public RenderStage RenderStage
		{
			get
			{
				return this.stage;
			}
			set
			{
				this.stage = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000541F File Offset: 0x0000361F
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00005427 File Offset: 0x00003627
		public DilateQuality DilateQuality
		{
			get
			{
				return this.dilateQuality;
			}
			set
			{
				this.dilateQuality = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00005430 File Offset: 0x00003630
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00005438 File Offset: 0x00003638
		public RenderingMode RenderingMode
		{
			get
			{
				return this.renderingMode;
			}
			set
			{
				this.renderingMode = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00005441 File Offset: 0x00003641
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00005449 File Offset: 0x00003649
		public float BlurShift
		{
			get
			{
				return this.blurShift;
			}
			set
			{
				this.blurShift = Mathf.Clamp(value, 0f, 2f);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00005461 File Offset: 0x00003661
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00005469 File Offset: 0x00003669
		public float DilateShift
		{
			get
			{
				return this.dilateShift;
			}
			set
			{
				this.dilateShift = Mathf.Clamp(value, 0f, 2f);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00005481 File Offset: 0x00003681
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00005489 File Offset: 0x00003689
		public long OutlineLayerMask
		{
			get
			{
				return this.outlineLayerMask;
			}
			set
			{
				this.outlineLayerMask = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00005492 File Offset: 0x00003692
		// (set) Token: 0x06000095 RID: 149 RVA: 0x0000549A File Offset: 0x0000369A
		public float PrimaryRendererScale
		{
			get
			{
				return this.primaryRendererScale;
			}
			set
			{
				this.primaryRendererScale = Mathf.Clamp(value, 0.1f, 1f);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000054B2 File Offset: 0x000036B2
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000054BA File Offset: 0x000036BA
		public int BlurIterations
		{
			get
			{
				return this.blurIterations;
			}
			set
			{
				this.blurIterations = ((value > 0) ? value : 0);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000054CA File Offset: 0x000036CA
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000054D2 File Offset: 0x000036D2
		public BlurType BlurType
		{
			get
			{
				return this.blurType;
			}
			set
			{
				this.blurType = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000054DB File Offset: 0x000036DB
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000054E3 File Offset: 0x000036E3
		public int DilateIterations
		{
			get
			{
				return this.dilateIterations;
			}
			set
			{
				this.dilateIterations = ((value > 0) ? value : 0);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000054F4 File Offset: 0x000036F4
		private void OnValidate()
		{
			if (this.blurIterations < 0)
			{
				this.blurIterations = 0;
			}
			if (this.dilateIterations < 0)
			{
				this.dilateIterations = 0;
			}
			if (this.primarySizeReference < 10)
			{
				this.primarySizeReference = 10;
			}
			this.primaryRendererScale = Mathf.Clamp(this.primaryRendererScale, 0.1f, 1f);
			if (this.blurType < BlurType.Box)
			{
				this.blurType = BlurType.Box;
			}
			if (this.blurType > BlurType.Gaussian13x13)
			{
				this.blurType = BlurType.Gaussian13x13;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000556E File Offset: 0x0000376E
		private void OnEnable()
		{
			if (this.targetCamera == null)
			{
				this.targetCamera = base.GetComponent<Camera>();
			}
			this.targetCamera.forceIntoRenderTexture = (this.targetCamera.stereoTargetEye == StereoTargetEyeMask.None || !XRSettings.enabled);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000055AD File Offset: 0x000037AD
		private void OnDestroy()
		{
			this.Parameters.Dispose();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000055BA File Offset: 0x000037BA
		private void OnDisable()
		{
			if (this.targetCamera != null)
			{
				this.UpdateBuffer(this.targetCamera, this.Parameters.Buffer, true);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000055E4 File Offset: 0x000037E4
		private void UpdateBuffer(Camera cameraToUpdate, CommandBufferWrapper buffer, bool removeOnly)
		{
			if (RenderPipelineManager.currentPipeline != null)
			{
				return;
			}
			IUnderlyingBufferProvider underlyingBufferProvider = buffer as IUnderlyingBufferProvider;
			if (underlyingBufferProvider == null)
			{
				return;
			}
			CommandBuffer underlyingBuffer = underlyingBufferProvider.UnderlyingBuffer;
			if (underlyingBuffer == null)
			{
				return;
			}
			cameraToUpdate.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, underlyingBuffer);
			cameraToUpdate.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, underlyingBuffer);
			if (removeOnly)
			{
				return;
			}
			cameraToUpdate.AddCommandBuffer(this.Event, underlyingBuffer);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005632 File Offset: 0x00003832
		private void OnPreRender()
		{
			if (PipelineFetcher.CurrentAsset != null)
			{
				return;
			}
			this.Parameters.OutlinablesToRender.Clear();
			this.SetupOutline(this.targetCamera, this.Parameters, false);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005668 File Offset: 0x00003868
		private void SetupOutline(Camera cameraToUse, OutlineParameters parametersToUse, bool isEditor)
		{
			this.UpdateBuffer(cameraToUse, parametersToUse.Buffer, false);
			this.PrepareParameters(parametersToUse, cameraToUse, isEditor);
			parametersToUse.Buffer.Clear();
			if (this.renderingStrategy == OutlineRenderingStrategy.Default)
			{
				OutlineEffect.SetupOutline(parametersToUse);
				parametersToUse.BlitMesh = null;
				parametersToUse.MeshPool.ReleaseAllMeshes();
				return;
			}
			Outliner.temporaryOutlinables.Clear();
			Outliner.temporaryOutlinables.AddRange(parametersToUse.OutlinablesToRender);
			parametersToUse.OutlinablesToRender.Clear();
			parametersToUse.OutlinablesToRender.Add(null);
			foreach (Outlinable value in Outliner.temporaryOutlinables)
			{
				parametersToUse.OutlinablesToRender[0] = value;
				OutlineEffect.SetupOutline(parametersToUse);
				parametersToUse.BlitMesh = null;
			}
			parametersToUse.MeshPool.ReleaseAllMeshes();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000574C File Offset: 0x0000394C
		public StereoTargetEyeMask GetTargetEyeMask(Camera cameraTarget)
		{
			if (!XRUtility.IsXRActive)
			{
				return StereoTargetEyeMask.None;
			}
			return cameraTarget.stereoTargetEye;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005760 File Offset: 0x00003960
		public void UpdateSharedParameters(OutlineParameters parametersToUpdate, Camera cameraToUpdate, bool editorCamera, bool forceNative, bool forceHDR)
		{
			parametersToUpdate.DilateQuality = this.DilateQuality;
			parametersToUpdate.Camera = cameraToUpdate;
			parametersToUpdate.IsEditorCamera = editorCamera;
			parametersToUpdate.PrimaryBufferScale = (forceNative ? 1f : this.primaryRendererScale);
			if (forceNative)
			{
				parametersToUpdate.PrimaryBufferSizeMode = BufferSizeMode.Native;
			}
			else
			{
				parametersToUpdate.PrimaryBufferSizeMode = this.primaryBufferSizeMode;
				parametersToUpdate.PrimaryBufferSizeReference = this.primarySizeReference;
			}
			parametersToUpdate.BlurIterations = this.blurIterations;
			parametersToUpdate.BlurType = this.blurType;
			parametersToUpdate.DilateIterations = this.dilateIterations;
			parametersToUpdate.BlurShift = this.blurShift;
			parametersToUpdate.DilateShift = this.dilateShift;
			parametersToUpdate.UseHDR = (forceHDR || (cameraToUpdate.allowHDR && this.RenderingMode == RenderingMode.HDR));
			parametersToUpdate.EyeMask = this.GetTargetEyeMask(cameraToUpdate);
			parametersToUpdate.OutlineLayerMask = this.outlineLayerMask;
			parametersToUpdate.Prepare();
			parametersToUpdate.TextureHandleMap.Clear();
			foreach (Outlinable outlinable in parametersToUpdate.OutlinablesToRender)
			{
				for (int i = 0; i < outlinable.OutlineTargets.Count; i++)
				{
					OutlineTarget outlineTarget = outlinable.OutlineTargets[i];
					if (outlineTarget.IsValidForCutout)
					{
						Texture cutoutTexture = outlineTarget.CutoutTexture;
						RTHandle value = parametersToUpdate.RTHandlePool.Allocate(cutoutTexture);
						parametersToUpdate.TextureHandleMap[cutoutTexture] = value;
					}
					SpriteRenderer spriteRenderer = outlineTarget.Renderer as SpriteRenderer;
					if (spriteRenderer != null)
					{
						Texture2D texture = spriteRenderer.sprite.texture;
						RTHandle value2 = parametersToUpdate.RTHandlePool.Allocate(texture);
						parametersToUpdate.TextureHandleMap[texture] = value2;
					}
				}
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005920 File Offset: 0x00003B20
		public void ReplaceHandles(OutlineParameters parametersToUpdate)
		{
			Outliner.Replace(ref parametersToUpdate.Handles.Target, parametersToUpdate.TargetWidth, parametersToUpdate.TargetHeight, parametersToUpdate, (int width, int height, OutlineParameters outlineParameters) => RenderTargetUtility.GetRT(outlineParameters, width, height, "Target"));
			Outliner.Replace(ref parametersToUpdate.Handles.InfoTarget, parametersToUpdate.TargetWidth, parametersToUpdate.TargetHeight, parametersToUpdate, (int width, int height, OutlineParameters outlineParameters) => RenderTargetUtility.GetRT(outlineParameters, width, height, "Info target"));
			ValueTuple<int, int> scaledSize = parametersToUpdate.ScaledSize;
			int item = scaledSize.Item1;
			int item2 = scaledSize.Item2;
			Outliner.Replace(ref parametersToUpdate.Handles.PrimaryTarget, item, item2, parametersToUpdate, (int width, int height, OutlineParameters outlineParameters) => RenderTargetUtility.GetRT(outlineParameters, width, height, "Primary target"));
			Outliner.Replace(ref parametersToUpdate.Handles.SecondaryTarget, item, item2, parametersToUpdate, (int width, int height, OutlineParameters outlineParameters) => RenderTargetUtility.GetRT(outlineParameters, width, height, "Secondary target"));
			Outliner.Replace(ref parametersToUpdate.Handles.PrimaryInfoBufferTarget, item, item2, parametersToUpdate, (int width, int height, OutlineParameters outlineParameters) => RenderTargetUtility.GetRT(outlineParameters, width, height, "Primary info target"));
			Outliner.Replace(ref parametersToUpdate.Handles.SecondaryInfoBufferTarget, item, item2, parametersToUpdate, (int width, int height, OutlineParameters outlineParameters) => RenderTargetUtility.GetRT(outlineParameters, width, height, "Secondary info target"));
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005A80 File Offset: 0x00003C80
		private static void Replace(ref RTHandle handle, int width, int height, OutlineParameters parameters, Func<int, int, OutlineParameters, RTHandle> newHandle)
		{
			RTHandleProperties rthandleProperties;
			if (handle != null)
			{
				rthandleProperties = handle.rtHandleProperties;
				if (width == rthandleProperties.currentRenderTargetSize.x)
				{
					rthandleProperties = handle.rtHandleProperties;
					if (height == rthandleProperties.currentRenderTargetSize.y && handle.rt.descriptor.msaaSamples == parameters.Antialiasing)
					{
						return;
					}
				}
				handle.Release();
			}
			handle = newHandle(width, height, parameters);
			RTHandle rthandle = handle;
			rthandleProperties = default(RTHandleProperties);
			rthandleProperties.currentRenderTargetSize = new Vector2Int(width, height);
			rthandle.SetCustomHandleProperties(rthandleProperties);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005B10 File Offset: 0x00003D10
		private void PrepareParameters(OutlineParameters parametersToPrepare, Camera cameraToUse, bool editorCamera)
		{
			parametersToPrepare.RTHandlePool.ReleaseAll();
			parametersToPrepare.DepthTarget = parametersToPrepare.RTHandlePool.Allocate(RenderTargetUtility.ComposeTarget(parametersToPrepare, BuiltinRenderTextureType.CameraTarget));
			parametersToPrepare.Target = parametersToPrepare.RTHandlePool.Allocate(RenderTargetUtility.ComposeTarget(parametersToPrepare, BuiltinRenderTextureType.CameraTarget));
			RenderTexture renderTexture = (cameraToUse.targetTexture == null) ? cameraToUse.activeTexture : cameraToUse.targetTexture;
			if (XRUtility.IsUsingVR(parametersToPrepare))
			{
				RenderTextureDescriptor vrrenderTextureDescriptor = XRUtility.VRRenderTextureDescriptor;
				parametersToPrepare.TargetWidth = vrrenderTextureDescriptor.width;
				parametersToPrepare.TargetHeight = vrrenderTextureDescriptor.height;
			}
			else
			{
				parametersToPrepare.TargetWidth = ((renderTexture != null) ? renderTexture.width : cameraToUse.scaledPixelWidth);
				parametersToPrepare.TargetHeight = ((renderTexture != null) ? renderTexture.height : cameraToUse.scaledPixelHeight);
			}
			parametersToPrepare.Viewport = new Rect(0f, 0f, (float)parametersToPrepare.TargetWidth, (float)parametersToPrepare.TargetHeight);
			parametersToPrepare.Antialiasing = (editorCamera ? ((renderTexture == null) ? 1 : renderTexture.antiAliasing) : CameraUtility.GetMSAA(this.targetCamera));
			parametersToPrepare.Camera = cameraToUse;
			ValueTuple<int, int> scaledSize = parametersToPrepare.ScaledSize;
			parametersToPrepare.ScaledBufferWidth = scaledSize.Item1;
			parametersToPrepare.ScaledBufferHeight = scaledSize.Item2;
			Outlinable.GetAllActiveOutlinables(parametersToPrepare.OutlinablesToRender);
			RendererFilteringUtility.Filter(parametersToPrepare.Camera, parametersToPrepare);
			this.UpdateSharedParameters(parametersToPrepare, cameraToUse, editorCamera, false, false);
			this.ReplaceHandles(parametersToPrepare);
		}

		// Token: 0x0400008D RID: 141
		private static List<Outlinable> temporaryOutlinables = new List<Outlinable>();

		// Token: 0x0400008E RID: 142
		private OutlineParameters parameters;

		// Token: 0x0400008F RID: 143
		private Camera targetCamera;

		// Token: 0x04000090 RID: 144
		[SerializeField]
		private RenderStage stage = RenderStage.AfterTransparents;

		// Token: 0x04000091 RID: 145
		[SerializeField]
		private OutlineRenderingStrategy renderingStrategy;

		// Token: 0x04000092 RID: 146
		[SerializeField]
		private RenderingMode renderingMode;

		// Token: 0x04000093 RID: 147
		[SerializeField]
		private long outlineLayerMask = -1L;

		// Token: 0x04000094 RID: 148
		[SerializeField]
		private BufferSizeMode primaryBufferSizeMode;

		// Token: 0x04000095 RID: 149
		[SerializeField]
		[Range(0.15f, 1f)]
		private float primaryRendererScale = 0.75f;

		// Token: 0x04000096 RID: 150
		[SerializeField]
		private int primarySizeReference = 800;

		// Token: 0x04000097 RID: 151
		[SerializeField]
		[Range(0f, 2f)]
		private float blurShift = 1f;

		// Token: 0x04000098 RID: 152
		[SerializeField]
		[Range(0f, 2f)]
		private float dilateShift = 1f;

		// Token: 0x04000099 RID: 153
		[SerializeField]
		private int dilateIterations = 1;

		// Token: 0x0400009A RID: 154
		[SerializeField]
		private DilateQuality dilateQuality;

		// Token: 0x0400009B RID: 155
		[SerializeField]
		private int blurIterations = 1;

		// Token: 0x0400009C RID: 156
		[SerializeField]
		private BlurType blurType = BlurType.Box;

		// Token: 0x0400009D RID: 157
		private RTHandle target;

		// Token: 0x0400009E RID: 158
		private RTHandle primaryBuffer;

		// Token: 0x0400009F RID: 159
		private RTHandle targetBuffer;
	}
}
