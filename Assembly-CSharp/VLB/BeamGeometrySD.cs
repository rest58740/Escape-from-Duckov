using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000037 RID: 55
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-sd/")]
	public class BeamGeometrySD : BeamGeometryAbstractBase, MaterialModifier.Interface
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00006E32 File Offset: 0x00005032
		protected override VolumetricLightBeamAbstractBase GetMaster()
		{
			return this.m_Master;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00006E3A File Offset: 0x0000503A
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00006E47 File Offset: 0x00005047
		private bool visible
		{
			get
			{
				return base.meshRenderer.enabled;
			}
			set
			{
				base.meshRenderer.enabled = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006E55 File Offset: 0x00005055
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00006E62 File Offset: 0x00005062
		public int sortingLayerID
		{
			get
			{
				return base.meshRenderer.sortingLayerID;
			}
			set
			{
				base.meshRenderer.sortingLayerID = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00006E70 File Offset: 0x00005070
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00006E7D File Offset: 0x0000507D
		public int sortingOrder
		{
			get
			{
				return base.meshRenderer.sortingOrder;
			}
			set
			{
				base.meshRenderer.sortingOrder = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00006E8B File Offset: 0x0000508B
		public bool _INTERNAL_IsFadeOutCoroutineRunning
		{
			get
			{
				return this.m_CoFadeOut != null;
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006E98 File Offset: 0x00005098
		private float ComputeFadeOutFactor(Transform camTransform)
		{
			if (this.m_Master.isFadeOutEnabled)
			{
				float value = Vector3.SqrMagnitude(base.meshRenderer.bounds.center - camTransform.position);
				return Mathf.InverseLerp(this.m_Master.fadeOutEnd * this.m_Master.fadeOutEnd, this.m_Master.fadeOutBegin * this.m_Master.fadeOutBegin, value);
			}
			return 1f;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006F10 File Offset: 0x00005110
		private IEnumerator CoUpdateFadeOut()
		{
			while (this.m_Master.isFadeOutEnabled)
			{
				this.ComputeFadeOutFactor();
				yield return null;
			}
			this.SetFadeOutFactorProp(1f);
			this.m_CoFadeOut = null;
			yield break;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006F20 File Offset: 0x00005120
		private void ComputeFadeOutFactor()
		{
			Transform fadeOutCameraTransform = Config.Instance.fadeOutCameraTransform;
			if (fadeOutCameraTransform)
			{
				float fadeOutFactorProp = this.ComputeFadeOutFactor(fadeOutCameraTransform);
				this.SetFadeOutFactorProp(fadeOutFactorProp);
				return;
			}
			this.SetFadeOutFactorProp(1f);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006F5B File Offset: 0x0000515B
		private void SetFadeOutFactorProp(float value)
		{
			if (value > 0f)
			{
				base.meshRenderer.enabled = true;
				this.MaterialChangeStart();
				this.SetMaterialProp(ShaderProperties.SD.FadeOutFactor, value);
				this.MaterialChangeStop();
				return;
			}
			base.meshRenderer.enabled = false;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006F96 File Offset: 0x00005196
		private void StopFadeOutCoroutine()
		{
			if (this.m_CoFadeOut != null)
			{
				base.StopCoroutine(this.m_CoFadeOut);
				this.m_CoFadeOut = null;
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006FB3 File Offset: 0x000051B3
		public void RestartFadeOutCoroutine()
		{
			this.StopFadeOutCoroutine();
			if (this.m_Master && this.m_Master.isFadeOutEnabled)
			{
				this.m_CoFadeOut = base.StartCoroutine(this.CoUpdateFadeOut());
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006FE7 File Offset: 0x000051E7
		public void OnMasterEnable()
		{
			this.visible = true;
			this.RestartFadeOutCoroutine();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006FF6 File Offset: 0x000051F6
		public void OnMasterDisable()
		{
			this.StopFadeOutCoroutine();
			this.visible = false;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007005 File Offset: 0x00005205
		private void OnDisable()
		{
			SRPHelper.UnregisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
			this.m_CurrentCameraRenderingSRP = null;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000701F File Offset: 0x0000521F
		public static bool isCustomRenderPipelineSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00007022 File Offset: 0x00005222
		private bool shouldUseGPUInstancedMaterial
		{
			get
			{
				return this.m_Master._INTERNAL_DynamicOcclusionMode != MaterialManager.SD.DynamicOcclusion.DepthTexture && Config.Instance.GetActualRenderingMode(ShaderMode.SD) == RenderingMode.GPUInstancing;
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007042 File Offset: 0x00005242
		private void OnEnable()
		{
			this.RestartFadeOutCoroutine();
			SRPHelper.RegisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000705C File Offset: 0x0000525C
		public void Initialize(VolumetricLightBeamSD master)
		{
			HideFlags proceduralObjectsHideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			this.m_Master = master;
			base.transform.SetParent(master.transform, false);
			base.meshRenderer = base.gameObject.GetOrAddComponent<MeshRenderer>();
			base.meshRenderer.hideFlags = proceduralObjectsHideFlags;
			base.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			base.meshRenderer.receiveShadows = false;
			base.meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			base.meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			if (!this.shouldUseGPUInstancedMaterial)
			{
				this.m_CustomMaterial = Config.Instance.NewMaterialTransient(ShaderMode.SD, false);
				this.ApplyMaterial();
			}
			if (SortingLayer.IsValid(this.m_Master.sortingLayerID))
			{
				this.sortingLayerID = this.m_Master.sortingLayerID;
			}
			else
			{
				Debug.LogError(string.Format("Beam '{0}' has an invalid sortingLayerID ({1}). Please fix it by setting a valid layer.", Utils.GetPath(this.m_Master.transform), this.m_Master.sortingLayerID));
			}
			this.sortingOrder = this.m_Master.sortingOrder;
			base.meshFilter = base.gameObject.GetOrAddComponent<MeshFilter>();
			base.meshFilter.hideFlags = proceduralObjectsHideFlags;
			base.gameObject.hideFlags = proceduralObjectsHideFlags;
			this.RestartFadeOutCoroutine();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000718C File Offset: 0x0000538C
		public void RegenerateMesh(bool masterEnabled)
		{
			if (Config.Instance.geometryOverrideLayer)
			{
				base.gameObject.layer = Config.Instance.geometryLayerID;
			}
			else
			{
				base.gameObject.layer = this.m_Master.gameObject.layer;
			}
			base.gameObject.tag = Config.Instance.geometryTag;
			if (base.coneMesh && this.m_CurrentMeshType == MeshType.Custom)
			{
				UnityEngine.Object.DestroyImmediate(base.coneMesh);
			}
			this.m_CurrentMeshType = this.m_Master.geomMeshType;
			MeshType geomMeshType = this.m_Master.geomMeshType;
			if (geomMeshType != MeshType.Shared)
			{
				if (geomMeshType == MeshType.Custom)
				{
					base.coneMesh = MeshGenerator.GenerateConeZ_Radii(1f, 1f, 1f, this.m_Master.geomCustomSides, this.m_Master.geomCustomSegments, this.m_Master.geomCap, Config.Instance.SD_requiresDoubleSidedMesh);
					base.coneMesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
					base.meshFilter.mesh = base.coneMesh;
				}
				else
				{
					Debug.LogError("Unsupported MeshType");
				}
			}
			else
			{
				base.coneMesh = GlobalMeshSD.Get();
				base.meshFilter.sharedMesh = base.coneMesh;
			}
			this.UpdateMaterialAndBounds();
			this.visible = masterEnabled;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000072D4 File Offset: 0x000054D4
		private Vector3 ComputeLocalMatrix()
		{
			float num = Mathf.Max(this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd);
			base.transform.localScale = new Vector3(num, num, this.m_Master.maxGeometryDistance);
			base.transform.localRotation = this.m_Master.beamInternalLocalRotation;
			return base.transform.localScale;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000733B File Offset: 0x0000553B
		private bool isNoiseEnabled
		{
			get
			{
				return this.m_Master.isNoiseEnabled && this.m_Master.noiseIntensity > 0f && Noise3D.isSupported;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00007363 File Offset: 0x00005563
		private bool isDepthBlendEnabled
		{
			get
			{
				return BatchingHelper.forceEnableDepthBlend || this.m_Master.depthBlendDistance > 0f;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007380 File Offset: 0x00005580
		private MaterialManager.StaticPropertiesSD ComputeMaterialStaticProperties()
		{
			MaterialManager.ColorGradient colorGradient = MaterialManager.ColorGradient.Off;
			if (this.m_Master.colorMode == ColorMode.Gradient)
			{
				colorGradient = ((Utils.GetFloatPackingPrecision() == Utils.FloatPackingPrecision.High) ? MaterialManager.ColorGradient.MatrixHigh : MaterialManager.ColorGradient.MatrixLow);
			}
			return new MaterialManager.StaticPropertiesSD
			{
				blendingMode = (MaterialManager.BlendingMode)this.m_Master.blendingMode,
				noise3D = (this.isNoiseEnabled ? MaterialManager.Noise3D.On : MaterialManager.Noise3D.Off),
				depthBlend = (this.isDepthBlendEnabled ? MaterialManager.SD.DepthBlend.On : MaterialManager.SD.DepthBlend.Off),
				colorGradient = colorGradient,
				dynamicOcclusion = this.m_Master._INTERNAL_DynamicOcclusionMode_Runtime,
				meshSkewing = (this.m_Master.hasMeshSkewing ? MaterialManager.SD.MeshSkewing.On : MaterialManager.SD.MeshSkewing.Off),
				shaderAccuracy = ((this.m_Master.shaderAccuracy == ShaderAccuracy.Fast) ? MaterialManager.SD.ShaderAccuracy.Fast : MaterialManager.SD.ShaderAccuracy.High)
			};
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007438 File Offset: 0x00005638
		private bool ApplyMaterial()
		{
			MaterialManager.StaticPropertiesSD staticPropertiesSD = this.ComputeMaterialStaticProperties();
			Material material;
			if (!this.shouldUseGPUInstancedMaterial)
			{
				material = this.m_CustomMaterial;
				if (material)
				{
					staticPropertiesSD.ApplyToMaterial(material);
				}
			}
			else
			{
				material = MaterialManager.GetInstancedMaterial(this.m_Master._INTERNAL_InstancedMaterialGroupID, ref staticPropertiesSD);
			}
			base.meshRenderer.material = material;
			return material != null;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007495 File Offset: 0x00005695
		public void SetMaterialProp(int nameID, float value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetFloat(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetFloat(nameID, value);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000074BE File Offset: 0x000056BE
		public void SetMaterialProp(int nameID, Vector4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetVector(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetVector(nameID, value);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000074E7 File Offset: 0x000056E7
		public void SetMaterialProp(int nameID, Color value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetColor(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetColor(nameID, value);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007510 File Offset: 0x00005710
		public void SetMaterialProp(int nameID, Matrix4x4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetMatrix(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetMatrix(nameID, value);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007539 File Offset: 0x00005739
		public void SetMaterialProp(int nameID, Texture value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetTexture(nameID, value);
				return;
			}
			Debug.LogError("Setting a Texture property to a GPU instanced material is not supported");
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007560 File Offset: 0x00005760
		private void MaterialChangeStart()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.GetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007580 File Offset: 0x00005780
		private void MaterialChangeStop()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.SetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000075A0 File Offset: 0x000057A0
		public void SetDynamicOcclusionCallback(string shaderKeyword, MaterialModifier.Callback cb)
		{
			this.m_MaterialModifierCallback = cb;
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetKeywordEnabled(shaderKeyword, cb != null);
				if (cb != null)
				{
					cb(this);
					return;
				}
			}
			else
			{
				this.UpdateMaterialAndBounds();
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000075D8 File Offset: 0x000057D8
		public void UpdateMaterialAndBounds()
		{
			if (!this.ApplyMaterial())
			{
				return;
			}
			this.MaterialChangeStart();
			if (this.m_CustomMaterial == null && this.m_MaterialModifierCallback != null)
			{
				this.m_MaterialModifierCallback(this);
			}
			float f = this.m_Master.coneAngle * 0.017453292f / 2f;
			this.SetMaterialProp(ShaderProperties.SD.ConeSlopeCosSin, new Vector2(Mathf.Cos(f), Mathf.Sin(f)));
			Vector2 v = new Vector2(Mathf.Max(this.m_Master.coneRadiusStart, 0.0001f), Mathf.Max(this.m_Master.coneRadiusEnd, 0.0001f));
			this.SetMaterialProp(ShaderProperties.ConeRadius, v);
			float x = Mathf.Sign(this.m_Master.coneApexOffsetZ) * Mathf.Max(Mathf.Abs(this.m_Master.coneApexOffsetZ), 0.0001f);
			this.SetMaterialProp(ShaderProperties.ConeGeomProps, new Vector2(x, (float)this.m_Master.geomSides));
			if (this.m_Master.usedColorMode == ColorMode.Flat)
			{
				this.SetMaterialProp(ShaderProperties.ColorFlat, this.m_Master.color);
			}
			else
			{
				Utils.FloatPackingPrecision floatPackingPrecision = Utils.GetFloatPackingPrecision();
				this.m_ColorGradientMatrix = this.m_Master.colorGradient.SampleInMatrix((int)floatPackingPrecision);
			}
			float value;
			float value2;
			this.m_Master.GetInsideAndOutsideIntensity(out value, out value2);
			this.SetMaterialProp(ShaderProperties.SD.AlphaInside, value);
			this.SetMaterialProp(ShaderProperties.SD.AlphaOutside, value2);
			this.SetMaterialProp(ShaderProperties.SD.AttenuationLerpLinearQuad, this.m_Master.attenuationLerpLinearQuad);
			this.SetMaterialProp(ShaderProperties.DistanceFallOff, new Vector3(this.m_Master.fallOffStart, this.m_Master.fallOffEnd, this.m_Master.maxGeometryDistance));
			this.SetMaterialProp(ShaderProperties.SD.DistanceCamClipping, this.m_Master.cameraClippingDistance);
			this.SetMaterialProp(ShaderProperties.SD.FresnelPow, Mathf.Max(0.001f, this.m_Master.fresnelPow));
			this.SetMaterialProp(ShaderProperties.SD.GlareBehind, this.m_Master.glareBehind);
			this.SetMaterialProp(ShaderProperties.SD.GlareFrontal, this.m_Master.glareFrontal);
			this.SetMaterialProp(ShaderProperties.SD.DrawCap, (float)(this.m_Master.geomCap ? 1 : 0));
			this.SetMaterialProp(ShaderProperties.SD.TiltVector, this.m_Master.tiltFactor);
			this.SetMaterialProp(ShaderProperties.SD.AdditionalClippingPlaneWS, this.m_Master.additionalClippingPlane);
			if (Config.Instance.isHDRPExposureWeightSupported)
			{
				this.SetMaterialProp(ShaderProperties.HDRPExposureWeight, this.m_Master.hdrpExposureWeight);
			}
			if (this.isDepthBlendEnabled)
			{
				this.SetMaterialProp(ShaderProperties.SD.DepthBlendDistance, this.m_Master.depthBlendDistance);
			}
			if (this.isNoiseEnabled)
			{
				Noise3D.LoadIfNeeded();
				Vector3 vector = this.m_Master.noiseVelocityUseGlobal ? Config.Instance.globalNoiseVelocity : this.m_Master.noiseVelocityLocal;
				float w = this.m_Master.noiseScaleUseGlobal ? Config.Instance.globalNoiseScale : this.m_Master.noiseScaleLocal;
				this.SetMaterialProp(ShaderProperties.NoiseVelocityAndScale, new Vector4(vector.x, vector.y, vector.z, w));
				this.SetMaterialProp(ShaderProperties.NoiseParam, new Vector2(this.m_Master.noiseIntensity, (this.m_Master.noiseMode == NoiseMode.WorldSpace) ? 0f : 1f));
			}
			Vector3 vector2 = this.ComputeLocalMatrix();
			if (this.m_Master.hasMeshSkewing)
			{
				Vector3 skewingLocalForwardDirectionNormalized = this.m_Master.skewingLocalForwardDirectionNormalized;
				this.SetMaterialProp(ShaderProperties.SD.LocalForwardDirection, skewingLocalForwardDirectionNormalized);
				if (base.coneMesh != null)
				{
					Vector3 vector3 = skewingLocalForwardDirectionNormalized;
					vector3 /= vector3.z;
					vector3 *= this.m_Master.fallOffEnd;
					vector3.x /= vector2.x;
					vector3.y /= vector2.y;
					Bounds bounds = MeshGenerator.ComputeBounds(1f, 1f, 1f);
					Vector3 min = bounds.min;
					Vector3 max = bounds.max;
					if (vector3.x > 0f)
					{
						max.x += vector3.x;
					}
					else
					{
						min.x += vector3.x;
					}
					if (vector3.y > 0f)
					{
						max.y += vector3.y;
					}
					else
					{
						min.y += vector3.y;
					}
					bounds.min = min;
					bounds.max = max;
					base.coneMesh.bounds = bounds;
				}
			}
			this.UpdateMatricesPropertiesForGPUInstancingSRP();
			this.MaterialChangeStop();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007A94 File Offset: 0x00005C94
		private void UpdateMatricesPropertiesForGPUInstancingSRP()
		{
			if (SRPHelper.IsUsingCustomRenderPipeline() && Config.Instance.GetActualRenderingMode(ShaderMode.SD) == RenderingMode.GPUInstancing)
			{
				this.SetMaterialProp(ShaderProperties.LocalToWorldMatrix, base.transform.localToWorldMatrix);
				this.SetMaterialProp(ShaderProperties.WorldToLocalMatrix, base.transform.worldToLocalMatrix);
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007AE2 File Offset: 0x00005CE2
		private void OnBeginCameraRenderingSRP(ScriptableRenderContext context, Camera cam)
		{
			this.m_CurrentCameraRenderingSRP = cam;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007AEC File Offset: 0x00005CEC
		private void OnWillRenderObject()
		{
			Camera cam;
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				cam = this.m_CurrentCameraRenderingSRP;
			}
			else
			{
				cam = Camera.current;
			}
			this.OnWillCameraRenderThisBeam(cam);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007B18 File Offset: 0x00005D18
		private void OnWillCameraRenderThisBeam(Camera cam)
		{
			if (this.m_Master && cam && cam.enabled)
			{
				this.UpdateCameraRelatedProperties(cam);
				this.m_Master._INTERNAL_OnWillCameraRenderThisBeam(cam);
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007B4C File Offset: 0x00005D4C
		private void UpdateCameraRelatedProperties(Camera cam)
		{
			if (cam && this.m_Master)
			{
				this.MaterialChangeStart();
				Vector3 posOS = this.m_Master.transform.InverseTransformPoint(cam.transform.position);
				Vector3 normalized = base.transform.InverseTransformDirection(cam.transform.forward).normalized;
				float w = cam.orthographic ? -1f : this.m_Master.GetInsideBeamFactorFromObjectSpacePos(posOS);
				this.SetMaterialProp(ShaderProperties.SD.CameraParams, new Vector4(normalized.x, normalized.y, normalized.z, w));
				this.UpdateMatricesPropertiesForGPUInstancingSRP();
				if (this.m_Master.usedColorMode == ColorMode.Gradient)
				{
					this.SetMaterialProp(ShaderProperties.ColorGradientMatrix, this.m_ColorGradientMatrix);
				}
				this.MaterialChangeStop();
				if (this.m_Master.depthBlendDistance > 0f)
				{
					cam.depthTextureMode |= DepthTextureMode.Depth;
				}
			}
		}

		// Token: 0x0400011D RID: 285
		private VolumetricLightBeamSD m_Master;

		// Token: 0x0400011E RID: 286
		private MeshType m_CurrentMeshType;

		// Token: 0x0400011F RID: 287
		private MaterialModifier.Callback m_MaterialModifierCallback;

		// Token: 0x04000120 RID: 288
		private Coroutine m_CoFadeOut;

		// Token: 0x04000121 RID: 289
		private Camera m_CurrentCameraRenderingSRP;
	}
}
