using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200002C RID: 44
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamHD))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-shadow-hd/")]
	[AddComponentMenu("VLB/HD/Volumetric Shadow HD")]
	public class VolumetricShadowHD : MonoBehaviour
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000056D2 File Offset: 0x000038D2
		// (set) Token: 0x06000122 RID: 290 RVA: 0x000056DA File Offset: 0x000038DA
		public float strength
		{
			get
			{
				return this.m_Strength;
			}
			set
			{
				if (this.m_Strength != value)
				{
					this.m_Strength = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000123 RID: 291 RVA: 0x000056F2 File Offset: 0x000038F2
		// (set) Token: 0x06000124 RID: 292 RVA: 0x000056FA File Offset: 0x000038FA
		public ShadowUpdateRate updateRate
		{
			get
			{
				return this.m_UpdateRate;
			}
			set
			{
				this.m_UpdateRate = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00005703 File Offset: 0x00003903
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000570B File Offset: 0x0000390B
		public int waitXFrames
		{
			get
			{
				return this.m_WaitXFrames;
			}
			set
			{
				this.m_WaitXFrames = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00005714 File Offset: 0x00003914
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000571C File Offset: 0x0000391C
		public LayerMask layerMask
		{
			get
			{
				return this.m_LayerMask;
			}
			set
			{
				this.m_LayerMask = value;
				this.UpdateDepthCameraProperties();
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000572B File Offset: 0x0000392B
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00005733 File Offset: 0x00003933
		public bool useOcclusionCulling
		{
			get
			{
				return this.m_UseOcclusionCulling;
			}
			set
			{
				this.m_UseOcclusionCulling = value;
				this.UpdateDepthCameraProperties();
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00005742 File Offset: 0x00003942
		// (set) Token: 0x0600012C RID: 300 RVA: 0x0000574A File Offset: 0x0000394A
		public int depthMapResolution
		{
			get
			{
				return this.m_DepthMapResolution;
			}
			set
			{
				if (this.m_DepthCamera != null && Application.isPlaying)
				{
					Debug.LogErrorFormat(Consts.Shadow.GetErrorChangeRuntimeDepthMapResolution(this), Array.Empty<object>());
				}
				this.m_DepthMapResolution = value;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005778 File Offset: 0x00003978
		public void ProcessOcclusionManually()
		{
			this.ProcessOcclusion(VolumetricShadowHD.ProcessOcclusionSource.User);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005781 File Offset: 0x00003981
		public void UpdateDepthCameraProperties()
		{
			if (this.m_DepthCamera)
			{
				this.m_DepthCamera.cullingMask = this.layerMask;
				this.m_DepthCamera.useOcclusionCulling = this.useOcclusionCulling;
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000057B8 File Offset: 0x000039B8
		private void ProcessOcclusion(VolumetricShadowHD.ProcessOcclusionSource source)
		{
			if (!Config.Instance.featureEnabledShadow)
			{
				return;
			}
			if (this.m_LastFrameRendered == Time.frameCount && Application.isPlaying && source == VolumetricShadowHD.ProcessOcclusionSource.OnEnable)
			{
				return;
			}
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				this.m_NeedToUpdateOcclusionNextFrame = true;
			}
			else
			{
				this.ProcessOcclusionInternal();
			}
			this.SetDirty();
			if (this.updateRate.HasFlag(ShadowUpdateRate.OnBeamMove))
			{
				this.m_TransformPacked = base.transform.GetWorldPacked();
			}
			bool flag = this.m_LastFrameRendered < 0;
			this.m_LastFrameRendered = Time.frameCount;
			if (flag && VolumetricShadowHD._INTERNAL_ApplyRandomFrameOffset)
			{
				this.m_LastFrameRendered += UnityEngine.Random.Range(0, this.waitXFrames);
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005868 File Offset: 0x00003A68
		public static void ApplyMaterialProperties(VolumetricShadowHD instance, BeamGeometryHD geom)
		{
			if (instance && instance.enabled)
			{
				geom.SetMaterialProp(ShaderProperties.HD.ShadowDepthTexture, instance.m_DepthCamera.targetTexture);
				Vector3 vector = instance.m_Master.scalable ? instance.m_Master.GetLossyScale() : Vector3.one;
				geom.SetMaterialProp(ShaderProperties.HD.ShadowProps, new Vector4(Mathf.Sign(vector.x) * Mathf.Sign(vector.z), Mathf.Sign(vector.y), instance.m_Strength, instance.m_DepthCamera.orthographic ? 0f : 1f));
				return;
			}
			geom.SetMaterialProp(ShaderProperties.HD.ShadowDepthTexture, BeamGeometryHD.InvalidTexture.NoDepth);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000591F File Offset: 0x00003B1F
		private void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamHD>();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000592D File Offset: 0x00003B2D
		private void OnEnable()
		{
			this.OnValidateProperties();
			this.InstantiateOrActivateDepthCamera();
			this.OnBeamEnabled();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005941 File Offset: 0x00003B41
		private void OnDisable()
		{
			if (this.m_DepthCamera)
			{
				this.m_DepthCamera.gameObject.SetActive(false);
			}
			this.SetDirty();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005967 File Offset: 0x00003B67
		private void OnDestroy()
		{
			this.DestroyDepthCamera();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000596F File Offset: 0x00003B6F
		private void ProcessOcclusionInternal()
		{
			this.UpdateDepthCameraPropertiesAccordingToBeam();
			this.m_DepthCamera.Render();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005982 File Offset: 0x00003B82
		private void OnBeamEnabled()
		{
			if (!base.enabled)
			{
				return;
			}
			if (!this.updateRate.HasFlag(ShadowUpdateRate.Never))
			{
				this.ProcessOcclusion(VolumetricShadowHD.ProcessOcclusionSource.OnEnable);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000059AC File Offset: 0x00003BAC
		public void OnWillCameraRenderThisBeam(Camera cam, BeamGeometryHD beamGeom)
		{
			if (!base.enabled)
			{
				return;
			}
			if (cam != null && cam.enabled && Time.frameCount != this.m_LastFrameRendered && this.updateRate != ShadowUpdateRate.Never)
			{
				bool flag = false;
				if (!flag && this.updateRate.HasFlag(ShadowUpdateRate.OnBeamMove) && !this.m_TransformPacked.IsSame(base.transform))
				{
					flag = true;
				}
				if (!flag && this.updateRate.HasFlag(ShadowUpdateRate.EveryXFrames) && Time.frameCount >= this.m_LastFrameRendered + this.waitXFrames)
				{
					flag = true;
				}
				if (flag)
				{
					this.ProcessOcclusion(VolumetricShadowHD.ProcessOcclusionSource.RenderLoop);
				}
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005A5C File Offset: 0x00003C5C
		private void Update()
		{
			if (this.m_NeedToUpdateOcclusionNextFrame && this.m_Master && this.m_DepthCamera && Time.frameCount > 1)
			{
				this.ProcessOcclusionInternal();
				this.m_NeedToUpdateOcclusionNextFrame = false;
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005A98 File Offset: 0x00003C98
		private void UpdateDepthCameraPropertiesAccordingToBeam()
		{
			Utils.SetupDepthCamera(this.m_DepthCamera, this.m_Master.GetConeApexOffsetZ(true), this.m_Master.maxGeometryDistance, this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd, this.m_Master.beamLocalForward, this.m_Master.GetLossyScale(), this.m_Master.scalable, this.m_Master.beamInternalLocalRotation, false);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005B0C File Offset: 0x00003D0C
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
				this.UpdateDepthCameraProperties();
				this.m_DepthCamera.clearFlags = CameraClearFlags.Depth;
				this.m_DepthCamera.depthTextureMode = DepthTextureMode.Depth;
				this.m_DepthCamera.renderingPath = RenderingPath.Forward;
				this.m_DepthCamera.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				this.m_DepthCamera.transform.SetParent(base.transform, false);
				Config.Instance.SetURPScriptableRendererIndexToDepthCamera(this.m_DepthCamera);
				RenderTexture targetTexture = new RenderTexture(this.depthMapResolution, this.depthMapResolution, 16, RenderTextureFormat.Depth);
				this.m_DepthCamera.targetTexture = targetTexture;
				this.UpdateDepthCameraPropertiesAccordingToBeam();
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005C30 File Offset: 0x00003E30
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

		// Token: 0x0600013C RID: 316 RVA: 0x00005C9F File Offset: 0x00003E9F
		private void OnValidateProperties()
		{
			this.m_WaitXFrames = Mathf.Clamp(this.m_WaitXFrames, 1, 60);
			this.m_DepthMapResolution = Mathf.Clamp(Mathf.NextPowerOfTwo(this.m_DepthMapResolution), 8, 2048);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005CD1 File Offset: 0x00003ED1
		private void SetDirty()
		{
			if (this.m_Master)
			{
				this.m_Master.SetPropertyDirty(DirtyProps.ShadowProps);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00005CF0 File Offset: 0x00003EF0
		public int _INTERNAL_LastFrameRendered
		{
			get
			{
				return this.m_LastFrameRendered;
			}
		}

		// Token: 0x040000FB RID: 251
		public const string ClassName = "VolumetricShadowHD";

		// Token: 0x040000FC RID: 252
		[SerializeField]
		private float m_Strength = 1f;

		// Token: 0x040000FD RID: 253
		[SerializeField]
		private ShadowUpdateRate m_UpdateRate = ShadowUpdateRate.EveryXFrames;

		// Token: 0x040000FE RID: 254
		[SerializeField]
		private int m_WaitXFrames = 3;

		// Token: 0x040000FF RID: 255
		[SerializeField]
		private LayerMask m_LayerMask = Consts.Shadow.LayerMaskDefault;

		// Token: 0x04000100 RID: 256
		[SerializeField]
		private bool m_UseOcclusionCulling = true;

		// Token: 0x04000101 RID: 257
		[SerializeField]
		private int m_DepthMapResolution = 128;

		// Token: 0x04000102 RID: 258
		private VolumetricLightBeamHD m_Master;

		// Token: 0x04000103 RID: 259
		private TransformUtils.Packed m_TransformPacked;

		// Token: 0x04000104 RID: 260
		private int m_LastFrameRendered = int.MinValue;

		// Token: 0x04000105 RID: 261
		private Camera m_DepthCamera;

		// Token: 0x04000106 RID: 262
		private bool m_NeedToUpdateOcclusionNextFrame;

		// Token: 0x04000107 RID: 263
		public static bool _INTERNAL_ApplyRandomFrameOffset = true;

		// Token: 0x020000A6 RID: 166
		private enum ProcessOcclusionSource
		{
			// Token: 0x0400038D RID: 909
			RenderLoop,
			// Token: 0x0400038E RID: 910
			OnEnable,
			// Token: 0x0400038F RID: 911
			EditorUpdate,
			// Token: 0x04000390 RID: 912
			User
		}
	}
}
