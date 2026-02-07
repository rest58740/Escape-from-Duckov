using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FOW
{
	// Token: 0x02000013 RID: 19
	public class FogOfWarPass : ScriptableRenderPass
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00007A06 File Offset: 0x00005C06
		public FogOfWarPass(string tag)
		{
			this.m_ProfilerTag = tag;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00007A1C File Offset: 0x00005C1C
		private void SetShaderProperties(Camera camera)
		{
			if (!FogOfWarWorld.instance.is2D)
			{
				Matrix4x4 cameraToWorldMatrix = camera.cameraToWorldMatrix;
				FogOfWarWorld.instance.FogOfWarMaterial.SetMatrix("_camToWorldMatrix", cameraToWorldMatrix);
				return;
			}
			FogOfWarWorld.instance.FogOfWarMaterial.SetFloat("_cameraSize", camera.orthographicSize);
			FogOfWarWorld.instance.FogOfWarMaterial.SetVector("_cameraPosition", camera.transform.position);
			FogOfWarWorld.instance.FogOfWarMaterial.SetFloat("_cameraRotation", Mathf.DeltaAngle(0f, camera.transform.eulerAngles.z));
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00007AC0 File Offset: 0x00005CC0
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			FogOfWarPass.instance = this;
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			ScriptableRenderer renderer = renderingData.cameraData.renderer;
			this.source = renderer.cameraColorTargetHandle;
			cmd.GetTemporaryRT(FogOfWarPass.temporaryRTId, cameraTargetDescriptor);
			this.destination = new RenderTargetIdentifier(FogOfWarPass.temporaryRTId);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00007B18 File Offset: 0x00005D18
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (FogOfWarWorld.instance == null || !FogOfWarWorld.instance.enabled || !this.EffectEnabled)
			{
				return;
			}
			if (renderingData.cameraData.camera.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay)
			{
				return;
			}
			CommandBuffer commandBuffer = CommandBufferPool.Get(this.m_ProfilerTag);
			renderingData.cameraData.camera.depthTextureMode = DepthTextureMode.DepthNormals;
			this.SetShaderProperties(renderingData.cameraData.camera);
			commandBuffer.SetGlobalTexture(FogOfWarPass.kBlitTexturePropertyId, this.source);
			commandBuffer.SetGlobalVector(FogOfWarPass.kBlitScaleBiasPropertyId, new Vector4(1f, 1f, 0f, 0f));
			commandBuffer.Blit(this.source, this.destination, FogOfWarWorld.instance.FogOfWarMaterial, 0);
			commandBuffer.Blit(this.destination, this.source);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00007C00 File Offset: 0x00005E00
		public override void FrameCleanup(CommandBuffer cmd)
		{
			if (FogOfWarPass.temporaryRTId != -1)
			{
				cmd.ReleaseTemporaryRT(FogOfWarPass.temporaryRTId);
			}
		}

		// Token: 0x040000EF RID: 239
		public static FogOfWarPass instance;

		// Token: 0x040000F0 RID: 240
		public bool EffectEnabled = true;

		// Token: 0x040000F1 RID: 241
		private string m_ProfilerTag;

		// Token: 0x040000F2 RID: 242
		private RenderTargetIdentifier source;

		// Token: 0x040000F3 RID: 243
		private RenderTargetIdentifier destination;

		// Token: 0x040000F4 RID: 244
		private static readonly int temporaryRTId = Shader.PropertyToID("_FowTempRT");

		// Token: 0x040000F5 RID: 245
		private static readonly int kBlitTexturePropertyId = Shader.PropertyToID("_BlitTexture");

		// Token: 0x040000F6 RID: 246
		private static readonly int kBlitScaleBiasPropertyId = Shader.PropertyToID("_BlitScaleBias");
	}
}
