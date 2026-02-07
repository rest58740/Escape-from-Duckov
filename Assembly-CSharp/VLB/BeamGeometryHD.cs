using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000025 RID: 37
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-hd/")]
	public class BeamGeometryHD : BeamGeometryAbstractBase
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00003A11 File Offset: 0x00001C11
		protected override VolumetricLightBeamAbstractBase GetMaster()
		{
			return this.m_Master;
		}

		// Token: 0x17000012 RID: 18
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00003A19 File Offset: 0x00001C19
		public bool visible
		{
			set
			{
				if (base.meshRenderer)
				{
					base.meshRenderer.enabled = value;
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003A34 File Offset: 0x00001C34
		public int sortingLayerID
		{
			set
			{
				if (base.meshRenderer)
				{
					base.meshRenderer.sortingLayerID = value;
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003A4F File Offset: 0x00001C4F
		public int sortingOrder
		{
			set
			{
				if (base.meshRenderer)
				{
					base.meshRenderer.sortingOrder = value;
				}
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003A6A File Offset: 0x00001C6A
		private void OnDisable()
		{
			SRPHelper.UnregisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
			this.m_CurrentCameraRenderingSRP = null;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003A84 File Offset: 0x00001C84
		public static bool isCustomRenderPipelineSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003A87 File Offset: 0x00001C87
		private bool shouldUseGPUInstancedMaterial
		{
			get
			{
				return Config.Instance.GetActualRenderingMode(ShaderMode.HD) == RenderingMode.GPUInstancing && this.m_Cookie == null && this.m_Shadow == null;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003AB5 File Offset: 0x00001CB5
		private void OnEnable()
		{
			SRPHelper.RegisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003AC8 File Offset: 0x00001CC8
		public void Initialize(VolumetricLightBeamHD master)
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
			this.m_Cookie = this.m_Master.GetAdditionalComponentCookie();
			this.m_Shadow = this.m_Master.GetAdditionalComponentShadow();
			if (!this.shouldUseGPUInstancedMaterial)
			{
				this.m_CustomMaterial = Config.Instance.NewMaterialTransient(ShaderMode.HD, false);
				this.ApplyMaterial();
			}
			if (this.m_Master.DoesSupportSorting2D())
			{
				if (SortingLayer.IsValid(this.m_Master.GetSortingLayerID()))
				{
					this.sortingLayerID = this.m_Master.GetSortingLayerID();
				}
				else
				{
					Debug.LogError(string.Format("Beam '{0}' has an invalid sortingLayerID ({1}). Please fix it by setting a valid layer.", Utils.GetPath(this.m_Master.transform), this.m_Master.GetSortingLayerID()));
				}
				this.sortingOrder = this.m_Master.GetSortingOrder();
			}
			base.meshFilter = base.gameObject.GetOrAddComponent<MeshFilter>();
			base.meshFilter.hideFlags = proceduralObjectsHideFlags;
			base.gameObject.hideFlags = proceduralObjectsHideFlags;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003C20 File Offset: 0x00001E20
		public void RegenerateMesh()
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
			base.coneMesh = GlobalMeshHD.Get();
			base.meshFilter.sharedMesh = base.coneMesh;
			this.UpdateMaterialAndBounds();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003CA4 File Offset: 0x00001EA4
		private Vector3 ComputeLocalMatrix()
		{
			float num = Mathf.Max(this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd);
			Vector3 vector = new Vector3(num, num, this.m_Master.maxGeometryDistance);
			if (!this.m_Master.scalable)
			{
				vector = vector.Divide(this.m_Master.GetLossyScale());
			}
			base.transform.localScale = vector;
			base.transform.localRotation = this.m_Master.beamInternalLocalRotation;
			return vector;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003D23 File Offset: 0x00001F23
		private bool isNoiseEnabled
		{
			get
			{
				return this.m_Master.isNoiseEnabled && this.m_Master.noiseIntensity > 0f && Noise3D.isSupported;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003D4C File Offset: 0x00001F4C
		private MaterialManager.StaticPropertiesHD ComputeMaterialStaticProperties()
		{
			MaterialManager.ColorGradient colorGradient = MaterialManager.ColorGradient.Off;
			if (this.m_Master.colorMode == ColorMode.Gradient)
			{
				colorGradient = ((Utils.GetFloatPackingPrecision() == Utils.FloatPackingPrecision.High) ? MaterialManager.ColorGradient.MatrixHigh : MaterialManager.ColorGradient.MatrixLow);
			}
			return new MaterialManager.StaticPropertiesHD
			{
				blendingMode = (MaterialManager.BlendingMode)this.m_Master.blendingMode,
				attenuation = ((this.m_Master.attenuationEquation == AttenuationEquationHD.Linear) ? MaterialManager.HD.Attenuation.Linear : MaterialManager.HD.Attenuation.Quadratic),
				noise3D = (this.isNoiseEnabled ? MaterialManager.Noise3D.On : MaterialManager.Noise3D.Off),
				colorGradient = colorGradient,
				shadow = ((this.m_Shadow != null) ? MaterialManager.HD.Shadow.On : MaterialManager.HD.Shadow.Off),
				cookie = ((this.m_Cookie != null) ? ((this.m_Cookie.channel == CookieChannel.RGBA) ? MaterialManager.HD.Cookie.RGBA : MaterialManager.HD.Cookie.SingleChannel) : MaterialManager.HD.Cookie.Off),
				raymarchingQualityIndex = this.m_Master.raymarchingQualityIndex
			};
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003E1C File Offset: 0x0000201C
		private bool ApplyMaterial()
		{
			MaterialManager.StaticPropertiesHD staticPropertiesHD = this.ComputeMaterialStaticProperties();
			Material material;
			if (!this.shouldUseGPUInstancedMaterial)
			{
				material = this.m_CustomMaterial;
				if (material)
				{
					staticPropertiesHD.ApplyToMaterial(material);
				}
			}
			else
			{
				material = MaterialManager.GetInstancedMaterial(this.m_Master._INTERNAL_InstancedMaterialGroupID, ref staticPropertiesHD);
			}
			base.meshRenderer.material = material;
			return material != null;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003E79 File Offset: 0x00002079
		public void SetMaterialProp(int nameID, float value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetFloat(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetFloat(nameID, value);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003EA2 File Offset: 0x000020A2
		public void SetMaterialProp(int nameID, Vector4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetVector(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetVector(nameID, value);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003ECB File Offset: 0x000020CB
		public void SetMaterialProp(int nameID, Color value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetColor(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetColor(nameID, value);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003EF4 File Offset: 0x000020F4
		public void SetMaterialProp(int nameID, Matrix4x4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetMatrix(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetMatrix(nameID, value);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003F1D File Offset: 0x0000211D
		public void SetMaterialProp(int nameID, Texture value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetTexture(nameID, value);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003F3C File Offset: 0x0000213C
		public void SetMaterialProp(int nameID, BeamGeometryHD.InvalidTexture invalidTexture)
		{
			if (this.m_CustomMaterial)
			{
				Texture value = null;
				if (invalidTexture == BeamGeometryHD.InvalidTexture.NoDepth)
				{
					value = (SystemInfo.usesReversedZBuffer ? Texture2D.blackTexture : Texture2D.whiteTexture);
				}
				this.m_CustomMaterial.SetTexture(nameID, value);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003F7D File Offset: 0x0000217D
		private void MaterialChangeStart()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.GetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003F9D File Offset: 0x0000219D
		private void MaterialChangeStop()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.SetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003FBD File Offset: 0x000021BD
		public void SetPropertyDirty(DirtyProps prop)
		{
			this.m_DirtyProps |= prop;
			if (prop.HasAtLeastOneFlag(DirtyProps.OnlyMaterialChangeOnly))
			{
				this.UpdateMaterialAndBounds();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003FEA File Offset: 0x000021EA
		private void UpdateMaterialAndBounds()
		{
			if (!this.ApplyMaterial())
			{
				return;
			}
			this.MaterialChangeStart();
			this.m_DirtyProps = DirtyProps.All;
			if (this.isNoiseEnabled)
			{
				Noise3D.LoadIfNeeded();
			}
			this.ComputeLocalMatrix();
			this.UpdateMatricesPropertiesForGPUInstancingSRP();
			this.MaterialChangeStop();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004028 File Offset: 0x00002228
		private void UpdateMatricesPropertiesForGPUInstancingSRP()
		{
			if (SRPHelper.IsUsingCustomRenderPipeline() && Config.Instance.GetActualRenderingMode(ShaderMode.HD) == RenderingMode.GPUInstancing)
			{
				this.SetMaterialProp(ShaderProperties.LocalToWorldMatrix, base.transform.localToWorldMatrix);
				this.SetMaterialProp(ShaderProperties.WorldToLocalMatrix, base.transform.worldToLocalMatrix);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004076 File Offset: 0x00002276
		private void OnBeginCameraRenderingSRP(ScriptableRenderContext context, Camera cam)
		{
			this.m_CurrentCameraRenderingSRP = cam;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004080 File Offset: 0x00002280
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

		// Token: 0x06000088 RID: 136 RVA: 0x000040AC File Offset: 0x000022AC
		private void OnWillCameraRenderThisBeam(Camera cam)
		{
			if (this.m_Master && cam && cam.enabled)
			{
				this.UpdateMaterialPropertiesForCamera(cam);
				if (this.m_Shadow)
				{
					this.m_Shadow.OnWillCameraRenderThisBeam(cam, this);
				}
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000040EC File Offset: 0x000022EC
		private void UpdateDirtyMaterialProperties()
		{
			if (this.m_DirtyProps != DirtyProps.None)
			{
				if (this.m_DirtyProps.HasFlag(DirtyProps.Intensity))
				{
					this.SetMaterialProp(ShaderProperties.HD.Intensity, this.m_Master.intensity);
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.HDRPExposureWeight) && Config.Instance.isHDRPExposureWeightSupported)
				{
					this.SetMaterialProp(ShaderProperties.HDRPExposureWeight, this.m_Master.hdrpExposureWeight);
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.SideSoftness))
				{
					this.SetMaterialProp(ShaderProperties.HD.SideSoftness, this.m_Master.sideSoftness);
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.Color))
				{
					if (this.m_Master.colorMode == ColorMode.Flat)
					{
						this.SetMaterialProp(ShaderProperties.ColorFlat, this.m_Master.colorFlat);
					}
					else
					{
						Utils.FloatPackingPrecision floatPackingPrecision = Utils.GetFloatPackingPrecision();
						this.m_ColorGradientMatrix = this.m_Master.colorGradient.SampleInMatrix((int)floatPackingPrecision);
					}
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.Cone))
				{
					Vector2 v = new Vector2(Mathf.Max(this.m_Master.coneRadiusStart, 0.0001f), Mathf.Max(this.m_Master.coneRadiusEnd, 0.0001f));
					this.SetMaterialProp(ShaderProperties.ConeRadius, v);
					float coneApexOffsetZ = this.m_Master.GetConeApexOffsetZ(false);
					float x = Mathf.Sign(coneApexOffsetZ) * Mathf.Max(Mathf.Abs(coneApexOffsetZ), 0.0001f);
					this.SetMaterialProp(ShaderProperties.ConeGeomProps, new Vector2(x, (float)Config.Instance.sharedMeshSides));
					this.SetMaterialProp(ShaderProperties.DistanceFallOff, new Vector3(this.m_Master.fallOffStart, this.m_Master.fallOffEnd, this.m_Master.maxGeometryDistance));
					this.ComputeLocalMatrix();
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.Jittering))
				{
					this.SetMaterialProp(ShaderProperties.HD.Jittering, new Vector4(this.m_Master.jitteringFactor, (float)this.m_Master.jitteringFrameRate, this.m_Master.jitteringLerpRange.minValue, this.m_Master.jitteringLerpRange.maxValue));
				}
				if (this.isNoiseEnabled)
				{
					if (this.m_DirtyProps.HasFlag(DirtyProps.NoiseMode) || this.m_DirtyProps.HasFlag(DirtyProps.NoiseIntensity))
					{
						this.SetMaterialProp(ShaderProperties.NoiseParam, new Vector2(this.m_Master.noiseIntensity, (this.m_Master.noiseMode == NoiseMode.WorldSpace) ? 0f : 1f));
					}
					if (this.m_DirtyProps.HasFlag(DirtyProps.NoiseVelocityAndScale))
					{
						Vector3 vector = this.m_Master.noiseVelocityUseGlobal ? Config.Instance.globalNoiseVelocity : this.m_Master.noiseVelocityLocal;
						float w = this.m_Master.noiseScaleUseGlobal ? Config.Instance.globalNoiseScale : this.m_Master.noiseScaleLocal;
						this.SetMaterialProp(ShaderProperties.NoiseVelocityAndScale, new Vector4(vector.x, vector.y, vector.z, w));
					}
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.CookieProps))
				{
					VolumetricCookieHD.ApplyMaterialProperties(this.m_Cookie, this);
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.ShadowProps))
				{
					VolumetricShadowHD.ApplyMaterialProperties(this.m_Shadow, this);
				}
				this.m_DirtyProps = DirtyProps.None;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000044A8 File Offset: 0x000026A8
		private void UpdateMaterialPropertiesForCamera(Camera cam)
		{
			if (cam && this.m_Master)
			{
				this.MaterialChangeStart();
				this.SetMaterialProp(ShaderProperties.HD.TransformScale, this.m_Master.scalable ? this.m_Master.GetLossyScale() : Vector3.one);
				Vector3 normalized = base.transform.InverseTransformDirection(cam.transform.forward).normalized;
				this.SetMaterialProp(ShaderProperties.HD.CameraForwardOS, normalized);
				this.SetMaterialProp(ShaderProperties.HD.CameraForwardWS, cam.transform.forward);
				this.UpdateDirtyMaterialProperties();
				if (this.m_Master.colorMode == ColorMode.Gradient)
				{
					this.SetMaterialProp(ShaderProperties.ColorGradientMatrix, this.m_ColorGradientMatrix);
				}
				this.UpdateMatricesPropertiesForGPUInstancingSRP();
				this.MaterialChangeStop();
				cam.depthTextureMode |= DepthTextureMode.Depth;
			}
		}

		// Token: 0x040000C5 RID: 197
		private VolumetricLightBeamHD m_Master;

		// Token: 0x040000C6 RID: 198
		private VolumetricCookieHD m_Cookie;

		// Token: 0x040000C7 RID: 199
		private VolumetricShadowHD m_Shadow;

		// Token: 0x040000C8 RID: 200
		private Camera m_CurrentCameraRenderingSRP;

		// Token: 0x040000C9 RID: 201
		private DirtyProps m_DirtyProps;

		// Token: 0x020000A5 RID: 165
		public enum InvalidTexture
		{
			// Token: 0x0400038A RID: 906
			Null,
			// Token: 0x0400038B RID: 907
			NoDepth
		}
	}
}
