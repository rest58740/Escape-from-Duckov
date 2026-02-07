using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000039 RID: 57
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-depthbuffer/")]
	[AddComponentMenu("VLB/SD/Dynamic Occlusion (Depth Buffer)")]
	public class DynamicOcclusionDepthBuffer : DynamicOcclusionAbstractBase
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x00007F5D File Offset: 0x0000615D
		protected override string GetShaderKeyword()
		{
			return "VLB_OCCLUSION_DEPTH_TEXTURE";
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007F64 File Offset: 0x00006164
		protected override MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode()
		{
			return MaterialManager.SD.DynamicOcclusion.DepthTexture;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00007F67 File Offset: 0x00006167
		private void ProcessOcclusionInternal()
		{
			this.UpdateDepthCameraPropertiesAccordingToBeam();
			this.m_DepthCamera.Render();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00007F7A File Offset: 0x0000617A
		protected override bool OnProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				this.m_NeedToUpdateOcclusionNextFrame = true;
			}
			else
			{
				this.ProcessOcclusionInternal();
			}
			return true;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007F93 File Offset: 0x00006193
		private void Update()
		{
			if (this.m_NeedToUpdateOcclusionNextFrame && this.m_Master && this.m_DepthCamera && Time.frameCount > 1)
			{
				this.ProcessOcclusionInternal();
				this.m_NeedToUpdateOcclusionNextFrame = false;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00007FCC File Offset: 0x000061CC
		private void UpdateDepthCameraPropertiesAccordingToBeam()
		{
			Utils.SetupDepthCamera(this.m_DepthCamera, this.m_Master.coneApexOffsetZ, this.m_Master.maxGeometryDistance, this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd, this.m_Master.beamLocalForward, this.m_Master.GetLossyScale(), this.m_Master.IsScalable(), this.m_Master.beamInternalLocalRotation, true);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008040 File Offset: 0x00006240
		public bool HasLayerMaskIssues()
		{
			if (Config.Instance.geometryOverrideLayer)
			{
				int num = 1 << Config.Instance.geometryLayerID;
				return (this.layerMask.value & num) == num;
			}
			return false;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000807B File Offset: 0x0000627B
		protected override void OnValidateProperties()
		{
			base.OnValidateProperties();
			this.depthMapResolution = Mathf.Clamp(Mathf.NextPowerOfTwo(this.depthMapResolution), 8, 2048);
			this.fadeDistanceToSurface = Mathf.Max(this.fadeDistanceToSurface, 0f);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000080B8 File Offset: 0x000062B8
		private void InstantiateOrActivateDepthCamera()
		{
			if (this.m_DepthCamera != null)
			{
				this.m_DepthCamera.gameObject.SetActive(true);
				return;
			}
			base.gameObject.ForeachComponentsInDirectChildrenOnly(delegate(Camera cam)
			{
				UnityEngine.Object.DestroyImmediate(cam.gameObject);
			}, true);
			this.m_DepthCamera = Utils.NewWithComponent<Camera>("Depth Camera");
			if (this.m_DepthCamera && this.m_Master)
			{
				this.m_DepthCamera.enabled = false;
				this.m_DepthCamera.cullingMask = this.layerMask;
				this.m_DepthCamera.clearFlags = CameraClearFlags.Depth;
				this.m_DepthCamera.depthTextureMode = DepthTextureMode.Depth;
				this.m_DepthCamera.renderingPath = RenderingPath.VertexLit;
				this.m_DepthCamera.useOcclusionCulling = this.useOcclusionCulling;
				this.m_DepthCamera.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				this.m_DepthCamera.transform.SetParent(base.transform, false);
				Config.Instance.SetURPScriptableRendererIndexToDepthCamera(this.m_DepthCamera);
				RenderTexture targetTexture = new RenderTexture(this.depthMapResolution, this.depthMapResolution, 16, RenderTextureFormat.Depth);
				this.m_DepthCamera.targetTexture = targetTexture;
				this.UpdateDepthCameraPropertiesAccordingToBeam();
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000081FA File Offset: 0x000063FA
		protected override void OnEnablePostValidate()
		{
			this.InstantiateOrActivateDepthCamera();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008202 File Offset: 0x00006402
		protected override void OnDisable()
		{
			base.OnDisable();
			if (this.m_DepthCamera)
			{
				this.m_DepthCamera.gameObject.SetActive(false);
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008228 File Offset: 0x00006428
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008230 File Offset: 0x00006430
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.DestroyDepthCamera();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008240 File Offset: 0x00006440
		private void DestroyDepthCamera()
		{
			if (this.m_DepthCamera)
			{
				if (this.m_DepthCamera.targetTexture)
				{
					this.m_DepthCamera.targetTexture.Release();
					UnityEngine.Object.DestroyImmediate(this.m_DepthCamera.targetTexture);
					this.m_DepthCamera.targetTexture = null;
				}
				UnityEngine.Object.DestroyImmediate(this.m_DepthCamera.gameObject);
				this.m_DepthCamera = null;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000082B0 File Offset: 0x000064B0
		protected override void OnModifyMaterialCallback(MaterialModifier.Interface owner)
		{
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionDepthTexture, this.m_DepthCamera.targetTexture);
			Vector3 lossyScale = this.m_Master.GetLossyScale();
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionDepthProps, new Vector4(Mathf.Sign(lossyScale.x) * Mathf.Sign(lossyScale.z), Mathf.Sign(lossyScale.y), this.fadeDistanceToSurface, this.m_DepthCamera.orthographic ? 0f : 1f));
		}

		// Token: 0x0400012B RID: 299
		public new const string ClassName = "DynamicOcclusionDepthBuffer";

		// Token: 0x0400012C RID: 300
		public LayerMask layerMask = Consts.DynOcclusion.LayerMaskDefault;

		// Token: 0x0400012D RID: 301
		public bool useOcclusionCulling = true;

		// Token: 0x0400012E RID: 302
		public int depthMapResolution = 128;

		// Token: 0x0400012F RID: 303
		public float fadeDistanceToSurface;

		// Token: 0x04000130 RID: 304
		private Camera m_DepthCamera;

		// Token: 0x04000131 RID: 305
		private bool m_NeedToUpdateOcclusionNextFrame;
	}
}
