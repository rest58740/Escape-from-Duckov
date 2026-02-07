using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200000B RID: 11
	public static class Consts
	{
		// Token: 0x0400003F RID: 63
		public const string PluginFolder = "VolumetricLightBeam";

		// Token: 0x02000097 RID: 151
		public static class Help
		{
			// Token: 0x040002EC RID: 748
			private const string UrlBase = "http://saladgamer.com/vlb-doc/";

			// Token: 0x040002ED RID: 749
			private const string UrlSuffix = "/";

			// Token: 0x040002EE RID: 750
			public const string UrlDustParticles = "http://saladgamer.com/vlb-doc/comp-dustparticles/";

			// Token: 0x040002EF RID: 751
			public const string UrlTriggerZone = "http://saladgamer.com/vlb-doc/comp-triggerzone/";

			// Token: 0x040002F0 RID: 752
			public const string UrlEffectFlicker = "http://saladgamer.com/vlb-doc/comp-effect-flicker/";

			// Token: 0x040002F1 RID: 753
			public const string UrlEffectPulse = "http://saladgamer.com/vlb-doc/comp-effect-pulse/";

			// Token: 0x040002F2 RID: 754
			public const string UrlEffectFromProfile = "http://saladgamer.com/vlb-doc/comp-effect-from-profile/";

			// Token: 0x040002F3 RID: 755
			public const string UrlLODBeamGroup = "http://saladgamer.com/vlb-doc/comp-lodbeamgroup/";

			// Token: 0x040002F4 RID: 756
			public const string UrlConfig = "http://saladgamer.com/vlb-doc/config/";

			// Token: 0x040002F5 RID: 757
			public const string AddComponentMenuBase = "VLB/";

			// Token: 0x040002F6 RID: 758
			public const string AddComponentMenuCommon = "VLB/Common/";

			// Token: 0x040002F7 RID: 759
			public const string AddComponentMenuDustParticles = "VLB/Common/Volumetric Dust Particles";

			// Token: 0x040002F8 RID: 760
			public const string AddComponentMenuTriggerZone = "VLB/Common/Trigger Zone";

			// Token: 0x040002F9 RID: 761
			public const string AddComponentMenuEffectFlicker = "VLB/Common/Effect Flicker";

			// Token: 0x040002FA RID: 762
			public const string AddComponentMenuEffectPulse = "VLB/Common/Effect Pulse";

			// Token: 0x040002FB RID: 763
			public const string AddComponentMenuEffectFromProfile = "VLB/Common/Effect From Profile";

			// Token: 0x020000D6 RID: 214
			public static class SD
			{
				// Token: 0x04000431 RID: 1073
				public const string UrlBeam = "http://saladgamer.com/vlb-doc/comp-lightbeam-sd/";

				// Token: 0x04000432 RID: 1074
				public const string UrlDynamicOcclusionRaycasting = "http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-raycasting/";

				// Token: 0x04000433 RID: 1075
				public const string UrlDynamicOcclusionDepthBuffer = "http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-depthbuffer/";

				// Token: 0x04000434 RID: 1076
				public const string UrlSkewingHandle = "http://saladgamer.com/vlb-doc/comp-skewinghandle-sd/";

				// Token: 0x04000435 RID: 1077
				public const string AddComponentMenuSD = "VLB/SD/";

				// Token: 0x04000436 RID: 1078
				public const string AddComponentMenuBeam = "VLB/SD/Volumetric Light Beam SD";

				// Token: 0x04000437 RID: 1079
				public const string AddComponentMenuDynamicOcclusionRaycasting = "VLB/SD/Dynamic Occlusion (Raycasting)";

				// Token: 0x04000438 RID: 1080
				public const string AddComponentMenuDynamicOcclusionDepthBuffer = "VLB/SD/Dynamic Occlusion (Depth Buffer)";
			}

			// Token: 0x020000D7 RID: 215
			public static class HD
			{
				// Token: 0x04000439 RID: 1081
				public const string UrlBeam = "http://saladgamer.com/vlb-doc/comp-lightbeam-hd/";

				// Token: 0x0400043A RID: 1082
				public const string UrlShadow = "http://saladgamer.com/vlb-doc/comp-shadow-hd/";

				// Token: 0x0400043B RID: 1083
				public const string UrlCookie = "http://saladgamer.com/vlb-doc/comp-cookie-hd/";

				// Token: 0x0400043C RID: 1084
				public const string UrlTrackRealtimeChangesOnLight = "http://saladgamer.com/vlb-doc/comp-trackrealtimechanges-hd/";

				// Token: 0x0400043D RID: 1085
				public const string AddComponentMenuHD = "VLB/HD/";

				// Token: 0x0400043E RID: 1086
				public const string AddComponentMenuBeam3D = "VLB/HD/Volumetric Light Beam HD";

				// Token: 0x0400043F RID: 1087
				public const string AddComponentMenuBeam2D = "VLB/HD/Volumetric Light Beam HD (2D)";

				// Token: 0x04000440 RID: 1088
				public const string AddComponentMenuShadow = "VLB/HD/Volumetric Shadow HD";

				// Token: 0x04000441 RID: 1089
				public const string AddComponentMenuCookie = "VLB/HD/Volumetric Cookie HD";

				// Token: 0x04000442 RID: 1090
				public const string AddComponentMenuTrackRealtimeChangesOnLight = "VLB/HD/Track Realtime Changes On Light";
			}
		}

		// Token: 0x02000098 RID: 152
		public static class Internal
		{
			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x06000478 RID: 1144 RVA: 0x00012FD1 File Offset: 0x000111D1
			public static HideFlags ProceduralObjectsHideFlags
			{
				get
				{
					if (!Consts.Internal.ProceduralObjectsVisibleInEditor)
					{
						return HideFlags.HideAndDontSave;
					}
					return HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
				}
			}

			// Token: 0x040002FC RID: 764
			public static readonly bool ProceduralObjectsVisibleInEditor = true;
		}

		// Token: 0x02000099 RID: 153
		public static class Beam
		{
			// Token: 0x040002FD RID: 765
			public static readonly Color FlatColor = Color.white;

			// Token: 0x040002FE RID: 766
			public const ColorMode ColorModeDefault = ColorMode.Flat;

			// Token: 0x040002FF RID: 767
			public const float MultiplierDefault = 1f;

			// Token: 0x04000300 RID: 768
			public const float MultiplierMin = 0f;

			// Token: 0x04000301 RID: 769
			public const float IntensityDefault = 1f;

			// Token: 0x04000302 RID: 770
			public const float IntensityMin = 0f;

			// Token: 0x04000303 RID: 771
			public const float HDRPExposureWeightDefault = 0f;

			// Token: 0x04000304 RID: 772
			public const float HDRPExposureWeightMin = 0f;

			// Token: 0x04000305 RID: 773
			public const float HDRPExposureWeightMax = 1f;

			// Token: 0x04000306 RID: 774
			public const float SpotAngleDefault = 35f;

			// Token: 0x04000307 RID: 775
			public const float SpotAngleMin = 0.1f;

			// Token: 0x04000308 RID: 776
			public const float SpotAngleMax = 179.9f;

			// Token: 0x04000309 RID: 777
			public const float ConeRadiusStart = 0.1f;

			// Token: 0x0400030A RID: 778
			public const MeshType GeomMeshType = MeshType.Shared;

			// Token: 0x0400030B RID: 779
			public const int GeomSidesDefault = 18;

			// Token: 0x0400030C RID: 780
			public const int GeomSidesMin = 3;

			// Token: 0x0400030D RID: 781
			public const int GeomSidesMax = 256;

			// Token: 0x0400030E RID: 782
			public const int GeomSegmentsDefault = 5;

			// Token: 0x0400030F RID: 783
			public const int GeomSegmentsMin = 0;

			// Token: 0x04000310 RID: 784
			public const int GeomSegmentsMax = 64;

			// Token: 0x04000311 RID: 785
			public const bool GeomCap = false;

			// Token: 0x04000312 RID: 786
			public const bool ScalableDefault = true;

			// Token: 0x04000313 RID: 787
			public const AttenuationEquation AttenuationEquationDefault = AttenuationEquation.Quadratic;

			// Token: 0x04000314 RID: 788
			public const float AttenuationCustomBlendingDefault = 0.5f;

			// Token: 0x04000315 RID: 789
			public const float AttenuationCustomBlendingMin = 0f;

			// Token: 0x04000316 RID: 790
			public const float AttenuationCustomBlendingMax = 1f;

			// Token: 0x04000317 RID: 791
			public const float FallOffStart = 0f;

			// Token: 0x04000318 RID: 792
			public const float FallOffEnd = 3f;

			// Token: 0x04000319 RID: 793
			public const float FallOffDistancesMinThreshold = 0.01f;

			// Token: 0x0400031A RID: 794
			public const float DepthBlendDistance = 2f;

			// Token: 0x0400031B RID: 795
			public const float CameraClippingDistance = 0.5f;

			// Token: 0x0400031C RID: 796
			public const NoiseMode NoiseModeDefault = NoiseMode.Disabled;

			// Token: 0x0400031D RID: 797
			public const float NoiseIntensityMin = 0f;

			// Token: 0x0400031E RID: 798
			public const float NoiseIntensityMax = 1f;

			// Token: 0x0400031F RID: 799
			public const float NoiseIntensityDefault = 0.5f;

			// Token: 0x04000320 RID: 800
			public const float NoiseScaleMin = 0.01f;

			// Token: 0x04000321 RID: 801
			public const float NoiseScaleMax = 2f;

			// Token: 0x04000322 RID: 802
			public const float NoiseScaleDefault = 0.5f;

			// Token: 0x04000323 RID: 803
			public static readonly Vector3 NoiseVelocityDefault = new Vector3(0.07f, 0.18f, 0.05f);

			// Token: 0x04000324 RID: 804
			public const BlendingMode BlendingModeDefault = BlendingMode.Additive;

			// Token: 0x04000325 RID: 805
			public const ShaderAccuracy ShaderAccuracyDefault = ShaderAccuracy.Fast;

			// Token: 0x04000326 RID: 806
			public const float FadeOutBeginDefault = -150f;

			// Token: 0x04000327 RID: 807
			public const float FadeOutEndDefault = -200f;

			// Token: 0x04000328 RID: 808
			public const Dimensions DimensionsDefault = Dimensions.Dim3D;

			// Token: 0x020000D8 RID: 216
			public static class SD
			{
				// Token: 0x04000443 RID: 1091
				public const float FresnelPowMaxValue = 10f;

				// Token: 0x04000444 RID: 1092
				public const float FresnelPow = 8f;

				// Token: 0x04000445 RID: 1093
				public const float GlareFrontalDefault = 0.5f;

				// Token: 0x04000446 RID: 1094
				public const float GlareBehindDefault = 0.5f;

				// Token: 0x04000447 RID: 1095
				public const float GlareMin = 0f;

				// Token: 0x04000448 RID: 1096
				public const float GlareMax = 1f;

				// Token: 0x04000449 RID: 1097
				public static readonly Vector2 TiltDefault = Vector2.zero;

				// Token: 0x0400044A RID: 1098
				public static readonly Vector3 SkewingLocalForwardDirectionDefault = Vector3.forward;

				// Token: 0x0400044B RID: 1099
				public const Transform ClippingPlaneTransformDefault = null;
			}

			// Token: 0x020000D9 RID: 217
			public static class HD
			{
				// Token: 0x0400044C RID: 1100
				public const AttenuationEquationHD AttenuationEquationDefault = AttenuationEquationHD.Quadratic;

				// Token: 0x0400044D RID: 1101
				public const float SideSoftnessDefault = 1f;

				// Token: 0x0400044E RID: 1102
				public const float SideSoftnessMin = 0.0001f;

				// Token: 0x0400044F RID: 1103
				public const float SideSoftnessMax = 10f;

				// Token: 0x04000450 RID: 1104
				public const float JitteringFactorDefault = 0f;

				// Token: 0x04000451 RID: 1105
				public const float JitteringFactorMin = 0f;

				// Token: 0x04000452 RID: 1106
				public const int JitteringFrameRateDefault = 60;

				// Token: 0x04000453 RID: 1107
				public const int JitteringFrameRateMin = 0;

				// Token: 0x04000454 RID: 1108
				public const int JitteringFrameRateMax = 120;

				// Token: 0x04000455 RID: 1109
				public static readonly MinMaxRangeFloat JitteringLerpRange = new MinMaxRangeFloat(0f, 0.33f);
			}
		}

		// Token: 0x0200009A RID: 154
		public static class DustParticles
		{
			// Token: 0x04000329 RID: 809
			public const float AlphaDefault = 0.5f;

			// Token: 0x0400032A RID: 810
			public const float SizeDefault = 0.01f;

			// Token: 0x0400032B RID: 811
			public const ParticlesDirection DirectionDefault = ParticlesDirection.Random;

			// Token: 0x0400032C RID: 812
			public static readonly Vector3 VelocityDefault = new Vector3(0f, 0f, 0.03f);

			// Token: 0x0400032D RID: 813
			public const float DensityDefault = 5f;

			// Token: 0x0400032E RID: 814
			public const float DensityMin = 0f;

			// Token: 0x0400032F RID: 815
			public const float DensityMax = 1000f;

			// Token: 0x04000330 RID: 816
			public static readonly MinMaxRangeFloat SpawnDistanceRangeDefault = new MinMaxRangeFloat(0f, 0.7f);

			// Token: 0x04000331 RID: 817
			public const bool CullingEnabledDefault = false;

			// Token: 0x04000332 RID: 818
			public const float CullingMaxDistanceDefault = 10f;

			// Token: 0x04000333 RID: 819
			public const float CullingMaxDistanceMin = 1f;
		}

		// Token: 0x0200009B RID: 155
		public static class DynOcclusion
		{
			// Token: 0x04000334 RID: 820
			public static readonly LayerMask LayerMaskDefault = 1;

			// Token: 0x04000335 RID: 821
			public const DynamicOcclusionUpdateRate UpdateRateDefault = DynamicOcclusionUpdateRate.EveryXFrames;

			// Token: 0x04000336 RID: 822
			public const int WaitFramesCountDefault = 3;

			// Token: 0x04000337 RID: 823
			public const Dimensions RaycastingDimensionsDefault = Dimensions.Dim3D;

			// Token: 0x04000338 RID: 824
			public const bool RaycastingConsiderTriggersDefault = false;

			// Token: 0x04000339 RID: 825
			public const float RaycastingMinOccluderAreaDefault = 0f;

			// Token: 0x0400033A RID: 826
			public const float RaycastingMinSurfaceRatioDefault = 0.5f;

			// Token: 0x0400033B RID: 827
			public const float RaycastingMinSurfaceRatioMin = 50f;

			// Token: 0x0400033C RID: 828
			public const float RaycastingMinSurfaceRatioMax = 100f;

			// Token: 0x0400033D RID: 829
			public const float RaycastingMaxSurfaceDotDefault = 0.25f;

			// Token: 0x0400033E RID: 830
			public const float RaycastingMaxSurfaceAngleMin = 45f;

			// Token: 0x0400033F RID: 831
			public const float RaycastingMaxSurfaceAngleMax = 90f;

			// Token: 0x04000340 RID: 832
			public const PlaneAlignment RaycastingPlaneAlignmentDefault = PlaneAlignment.Surface;

			// Token: 0x04000341 RID: 833
			public const float RaycastingPlaneOffsetDefault = 0.1f;

			// Token: 0x04000342 RID: 834
			public const float RaycastingFadeDistanceToSurfaceDefault = 0.25f;

			// Token: 0x04000343 RID: 835
			public const int DepthBufferDepthMapResolutionDefault = 128;

			// Token: 0x04000344 RID: 836
			public const bool DepthBufferOcclusionCullingDefault = true;

			// Token: 0x04000345 RID: 837
			public const float DepthBufferFadeDistanceToSurfaceDefault = 0f;
		}

		// Token: 0x0200009C RID: 156
		public static class Effects
		{
			// Token: 0x04000346 RID: 838
			public const EffectAbstractBase.ComponentsToChange ComponentsToChangeDefault = (EffectAbstractBase.ComponentsToChange)2147483647;

			// Token: 0x04000347 RID: 839
			public const bool RestoreIntensityOnDisableDefault = true;

			// Token: 0x04000348 RID: 840
			public const float FrequencyDefault = 10f;

			// Token: 0x04000349 RID: 841
			public const bool PerformPausesDefault = false;

			// Token: 0x0400034A RID: 842
			public const bool RestoreIntensityOnPauseDefault = false;

			// Token: 0x0400034B RID: 843
			public static readonly MinMaxRangeFloat FlickeringDurationDefault = new MinMaxRangeFloat(1f, 4f);

			// Token: 0x0400034C RID: 844
			public static readonly MinMaxRangeFloat PauseDurationDefault = new MinMaxRangeFloat(0f, 1f);

			// Token: 0x0400034D RID: 845
			public static readonly MinMaxRangeFloat IntensityAmplitudeDefault = new MinMaxRangeFloat(-1f, 1f);

			// Token: 0x0400034E RID: 846
			public const float SmoothingDefault = 0.05f;
		}

		// Token: 0x0200009D RID: 157
		public static class Shadow
		{
			// Token: 0x0600047E RID: 1150 RVA: 0x00013086 File Offset: 0x00011286
			public static string GetErrorChangeRuntimeDepthMapResolution(VolumetricShadowHD comp)
			{
				return string.Format("Can't change {0} Shadow.depthMapResolution property at runtime after DepthCamera initialization", comp.name);
			}

			// Token: 0x0400034F RID: 847
			public const float StrengthDefault = 1f;

			// Token: 0x04000350 RID: 848
			public const float StrengthMin = 0f;

			// Token: 0x04000351 RID: 849
			public const float StrengthMax = 1f;

			// Token: 0x04000352 RID: 850
			public static readonly LayerMask LayerMaskDefault = 1;

			// Token: 0x04000353 RID: 851
			public const ShadowUpdateRate UpdateRateDefault = ShadowUpdateRate.EveryXFrames;

			// Token: 0x04000354 RID: 852
			public const int WaitFramesCountDefault = 3;

			// Token: 0x04000355 RID: 853
			public const int DepthMapResolutionDefault = 128;

			// Token: 0x04000356 RID: 854
			public const bool OcclusionCullingDefault = true;
		}

		// Token: 0x0200009E RID: 158
		public static class Cookie
		{
			// Token: 0x04000357 RID: 855
			public const float ContributionDefault = 1f;

			// Token: 0x04000358 RID: 856
			public const float ContributionMin = 0f;

			// Token: 0x04000359 RID: 857
			public const float ContributionMax = 1f;

			// Token: 0x0400035A RID: 858
			public const Texture CookieTextureDefault = null;

			// Token: 0x0400035B RID: 859
			public const CookieChannel ChannelDefault = CookieChannel.Alpha;

			// Token: 0x0400035C RID: 860
			public const bool NegativeDefault = false;

			// Token: 0x0400035D RID: 861
			public static readonly Vector2 TranslationDefault = Vector2.zero;

			// Token: 0x0400035E RID: 862
			public const float RotationDefault = 0f;

			// Token: 0x0400035F RID: 863
			public static readonly Vector2 ScaleDefault = Vector2.one;
		}

		// Token: 0x0200009F RID: 159
		public static class Config
		{
			// Token: 0x04000360 RID: 864
			public const bool GeometryOverrideLayerDefault = true;

			// Token: 0x04000361 RID: 865
			public const int GeometryLayerIDDefault = 1;

			// Token: 0x04000362 RID: 866
			public const string GeometryTagDefault = "Untagged";

			// Token: 0x04000363 RID: 867
			public const string FadeOutCameraTagDefault = "MainCamera";

			// Token: 0x04000364 RID: 868
			public const RenderQueue GeometryRenderQueueDefault = RenderQueue.Transparent;

			// Token: 0x04000365 RID: 869
			public const RenderPipeline GeometryRenderPipelineDefault = RenderPipeline.BuiltIn;

			// Token: 0x04000366 RID: 870
			public const RenderingMode GeometryRenderingModeDefault = RenderingMode.Default;

			// Token: 0x04000367 RID: 871
			public const int Noise3DSizeDefault = 64;

			// Token: 0x04000368 RID: 872
			public const float DitheringFactor = 0f;

			// Token: 0x04000369 RID: 873
			public const bool UseLightColorTemperatureDefault = true;

			// Token: 0x0400036A RID: 874
			public const bool FeatureEnabledDefault = true;

			// Token: 0x0400036B RID: 875
			public const FeatureEnabledColorGradient FeatureEnabledColorGradientDefault = FeatureEnabledColorGradient.HighOnly;

			// Token: 0x0400036C RID: 876
			public const int SharedMeshSidesDefault = 24;

			// Token: 0x0400036D RID: 877
			public const int SharedMeshSidesMin = 3;

			// Token: 0x0400036E RID: 878
			public const int SharedMeshSidesMax = 256;

			// Token: 0x0400036F RID: 879
			public const int SharedMeshSegmentsDefault = 5;

			// Token: 0x04000370 RID: 880
			public const int SharedMeshSegmentsMin = 0;

			// Token: 0x04000371 RID: 881
			public const int SharedMeshSegmentsMax = 64;

			// Token: 0x020000DA RID: 218
			public static class HD
			{
				// Token: 0x04000456 RID: 1110
				public const RenderQueue GeometryRenderQueueDefault = (RenderQueue)3100;

				// Token: 0x04000457 RID: 1111
				public const float CameraBlendingDistance = 0.5f;

				// Token: 0x04000458 RID: 1112
				public const int RaymarchingQualitiesStepsMin = 2;
			}
		}
	}
}
