using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x0200000A RID: 10
	[HelpURL("http://saladgamer.com/vlb-doc/config/")]
	public class Config : ScriptableObject
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002E5A File Offset: 0x0000105A
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002E62 File Offset: 0x00001062
		public RenderPipeline renderPipeline
		{
			get
			{
				return this.m_RenderPipeline;
			}
			set
			{
				Debug.LogError("Modifying the RenderPipeline in standalone builds is not permitted");
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002E6E File Offset: 0x0000106E
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002E76 File Offset: 0x00001076
		public RenderingMode renderingMode
		{
			get
			{
				return this.m_RenderingMode;
			}
			set
			{
				Debug.LogError("Modifying the RenderingMode in standalone builds is not permitted");
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002E84 File Offset: 0x00001084
		public bool IsSRPBatcherSupported()
		{
			if (this.renderPipeline == RenderPipeline.BuiltIn)
			{
				return false;
			}
			RenderPipeline projectRenderPipeline = SRPHelper.projectRenderPipeline;
			return projectRenderPipeline == RenderPipeline.URP || projectRenderPipeline == RenderPipeline.HDRP;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002EAB File Offset: 0x000010AB
		public RenderingMode GetActualRenderingMode(ShaderMode shaderMode)
		{
			if (this.renderingMode == RenderingMode.SRPBatcher && !this.IsSRPBatcherSupported())
			{
				return RenderingMode.Default;
			}
			if (this.renderPipeline != RenderPipeline.BuiltIn && this.renderingMode == RenderingMode.MultiPass)
			{
				return RenderingMode.Default;
			}
			if (shaderMode == ShaderMode.HD && this.renderingMode == RenderingMode.MultiPass)
			{
				return RenderingMode.Default;
			}
			return this.renderingMode;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002EE6 File Offset: 0x000010E6
		public bool SD_useSinglePassShader
		{
			get
			{
				return this.GetActualRenderingMode(ShaderMode.SD) > RenderingMode.MultiPass;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002EF2 File Offset: 0x000010F2
		public bool SD_requiresDoubleSidedMesh
		{
			get
			{
				return this.SD_useSinglePassShader;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002EFA File Offset: 0x000010FA
		public unsafe Shader GetBeamShader(ShaderMode mode)
		{
			return *this.GetBeamShaderInternal(mode);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002F04 File Offset: 0x00001104
		private ref Shader GetBeamShaderInternal(ShaderMode mode)
		{
			if (mode == ShaderMode.SD)
			{
				return ref this._BeamShader;
			}
			return ref this._BeamShaderHD;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002F16 File Offset: 0x00001116
		private int GetRenderQueueInternal(ShaderMode mode)
		{
			if (mode == ShaderMode.SD)
			{
				return this.geometryRenderQueue;
			}
			return this.geometryRenderQueueHD;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002F28 File Offset: 0x00001128
		public Material NewMaterialTransient(ShaderMode mode, bool gpuInstanced)
		{
			Material material = MaterialManager.NewMaterialPersistent(this.GetBeamShader(mode), gpuInstanced);
			if (material)
			{
				material.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				material.renderQueue = this.GetRenderQueueInternal(mode);
			}
			return material;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002F64 File Offset: 0x00001164
		public void SetURPScriptableRendererIndexToDepthCamera(Camera camera)
		{
			if (this.urpDepthCameraScriptableRendererIndex < 0)
			{
				return;
			}
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
			if (universalAdditionalCameraData)
			{
				universalAdditionalCameraData.SetRenderer(this.urpDepthCameraScriptableRendererIndex);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002F96 File Offset: 0x00001196
		public Transform fadeOutCameraTransform
		{
			get
			{
				if (this.m_CachedFadeOutCamera == null || !this.m_CachedFadeOutCamera.isActiveAndEnabled)
				{
					this.ForceUpdateFadeOutCamera();
				}
				if (!(this.m_CachedFadeOutCamera != null))
				{
					return null;
				}
				return this.m_CachedFadeOutCamera.transform;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002FD4 File Offset: 0x000011D4
		public string fadeOutCameraName
		{
			get
			{
				if (!(this.m_CachedFadeOutCamera != null))
				{
					return "Invalid Camera";
				}
				return this.m_CachedFadeOutCamera.name;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002FF8 File Offset: 0x000011F8
		public void ForceUpdateFadeOutCamera()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(this.fadeOutCameraTag);
			if (array != null)
			{
				foreach (GameObject gameObject in array)
				{
					if (gameObject)
					{
						Camera component = gameObject.GetComponent<Camera>();
						if (component && component.isActiveAndEnabled)
						{
							this.m_CachedFadeOutCamera = component;
							return;
						}
					}
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003053 File Offset: 0x00001253
		public int defaultRaymarchingQualityUniqueID
		{
			get
			{
				return this.m_DefaultRaymarchingQualityUniqueID;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000305B File Offset: 0x0000125B
		public RaymarchingQuality GetRaymarchingQualityForIndex(int index)
		{
			return this.m_RaymarchingQualities[index];
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003068 File Offset: 0x00001268
		public RaymarchingQuality GetRaymarchingQualityForUniqueID(int id)
		{
			int raymarchingQualityIndexForUniqueID = this.GetRaymarchingQualityIndexForUniqueID(id);
			if (raymarchingQualityIndexForUniqueID >= 0)
			{
				return this.GetRaymarchingQualityForIndex(raymarchingQualityIndexForUniqueID);
			}
			return null;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000308C File Offset: 0x0000128C
		public int GetRaymarchingQualityIndexForUniqueID(int id)
		{
			for (int i = 0; i < this.m_RaymarchingQualities.Length; i++)
			{
				RaymarchingQuality raymarchingQuality = this.m_RaymarchingQualities[i];
				if (raymarchingQuality != null && raymarchingQuality.uniqueID == id)
				{
					return i;
				}
			}
			Debug.LogErrorFormat("Failed to find RaymarchingQualityIndex for Unique ID {0}", new object[]
			{
				id
			});
			return -1;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000030DD File Offset: 0x000012DD
		public bool IsRaymarchingQualityUniqueIDValid(int id)
		{
			return this.GetRaymarchingQualityIndexForUniqueID(id) >= 0;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000030EC File Offset: 0x000012EC
		public int raymarchingQualitiesCount
		{
			get
			{
				return Mathf.Max(1, (this.m_RaymarchingQualities != null) ? this.m_RaymarchingQualities.Length : 1);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003108 File Offset: 0x00001308
		private void CreateDefaultRaymarchingQualityPreset(bool onlyIfNeeded)
		{
			if (this.m_RaymarchingQualities == null || this.m_RaymarchingQualities.Length == 0 || !onlyIfNeeded)
			{
				this.m_RaymarchingQualities = new RaymarchingQuality[3];
				this.m_RaymarchingQualities[0] = RaymarchingQuality.New("Fast", 1, 5);
				this.m_RaymarchingQualities[1] = RaymarchingQuality.New("Balanced", 2, 10);
				this.m_RaymarchingQualities[2] = RaymarchingQuality.New("High", 3, 20);
				this.m_DefaultRaymarchingQualityUniqueID = this.m_RaymarchingQualities[1].uniqueID;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003186 File Offset: 0x00001386
		public bool isHDRPExposureWeightSupported
		{
			get
			{
				return this.renderPipeline == RenderPipeline.HDRP;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003191 File Offset: 0x00001391
		public bool hasRenderPipelineMismatch
		{
			get
			{
				return SRPHelper.projectRenderPipeline == RenderPipeline.BuiltIn != (this.m_RenderPipeline == RenderPipeline.BuiltIn);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000031A9 File Offset: 0x000013A9
		[RuntimeInitializeOnLoadMethod]
		private static void OnStartup()
		{
			Config.Instance.m_CachedFadeOutCamera = null;
			Config.Instance.RefreshGlobalShaderProperties();
			if (Config.Instance.hasRenderPipelineMismatch)
			{
				Debug.LogError("It looks like the 'Render Pipeline' is not correctly set in the config. Please make sure to select the proper value depending on your pipeline in use.", Config.Instance);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000031DC File Offset: 0x000013DC
		public void Reset()
		{
			this.geometryOverrideLayer = true;
			this.geometryLayerID = 1;
			this.geometryTag = "Untagged";
			this.geometryRenderQueue = 3000;
			this.geometryRenderQueueHD = 3100;
			this.sharedMeshSides = 24;
			this.sharedMeshSegments = 5;
			this.globalNoiseScale = 0.5f;
			this.globalNoiseVelocity = Consts.Beam.NoiseVelocityDefault;
			this.renderPipeline = RenderPipeline.BuiltIn;
			this.renderingMode = RenderingMode.Default;
			this.ditheringFactor = 0f;
			this.useLightColorTemperature = true;
			this.fadeOutCameraTag = "MainCamera";
			this.featureEnabledColorGradient = FeatureEnabledColorGradient.HighOnly;
			this.featureEnabledDepthBlend = true;
			this.featureEnabledNoise3D = true;
			this.featureEnabledDynamicOcclusion = true;
			this.featureEnabledMeshSkewing = true;
			this.featureEnabledShaderAccuracyHigh = true;
			this.hdBeamsCameraBlendingDistance = 0.5f;
			this.urpDepthCameraScriptableRendererIndex = -1;
			this.CreateDefaultRaymarchingQualityPreset(false);
			this.ResetInternalData();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000032B4 File Offset: 0x000014B4
		private void RefreshGlobalShaderProperties()
		{
			Shader.SetGlobalFloat(ShaderProperties.GlobalUsesReversedZBuffer, SystemInfo.usesReversedZBuffer ? 1f : 0f);
			Shader.SetGlobalFloat(ShaderProperties.GlobalDitheringFactor, this.ditheringFactor);
			Shader.SetGlobalTexture(ShaderProperties.GlobalDitheringNoiseTex, this.ditheringNoiseTexture);
			Shader.SetGlobalFloat(ShaderProperties.HD.GlobalCameraBlendingDistance, this.hdBeamsCameraBlendingDistance);
			Shader.SetGlobalTexture(ShaderProperties.HD.GlobalJitteringNoiseTex, this.jitteringNoiseTexture);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003320 File Offset: 0x00001520
		public void ResetInternalData()
		{
			this.noiseTexture3D = (Resources.Load("Noise3D_64x64x64") as Texture3D);
			this.dustParticlesPrefab = (Resources.Load("DustParticles", typeof(ParticleSystem)) as ParticleSystem);
			this.ditheringNoiseTexture = (Resources.Load("VLBDitheringNoise", typeof(Texture2D)) as Texture2D);
			this.jitteringNoiseTexture = (Resources.Load("VLBBlueNoise", typeof(Texture2D)) as Texture2D);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000033A0 File Offset: 0x000015A0
		public ParticleSystem NewVolumetricDustParticles()
		{
			if (!this.dustParticlesPrefab)
			{
				if (Application.isPlaying)
				{
					Debug.LogError("Failed to instantiate VolumetricDustParticles prefab.");
				}
				return null;
			}
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.dustParticlesPrefab);
			particleSystem.useAutoRandomSeed = false;
			particleSystem.name = "Dust Particles";
			particleSystem.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			particleSystem.gameObject.SetActive(true);
			return particleSystem;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003406 File Offset: 0x00001606
		private void OnEnable()
		{
			this.CreateDefaultRaymarchingQualityPreset(true);
			this.HandleBackwardCompatibility(this.pluginVersion, 20200);
			this.pluginVersion = 20200;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000342B File Offset: 0x0000162B
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000342D File Offset: 0x0000162D
		public static Config Instance
		{
			get
			{
				return Config.GetInstance(true);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003435 File Offset: 0x00001635
		private static Config LoadAssetInternal(string assetName)
		{
			return Resources.Load<Config>(assetName);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003440 File Offset: 0x00001640
		private static Config GetInstance(bool assertIfNotFound)
		{
			if (Config.ms_Instance == null)
			{
				Config x = Config.LoadAssetInternal("VLBConfigOverride" + PlatformHelper.GetCurrentPlatformSuffix());
				if (x == null)
				{
					x = Config.LoadAssetInternal("VLBConfigOverride");
				}
				Config.ms_Instance = x;
				Config.ms_Instance == null;
			}
			return Config.ms_Instance;
		}

		// Token: 0x04000017 RID: 23
		public const string ClassName = "Config";

		// Token: 0x04000018 RID: 24
		public const string kAssetName = "VLBConfigOverride";

		// Token: 0x04000019 RID: 25
		public const string kAssetNameExt = ".asset";

		// Token: 0x0400001A RID: 26
		public bool geometryOverrideLayer = true;

		// Token: 0x0400001B RID: 27
		public int geometryLayerID = 1;

		// Token: 0x0400001C RID: 28
		public string geometryTag = "Untagged";

		// Token: 0x0400001D RID: 29
		public int geometryRenderQueue = 3000;

		// Token: 0x0400001E RID: 30
		public int geometryRenderQueueHD = 3100;

		// Token: 0x0400001F RID: 31
		[FormerlySerializedAs("renderPipeline")]
		[FormerlySerializedAs("_RenderPipeline")]
		[SerializeField]
		private RenderPipeline m_RenderPipeline;

		// Token: 0x04000020 RID: 32
		[FormerlySerializedAs("renderingMode")]
		[FormerlySerializedAs("_RenderingMode")]
		[SerializeField]
		private RenderingMode m_RenderingMode = RenderingMode.Default;

		// Token: 0x04000021 RID: 33
		public float ditheringFactor;

		// Token: 0x04000022 RID: 34
		public bool useLightColorTemperature = true;

		// Token: 0x04000023 RID: 35
		public int sharedMeshSides = 24;

		// Token: 0x04000024 RID: 36
		public int sharedMeshSegments = 5;

		// Token: 0x04000025 RID: 37
		public float hdBeamsCameraBlendingDistance = 0.5f;

		// Token: 0x04000026 RID: 38
		public int urpDepthCameraScriptableRendererIndex = -1;

		// Token: 0x04000027 RID: 39
		[Range(0.01f, 2f)]
		public float globalNoiseScale = 0.5f;

		// Token: 0x04000028 RID: 40
		public Vector3 globalNoiseVelocity = Consts.Beam.NoiseVelocityDefault;

		// Token: 0x04000029 RID: 41
		public string fadeOutCameraTag = "MainCamera";

		// Token: 0x0400002A RID: 42
		[HighlightNull]
		public Texture3D noiseTexture3D;

		// Token: 0x0400002B RID: 43
		[HighlightNull]
		public ParticleSystem dustParticlesPrefab;

		// Token: 0x0400002C RID: 44
		[HighlightNull]
		public Texture2D ditheringNoiseTexture;

		// Token: 0x0400002D RID: 45
		[HighlightNull]
		public Texture2D jitteringNoiseTexture;

		// Token: 0x0400002E RID: 46
		public FeatureEnabledColorGradient featureEnabledColorGradient = FeatureEnabledColorGradient.HighOnly;

		// Token: 0x0400002F RID: 47
		public bool featureEnabledDepthBlend = true;

		// Token: 0x04000030 RID: 48
		public bool featureEnabledNoise3D = true;

		// Token: 0x04000031 RID: 49
		public bool featureEnabledDynamicOcclusion = true;

		// Token: 0x04000032 RID: 50
		public bool featureEnabledMeshSkewing = true;

		// Token: 0x04000033 RID: 51
		public bool featureEnabledShaderAccuracyHigh = true;

		// Token: 0x04000034 RID: 52
		public bool featureEnabledShadow = true;

		// Token: 0x04000035 RID: 53
		public bool featureEnabledCookie = true;

		// Token: 0x04000036 RID: 54
		[SerializeField]
		private RaymarchingQuality[] m_RaymarchingQualities;

		// Token: 0x04000037 RID: 55
		[SerializeField]
		private int m_DefaultRaymarchingQualityUniqueID;

		// Token: 0x04000038 RID: 56
		[SerializeField]
		private int pluginVersion = -1;

		// Token: 0x04000039 RID: 57
		[SerializeField]
		private Material _DummyMaterial;

		// Token: 0x0400003A RID: 58
		[SerializeField]
		private Material _DummyMaterialHD;

		// Token: 0x0400003B RID: 59
		[SerializeField]
		private Shader _BeamShader;

		// Token: 0x0400003C RID: 60
		[SerializeField]
		private Shader _BeamShaderHD;

		// Token: 0x0400003D RID: 61
		private Camera m_CachedFadeOutCamera;

		// Token: 0x0400003E RID: 62
		private static Config ms_Instance;
	}
}
