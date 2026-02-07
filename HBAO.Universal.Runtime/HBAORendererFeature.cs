using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

namespace HorizonBasedAmbientOcclusion.Universal
{
	// Token: 0x02000004 RID: 4
	public class HBAORendererFeature : ScriptableRendererFeature
	{
		// Token: 0x0600003D RID: 61 RVA: 0x0000285B File Offset: 0x00000A5B
		private void OnDisable()
		{
			HBAORendererFeature.HBAORenderPass hbaorenderPass = this.m_HBAORenderPass;
			if (hbaorenderPass == null)
			{
				return;
			}
			hbaorenderPass.Cleanup();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002870 File Offset: 0x00000A70
		public override void Create()
		{
			if (!base.isActive)
			{
				HBAORendererFeature.HBAORenderPass hbaorenderPass = this.m_HBAORenderPass;
				if (hbaorenderPass != null)
				{
					hbaorenderPass.Cleanup();
				}
				this.m_HBAORenderPass = null;
				return;
			}
			base.name = "HBAO";
			this.m_HBAORenderPass = new HBAORendererFeature.HBAORenderPass();
			this.m_HBAORenderPass.FillSupportedRenderTextureFormats();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000028BF File Offset: 0x00000ABF
		protected override void Dispose(bool disposing)
		{
			HBAORendererFeature.HBAORenderPass hbaorenderPass = this.m_HBAORenderPass;
			if (hbaorenderPass != null)
			{
				hbaorenderPass.Cleanup();
			}
			this.m_HBAORenderPass = null;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000028DC File Offset: 0x00000ADC
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			this.shader = Shader.Find("Hidden/Universal Render Pipeline/HBAO");
			if (this.shader == null)
			{
				Debug.LogWarning("HBAO shader was not found. Please ensure it compiles correctly");
				return;
			}
			if (renderingData.cameraData.postProcessEnabled)
			{
				this.m_HBAORenderPass.Setup(this.shader, renderer, renderingData);
				renderer.EnqueuePass(this.m_HBAORenderPass);
			}
		}

		// Token: 0x0400001D RID: 29
		[SerializeField]
		[HideInInspector]
		private Shader shader;

		// Token: 0x0400001E RID: 30
		private HBAORendererFeature.HBAORenderPass m_HBAORenderPass;

		// Token: 0x02000026 RID: 38
		private class HBAORenderPass : ScriptableRenderPass
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x06000056 RID: 86 RVA: 0x00002A20 File Offset: 0x00000C20
			// (set) Token: 0x06000057 RID: 87 RVA: 0x00002A28 File Offset: 0x00000C28
			private Material material { get; set; }

			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000058 RID: 88 RVA: 0x00002A31 File Offset: 0x00000C31
			// (set) Token: 0x06000059 RID: 89 RVA: 0x00002A39 File Offset: 0x00000C39
			private RenderTargetIdentifier source { get; set; }

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600005A RID: 90 RVA: 0x00002A42 File Offset: 0x00000C42
			// (set) Token: 0x0600005B RID: 91 RVA: 0x00002A4A File Offset: 0x00000C4A
			private CameraData cameraData { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600005C RID: 92 RVA: 0x00002A53 File Offset: 0x00000C53
			// (set) Token: 0x0600005D RID: 93 RVA: 0x00002A5B File Offset: 0x00000C5B
			private RenderTextureDescriptor sourceDesc { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x0600005E RID: 94 RVA: 0x00002A64 File Offset: 0x00000C64
			// (set) Token: 0x0600005F RID: 95 RVA: 0x00002A6C File Offset: 0x00000C6C
			private RenderTextureDescriptor aoDesc { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000060 RID: 96 RVA: 0x00002A75 File Offset: 0x00000C75
			// (set) Token: 0x06000061 RID: 97 RVA: 0x00002A7D File Offset: 0x00000C7D
			private RenderTextureDescriptor deinterleavedDepthDesc { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000062 RID: 98 RVA: 0x00002A86 File Offset: 0x00000C86
			// (set) Token: 0x06000063 RID: 99 RVA: 0x00002A8E File Offset: 0x00000C8E
			private RenderTextureDescriptor deinterleavedNormalsDesc { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000064 RID: 100 RVA: 0x00002A97 File Offset: 0x00000C97
			// (set) Token: 0x06000065 RID: 101 RVA: 0x00002A9F File Offset: 0x00000C9F
			private RenderTextureDescriptor deinterleavedAoDesc { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000066 RID: 102 RVA: 0x00002AA8 File Offset: 0x00000CA8
			// (set) Token: 0x06000067 RID: 103 RVA: 0x00002AB0 File Offset: 0x00000CB0
			private RenderTextureDescriptor reinterleavedAoDesc { get; set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000068 RID: 104 RVA: 0x00002AB9 File Offset: 0x00000CB9
			// (set) Token: 0x06000069 RID: 105 RVA: 0x00002AC1 File Offset: 0x00000CC1
			private RenderTextureDescriptor ssaoDesc { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x0600006A RID: 106 RVA: 0x00002ACA File Offset: 0x00000CCA
			// (set) Token: 0x0600006B RID: 107 RVA: 0x00002AD2 File Offset: 0x00000CD2
			private RenderTextureFormat colorFormat { get; set; }

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x0600006C RID: 108 RVA: 0x00002ADB File Offset: 0x00000CDB
			// (set) Token: 0x0600006D RID: 109 RVA: 0x00002AE3 File Offset: 0x00000CE3
			private RenderTextureFormat ssaoFormat { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x0600006E RID: 110 RVA: 0x00002AEC File Offset: 0x00000CEC
			// (set) Token: 0x0600006F RID: 111 RVA: 0x00002AF4 File Offset: 0x00000CF4
			private GraphicsFormat graphicsColorFormat { get; set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000070 RID: 112 RVA: 0x00002AFD File Offset: 0x00000CFD
			// (set) Token: 0x06000071 RID: 113 RVA: 0x00002B05 File Offset: 0x00000D05
			private GraphicsFormat graphicsDepthFormat { get; set; }

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000072 RID: 114 RVA: 0x00002B0E File Offset: 0x00000D0E
			// (set) Token: 0x06000073 RID: 115 RVA: 0x00002B16 File Offset: 0x00000D16
			private GraphicsFormat graphicsNormalsFormat { get; set; }

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x06000074 RID: 116 RVA: 0x00002B1F File Offset: 0x00000D1F
			// (set) Token: 0x06000075 RID: 117 RVA: 0x00002B27 File Offset: 0x00000D27
			private RenderTextureFormat depthFormat { get; set; }

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000076 RID: 118 RVA: 0x00002B30 File Offset: 0x00000D30
			// (set) Token: 0x06000077 RID: 119 RVA: 0x00002B38 File Offset: 0x00000D38
			private RenderTextureFormat normalsFormat { get; set; }

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000078 RID: 120 RVA: 0x00002B41 File Offset: 0x00000D41
			// (set) Token: 0x06000079 RID: 121 RVA: 0x00002B49 File Offset: 0x00000D49
			private bool motionVectorsSupported { get; set; }

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x0600007A RID: 122 RVA: 0x00002B52 File Offset: 0x00000D52
			// (set) Token: 0x0600007B RID: 123 RVA: 0x00002B5A File Offset: 0x00000D5A
			private Texture2D noiseTex { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x0600007C RID: 124 RVA: 0x00002B63 File Offset: 0x00000D63
			private static bool isLinearColorSpace
			{
				get
				{
					return QualitySettings.activeColorSpace == ColorSpace.Linear;
				}
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x0600007D RID: 125 RVA: 0x00002B6D File Offset: 0x00000D6D
			private bool renderingInSceneView
			{
				get
				{
					return this.cameraData.camera.cameraType == CameraType.SceneView;
				}
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x0600007E RID: 126 RVA: 0x00002B84 File Offset: 0x00000D84
			private Mesh fullscreenTriangle
			{
				get
				{
					if (this.m_FullscreenTriangle != null)
					{
						return this.m_FullscreenTriangle;
					}
					this.m_FullscreenTriangle = new Mesh
					{
						name = "Fullscreen Triangle"
					};
					this.m_FullscreenTriangle.SetVertices(new List<Vector3>
					{
						new Vector3(-1f, -1f, 0f),
						new Vector3(-1f, 3f, 0f),
						new Vector3(3f, -1f, 0f)
					});
					this.m_FullscreenTriangle.SetIndices(new int[]
					{
						0,
						1,
						2
					}, MeshTopology.Triangles, 0, false);
					this.m_FullscreenTriangle.UploadMeshData(false);
					return this.m_FullscreenTriangle;
				}
			}

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x0600007F RID: 127 RVA: 0x00002C48 File Offset: 0x00000E48
			private MaterialPropertyBlock materialPropertyBlock
			{
				get
				{
					if (this.m_MaterialPropertyBlock != null)
					{
						return this.m_MaterialPropertyBlock;
					}
					this.m_MaterialPropertyBlock = new MaterialPropertyBlock();
					return this.m_MaterialPropertyBlock;
				}
			}

			// Token: 0x06000080 RID: 128 RVA: 0x00002C6C File Offset: 0x00000E6C
			public void FillSupportedRenderTextureFormats()
			{
				this.colorFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.Default);
				this.ssaoFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32);
				this.graphicsColorFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) ? GraphicsFormat.R16G16B16A16_SFloat : GraphicsFormat.R8G8B8A8_SRGB);
				this.graphicsDepthFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat) ? GraphicsFormat.R32_SFloat : GraphicsFormat.R16_SFloat);
				this.graphicsNormalsFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010) ? GraphicsFormat.A2R10G10B10_UNormPack32 : GraphicsFormat.R8G8B8A8_SRGB);
				this.depthFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat) ? RenderTextureFormat.RFloat : RenderTextureFormat.RHalf);
				this.normalsFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010) ? RenderTextureFormat.ARGB2101010 : RenderTextureFormat.Default);
				this.motionVectorsSupported = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf);
			}

			// Token: 0x06000081 RID: 129 RVA: 0x00002D10 File Offset: 0x00000F10
			public void Setup(Shader shader, ScriptableRenderer renderer, RenderingData renderingData)
			{
				if (this.material == null)
				{
					this.material = CoreUtils.CreateEngineMaterial(shader);
				}
				this.FetchVolumeComponent();
				ScriptableRenderPassInput scriptableRenderPassInput = ScriptableRenderPassInput.Depth;
				if (this.hbao.perPixelNormals.value == HBAO.PerPixelNormals.Camera)
				{
					scriptableRenderPassInput |= ScriptableRenderPassInput.Normal;
				}
				if (this.hbao.temporalFilterEnabled.value)
				{
					scriptableRenderPassInput |= ScriptableRenderPassInput.Motion;
				}
				base.ConfigureInput(scriptableRenderPassInput);
				base.ConfigureColorStoreAction(RenderBufferStoreAction.DontCare, 0U);
				base.renderPassEvent = ((this.hbao.debugMode.value == HBAO.DebugMode.Disabled) ? ((this.hbao.mode.value == HBAO.Mode.LitAO) ? ((this.hbao.renderingPath.value == HBAO.RenderingPath.Deferred) ? RenderPassEvent.AfterRenderingGbuffer : ((RenderPassEvent)201)) : RenderPassEvent.BeforeRenderingTransparents) : RenderPassEvent.AfterRenderingTransparents);
			}

			// Token: 0x06000082 RID: 130 RVA: 0x00002DD3 File Offset: 0x00000FD3
			public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
			{
				this.source = renderingData.cameraData.renderer.cameraColorTargetHandle;
				this.cameraData = renderingData.cameraData;
			}

			// Token: 0x06000083 RID: 131 RVA: 0x00002DFC File Offset: 0x00000FFC
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				if (this.material == null)
				{
					return;
				}
				this.FetchVolumeComponent();
				if (!this.hbao.IsActive())
				{
					return;
				}
				this.FetchRenderParameters(cameraTextureDescriptor);
				this.CheckParameters();
				this.UpdateMaterialProperties();
				this.UpdateShaderKeywords();
			}

			// Token: 0x06000084 RID: 132 RVA: 0x00002E3C File Offset: 0x0000103C
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				if (this.material == null)
				{
					Debug.LogError("HBAO material has not been correctly initialized...");
					return;
				}
				if (!this.hbao.IsActive())
				{
					return;
				}
				HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers currentCameraHistoryBuffers = this.GetCurrentCameraHistoryBuffers();
				if (currentCameraHistoryBuffers != null)
				{
					currentCameraHistoryBuffers.historyRTSystem.SwapAndSetReferenceSize(this.aoDesc.width, this.aoDesc.height);
				}
				CommandBuffer commandBuffer = CommandBufferPool.Get("HBAO");
				if (this.hbao.mode.value == HBAO.Mode.LitAO)
				{
					CoreUtils.SetKeyword(commandBuffer, "_SCREEN_SPACE_OCCLUSION", true);
					commandBuffer.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.ssaoTex, this.aoDesc, FilterMode.Bilinear);
				}
				else
				{
					commandBuffer.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.inputTex, this.sourceDesc, FilterMode.Point);
					this.CopySource(commandBuffer);
				}
				commandBuffer.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.temporalParams, (currentCameraHistoryBuffers != null) ? new Vector2(HBAORendererFeature.HBAORenderPass.s_temporalRotations[currentCameraHistoryBuffers.frameCount % 6] / 360f, HBAORendererFeature.HBAORenderPass.s_temporalOffsets[currentCameraHistoryBuffers.frameCount % 4]) : Vector2.zero);
				if (this.hbao.deinterleaving.value == HBAO.Deinterleaving.Disabled)
				{
					commandBuffer.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex, this.aoDesc, FilterMode.Bilinear);
					this.AO(commandBuffer);
				}
				else
				{
					commandBuffer.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex, this.reinterleavedAoDesc, FilterMode.Bilinear);
					this.DeinterleavedAO(commandBuffer);
				}
				this.Blur(commandBuffer);
				this.TemporalFilter(commandBuffer, currentCameraHistoryBuffers);
				this.Composite(commandBuffer);
				commandBuffer.ReleaseTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex);
				if (this.hbao.mode.value != HBAO.Mode.LitAO)
				{
					commandBuffer.ReleaseTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.inputTex);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}

			// Token: 0x06000085 RID: 133 RVA: 0x00002FCC File Offset: 0x000011CC
			public override void FrameCleanup(CommandBuffer cmd)
			{
				if (this.hbao.mode.value == HBAO.Mode.LitAO)
				{
					cmd.ReleaseTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.ssaoTex);
					CoreUtils.SetKeyword(cmd, "_SCREEN_SPACE_OCCLUSION", false);
				}
				for (int i = this.m_CameraHistoryBuffers.Count - 1; i >= 0; i--)
				{
					HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers cameraHistoryBuffers = this.m_CameraHistoryBuffers[i];
					if (Time.frameCount - cameraHistoryBuffers.lastRenderedFrame > 1)
					{
						this.ReleaseCameraHistoryBuffers(ref cameraHistoryBuffers);
					}
				}
			}

			// Token: 0x06000086 RID: 134 RVA: 0x00003040 File Offset: 0x00001240
			public void Cleanup()
			{
				for (int i = this.m_CameraHistoryBuffers.Count - 1; i >= 0; i--)
				{
					HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers cameraHistoryBuffers = this.m_CameraHistoryBuffers[i];
					this.ReleaseCameraHistoryBuffers(ref cameraHistoryBuffers);
				}
				CoreUtils.Destroy(this.material);
				CoreUtils.Destroy(this.noiseTex);
			}

			// Token: 0x06000087 RID: 135 RVA: 0x00003090 File Offset: 0x00001290
			private void FetchVolumeComponent()
			{
				if (this.hbao == null)
				{
					this.hbao = VolumeManager.instance.stack.GetComponent<HBAO>();
				}
			}

			// Token: 0x06000088 RID: 136 RVA: 0x000030B8 File Offset: 0x000012B8
			private void FetchRenderParameters(RenderTextureDescriptor cameraTextureDesc)
			{
				cameraTextureDesc.msaaSamples = 1;
				cameraTextureDesc.depthBufferBits = 0;
				this.sourceDesc = cameraTextureDesc;
				int num = cameraTextureDesc.width;
				int num2 = cameraTextureDesc.height;
				int num3 = (this.hbao.resolution.value == HBAO.Resolution.Full) ? 1 : ((this.hbao.deinterleaving.value == HBAO.Deinterleaving.Disabled) ? 2 : 1);
				if (num3 > 1)
				{
					num = (num + num % 2) / num3;
					num2 = (num2 + num2 % 2) / num3;
				}
				this.aoDesc = this.GetStereoCompatibleDescriptor(num, num2, this.colorFormat, 0, RenderTextureReadWrite.Linear);
				this.ssaoDesc = this.GetStereoCompatibleDescriptor(num, num2, this.ssaoFormat, 0, RenderTextureReadWrite.Linear);
				if (this.hbao.deinterleaving.value != HBAO.Deinterleaving.Disabled)
				{
					int num4 = cameraTextureDesc.width + ((cameraTextureDesc.width % 4 == 0) ? 0 : (4 - cameraTextureDesc.width % 4));
					int num5 = cameraTextureDesc.height + ((cameraTextureDesc.height % 4 == 0) ? 0 : (4 - cameraTextureDesc.height % 4));
					int width = num4 / 4;
					int height = num5 / 4;
					this.deinterleavedDepthDesc = this.GetStereoCompatibleDescriptor(width, height, this.depthFormat, 0, RenderTextureReadWrite.Linear);
					this.deinterleavedNormalsDesc = this.GetStereoCompatibleDescriptor(width, height, this.normalsFormat, 0, RenderTextureReadWrite.Linear);
					this.deinterleavedAoDesc = this.GetStereoCompatibleDescriptor(width, height, this.colorFormat, 0, RenderTextureReadWrite.Linear);
					this.reinterleavedAoDesc = this.GetStereoCompatibleDescriptor(num4, num5, this.colorFormat, 0, RenderTextureReadWrite.Linear);
				}
			}

			// Token: 0x06000089 RID: 137 RVA: 0x0000321C File Offset: 0x0000141C
			private RTHandle HistoryBufferAllocator(RTHandleSystem rtHandleSystem, int frameIndex)
			{
				TextureDimension textureDimension = TextureDimension.Tex2D;
				int slices = 1;
				if (XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced)
				{
					textureDimension = TextureDimension.Tex2DArray;
					slices = 2;
				}
				Vector2 one = Vector2.one;
				GraphicsFormat graphicsColorFormat = this.graphicsColorFormat;
				string name = "HBAO_HistoryBuffer_" + frameIndex.ToString();
				TextureDimension dimension = textureDimension;
				return rtHandleSystem.Alloc(one, slices, DepthBits.None, graphicsColorFormat, FilterMode.Point, TextureWrapMode.Repeat, dimension, false, false, true, false, 1, 0f, MSAASamples.None, false, true, RenderTextureMemoryless.None, VRTextureUsage.None, name);
			}

			// Token: 0x0600008A RID: 138 RVA: 0x00003280 File Offset: 0x00001480
			private void AllocCameraHistoryBuffers(ref HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers buffers)
			{
				buffers = new HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers();
				buffers.camera = this.cameraData.camera;
				buffers.frameCount = 0;
				buffers.historyRTSystem = new BufferedRTHandleSystem();
				buffers.historyRTSystem.AllocBuffer(0, new Func<RTHandleSystem, int, RTHandle>(this.HistoryBufferAllocator), 2);
				if (this.hbao.colorBleedingEnabled.value)
				{
					buffers.historyRTSystem.AllocBuffer(1, new Func<RTHandleSystem, int, RTHandle>(this.HistoryBufferAllocator), 2);
				}
				this.m_CameraHistoryBuffers.Add(buffers);
			}

			// Token: 0x0600008B RID: 139 RVA: 0x0000330D File Offset: 0x0000150D
			private void ReleaseCameraHistoryBuffers(ref HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers buffers)
			{
				buffers.historyRTSystem.ReleaseAll();
				buffers.historyRTSystem.Dispose();
				this.m_CameraHistoryBuffers.Remove(buffers);
				buffers = null;
			}

			// Token: 0x0600008C RID: 140 RVA: 0x00003338 File Offset: 0x00001538
			private HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers GetCurrentCameraHistoryBuffers()
			{
				HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers cameraHistoryBuffers = null;
				if (this.hbao.temporalFilterEnabled.value && !this.renderingInSceneView)
				{
					for (int i = 0; i < this.m_CameraHistoryBuffers.Count; i++)
					{
						if (this.m_CameraHistoryBuffers[i].camera == this.cameraData.camera)
						{
							cameraHistoryBuffers = this.m_CameraHistoryBuffers[i];
							break;
						}
					}
					if (this.m_PreviousColorBleedingEnabled == this.hbao.colorBleedingEnabled.value && this.m_PrevStereoRenderingMode == XRSettings.stereoRenderingMode)
					{
						HBAO.Resolution? previousResolution = this.m_PreviousResolution;
						HBAO.Resolution value = this.hbao.resolution.value;
						if (previousResolution.GetValueOrDefault() == value & previousResolution != null)
						{
							goto IL_102;
						}
					}
					if (cameraHistoryBuffers != null)
					{
						this.ReleaseCameraHistoryBuffers(ref cameraHistoryBuffers);
						this.m_PreviousColorBleedingEnabled = this.hbao.colorBleedingEnabled.value;
						this.m_PreviousResolution = new HBAO.Resolution?(this.hbao.resolution.value);
						this.m_PrevStereoRenderingMode = XRSettings.stereoRenderingMode;
					}
					IL_102:
					if (cameraHistoryBuffers == null)
					{
						this.AllocCameraHistoryBuffers(ref cameraHistoryBuffers);
					}
				}
				return cameraHistoryBuffers;
			}

			// Token: 0x0600008D RID: 141 RVA: 0x00003453 File Offset: 0x00001653
			private void CopySource(CommandBuffer cmd)
			{
				HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, this.source, HBAORendererFeature.HBAORenderPass.ShaderProperties.inputTex, this.material, this.fullscreenTriangle, 8);
			}

			// Token: 0x0600008E RID: 142 RVA: 0x00003478 File Offset: 0x00001678
			private void AO(CommandBuffer cmd)
			{
				HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangleWithClear(cmd, (this.hbao.mode.value == HBAO.Mode.LitAO) ? this.source : HBAORendererFeature.HBAORenderPass.ShaderProperties.inputTex, HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex, this.material, new Color(0f, 0f, 0f, 1f), this.fullscreenTriangle, 0);
			}

			// Token: 0x0600008F RID: 143 RVA: 0x000034E0 File Offset: 0x000016E0
			private void DeinterleavedAO(CommandBuffer cmd)
			{
				for (int i = 0; i < 4; i++)
				{
					this.m_RtsDepth[0] = HBAORendererFeature.HBAORenderPass.ShaderProperties.depthSliceTex[i << 2];
					this.m_RtsDepth[1] = HBAORendererFeature.HBAORenderPass.ShaderProperties.depthSliceTex[(i << 2) + 1];
					this.m_RtsDepth[2] = HBAORendererFeature.HBAORenderPass.ShaderProperties.depthSliceTex[(i << 2) + 2];
					this.m_RtsDepth[3] = HBAORendererFeature.HBAORenderPass.ShaderProperties.depthSliceTex[(i << 2) + 3];
					this.m_RtsNormals[0] = HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsSliceTex[i << 2];
					this.m_RtsNormals[1] = HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsSliceTex[(i << 2) + 1];
					this.m_RtsNormals[2] = HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsSliceTex[(i << 2) + 2];
					this.m_RtsNormals[3] = HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsSliceTex[(i << 2) + 3];
					int num = (i & 1) << 1;
					int num2 = i >> 1 << 1;
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.deinterleaveOffset[0], new Vector2((float)num, (float)num2));
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.deinterleaveOffset[1], new Vector2((float)(num + 1), (float)num2));
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.deinterleaveOffset[2], new Vector2((float)num, (float)(num2 + 1)));
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.deinterleaveOffset[3], new Vector2((float)(num + 1), (float)(num2 + 1)));
					for (int j = 0; j < 4; j++)
					{
						cmd.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.depthSliceTex[j + 4 * i], this.deinterleavedDepthDesc, FilterMode.Point);
						cmd.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsSliceTex[j + 4 * i], this.deinterleavedNormalsDesc, FilterMode.Point);
					}
					HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, this.m_RtsDepth, this.material, this.fullscreenTriangle, 2);
					HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, this.m_RtsNormals, this.material, this.fullscreenTriangle, 3);
				}
				for (int k = 0; k < 16; k++)
				{
					cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.depthTex, HBAORendererFeature.HBAORenderPass.ShaderProperties.depthSliceTex[k]);
					cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsTex, HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsSliceTex[k]);
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.jitter, HBAORendererFeature.HBAORenderPass.s_jitter[k]);
					cmd.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.aoSliceTex[k], this.deinterleavedAoDesc, FilterMode.Point);
					HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangleWithClear(cmd, (this.hbao.mode.value == HBAO.Mode.LitAO) ? this.source : HBAORendererFeature.HBAORenderPass.ShaderProperties.inputTex, HBAORendererFeature.HBAORenderPass.ShaderProperties.aoSliceTex[k], this.material, new Color(0f, 0f, 0f, 1f), this.fullscreenTriangle, 1);
					cmd.ReleaseTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.depthSliceTex[k]);
					cmd.ReleaseTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsSliceTex[k]);
				}
				cmd.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex, this.reinterleavedAoDesc, FilterMode.Point);
				for (int l = 0; l < 16; l++)
				{
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.atlasOffset, new Vector2((float)(((l & 1) + ((l & 7) >> 2 << 1)) * this.deinterleavedAoDesc.width), (float)((((l & 3) >> 1) + (l >> 3 << 1)) * this.deinterleavedAoDesc.height)));
					HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, HBAORendererFeature.HBAORenderPass.ShaderProperties.aoSliceTex[l], HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex, this.material, this.fullscreenTriangle, 4);
					cmd.ReleaseTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.aoSliceTex[l]);
				}
				HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex, HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex, this.material, this.fullscreenTriangle, 5);
				cmd.ReleaseTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex);
			}

			// Token: 0x06000090 RID: 144 RVA: 0x000038AC File Offset: 0x00001AAC
			private void Blur(CommandBuffer cmd)
			{
				if (this.hbao.blurType.value != HBAO.BlurType.None)
				{
					float num = (float)this.aoDesc.width;
					float num2 = (float)this.aoDesc.height;
					if (this.sourceDesc.useDynamicScale)
					{
						num *= ScalableBufferManager.widthScaleFactor;
						num2 *= ScalableBufferManager.heightScaleFactor;
					}
					cmd.GetTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex, this.aoDesc, FilterMode.Bilinear);
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.blurDeltaUV, new Vector2(1f / num, 0f));
					HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex, HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex, this.material, this.fullscreenTriangle, 6);
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.blurDeltaUV, new Vector2(0f, 1f / num2));
					HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex, HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex, this.material, this.fullscreenTriangle, 6);
					cmd.ReleaseTemporaryRT(HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex);
				}
			}

			// Token: 0x06000091 RID: 145 RVA: 0x000039BC File Offset: 0x00001BBC
			private void TemporalFilter(CommandBuffer cmd, HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers buffers)
			{
				if (this.hbao.temporalFilterEnabled.value && !this.renderingInSceneView && buffers != null)
				{
					cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.historyBufferRTHandleScale, buffers.historyRTSystem.rtHandleProperties.rtHandleScale);
					if (buffers.frameCount == 0)
					{
						cmd.SetRenderTarget(buffers.historyRTSystem.GetFrameRT(0, 1), 0, CubemapFace.Unknown, -1);
						cmd.ClearRenderTarget(false, true, Color.white);
						if (this.hbao.colorBleedingEnabled.value)
						{
							cmd.SetRenderTarget(buffers.historyRTSystem.GetFrameRT(1, 1), 0, CubemapFace.Unknown, -1);
							cmd.ClearRenderTarget(false, true, new Color(0f, 0f, 0f, 1f));
						}
					}
					Rect viewportRect = new Rect(Vector2.zero, buffers.historyRTSystem.rtHandleProperties.currentViewportSize);
					if (this.hbao.colorBleedingEnabled.value)
					{
						RTHandle frameRT = buffers.historyRTSystem.GetFrameRT(0, 0);
						RTHandle frameRT2 = buffers.historyRTSystem.GetFrameRT(1, 0);
						RTHandle frameRT3 = buffers.historyRTSystem.GetFrameRT(0, 1);
						RTHandle frameRT4 = buffers.historyRTSystem.GetFrameRT(1, 1);
						RenderTargetIdentifier[] destinations = new RenderTargetIdentifier[]
						{
							frameRT,
							frameRT2
						};
						cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.tempTex, frameRT4);
						HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, frameRT3, destinations, viewportRect, this.material, this.fullscreenTriangle, 7);
						cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex, frameRT2);
					}
					else
					{
						RTHandle frameRT5 = buffers.historyRTSystem.GetFrameRT(0, 0);
						RTHandle frameRT6 = buffers.historyRTSystem.GetFrameRT(0, 1);
						HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, frameRT6, frameRT5, viewportRect, this.material, this.fullscreenTriangle, 7);
						cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.hbaoTex, frameRT5);
					}
					int frameCount = buffers.frameCount;
					buffers.frameCount = frameCount + 1;
					buffers.lastRenderedFrame = Time.frameCount;
					return;
				}
				cmd.SetGlobalVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.historyBufferRTHandleScale, Vector4.one);
			}

			// Token: 0x06000092 RID: 146 RVA: 0x00003BDC File Offset: 0x00001DDC
			private void Composite(CommandBuffer cmd)
			{
				HBAORendererFeature.HBAORenderPass.BlitFullscreenTriangle(cmd, (this.hbao.mode.value == HBAO.Mode.LitAO) ? this.source : HBAORendererFeature.HBAORenderPass.ShaderProperties.inputTex, (this.hbao.mode.value == HBAO.Mode.LitAO && this.hbao.debugMode.value == HBAO.DebugMode.Disabled) ? HBAORendererFeature.HBAORenderPass.ShaderProperties.ssaoTex : this.source, this.material, this.fullscreenTriangle, (this.hbao.debugMode.value == HBAO.DebugMode.ViewNormals) ? 10 : 9);
				if (this.hbao.mode.value == HBAO.Mode.LitAO)
				{
					cmd.SetGlobalTexture("_ScreenSpaceOcclusionTexture", HBAORendererFeature.HBAORenderPass.ShaderProperties.ssaoTex);
					cmd.SetGlobalVector("_AmbientOcclusionParam", new Vector4(1f, 0f, 0f, this.hbao.directLightingStrength.value));
				}
			}

			// Token: 0x06000093 RID: 147 RVA: 0x00003CC8 File Offset: 0x00001EC8
			private void UpdateMaterialProperties()
			{
				CameraData cameraData = this.cameraData;
				int width = cameraData.cameraTargetDescriptor.width;
				cameraData = this.cameraData;
				int height = cameraData.cameraTargetDescriptor.height;
				int num = (XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced && !this.renderingInSceneView) ? 2 : 1;
				for (int i = 0; i < num; i++)
				{
					cameraData = this.cameraData;
					Matrix4x4 projectionMatrix = cameraData.GetProjectionMatrix(i);
					float m = projectionMatrix.m00;
					float m2 = projectionMatrix.m11;
					this.m_UVToViewPerEye[i] = new Vector4(2f / m, -2f / m2, -1f / m, 1f / m2);
					this.m_RadiusPerEye[i] = this.hbao.radius.value * 0.5f * ((float)(height / ((this.hbao.deinterleaving.value == HBAO.Deinterleaving.x4) ? 4 : 1)) / (2f / m2));
				}
				float num2 = Mathf.Max(16f, this.hbao.maxRadiusPixels.value * Mathf.Sqrt((float)(width * height) / 2073600f));
				num2 /= (float)((this.hbao.deinterleaving.value == HBAO.Deinterleaving.x4) ? 4 : 1);
				Vector4 value = (this.hbao.deinterleaving.value == HBAO.Deinterleaving.x4) ? new Vector4((float)this.reinterleavedAoDesc.width / (float)width, (float)this.reinterleavedAoDesc.height / (float)height, 1f / ((float)this.reinterleavedAoDesc.width / (float)width), 1f / ((float)this.reinterleavedAoDesc.height / (float)height)) : ((this.hbao.resolution.value == HBAO.Resolution.Half) ? new Vector4(((float)width + 0.5f) / (float)width, ((float)height + 0.5f) / (float)height, 1f, 1f) : Vector4.one);
				this.material.SetTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.noiseTex, this.noiseTex);
				this.material.SetVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.inputTexelSize, new Vector4(1f / (float)width, 1f / (float)height, (float)width, (float)height));
				if (this.sourceDesc.useDynamicScale)
				{
					this.material.SetVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.aoTexelSize, new Vector4(1f / ((float)this.aoDesc.width * ScalableBufferManager.widthScaleFactor), 1f / ((float)this.aoDesc.height * ScalableBufferManager.heightScaleFactor), (float)this.aoDesc.width * ScalableBufferManager.widthScaleFactor, (float)this.aoDesc.height * ScalableBufferManager.heightScaleFactor));
				}
				else
				{
					this.material.SetVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.aoTexelSize, new Vector4(1f / (float)this.aoDesc.width, 1f / (float)this.aoDesc.height, (float)this.aoDesc.width, (float)this.aoDesc.height));
				}
				this.material.SetVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.deinterleavedAOTexelSize, new Vector4(1f / (float)this.deinterleavedAoDesc.width, 1f / (float)this.deinterleavedAoDesc.height, (float)this.deinterleavedAoDesc.width, (float)this.deinterleavedAoDesc.height));
				this.material.SetVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.reinterleavedAOTexelSize, new Vector4(1f / (float)this.reinterleavedAoDesc.width, 1f / (float)this.reinterleavedAoDesc.height, (float)this.reinterleavedAoDesc.width, (float)this.reinterleavedAoDesc.height));
				this.material.SetVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.targetScale, value);
				this.material.SetVectorArray(HBAORendererFeature.HBAORenderPass.ShaderProperties.uvToView, this.m_UVToViewPerEye);
				this.material.SetFloatArray(HBAORendererFeature.HBAORenderPass.ShaderProperties.radius, this.m_RadiusPerEye);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.maxRadiusPixels, num2);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.negInvRadius2, -1f / (this.hbao.radius.value * this.hbao.radius.value));
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.angleBias, this.hbao.bias.value);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.aoMultiplier, 2f * (1f / (1f - this.hbao.bias.value)));
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.intensity, HBAORendererFeature.HBAORenderPass.isLinearColorSpace ? this.hbao.intensity.value : (this.hbao.intensity.value * 0.45454547f));
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.multiBounceInfluence, this.hbao.multiBounceInfluence.value);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.offscreenSamplesContrib, this.hbao.offscreenSamplesContribution.value);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.maxDistance, this.hbao.maxDistance.value);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.distanceFalloff, this.hbao.distanceFalloff.value);
				this.material.SetColor(HBAORendererFeature.HBAORenderPass.ShaderProperties.baseColor, this.hbao.baseColor.value);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.blurSharpness, this.hbao.sharpness.value);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.colorBleedSaturation, this.hbao.saturation.value);
				this.material.SetFloat(HBAORendererFeature.HBAORenderPass.ShaderProperties.colorBleedBrightnessMask, this.hbao.brightnessMask.value);
				this.material.SetVector(HBAORendererFeature.HBAORenderPass.ShaderProperties.colorBleedBrightnessMaskRange, this.AdjustBrightnessMaskToGammaSpace(new Vector2(Mathf.Pow(this.hbao.brightnessMaskRange.value.x, 3f), Mathf.Pow(this.hbao.brightnessMaskRange.value.y, 3f))));
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00004328 File Offset: 0x00002528
			private void UpdateShaderKeywords()
			{
				if (this.m_ShaderKeywords == null || this.m_ShaderKeywords.Length != 12)
				{
					this.m_ShaderKeywords = new string[12];
				}
				this.m_ShaderKeywords[0] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetOrthographicProjectionKeyword(this.cameraData.camera.orthographic);
				this.m_ShaderKeywords[1] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetQualityKeyword(this.hbao.quality.value);
				this.m_ShaderKeywords[2] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetNoiseKeyword(this.hbao.noiseType.value);
				this.m_ShaderKeywords[3] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetDeinterleavingKeyword(this.hbao.deinterleaving.value);
				this.m_ShaderKeywords[4] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetDebugKeyword(this.hbao.debugMode.value);
				this.m_ShaderKeywords[5] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetMultibounceKeyword(this.hbao.useMultiBounce.value, this.hbao.mode.value == HBAO.Mode.LitAO);
				this.m_ShaderKeywords[6] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetOffscreenSamplesContributionKeyword(this.hbao.offscreenSamplesContribution.value);
				this.m_ShaderKeywords[7] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetPerPixelNormalsKeyword(this.hbao.perPixelNormals.value);
				this.m_ShaderKeywords[8] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetBlurRadiusKeyword(this.hbao.blurType.value);
				this.m_ShaderKeywords[9] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetVarianceClippingKeyword(this.hbao.varianceClipping.value);
				this.m_ShaderKeywords[10] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetColorBleedingKeyword(this.hbao.colorBleedingEnabled.value, this.hbao.mode.value == HBAO.Mode.LitAO);
				this.m_ShaderKeywords[11] = HBAORendererFeature.HBAORenderPass.ShaderProperties.GetModeKeyword(this.hbao.mode.value);
				this.material.shaderKeywords = this.m_ShaderKeywords;
			}

			// Token: 0x06000095 RID: 149 RVA: 0x000044EC File Offset: 0x000026EC
			private void CheckParameters()
			{
				if (this.hbao.deinterleaving.value != HBAO.Deinterleaving.Disabled && SystemInfo.supportedRenderTargetCount < 4)
				{
					this.hbao.SetDeinterleaving(HBAO.Deinterleaving.Disabled);
				}
				if (this.hbao.temporalFilterEnabled.value && !this.motionVectorsSupported)
				{
					this.hbao.EnableTemporalFilter(false);
				}
				if (this.hbao.colorBleedingEnabled.value && this.hbao.temporalFilterEnabled.value && SystemInfo.supportedRenderTargetCount < 2)
				{
					this.hbao.EnableTemporalFilter(false);
				}
				if (this.hbao.colorBleedingEnabled.value && this.hbao.mode.value == HBAO.Mode.LitAO)
				{
					this.hbao.EnableColorBleeding(false);
				}
				if (!(this.noiseTex == null))
				{
					HBAO.NoiseType? previousNoiseType = this.m_PreviousNoiseType;
					HBAO.NoiseType value = this.hbao.noiseType.value;
					if (previousNoiseType.GetValueOrDefault() == value & previousNoiseType != null)
					{
						return;
					}
				}
				CoreUtils.Destroy(this.noiseTex);
				this.CreateNoiseTexture();
				this.m_PreviousNoiseType = new HBAO.NoiseType?(this.hbao.noiseType.value);
			}

			// Token: 0x06000096 RID: 150 RVA: 0x00004614 File Offset: 0x00002814
			private RenderTextureDescriptor GetStereoCompatibleDescriptor(int width, int height, RenderTextureFormat format = RenderTextureFormat.Default, int depthBufferBits = 0, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default)
			{
				RenderTextureDescriptor sourceDesc = this.sourceDesc;
				sourceDesc.depthBufferBits = depthBufferBits;
				sourceDesc.msaaSamples = 1;
				sourceDesc.width = width;
				sourceDesc.height = height;
				sourceDesc.colorFormat = format;
				if (readWrite == RenderTextureReadWrite.sRGB)
				{
					sourceDesc.sRGB = true;
				}
				else if (readWrite == RenderTextureReadWrite.Linear)
				{
					sourceDesc.sRGB = false;
				}
				else if (readWrite == RenderTextureReadWrite.Default)
				{
					sourceDesc.sRGB = HBAORendererFeature.HBAORenderPass.isLinearColorSpace;
				}
				return sourceDesc;
			}

			// Token: 0x06000097 RID: 151 RVA: 0x00004680 File Offset: 0x00002880
			private static void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, Mesh fullscreenTriangle, int passIndex = 0)
			{
				cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			// Token: 0x06000098 RID: 152 RVA: 0x000046A9 File Offset: 0x000028A9
			private static void BlitFullscreenTriangle(CommandBuffer cmd, RenderTexture source, RenderTargetIdentifier destination, Material material, Mesh fullscreenTriangle, int passIndex, MaterialPropertyBlock properties)
			{
				properties.SetTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex, properties);
			}

			// Token: 0x06000099 RID: 153 RVA: 0x000046DC File Offset: 0x000028DC
			private static void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Rect viewportRect, Material material, Mesh fullscreenTriangle, int passIndex = 0)
			{
				cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.SetViewport(viewportRect);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			// Token: 0x0600009A RID: 154 RVA: 0x0000470D File Offset: 0x0000290D
			private static void BlitFullscreenTriangle(CommandBuffer cmd, RenderTexture source, RenderTargetIdentifier destination, Rect viewportRect, Material material, Mesh fullscreenTriangle, int passIndex, MaterialPropertyBlock properties)
			{
				properties.SetTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
				cmd.SetViewport(viewportRect);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex, properties);
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00004748 File Offset: 0x00002948
			private static void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier[] destinations, Material material, Mesh fullscreenTriangle, int passIndex = 0)
			{
				cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destinations, destinations[0], 0, CubemapFace.Unknown, -1);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			// Token: 0x0600009C RID: 156 RVA: 0x00004778 File Offset: 0x00002978
			private static void BlitFullscreenTriangle(CommandBuffer cmd, RenderTexture source, RenderTargetIdentifier[] destinations, Material material, Mesh fullscreenTriangle, int passIndex, MaterialPropertyBlock properties)
			{
				properties.SetTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destinations, destinations[0], 0, CubemapFace.Unknown, -1);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex, properties);
			}

			// Token: 0x0600009D RID: 157 RVA: 0x000047AB File Offset: 0x000029AB
			private static void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier[] destinations, Rect viewportRect, Material material, Mesh fullscreenTriangle, int passIndex = 0)
			{
				cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destinations, destinations[0], 0, CubemapFace.Unknown, -1);
				cmd.SetViewport(viewportRect);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			// Token: 0x0600009E RID: 158 RVA: 0x000047E3 File Offset: 0x000029E3
			private static void BlitFullscreenTriangle(CommandBuffer cmd, RenderTexture source, RenderTargetIdentifier[] destinations, Rect viewportRect, Material material, Mesh fullscreenTriangle, int passIndex, MaterialPropertyBlock properties)
			{
				properties.SetTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destinations, destinations[0], 0, CubemapFace.Unknown, -1);
				cmd.SetViewport(viewportRect);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex, properties);
			}

			// Token: 0x0600009F RID: 159 RVA: 0x0000481E File Offset: 0x00002A1E
			private static void BlitFullscreenTriangleWithClear(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, Color clearColor, Mesh fullscreenTriangle, int passIndex = 0)
			{
				cmd.SetGlobalTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.ClearRenderTarget(false, true, clearColor);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x00004851 File Offset: 0x00002A51
			private static void BlitFullscreenTriangleWithClear(CommandBuffer cmd, RenderTexture source, RenderTargetIdentifier destination, Material material, Color clearColor, Mesh fullscreenTriangle, int passIndex, MaterialPropertyBlock properties)
			{
				properties.SetTexture(HBAORendererFeature.HBAORenderPass.ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
				cmd.ClearRenderTarget(false, true, clearColor);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex, properties);
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x0000488E File Offset: 0x00002A8E
			private Vector2 AdjustBrightnessMaskToGammaSpace(Vector2 v)
			{
				if (!HBAORendererFeature.HBAORenderPass.isLinearColorSpace)
				{
					return this.ToGammaSpace(v);
				}
				return v;
			}

			// Token: 0x060000A2 RID: 162 RVA: 0x000048A0 File Offset: 0x00002AA0
			private float ToGammaSpace(float v)
			{
				return Mathf.Pow(v, 0.45454547f);
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x000048AD File Offset: 0x00002AAD
			private Vector2 ToGammaSpace(Vector2 v)
			{
				return new Vector2(this.ToGammaSpace(v.x), this.ToGammaSpace(v.y));
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x000048CC File Offset: 0x00002ACC
			private void CreateNoiseTexture()
			{
				this.noiseTex = new Texture2D(4, 4, SystemInfo.SupportsTextureFormat(TextureFormat.RGHalf) ? TextureFormat.RGHalf : TextureFormat.RGB24, false, true);
				this.noiseTex.filterMode = FilterMode.Point;
				this.noiseTex.wrapMode = TextureWrapMode.Repeat;
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						float r = (this.hbao.noiseType.value != HBAO.NoiseType.Dither) ? (0.25f * (0.0625f * (float)((i + j & 3) << 2) + (float)(i & 3))) : HBAORendererFeature.HBAORenderPass.MersenneTwister.Numbers[num++];
						float g = (this.hbao.noiseType.value != HBAO.NoiseType.Dither) ? (0.25f * (float)(j - i & 3)) : HBAORendererFeature.HBAORenderPass.MersenneTwister.Numbers[num++];
						Color color = new Color(r, g, 0f);
						this.noiseTex.SetPixel(i, j, color);
					}
				}
				this.noiseTex.Apply();
				int k = 0;
				int num2 = 0;
				while (k < HBAORendererFeature.HBAORenderPass.s_jitter.Length)
				{
					float x = HBAORendererFeature.HBAORenderPass.MersenneTwister.Numbers[num2++];
					float y = HBAORendererFeature.HBAORenderPass.MersenneTwister.Numbers[num2++];
					HBAORendererFeature.HBAORenderPass.s_jitter[k] = new Vector2(x, y);
					k++;
				}
			}

			// Token: 0x04000060 RID: 96
			public HBAO hbao;

			// Token: 0x04000061 RID: 97
			private static readonly Vector2[] s_jitter = new Vector2[16];

			// Token: 0x04000062 RID: 98
			private static readonly float[] s_temporalRotations = new float[]
			{
				60f,
				300f,
				180f,
				240f,
				120f,
				0f
			};

			// Token: 0x04000063 RID: 99
			private static readonly float[] s_temporalOffsets = new float[]
			{
				0f,
				0.5f,
				0.25f,
				0.75f
			};

			// Token: 0x04000077 RID: 119
			private Mesh m_FullscreenTriangle;

			// Token: 0x04000078 RID: 120
			private MaterialPropertyBlock m_MaterialPropertyBlock;

			// Token: 0x04000079 RID: 121
			private HBAO.Resolution? m_PreviousResolution;

			// Token: 0x0400007A RID: 122
			private HBAO.NoiseType? m_PreviousNoiseType;

			// Token: 0x0400007B RID: 123
			private bool m_PreviousColorBleedingEnabled;

			// Token: 0x0400007C RID: 124
			private XRSettings.StereoRenderingMode m_PrevStereoRenderingMode;

			// Token: 0x0400007D RID: 125
			private string[] m_ShaderKeywords;

			// Token: 0x0400007E RID: 126
			private RenderTargetIdentifier[] m_RtsDepth = new RenderTargetIdentifier[4];

			// Token: 0x0400007F RID: 127
			private RenderTargetIdentifier[] m_RtsNormals = new RenderTargetIdentifier[4];

			// Token: 0x04000080 RID: 128
			private RenderTargetIdentifier[] m_RtsTemporalFilter = new RenderTargetIdentifier[2];

			// Token: 0x04000081 RID: 129
			private List<HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers> m_CameraHistoryBuffers = new List<HBAORendererFeature.HBAORenderPass.CameraHistoryBuffers>();

			// Token: 0x04000082 RID: 130
			private Vector4[] m_UVToViewPerEye = new Vector4[2];

			// Token: 0x04000083 RID: 131
			private float[] m_RadiusPerEye = new float[2];

			// Token: 0x04000084 RID: 132
			private ProfilingSampler m_ProfilingSampler = new ProfilingSampler("HBAO");

			// Token: 0x0200002C RID: 44
			private static class Pass
			{
				// Token: 0x04000085 RID: 133
				public const int AO = 0;

				// Token: 0x04000086 RID: 134
				public const int AO_Deinterleaved = 1;

				// Token: 0x04000087 RID: 135
				public const int Deinterleave_Depth = 2;

				// Token: 0x04000088 RID: 136
				public const int Deinterleave_Normals = 3;

				// Token: 0x04000089 RID: 137
				public const int Atlas_AO_Deinterleaved = 4;

				// Token: 0x0400008A RID: 138
				public const int Reinterleave_AO = 5;

				// Token: 0x0400008B RID: 139
				public const int Blur = 6;

				// Token: 0x0400008C RID: 140
				public const int Temporal_Filter = 7;

				// Token: 0x0400008D RID: 141
				public const int Copy = 8;

				// Token: 0x0400008E RID: 142
				public const int Composite = 9;

				// Token: 0x0400008F RID: 143
				public const int Debug_ViewNormals = 10;
			}

			// Token: 0x0200002D RID: 45
			private static class ShaderProperties
			{
				// Token: 0x060000A7 RID: 167 RVA: 0x00004AB4 File Offset: 0x00002CB4
				static ShaderProperties()
				{
					for (int i = 0; i < 16; i++)
					{
						HBAORendererFeature.HBAORenderPass.ShaderProperties.depthSliceTex[i] = Shader.PropertyToID("_DepthSliceTex" + i.ToString());
						HBAORendererFeature.HBAORenderPass.ShaderProperties.normalsSliceTex[i] = Shader.PropertyToID("_NormalsSliceTex" + i.ToString());
						HBAORendererFeature.HBAORenderPass.ShaderProperties.aoSliceTex[i] = Shader.PropertyToID("_AOSliceTex" + i.ToString());
					}
					HBAORendererFeature.HBAORenderPass.ShaderProperties.deinterleaveOffset = new int[]
					{
						Shader.PropertyToID("_Deinterleave_Offset00"),
						Shader.PropertyToID("_Deinterleave_Offset10"),
						Shader.PropertyToID("_Deinterleave_Offset01"),
						Shader.PropertyToID("_Deinterleave_Offset11")
					};
					HBAORendererFeature.HBAORenderPass.ShaderProperties.atlasOffset = Shader.PropertyToID("_AtlasOffset");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.jitter = Shader.PropertyToID("_Jitter");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.uvTransform = Shader.PropertyToID("_UVTransform");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.inputTexelSize = Shader.PropertyToID("_Input_TexelSize");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.aoTexelSize = Shader.PropertyToID("_AO_TexelSize");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.deinterleavedAOTexelSize = Shader.PropertyToID("_DeinterleavedAO_TexelSize");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.reinterleavedAOTexelSize = Shader.PropertyToID("_ReinterleavedAO_TexelSize");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.uvToView = Shader.PropertyToID("_UVToView");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.targetScale = Shader.PropertyToID("_TargetScale");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.radius = Shader.PropertyToID("_Radius");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.maxRadiusPixels = Shader.PropertyToID("_MaxRadiusPixels");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.negInvRadius2 = Shader.PropertyToID("_NegInvRadius2");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.angleBias = Shader.PropertyToID("_AngleBias");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.aoMultiplier = Shader.PropertyToID("_AOmultiplier");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.intensity = Shader.PropertyToID("_Intensity");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.multiBounceInfluence = Shader.PropertyToID("_MultiBounceInfluence");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.offscreenSamplesContrib = Shader.PropertyToID("_OffscreenSamplesContrib");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.maxDistance = Shader.PropertyToID("_MaxDistance");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.distanceFalloff = Shader.PropertyToID("_DistanceFalloff");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.baseColor = Shader.PropertyToID("_BaseColor");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.colorBleedSaturation = Shader.PropertyToID("_ColorBleedSaturation");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.albedoMultiplier = Shader.PropertyToID("_AlbedoMultiplier");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.colorBleedBrightnessMask = Shader.PropertyToID("_ColorBleedBrightnessMask");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.colorBleedBrightnessMaskRange = Shader.PropertyToID("_ColorBleedBrightnessMaskRange");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.blurDeltaUV = Shader.PropertyToID("_BlurDeltaUV");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.blurSharpness = Shader.PropertyToID("_BlurSharpness");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.temporalParams = Shader.PropertyToID("_TemporalParams");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.historyBufferRTHandleScale = Shader.PropertyToID("_HistoryBuffer_RTHandleScale");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.cameraDepthTexture = Shader.PropertyToID("_CameraDepthTexture");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.screenSpaceOcclusionTexture = Shader.PropertyToID("_ScreenSpaceOcclusionTexture");
					HBAORendererFeature.HBAORenderPass.ShaderProperties.screenSpaceOcclusionParam = Shader.PropertyToID("_AmbientOcclusionParam");
				}

				// Token: 0x060000A8 RID: 168 RVA: 0x00004DE0 File Offset: 0x00002FE0
				public static string GetOrthographicProjectionKeyword(bool orthographic)
				{
					if (!orthographic)
					{
						return "__";
					}
					return "ORTHOGRAPHIC_PROJECTION";
				}

				// Token: 0x060000A9 RID: 169 RVA: 0x00004DF0 File Offset: 0x00002FF0
				public static string GetQualityKeyword(HBAO.Quality quality)
				{
					switch (quality)
					{
					case HBAO.Quality.Lowest:
						return "QUALITY_LOWEST";
					case HBAO.Quality.Low:
						return "QUALITY_LOW";
					case HBAO.Quality.Medium:
						return "QUALITY_MEDIUM";
					case HBAO.Quality.High:
						return "QUALITY_HIGH";
					case HBAO.Quality.Highest:
						return "QUALITY_HIGHEST";
					default:
						return "QUALITY_MEDIUM";
					}
				}

				// Token: 0x060000AA RID: 170 RVA: 0x00004E3C File Offset: 0x0000303C
				public static string GetNoiseKeyword(HBAO.NoiseType noiseType)
				{
					switch (noiseType)
					{
					case HBAO.NoiseType.InterleavedGradientNoise:
						return "INTERLEAVED_GRADIENT_NOISE";
					}
					return "__";
				}

				// Token: 0x060000AB RID: 171 RVA: 0x00004E5D File Offset: 0x0000305D
				public static string GetDeinterleavingKeyword(HBAO.Deinterleaving deinterleaving)
				{
					if (deinterleaving != HBAO.Deinterleaving.Disabled && deinterleaving == HBAO.Deinterleaving.x4)
					{
						return "DEINTERLEAVED";
					}
					return "__";
				}

				// Token: 0x060000AC RID: 172 RVA: 0x00004E74 File Offset: 0x00003074
				public static string GetDebugKeyword(HBAO.DebugMode debugMode)
				{
					switch (debugMode)
					{
					case HBAO.DebugMode.AOOnly:
						return "DEBUG_AO";
					case HBAO.DebugMode.ColorBleedingOnly:
						return "DEBUG_COLORBLEEDING";
					case HBAO.DebugMode.SplitWithoutAOAndWithAO:
						return "DEBUG_NOAO_AO";
					case HBAO.DebugMode.SplitWithAOAndAOOnly:
						return "DEBUG_AO_AOONLY";
					case HBAO.DebugMode.SplitWithoutAOAndAOOnly:
						return "DEBUG_NOAO_AOONLY";
					}
					return "__";
				}

				// Token: 0x060000AD RID: 173 RVA: 0x00004EC4 File Offset: 0x000030C4
				public static string GetMultibounceKeyword(bool useMultiBounce, bool litAoModeEnabled)
				{
					if (!useMultiBounce || litAoModeEnabled)
					{
						return "__";
					}
					return "MULTIBOUNCE";
				}

				// Token: 0x060000AE RID: 174 RVA: 0x00004ED7 File Offset: 0x000030D7
				public static string GetOffscreenSamplesContributionKeyword(float offscreenSamplesContribution)
				{
					if (offscreenSamplesContribution <= 0f)
					{
						return "__";
					}
					return "OFFSCREEN_SAMPLES_CONTRIBUTION";
				}

				// Token: 0x060000AF RID: 175 RVA: 0x00004EEC File Offset: 0x000030EC
				public static string GetPerPixelNormalsKeyword(HBAO.PerPixelNormals perPixelNormals)
				{
					switch (perPixelNormals)
					{
					case HBAO.PerPixelNormals.Reconstruct2Samples:
						return "NORMALS_RECONSTRUCT2";
					case HBAO.PerPixelNormals.Reconstruct4Samples:
						return "NORMALS_RECONSTRUCT4";
					}
					return "__";
				}

				// Token: 0x060000B0 RID: 176 RVA: 0x00004F13 File Offset: 0x00003113
				public static string GetBlurRadiusKeyword(HBAO.BlurType blurType)
				{
					switch (blurType)
					{
					case HBAO.BlurType.Narrow:
						return "BLUR_RADIUS_2";
					case HBAO.BlurType.Medium:
						return "BLUR_RADIUS_3";
					case HBAO.BlurType.Wide:
						return "BLUR_RADIUS_4";
					case HBAO.BlurType.ExtraWide:
						return "BLUR_RADIUS_5";
					}
					return "BLUR_RADIUS_3";
				}

				// Token: 0x060000B1 RID: 177 RVA: 0x00004F4E File Offset: 0x0000314E
				public static string GetVarianceClippingKeyword(HBAO.VarianceClipping varianceClipping)
				{
					switch (varianceClipping)
					{
					case HBAO.VarianceClipping._4Tap:
						return "VARIANCE_CLIPPING_4TAP";
					case HBAO.VarianceClipping._8Tap:
						return "VARIANCE_CLIPPING_8TAP";
					}
					return "__";
				}

				// Token: 0x060000B2 RID: 178 RVA: 0x00004F75 File Offset: 0x00003175
				public static string GetColorBleedingKeyword(bool colorBleedingEnabled, bool litAoModeEnabled)
				{
					if (!colorBleedingEnabled || litAoModeEnabled)
					{
						return "__";
					}
					return "COLOR_BLEEDING";
				}

				// Token: 0x060000B3 RID: 179 RVA: 0x00004F88 File Offset: 0x00003188
				public static string GetModeKeyword(HBAO.Mode mode)
				{
					if (mode != HBAO.Mode.LitAO)
					{
						return "__";
					}
					return "LIT_AO";
				}

				// Token: 0x04000090 RID: 144
				public static int mainTex = Shader.PropertyToID("_MainTex");

				// Token: 0x04000091 RID: 145
				public static int inputTex = Shader.PropertyToID("_InputTex");

				// Token: 0x04000092 RID: 146
				public static int hbaoTex = Shader.PropertyToID("_HBAOTex");

				// Token: 0x04000093 RID: 147
				public static int tempTex = Shader.PropertyToID("_TempTex");

				// Token: 0x04000094 RID: 148
				public static int tempTex2 = Shader.PropertyToID("_TempTex2");

				// Token: 0x04000095 RID: 149
				public static int noiseTex = Shader.PropertyToID("_NoiseTex");

				// Token: 0x04000096 RID: 150
				public static int depthTex = Shader.PropertyToID("_DepthTex");

				// Token: 0x04000097 RID: 151
				public static int normalsTex = Shader.PropertyToID("_NormalsTex");

				// Token: 0x04000098 RID: 152
				public static int ssaoTex = Shader.PropertyToID("_SSAOTex");

				// Token: 0x04000099 RID: 153
				public static int[] depthSliceTex = new int[16];

				// Token: 0x0400009A RID: 154
				public static int[] normalsSliceTex = new int[16];

				// Token: 0x0400009B RID: 155
				public static int[] aoSliceTex = new int[16];

				// Token: 0x0400009C RID: 156
				public static int[] deinterleaveOffset;

				// Token: 0x0400009D RID: 157
				public static int atlasOffset;

				// Token: 0x0400009E RID: 158
				public static int jitter;

				// Token: 0x0400009F RID: 159
				public static int uvTransform;

				// Token: 0x040000A0 RID: 160
				public static int inputTexelSize;

				// Token: 0x040000A1 RID: 161
				public static int aoTexelSize;

				// Token: 0x040000A2 RID: 162
				public static int deinterleavedAOTexelSize;

				// Token: 0x040000A3 RID: 163
				public static int reinterleavedAOTexelSize;

				// Token: 0x040000A4 RID: 164
				public static int uvToView;

				// Token: 0x040000A5 RID: 165
				public static int targetScale;

				// Token: 0x040000A6 RID: 166
				public static int radius;

				// Token: 0x040000A7 RID: 167
				public static int maxRadiusPixels;

				// Token: 0x040000A8 RID: 168
				public static int negInvRadius2;

				// Token: 0x040000A9 RID: 169
				public static int angleBias;

				// Token: 0x040000AA RID: 170
				public static int aoMultiplier;

				// Token: 0x040000AB RID: 171
				public static int intensity;

				// Token: 0x040000AC RID: 172
				public static int multiBounceInfluence;

				// Token: 0x040000AD RID: 173
				public static int offscreenSamplesContrib;

				// Token: 0x040000AE RID: 174
				public static int maxDistance;

				// Token: 0x040000AF RID: 175
				public static int distanceFalloff;

				// Token: 0x040000B0 RID: 176
				public static int baseColor;

				// Token: 0x040000B1 RID: 177
				public static int colorBleedSaturation;

				// Token: 0x040000B2 RID: 178
				public static int albedoMultiplier;

				// Token: 0x040000B3 RID: 179
				public static int colorBleedBrightnessMask;

				// Token: 0x040000B4 RID: 180
				public static int colorBleedBrightnessMaskRange;

				// Token: 0x040000B5 RID: 181
				public static int blurDeltaUV;

				// Token: 0x040000B6 RID: 182
				public static int blurSharpness;

				// Token: 0x040000B7 RID: 183
				public static int temporalParams;

				// Token: 0x040000B8 RID: 184
				public static int historyBufferRTHandleScale;

				// Token: 0x040000B9 RID: 185
				public static int cameraDepthTexture;

				// Token: 0x040000BA RID: 186
				public static int screenSpaceOcclusionTexture;

				// Token: 0x040000BB RID: 187
				public static int screenSpaceOcclusionParam;
			}

			// Token: 0x0200002E RID: 46
			private static class MersenneTwister
			{
				// Token: 0x040000BC RID: 188
				public static float[] Numbers = new float[]
				{
					0.556725f,
					0.00552f,
					0.708315f,
					0.583199f,
					0.236644f,
					0.99238f,
					0.981091f,
					0.119804f,
					0.510866f,
					0.560499f,
					0.961497f,
					0.557862f,
					0.539955f,
					0.332871f,
					0.417807f,
					0.920779f,
					0.730747f,
					0.07669f,
					0.008562f,
					0.660104f,
					0.428921f,
					0.511342f,
					0.587871f,
					0.906406f,
					0.43798f,
					0.620309f,
					0.062196f,
					0.119485f,
					0.235646f,
					0.795892f,
					0.044437f,
					0.617311f
				};
			}

			// Token: 0x0200002F RID: 47
			private class CameraHistoryBuffers
			{
				// Token: 0x17000018 RID: 24
				// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004FB2 File Offset: 0x000031B2
				// (set) Token: 0x060000B6 RID: 182 RVA: 0x00004FBA File Offset: 0x000031BA
				public Camera camera { get; set; }

				// Token: 0x17000019 RID: 25
				// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004FC3 File Offset: 0x000031C3
				// (set) Token: 0x060000B8 RID: 184 RVA: 0x00004FCB File Offset: 0x000031CB
				public BufferedRTHandleSystem historyRTSystem { get; set; }

				// Token: 0x1700001A RID: 26
				// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004FD4 File Offset: 0x000031D4
				// (set) Token: 0x060000BA RID: 186 RVA: 0x00004FDC File Offset: 0x000031DC
				public int frameCount { get; set; }

				// Token: 0x1700001B RID: 27
				// (get) Token: 0x060000BB RID: 187 RVA: 0x00004FE5 File Offset: 0x000031E5
				// (set) Token: 0x060000BC RID: 188 RVA: 0x00004FED File Offset: 0x000031ED
				public int lastRenderedFrame { get; set; }
			}

			// Token: 0x02000030 RID: 48
			private enum HistoryBufferType
			{
				// Token: 0x040000C2 RID: 194
				AmbientOcclusion,
				// Token: 0x040000C3 RID: 195
				ColorBleeding
			}
		}
	}
}
