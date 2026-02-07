using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace HorizonBasedAmbientOcclusion
{
	// Token: 0x02000003 RID: 3
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[AddComponentMenu("Image Effects/HBAO")]
	[RequireComponent(typeof(Camera))]
	public class HBAO : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020C4 File Offset: 0x000002C4
		public HBAO.Presets presets
		{
			get
			{
				return this.m_Presets;
			}
			set
			{
				this.m_Presets = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020CD File Offset: 0x000002CD
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020D5 File Offset: 0x000002D5
		public HBAO.GeneralSettings generalSettings
		{
			get
			{
				return this.m_GeneralSettings;
			}
			set
			{
				this.m_GeneralSettings = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020DE File Offset: 0x000002DE
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000020E6 File Offset: 0x000002E6
		public HBAO.AOSettings aoSettings
		{
			get
			{
				return this.m_AOSettings;
			}
			set
			{
				this.m_AOSettings = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020EF File Offset: 0x000002EF
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000020F7 File Offset: 0x000002F7
		public HBAO.TemporalFilterSettings temporalFilterSettings
		{
			get
			{
				return this.m_TemporalFilterSettings;
			}
			set
			{
				this.m_TemporalFilterSettings = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002100 File Offset: 0x00000300
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002108 File Offset: 0x00000308
		public HBAO.BlurSettings blurSettings
		{
			get
			{
				return this.m_BlurSettings;
			}
			set
			{
				this.m_BlurSettings = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002111 File Offset: 0x00000311
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002119 File Offset: 0x00000319
		public HBAO.ColorBleedingSettings colorBleedingSettings
		{
			get
			{
				return this.m_ColorBleedingSettings;
			}
			set
			{
				this.m_ColorBleedingSettings = value;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002122 File Offset: 0x00000322
		public HBAO.Preset GetCurrentPreset()
		{
			return this.m_Presets.preset;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002130 File Offset: 0x00000330
		public void ApplyPreset(HBAO.Preset preset)
		{
			if (preset == HBAO.Preset.Custom)
			{
				this.m_Presets.preset = preset;
				return;
			}
			HBAO.DebugMode debugMode = this.generalSettings.debugMode;
			this.m_GeneralSettings = HBAO.GeneralSettings.defaults;
			this.m_AOSettings = HBAO.AOSettings.defaults;
			this.m_ColorBleedingSettings = HBAO.ColorBleedingSettings.defaults;
			this.m_BlurSettings = HBAO.BlurSettings.defaults;
			this.SetDebugMode(debugMode);
			switch (preset)
			{
			case HBAO.Preset.FastestPerformance:
				this.SetQuality(HBAO.Quality.Lowest);
				this.SetAoRadius(0.5f);
				this.SetAoMaxRadiusPixels(64f);
				this.SetBlurType(HBAO.BlurType.ExtraWide);
				break;
			case HBAO.Preset.FastPerformance:
				this.SetQuality(HBAO.Quality.Low);
				this.SetAoRadius(0.5f);
				this.SetAoMaxRadiusPixels(64f);
				this.SetBlurType(HBAO.BlurType.Wide);
				break;
			case HBAO.Preset.HighQuality:
				this.SetQuality(HBAO.Quality.High);
				this.SetAoRadius(1f);
				break;
			case HBAO.Preset.HighestQuality:
				this.SetQuality(HBAO.Quality.Highest);
				this.SetAoRadius(1.2f);
				this.SetAoMaxRadiusPixels(256f);
				this.SetBlurType(HBAO.BlurType.Narrow);
				break;
			}
			this.m_Presets.preset = preset;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000223C File Offset: 0x0000043C
		public HBAO.PipelineStage GetPipelineStage()
		{
			return this.m_GeneralSettings.pipelineStage;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002249 File Offset: 0x00000449
		public void SetPipelineStage(HBAO.PipelineStage pipelineStage)
		{
			this.m_GeneralSettings.pipelineStage = pipelineStage;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002257 File Offset: 0x00000457
		public HBAO.Quality GetQuality()
		{
			return this.m_GeneralSettings.quality;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002264 File Offset: 0x00000464
		public void SetQuality(HBAO.Quality quality)
		{
			this.m_GeneralSettings.quality = quality;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002272 File Offset: 0x00000472
		public HBAO.Deinterleaving GetDeinterleaving()
		{
			return this.m_GeneralSettings.deinterleaving;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000227F File Offset: 0x0000047F
		public void SetDeinterleaving(HBAO.Deinterleaving deinterleaving)
		{
			this.m_GeneralSettings.deinterleaving = deinterleaving;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000228D File Offset: 0x0000048D
		public HBAO.Resolution GetResolution()
		{
			return this.m_GeneralSettings.resolution;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000229A File Offset: 0x0000049A
		public void SetResolution(HBAO.Resolution resolution)
		{
			this.m_GeneralSettings.resolution = resolution;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000022A8 File Offset: 0x000004A8
		public HBAO.NoiseType GetNoiseType()
		{
			return this.m_GeneralSettings.noiseType;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000022B5 File Offset: 0x000004B5
		public void SetNoiseType(HBAO.NoiseType noiseType)
		{
			this.m_GeneralSettings.noiseType = noiseType;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022C3 File Offset: 0x000004C3
		public HBAO.DebugMode GetDebugMode()
		{
			return this.m_GeneralSettings.debugMode;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000022D0 File Offset: 0x000004D0
		public void SetDebugMode(HBAO.DebugMode debugMode)
		{
			this.m_GeneralSettings.debugMode = debugMode;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022DE File Offset: 0x000004DE
		public float GetAoRadius()
		{
			return this.m_AOSettings.radius;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000022EB File Offset: 0x000004EB
		public void SetAoRadius(float radius)
		{
			this.m_AOSettings.radius = Mathf.Clamp(radius, 0.25f, 5f);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002308 File Offset: 0x00000508
		public float GetAoMaxRadiusPixels()
		{
			return this.m_AOSettings.maxRadiusPixels;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002315 File Offset: 0x00000515
		public void SetAoMaxRadiusPixels(float maxRadiusPixels)
		{
			this.m_AOSettings.maxRadiusPixels = Mathf.Clamp(maxRadiusPixels, 16f, 256f);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002332 File Offset: 0x00000532
		public float GetAoBias()
		{
			return this.m_AOSettings.bias;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000233F File Offset: 0x0000053F
		public void SetAoBias(float bias)
		{
			this.m_AOSettings.bias = Mathf.Clamp(bias, 0f, 0.5f);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000235C File Offset: 0x0000055C
		public float GetAoOffscreenSamplesContribution()
		{
			return this.m_AOSettings.offscreenSamplesContribution;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002369 File Offset: 0x00000569
		public void SetAoOffscreenSamplesContribution(float contribution)
		{
			this.m_AOSettings.offscreenSamplesContribution = Mathf.Clamp01(contribution);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000237C File Offset: 0x0000057C
		public float GetAoMaxDistance()
		{
			return this.m_AOSettings.maxDistance;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002389 File Offset: 0x00000589
		public void SetAoMaxDistance(float maxDistance)
		{
			this.m_AOSettings.maxDistance = maxDistance;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002397 File Offset: 0x00000597
		public float GetAoDistanceFalloff()
		{
			return this.m_AOSettings.distanceFalloff;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000023A4 File Offset: 0x000005A4
		public void SetAoDistanceFalloff(float distanceFalloff)
		{
			this.m_AOSettings.distanceFalloff = distanceFalloff;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000023B2 File Offset: 0x000005B2
		public HBAO.PerPixelNormals GetAoPerPixelNormals()
		{
			return this.m_AOSettings.perPixelNormals;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000023BF File Offset: 0x000005BF
		public void SetAoPerPixelNormals(HBAO.PerPixelNormals perPixelNormals)
		{
			this.m_AOSettings.perPixelNormals = perPixelNormals;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000023CD File Offset: 0x000005CD
		public Color GetAoColor()
		{
			return this.m_AOSettings.baseColor;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000023DA File Offset: 0x000005DA
		public void SetAoColor(Color color)
		{
			this.m_AOSettings.baseColor = color;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000023E8 File Offset: 0x000005E8
		public float GetAoIntensity()
		{
			return this.m_AOSettings.intensity;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000023F5 File Offset: 0x000005F5
		public void SetAoIntensity(float intensity)
		{
			this.m_AOSettings.intensity = Mathf.Clamp(intensity, 0f, 4f);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002412 File Offset: 0x00000612
		public bool UseMultiBounce()
		{
			return this.m_AOSettings.useMultiBounce;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000241F File Offset: 0x0000061F
		public void EnableMultiBounce(bool enabled = true)
		{
			this.m_AOSettings.useMultiBounce = enabled;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000242D File Offset: 0x0000062D
		public float GetAoMultiBounceInfluence()
		{
			return this.m_AOSettings.multiBounceInfluence;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000243A File Offset: 0x0000063A
		public void SetAoMultiBounceInfluence(float multiBounceInfluence)
		{
			this.m_AOSettings.multiBounceInfluence = Mathf.Clamp01(multiBounceInfluence);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000244D File Offset: 0x0000064D
		public bool IsTemporalFilterEnabled()
		{
			return this.m_TemporalFilterSettings.enabled;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000245A File Offset: 0x0000065A
		public void EnableTemporalFilter(bool enabled = true)
		{
			this.m_TemporalFilterSettings.enabled = enabled;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002468 File Offset: 0x00000668
		public HBAO.VarianceClipping GetTemporalFilterVarianceClipping()
		{
			return this.m_TemporalFilterSettings.varianceClipping;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002475 File Offset: 0x00000675
		public void SetTemporalFilterVarianceClipping(HBAO.VarianceClipping varianceClipping)
		{
			this.m_TemporalFilterSettings.varianceClipping = varianceClipping;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002483 File Offset: 0x00000683
		public HBAO.BlurType GetBlurType()
		{
			return this.m_BlurSettings.type;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002490 File Offset: 0x00000690
		public void SetBlurType(HBAO.BlurType blurType)
		{
			this.m_BlurSettings.type = blurType;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000249E File Offset: 0x0000069E
		public float GetBlurSharpness()
		{
			return this.m_BlurSettings.sharpness;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000024AB File Offset: 0x000006AB
		public void SetBlurSharpness(float sharpness)
		{
			this.m_BlurSettings.sharpness = Mathf.Clamp(sharpness, 0f, 16f);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000024C8 File Offset: 0x000006C8
		public bool IsColorBleedingEnabled()
		{
			return this.m_ColorBleedingSettings.enabled;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000024D5 File Offset: 0x000006D5
		public void EnableColorBleeding(bool enabled = true)
		{
			this.m_ColorBleedingSettings.enabled = enabled;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000024E3 File Offset: 0x000006E3
		public float GetColorBleedingSaturation()
		{
			return this.m_ColorBleedingSettings.saturation;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000024F0 File Offset: 0x000006F0
		public void SetColorBleedingSaturation(float saturation)
		{
			this.m_ColorBleedingSettings.saturation = Mathf.Clamp(saturation, 0f, 4f);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000250D File Offset: 0x0000070D
		public float GetColorBleedingAlbedoMultiplier()
		{
			return this.m_ColorBleedingSettings.albedoMultiplier;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000251A File Offset: 0x0000071A
		public void SetColorBleedingAlbedoMultiplier(float albedoMultiplier)
		{
			this.m_ColorBleedingSettings.albedoMultiplier = Mathf.Clamp(albedoMultiplier, 0f, 32f);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002537 File Offset: 0x00000737
		public float GetColorBleedingBrightnessMask()
		{
			return this.m_ColorBleedingSettings.brightnessMask;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002544 File Offset: 0x00000744
		public void SetColorBleedingBrightnessMask(float brightnessMask)
		{
			this.m_ColorBleedingSettings.brightnessMask = Mathf.Clamp01(brightnessMask);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002557 File Offset: 0x00000757
		public Vector2 GetColorBleedingBrightnessMaskRange()
		{
			return this.m_ColorBleedingSettings.brightnessMaskRange;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002564 File Offset: 0x00000764
		public void SetColorBleedingBrightnessMaskRange(Vector2 brightnessMaskRange)
		{
			brightnessMaskRange.x = Mathf.Clamp(brightnessMaskRange.x, 0f, 2f);
			brightnessMaskRange.y = Mathf.Clamp(brightnessMaskRange.y, 0f, 2f);
			brightnessMaskRange.x = Mathf.Min(brightnessMaskRange.x, brightnessMaskRange.y);
			this.m_ColorBleedingSettings.brightnessMaskRange = brightnessMaskRange;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000025CD File Offset: 0x000007CD
		// (set) Token: 0x06000046 RID: 70 RVA: 0x000025D5 File Offset: 0x000007D5
		private Material material { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000025DE File Offset: 0x000007DE
		// (set) Token: 0x06000048 RID: 72 RVA: 0x000025E6 File Offset: 0x000007E6
		private Camera hbaoCamera { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000025EF File Offset: 0x000007EF
		// (set) Token: 0x0600004A RID: 74 RVA: 0x000025F7 File Offset: 0x000007F7
		private CommandBuffer cmdBuffer { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002600 File Offset: 0x00000800
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002608 File Offset: 0x00000808
		private int width { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002611 File Offset: 0x00000811
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002619 File Offset: 0x00000819
		private int height { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002622 File Offset: 0x00000822
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000262A File Offset: 0x0000082A
		private bool stereoActive { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002633 File Offset: 0x00000833
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000263B File Offset: 0x0000083B
		private int xrActiveEye { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002644 File Offset: 0x00000844
		// (set) Token: 0x06000054 RID: 84 RVA: 0x0000264C File Offset: 0x0000084C
		private HBAO.StereoRenderingMode stereoRenderingMode { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002655 File Offset: 0x00000855
		// (set) Token: 0x06000056 RID: 86 RVA: 0x0000265D File Offset: 0x0000085D
		private int screenWidth { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002666 File Offset: 0x00000866
		// (set) Token: 0x06000058 RID: 88 RVA: 0x0000266E File Offset: 0x0000086E
		private int screenHeight { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002677 File Offset: 0x00000877
		// (set) Token: 0x0600005A RID: 90 RVA: 0x0000267F File Offset: 0x0000087F
		private int aoWidth { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002688 File Offset: 0x00000888
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002690 File Offset: 0x00000890
		private int aoHeight { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002699 File Offset: 0x00000899
		// (set) Token: 0x0600005E RID: 94 RVA: 0x000026A1 File Offset: 0x000008A1
		private int reinterleavedAoWidth { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000026AA File Offset: 0x000008AA
		// (set) Token: 0x06000060 RID: 96 RVA: 0x000026B2 File Offset: 0x000008B2
		private int reinterleavedAoHeight { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000026BB File Offset: 0x000008BB
		// (set) Token: 0x06000062 RID: 98 RVA: 0x000026C3 File Offset: 0x000008C3
		private int deinterleavedAoWidth { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000026CC File Offset: 0x000008CC
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000026D4 File Offset: 0x000008D4
		private int deinterleavedAoHeight { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000026DD File Offset: 0x000008DD
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000026E5 File Offset: 0x000008E5
		private int frameCount { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000026EE File Offset: 0x000008EE
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000026F6 File Offset: 0x000008F6
		private bool motionVectorsSupported { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000026FF File Offset: 0x000008FF
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002707 File Offset: 0x00000907
		private RenderTexture aoHistoryBuffer { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002710 File Offset: 0x00000910
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002718 File Offset: 0x00000918
		private RenderTexture colorBleedingHistoryBuffer { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002721 File Offset: 0x00000921
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002729 File Offset: 0x00000929
		private Texture2D noiseTex { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002734 File Offset: 0x00000934
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000027F8 File Offset: 0x000009F8
		private CameraEvent cameraEvent
		{
			get
			{
				if (this.generalSettings.debugMode != HBAO.DebugMode.Disabled)
				{
					return CameraEvent.BeforeImageEffectsOpaque;
				}
				switch (this.generalSettings.pipelineStage)
				{
				case HBAO.PipelineStage.AfterLighting:
					return CameraEvent.AfterLighting;
				case HBAO.PipelineStage.BeforeReflections:
					return CameraEvent.BeforeReflections;
				}
				return CameraEvent.BeforeImageEffectsOpaque;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000071 RID: 113 RVA: 0x0000283C File Offset: 0x00000A3C
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00002A8E File Offset: 0x00000C8E
		private bool isCommandBufferDirty
		{
			get
			{
				if (!this.m_IsCommandBufferDirty)
				{
					HBAO.PipelineStage? previousPipelineStage = this.m_PreviousPipelineStage;
					HBAO.PipelineStage pipelineStage = this.generalSettings.pipelineStage;
					if (previousPipelineStage.GetValueOrDefault() == pipelineStage & previousPipelineStage != null)
					{
						HBAO.Resolution? previousResolution = this.m_PreviousResolution;
						HBAO.Resolution resolution = this.generalSettings.resolution;
						if (previousResolution.GetValueOrDefault() == resolution & previousResolution != null)
						{
							HBAO.DebugMode? previousDebugMode = this.m_PreviousDebugMode;
							HBAO.DebugMode debugMode = this.generalSettings.debugMode;
							if ((previousDebugMode.GetValueOrDefault() == debugMode & previousDebugMode != null) && this.m_PreviousAllowHDR == this.hbaoCamera.allowHDR && this.m_PreviousWidth == this.width && this.m_PreviousHeight == this.height)
							{
								HBAO.Deinterleaving? previousDeinterleaving = this.m_PreviousDeinterleaving;
								HBAO.Deinterleaving deinterleaving = this.generalSettings.deinterleaving;
								if (previousDeinterleaving.GetValueOrDefault() == deinterleaving & previousDeinterleaving != null)
								{
									HBAO.BlurType? previousBlurAmount = this.m_PreviousBlurAmount;
									HBAO.BlurType type = this.blurSettings.type;
									if ((previousBlurAmount.GetValueOrDefault() == type & previousBlurAmount != null) && this.m_PreviousUseMultibounce == this.aoSettings.useMultiBounce && this.m_PreviousColorBleedingEnabled == this.colorBleedingSettings.enabled && this.m_PreviousTemporalFilterEnabled == this.temporalFilterSettings.enabled && this.m_PreviousRenderingPath == this.hbaoCamera.actualRenderingPath)
									{
										return false;
									}
								}
							}
						}
					}
				}
				this.m_PreviousPipelineStage = new HBAO.PipelineStage?(this.generalSettings.pipelineStage);
				this.m_PreviousResolution = new HBAO.Resolution?(this.generalSettings.resolution);
				this.m_PreviousDebugMode = new HBAO.DebugMode?(this.generalSettings.debugMode);
				this.m_PreviousAllowHDR = this.hbaoCamera.allowHDR;
				this.m_PreviousWidth = this.width;
				this.m_PreviousHeight = this.height;
				this.m_PreviousDeinterleaving = new HBAO.Deinterleaving?(this.generalSettings.deinterleaving);
				this.m_PreviousBlurAmount = new HBAO.BlurType?(this.blurSettings.type);
				this.m_PreviousUseMultibounce = this.aoSettings.useMultiBounce;
				this.m_PreviousColorBleedingEnabled = this.colorBleedingSettings.enabled;
				this.m_PreviousTemporalFilterEnabled = this.temporalFilterSettings.enabled;
				this.m_PreviousRenderingPath = this.hbaoCamera.actualRenderingPath;
				return true;
			}
			set
			{
				this.m_IsCommandBufferDirty = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002A98 File Offset: 0x00000C98
		private bool isHistoryBufferDirty
		{
			get
			{
				if (!(this.aoHistoryBuffer == null) && (!this.colorBleedingSettings.enabled || !(this.colorBleedingHistoryBuffer == null)) && this.m_PreviousTemporalFilterEnabled == this.temporalFilterSettings.enabled)
				{
					HBAO.Resolution? previousResolution = this.m_PreviousResolution;
					HBAO.Resolution resolution = this.generalSettings.resolution;
					if ((previousResolution.GetValueOrDefault() == resolution & previousResolution != null) && this.m_PreviousColorBleedingEnabled == this.colorBleedingSettings.enabled && this.m_PrevStereoRenderingMode == this.stereoRenderingMode)
					{
						return false;
					}
				}
				this.m_PreviousTemporalFilterEnabled = this.temporalFilterSettings.enabled;
				this.m_PreviousResolution = new HBAO.Resolution?(this.generalSettings.resolution);
				this.m_PreviousColorBleedingEnabled = this.colorBleedingSettings.enabled;
				this.m_PrevStereoRenderingMode = this.stereoRenderingMode;
				return true;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002B70 File Offset: 0x00000D70
		private static RenderTextureFormat defaultHDRRenderTextureFormat
		{
			get
			{
				return RenderTextureFormat.DefaultHDR;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002B74 File Offset: 0x00000D74
		private RenderTextureFormat sourceFormat
		{
			get
			{
				if (!this.hbaoCamera.allowHDR)
				{
					return RenderTextureFormat.Default;
				}
				return HBAO.defaultHDRRenderTextureFormat;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002B8A File Offset: 0x00000D8A
		private static RenderTextureFormat colorFormat
		{
			get
			{
				if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
				{
					return RenderTextureFormat.Default;
				}
				return RenderTextureFormat.ARGBHalf;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002B97 File Offset: 0x00000D97
		private static RenderTextureFormat depthFormat
		{
			get
			{
				if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat))
				{
					return RenderTextureFormat.RHalf;
				}
				return RenderTextureFormat.RFloat;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002BA7 File Offset: 0x00000DA7
		private static RenderTextureFormat normalsFormat
		{
			get
			{
				if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010))
				{
					return RenderTextureFormat.Default;
				}
				return RenderTextureFormat.ARGB2101010;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002BB4 File Offset: 0x00000DB4
		private static bool isLinearColorSpace
		{
			get
			{
				return QualitySettings.activeColorSpace == ColorSpace.Linear;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002BBE File Offset: 0x00000DBE
		private bool renderingInSceneView
		{
			get
			{
				return this.hbaoCamera.cameraType == CameraType.SceneView;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002BD0 File Offset: 0x00000DD0
		private void OnEnable()
		{
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				Debug.LogWarning("HBAO shader is not supported on this platform.");
				base.enabled = false;
				return;
			}
			if (this.hbaoShader == null)
			{
				this.hbaoShader = Shader.Find("Hidden/HBAO");
			}
			if (this.hbaoShader == null)
			{
				Debug.LogError("HBAO shader was not found...");
				return;
			}
			if (!this.hbaoShader.isSupported)
			{
				Debug.LogWarning("HBAO shader is not supported on this platform.");
				base.enabled = false;
				return;
			}
			this.Initialize();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002C54 File Offset: 0x00000E54
		private void OnDisable()
		{
			this.ClearCommandBuffer(this.cmdBuffer);
			this.ReleaseHistoryBuffers();
			if (this.material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
			}
			if (this.noiseTex != null)
			{
				UnityEngine.Object.DestroyImmediate(this.noiseTex);
			}
			if (this.fullscreenTriangle != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fullscreenTriangle);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002CC0 File Offset: 0x00000EC0
		private void OnPreRender()
		{
			if (this.hbaoShader == null || this.hbaoCamera == null)
			{
				return;
			}
			this.FetchRenderParameters();
			this.CheckParameters();
			this.UpdateMaterialProperties();
			this.UpdateShaderKeywords();
			if (this.isCommandBufferDirty)
			{
				this.ClearCommandBuffer(this.cmdBuffer);
				this.BuildCommandBuffer(this.cmdBuffer, this.cameraEvent);
				this.hbaoCamera.AddCommandBuffer(this.cameraEvent, this.cmdBuffer);
				this.isCommandBufferDirty = false;
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002D48 File Offset: 0x00000F48
		private void OnPostRender()
		{
			int frameCount = this.frameCount;
			this.frameCount = frameCount + 1;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002D65 File Offset: 0x00000F65
		private void OnValidate()
		{
			if (this.hbaoShader == null || this.hbaoCamera == null)
			{
				return;
			}
			this.CheckParameters();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002D8C File Offset: 0x00000F8C
		private void Initialize()
		{
			this.m_sourceDescriptor = new RenderTextureDescriptor(0, 0);
			this.hbaoCamera = base.GetComponent<Camera>();
			this.hbaoCamera.forceIntoRenderTexture = true;
			this.material = new Material(this.hbaoShader);
			this.material.hideFlags = HideFlags.HideAndDontSave;
			this.motionVectorsSupported = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf);
			this.cmdBuffer = new CommandBuffer
			{
				name = "HBAO"
			};
			this.isCommandBufferDirty = true;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002E08 File Offset: 0x00001008
		private void FetchRenderParameters()
		{
			if (this.hbaoCamera.stereoEnabled)
			{
				RenderTextureDescriptor eyeTextureDesc = XRSettings.eyeTextureDesc;
				this.stereoRenderingMode = HBAO.StereoRenderingMode.MultiPass;
				if (eyeTextureDesc.dimension == TextureDimension.Tex2DArray)
				{
					this.stereoRenderingMode = HBAO.StereoRenderingMode.SinglePassInstanced;
				}
				this.width = eyeTextureDesc.width;
				this.height = eyeTextureDesc.height;
				this.m_sourceDescriptor = eyeTextureDesc;
				this.xrActiveEye = (int)this.hbaoCamera.stereoActiveEye;
				this.screenWidth = XRSettings.eyeTextureWidth;
				this.screenHeight = XRSettings.eyeTextureHeight;
				this.stereoActive = true;
			}
			else
			{
				this.width = this.hbaoCamera.pixelWidth;
				this.height = this.hbaoCamera.pixelHeight;
				this.m_sourceDescriptor.width = this.width;
				this.m_sourceDescriptor.height = this.height;
				this.xrActiveEye = 0;
				this.screenWidth = this.width;
				this.screenHeight = this.height;
				this.stereoActive = false;
			}
			int num = (this.generalSettings.resolution == HBAO.Resolution.Full) ? 1 : ((this.generalSettings.deinterleaving == HBAO.Deinterleaving.Disabled) ? 2 : 1);
			if (num > 1)
			{
				this.aoWidth = (this.width + this.width % 2) / num;
				this.aoHeight = (this.height + this.height % 2) / num;
			}
			else
			{
				this.aoWidth = this.width;
				this.aoHeight = this.height;
			}
			this.reinterleavedAoWidth = this.width + ((this.width % 4 == 0) ? 0 : (4 - this.width % 4));
			this.reinterleavedAoHeight = this.height + ((this.height % 4 == 0) ? 0 : (4 - this.height % 4));
			this.deinterleavedAoWidth = this.reinterleavedAoWidth / 4;
			this.deinterleavedAoHeight = this.reinterleavedAoHeight / 4;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002FD0 File Offset: 0x000011D0
		private void AllocateHistoryBuffers()
		{
			this.ReleaseHistoryBuffers();
			int depthBufferBits = 0;
			int num = this.aoWidth;
			int num2 = this.aoHeight;
			this.aoHistoryBuffer = this.GetScreenSpaceRT(depthBufferBits, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, num, num2);
			if (this.colorBleedingSettings.enabled)
			{
				int depthBufferBits2 = 0;
				num2 = this.aoWidth;
				num = this.aoHeight;
				this.colorBleedingHistoryBuffer = this.GetScreenSpaceRT(depthBufferBits2, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, num2, num);
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.aoHistoryBuffer;
			GL.Clear(false, true, Color.white);
			if (this.colorBleedingSettings.enabled)
			{
				RenderTexture.active = this.colorBleedingHistoryBuffer;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
			}
			RenderTexture.active = active;
			this.frameCount = 0;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003098 File Offset: 0x00001298
		private void ReleaseHistoryBuffers()
		{
			if (this.aoHistoryBuffer != null)
			{
				this.aoHistoryBuffer.Release();
			}
			if (this.colorBleedingHistoryBuffer != null)
			{
				this.colorBleedingHistoryBuffer.Release();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000030CC File Offset: 0x000012CC
		private void ClearCommandBuffer(CommandBuffer cmd)
		{
			if (cmd != null)
			{
				if (this.hbaoCamera != null)
				{
					this.hbaoCamera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, cmd);
					this.hbaoCamera.RemoveCommandBuffer(CameraEvent.AfterLighting, cmd);
					this.hbaoCamera.RemoveCommandBuffer(CameraEvent.BeforeReflections, cmd);
				}
				cmd.Clear();
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000311C File Offset: 0x0000131C
		private void BuildCommandBuffer(CommandBuffer cmd, CameraEvent cameraEvent)
		{
			if (this.generalSettings.deinterleaving == HBAO.Deinterleaving.Disabled)
			{
				int hbaoTex = HBAO.ShaderProperties.hbaoTex;
				int depthBufferBits = 0;
				int num = this.aoWidth;
				int num2 = this.aoHeight;
				this.GetScreenSpaceTemporaryRT(cmd, hbaoTex, depthBufferBits, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, num, num2);
				this.AO(cmd);
			}
			else
			{
				int hbaoTex2 = HBAO.ShaderProperties.hbaoTex;
				int depthBufferBits2 = 0;
				int num2 = this.reinterleavedAoWidth;
				int num = this.reinterleavedAoHeight;
				this.GetScreenSpaceTemporaryRT(cmd, hbaoTex2, depthBufferBits2, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, num2, num);
				this.DeinterleavedAO(cmd);
			}
			this.Blur(cmd);
			this.TemporalFilter(cmd);
			this.Composite(cmd, cameraEvent);
			this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.hbaoTex);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000031B0 File Offset: 0x000013B0
		private void AO(CommandBuffer cmd)
		{
			this.BlitFullscreenTriangleWithClear(cmd, BuiltinRenderTextureType.CameraTarget, HBAO.ShaderProperties.hbaoTex, this.material, new Color(0f, 0f, 0f, 1f), 0);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000031F4 File Offset: 0x000013F4
		private void DeinterleavedAO(CommandBuffer cmd)
		{
			int num3;
			int num4;
			for (int i = 0; i < 4; i++)
			{
				RenderTargetIdentifier[] destinations = new RenderTargetIdentifier[]
				{
					HBAO.ShaderProperties.depthSliceTex[i << 2],
					HBAO.ShaderProperties.depthSliceTex[(i << 2) + 1],
					HBAO.ShaderProperties.depthSliceTex[(i << 2) + 2],
					HBAO.ShaderProperties.depthSliceTex[(i << 2) + 3]
				};
				RenderTargetIdentifier[] destinations2 = new RenderTargetIdentifier[]
				{
					HBAO.ShaderProperties.normalsSliceTex[i << 2],
					HBAO.ShaderProperties.normalsSliceTex[(i << 2) + 1],
					HBAO.ShaderProperties.normalsSliceTex[(i << 2) + 2],
					HBAO.ShaderProperties.normalsSliceTex[(i << 2) + 3]
				};
				int num = (i & 1) << 1;
				int num2 = i >> 1 << 1;
				cmd.SetGlobalVector(HBAO.ShaderProperties.deinterleaveOffset[0], new Vector2((float)num, (float)num2));
				cmd.SetGlobalVector(HBAO.ShaderProperties.deinterleaveOffset[1], new Vector2((float)(num + 1), (float)num2));
				cmd.SetGlobalVector(HBAO.ShaderProperties.deinterleaveOffset[2], new Vector2((float)num, (float)(num2 + 1)));
				cmd.SetGlobalVector(HBAO.ShaderProperties.deinterleaveOffset[3], new Vector2((float)(num + 1), (float)(num2 + 1)));
				for (int j = 0; j < 4; j++)
				{
					int nameID = HBAO.ShaderProperties.depthSliceTex[j + 4 * i];
					int depthBufferBits = 0;
					num3 = this.deinterleavedAoWidth;
					num4 = this.deinterleavedAoHeight;
					this.GetScreenSpaceTemporaryRT(cmd, nameID, depthBufferBits, HBAO.depthFormat, RenderTextureReadWrite.Linear, FilterMode.Point, num3, num4);
					int nameID2 = HBAO.ShaderProperties.normalsSliceTex[j + 4 * i];
					int depthBufferBits2 = 0;
					num4 = this.deinterleavedAoWidth;
					num3 = this.deinterleavedAoHeight;
					this.GetScreenSpaceTemporaryRT(cmd, nameID2, depthBufferBits2, HBAO.normalsFormat, RenderTextureReadWrite.Linear, FilterMode.Point, num4, num3);
				}
				this.BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, destinations, this.material, 2);
				this.BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, destinations2, this.material, 3);
			}
			for (int k = 0; k < 16; k++)
			{
				cmd.SetGlobalTexture(HBAO.ShaderProperties.depthTex, HBAO.ShaderProperties.depthSliceTex[k]);
				cmd.SetGlobalTexture(HBAO.ShaderProperties.normalsTex, HBAO.ShaderProperties.normalsSliceTex[k]);
				cmd.SetGlobalVector(HBAO.ShaderProperties.jitter, HBAO.s_jitter[k]);
				int nameID3 = HBAO.ShaderProperties.aoSliceTex[k];
				int depthBufferBits3 = 0;
				num3 = this.deinterleavedAoWidth;
				num4 = this.deinterleavedAoHeight;
				this.GetScreenSpaceTemporaryRT(cmd, nameID3, depthBufferBits3, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Point, num3, num4);
				this.BlitFullscreenTriangleWithClear(cmd, BuiltinRenderTextureType.CameraTarget, HBAO.ShaderProperties.aoSliceTex[k], this.material, new Color(0f, 0f, 0f, 1f), 1);
				this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.depthSliceTex[k]);
				this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.normalsSliceTex[k]);
			}
			int tempTex = HBAO.ShaderProperties.tempTex;
			int depthBufferBits4 = 0;
			num4 = this.reinterleavedAoWidth;
			num3 = this.reinterleavedAoHeight;
			this.GetScreenSpaceTemporaryRT(cmd, tempTex, depthBufferBits4, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, num4, num3);
			for (int l = 0; l < 16; l++)
			{
				cmd.SetGlobalVector(HBAO.ShaderProperties.atlasOffset, new Vector2((float)(((l & 1) + ((l & 7) >> 2 << 1)) * this.deinterleavedAoWidth), (float)((((l & 3) >> 1) + (l >> 3 << 1)) * this.deinterleavedAoHeight)));
				this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.aoSliceTex[l], HBAO.ShaderProperties.tempTex, this.material, 4);
				this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.aoSliceTex[l]);
			}
			HBAO.ApplyFlip(cmd, true);
			this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.tempTex, HBAO.ShaderProperties.hbaoTex, this.material, 5);
			this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000035BC File Offset: 0x000017BC
		private void Blur(CommandBuffer cmd)
		{
			if (this.blurSettings.type != HBAO.BlurType.None)
			{
				float num = (float)this.aoWidth;
				float num2 = (float)this.aoHeight;
				if (this.hbaoCamera.allowDynamicResolution)
				{
					num *= ScalableBufferManager.widthScaleFactor;
					num2 *= ScalableBufferManager.heightScaleFactor;
				}
				int tempTex = HBAO.ShaderProperties.tempTex;
				int depthBufferBits = 0;
				int aoWidth = this.aoWidth;
				int aoHeight = this.aoHeight;
				this.GetScreenSpaceTemporaryRT(cmd, tempTex, depthBufferBits, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, aoWidth, aoHeight);
				cmd.SetGlobalVector(HBAO.ShaderProperties.blurDeltaUV, new Vector2(1f / num, 0f));
				this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.hbaoTex, HBAO.ShaderProperties.tempTex, this.material, 6);
				cmd.SetGlobalVector(HBAO.ShaderProperties.blurDeltaUV, new Vector2(0f, 1f / num2));
				this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.tempTex, HBAO.ShaderProperties.hbaoTex, this.material, 6);
				this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000036BC File Offset: 0x000018BC
		private void TemporalFilter(CommandBuffer cmd)
		{
			if (this.isHistoryBufferDirty && this.temporalFilterSettings.enabled)
			{
				this.AllocateHistoryBuffers();
			}
			if (this.temporalFilterSettings.enabled && !this.renderingInSceneView)
			{
				int num;
				int num2;
				if (this.colorBleedingSettings.enabled)
				{
					RenderTargetIdentifier[] destinations = new RenderTargetIdentifier[]
					{
						this.aoHistoryBuffer,
						this.colorBleedingHistoryBuffer
					};
					int tempTex = HBAO.ShaderProperties.tempTex;
					int depthBufferBits = 0;
					num = this.aoWidth;
					num2 = this.aoHeight;
					this.GetScreenSpaceTemporaryRT(cmd, tempTex, depthBufferBits, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, num, num2);
					int tempTex2 = HBAO.ShaderProperties.tempTex2;
					int depthBufferBits2 = 0;
					num2 = this.aoWidth;
					num = this.aoHeight;
					this.GetScreenSpaceTemporaryRT(cmd, tempTex2, depthBufferBits2, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, num2, num);
					this.BlitFullscreenTriangle(cmd, this.aoHistoryBuffer, HBAO.ShaderProperties.tempTex2, this.material, 8);
					this.BlitFullscreenTriangle(cmd, this.colorBleedingHistoryBuffer, HBAO.ShaderProperties.tempTex, this.material, 8);
					this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.tempTex2, destinations, this.material, 7);
					this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex);
					this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex2);
					cmd.SetGlobalTexture(HBAO.ShaderProperties.hbaoTex, this.colorBleedingHistoryBuffer);
					return;
				}
				int tempTex3 = HBAO.ShaderProperties.tempTex;
				int depthBufferBits3 = 0;
				num = this.aoWidth;
				num2 = this.aoHeight;
				this.GetScreenSpaceTemporaryRT(cmd, tempTex3, depthBufferBits3, HBAO.colorFormat, RenderTextureReadWrite.Linear, FilterMode.Bilinear, num, num2);
				this.BlitFullscreenTriangle(cmd, this.aoHistoryBuffer, HBAO.ShaderProperties.tempTex, this.material, 8);
				this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.tempTex, this.aoHistoryBuffer, this.material, 7);
				this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex);
				cmd.SetGlobalTexture(HBAO.ShaderProperties.hbaoTex, this.aoHistoryBuffer);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000389C File Offset: 0x00001A9C
		private void Composite(CommandBuffer cmd, CameraEvent cameraEvent)
		{
			if (this.generalSettings.debugMode != HBAO.DebugMode.Disabled)
			{
				this.CompositeDebug(cmd, (this.generalSettings.debugMode == HBAO.DebugMode.ViewNormals) ? 14 : 9);
				return;
			}
			if (cameraEvent == CameraEvent.BeforeReflections)
			{
				this.CompositeBeforeReflections(cmd);
				return;
			}
			if (cameraEvent == CameraEvent.AfterLighting)
			{
				this.CompositeAfterLighting(cmd);
				return;
			}
			this.CompositeBeforeImageEffectsOpaque(cmd);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000038F4 File Offset: 0x00001AF4
		private void CompositeBeforeReflections(CommandBuffer cmd)
		{
			bool allowHDR = this.hbaoCamera.allowHDR;
			RenderTargetIdentifier[] array = new RenderTargetIdentifier[]
			{
				BuiltinRenderTextureType.GBuffer0,
				allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3
			};
			this.GetScreenSpaceTemporaryRT(cmd, HBAO.ShaderProperties.tempTex, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
			this.GetScreenSpaceTemporaryRT(cmd, HBAO.ShaderProperties.tempTex2, 0, allowHDR ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB2101010, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
			this.BlitFullscreenTriangle(cmd, array[0], HBAO.ShaderProperties.tempTex, this.material, 8);
			this.BlitFullscreenTriangle(cmd, array[1], HBAO.ShaderProperties.tempTex2, this.material, 8);
			this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.tempTex2, array, this.material, 11);
			this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex);
			this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex2);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000039D0 File Offset: 0x00001BD0
		private void CompositeAfterLighting(CommandBuffer cmd)
		{
			bool allowHDR = this.hbaoCamera.allowHDR;
			BuiltinRenderTextureType type = allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3;
			this.GetScreenSpaceTemporaryRT(cmd, HBAO.ShaderProperties.tempTex, 0, allowHDR ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB2101010, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
			this.BlitFullscreenTriangle(cmd, type, HBAO.ShaderProperties.tempTex, this.material, 8);
			this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.tempTex, type, this.material, 10);
			this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003A54 File Offset: 0x00001C54
		private void CompositeBeforeImageEffectsOpaque(CommandBuffer cmd)
		{
			if (this.aoSettings.useMultiBounce)
			{
				this.GetScreenSpaceTemporaryRT(cmd, HBAO.ShaderProperties.tempTex, 0, this.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
				if (this.stereoActive && this.hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading)
				{
					cmd.Blit(BuiltinRenderTextureType.CameraTarget, HBAO.ShaderProperties.tempTex);
				}
				else
				{
					this.BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, HBAO.ShaderProperties.tempTex, this.material, 8);
				}
			}
			HBAO.ApplyFlip(cmd, SystemInfo.graphicsUVStartsAtTop);
			this.BlitFullscreenTriangle(cmd, this.aoSettings.useMultiBounce ? HBAO.ShaderProperties.tempTex : BuiltinRenderTextureType.None, BuiltinRenderTextureType.CameraTarget, this.material, 12);
			if (this.colorBleedingSettings.enabled)
			{
				this.BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.None, BuiltinRenderTextureType.CameraTarget, this.material, 13);
			}
			if (this.aoSettings.useMultiBounce)
			{
				this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003B54 File Offset: 0x00001D54
		private void CompositeDebug(CommandBuffer cmd, int finalPassId = 9)
		{
			this.GetScreenSpaceTemporaryRT(cmd, HBAO.ShaderProperties.tempTex, 0, this.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
			if (this.stereoActive && this.hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading)
			{
				cmd.Blit(BuiltinRenderTextureType.CameraTarget, HBAO.ShaderProperties.tempTex);
			}
			else
			{
				this.BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, HBAO.ShaderProperties.tempTex, this.material, 8);
			}
			HBAO.ApplyFlip(cmd, SystemInfo.graphicsUVStartsAtTop);
			this.BlitFullscreenTriangle(cmd, HBAO.ShaderProperties.tempTex, BuiltinRenderTextureType.CameraTarget, this.material, finalPassId);
			this.ReleaseTemporaryRT(cmd, HBAO.ShaderProperties.tempTex);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003BFC File Offset: 0x00001DFC
		private void UpdateMaterialProperties()
		{
			int num = (this.stereoActive && this.stereoRenderingMode == HBAO.StereoRenderingMode.SinglePassInstanced && !this.renderingInSceneView) ? 2 : 1;
			for (int i = 0; i < num; i++)
			{
				Matrix4x4 matrix4x = (i == 0) ? this.hbaoCamera.projectionMatrix : this.hbaoCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
				float m = matrix4x.m00;
				float m2 = matrix4x.m11;
				this.m_UVToViewPerEye[(i == 0) ? this.xrActiveEye : i] = new Vector4(2f / m, -2f / m2, -1f / m, 1f / m2);
				this.m_RadiusPerEye[(i == 0) ? this.xrActiveEye : i] = this.aoSettings.radius * 0.5f * ((float)(this.screenHeight / ((this.generalSettings.deinterleaving == HBAO.Deinterleaving.x4) ? 4 : 1)) / (2f / m2));
			}
			float num2 = Mathf.Max(16f, this.aoSettings.maxRadiusPixels * Mathf.Sqrt((float)(this.screenWidth * this.screenHeight) / 2073600f));
			num2 /= (float)((this.generalSettings.deinterleaving == HBAO.Deinterleaving.x4) ? 4 : 1);
			Vector4 value = (this.generalSettings.deinterleaving == HBAO.Deinterleaving.x4) ? new Vector4((float)this.reinterleavedAoWidth / (float)this.width, (float)this.reinterleavedAoHeight / (float)this.height, 1f / ((float)this.reinterleavedAoWidth / (float)this.width), 1f / ((float)this.reinterleavedAoHeight / (float)this.height)) : ((this.generalSettings.resolution == HBAO.Resolution.Half) ? new Vector4(((float)this.width + 0.5f) / (float)this.width, ((float)this.height + 0.5f) / (float)this.height, 1f, 1f) : Vector4.one);
			this.material.SetTexture(HBAO.ShaderProperties.noiseTex, this.noiseTex);
			this.material.SetVector(HBAO.ShaderProperties.inputTexelSize, new Vector4(1f / (float)this.width, 1f / (float)this.height, (float)this.width, (float)this.height));
			if (this.hbaoCamera.allowDynamicResolution)
			{
				this.material.SetVector(HBAO.ShaderProperties.aoTexelSize, new Vector4(1f / ((float)this.aoWidth * ScalableBufferManager.widthScaleFactor), 1f / ((float)this.aoHeight * ScalableBufferManager.heightScaleFactor), (float)this.aoWidth * ScalableBufferManager.widthScaleFactor, (float)this.aoHeight * ScalableBufferManager.heightScaleFactor));
			}
			else
			{
				this.material.SetVector(HBAO.ShaderProperties.aoTexelSize, new Vector4(1f / (float)this.aoWidth, 1f / (float)this.aoHeight, (float)this.aoWidth, (float)this.aoHeight));
			}
			this.material.SetVector(HBAO.ShaderProperties.deinterleavedAOTexelSize, new Vector4(1f / (float)this.deinterleavedAoWidth, 1f / (float)this.deinterleavedAoHeight, (float)this.deinterleavedAoWidth, (float)this.deinterleavedAoHeight));
			this.material.SetVector(HBAO.ShaderProperties.reinterleavedAOTexelSize, new Vector4(1f / (float)this.reinterleavedAoWidth, 1f / (float)this.reinterleavedAoHeight, (float)this.reinterleavedAoWidth, (float)this.reinterleavedAoHeight));
			this.material.SetVector(HBAO.ShaderProperties.targetScale, value);
			this.material.SetVectorArray(HBAO.ShaderProperties.uvToView, this.m_UVToViewPerEye);
			this.material.SetFloatArray(HBAO.ShaderProperties.radius, this.m_RadiusPerEye);
			this.material.SetFloat(HBAO.ShaderProperties.maxRadiusPixels, num2);
			this.material.SetFloat(HBAO.ShaderProperties.negInvRadius2, -1f / (this.aoSettings.radius * this.aoSettings.radius));
			this.material.SetFloat(HBAO.ShaderProperties.angleBias, this.aoSettings.bias);
			this.material.SetFloat(HBAO.ShaderProperties.aoMultiplier, 2f * (1f / (1f - this.aoSettings.bias)));
			this.material.SetFloat(HBAO.ShaderProperties.intensity, HBAO.isLinearColorSpace ? this.aoSettings.intensity : (this.aoSettings.intensity * 0.45454547f));
			this.material.SetColor(HBAO.ShaderProperties.baseColor, this.aoSettings.baseColor);
			this.material.SetFloat(HBAO.ShaderProperties.multiBounceInfluence, this.aoSettings.multiBounceInfluence);
			this.material.SetFloat(HBAO.ShaderProperties.offscreenSamplesContrib, this.aoSettings.offscreenSamplesContribution);
			this.material.SetFloat(HBAO.ShaderProperties.maxDistance, this.aoSettings.maxDistance);
			this.material.SetFloat(HBAO.ShaderProperties.distanceFalloff, this.aoSettings.distanceFalloff);
			this.material.SetFloat(HBAO.ShaderProperties.blurSharpness, this.blurSettings.sharpness);
			this.material.SetFloat(HBAO.ShaderProperties.colorBleedSaturation, this.colorBleedingSettings.saturation);
			this.material.SetFloat(HBAO.ShaderProperties.albedoMultiplier, this.colorBleedingSettings.albedoMultiplier);
			this.material.SetFloat(HBAO.ShaderProperties.colorBleedBrightnessMask, this.colorBleedingSettings.brightnessMask);
			this.material.SetVector(HBAO.ShaderProperties.colorBleedBrightnessMaskRange, HBAO.AdjustBrightnessMaskToGammaSpace(new Vector2(Mathf.Pow(this.colorBleedingSettings.brightnessMaskRange.x, 3f), Mathf.Pow(this.colorBleedingSettings.brightnessMaskRange.y, 3f))));
			this.material.SetVector(HBAO.ShaderProperties.temporalParams, (this.temporalFilterSettings.enabled && !this.renderingInSceneView) ? new Vector2(HBAO.s_temporalRotations[this.frameCount % 6] / 360f, HBAO.s_temporalOffsets[this.frameCount % 4]) : Vector2.zero);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000041E4 File Offset: 0x000023E4
		private void UpdateShaderKeywords()
		{
			if (this.m_ShaderKeywords == null || this.m_ShaderKeywords.Length != 12)
			{
				this.m_ShaderKeywords = new string[12];
			}
			this.m_ShaderKeywords[0] = HBAO.ShaderProperties.GetOrthographicOrDeferredKeyword(this.hbaoCamera.orthographic, this.generalSettings);
			this.m_ShaderKeywords[1] = HBAO.ShaderProperties.GetQualityKeyword(this.generalSettings);
			this.m_ShaderKeywords[2] = HBAO.ShaderProperties.GetNoiseKeyword(this.generalSettings);
			this.m_ShaderKeywords[3] = HBAO.ShaderProperties.GetDeinterleavingKeyword(this.generalSettings);
			this.m_ShaderKeywords[4] = HBAO.ShaderProperties.GetDebugKeyword(this.generalSettings);
			this.m_ShaderKeywords[5] = HBAO.ShaderProperties.GetMultibounceKeyword(this.aoSettings);
			this.m_ShaderKeywords[6] = HBAO.ShaderProperties.GetOffscreenSamplesContributionKeyword(this.aoSettings);
			this.m_ShaderKeywords[7] = HBAO.ShaderProperties.GetPerPixelNormalsKeyword(this.aoSettings);
			this.m_ShaderKeywords[8] = HBAO.ShaderProperties.GetBlurRadiusKeyword(this.blurSettings);
			this.m_ShaderKeywords[9] = HBAO.ShaderProperties.GetVarianceClippingKeyword(this.temporalFilterSettings);
			this.m_ShaderKeywords[10] = HBAO.ShaderProperties.GetColorBleedingKeyword(this.colorBleedingSettings);
			this.m_ShaderKeywords[11] = HBAO.ShaderProperties.GetLightingLogEncodedKeyword(this.hbaoCamera.allowHDR);
			this.material.shaderKeywords = this.m_ShaderKeywords;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000431C File Offset: 0x0000251C
		private void CheckParameters()
		{
			this.hbaoCamera.depthTextureMode |= DepthTextureMode.Depth;
			if (this.aoSettings.perPixelNormals == HBAO.PerPixelNormals.Camera)
			{
				this.hbaoCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			if (this.temporalFilterSettings.enabled)
			{
				this.hbaoCamera.depthTextureMode |= DepthTextureMode.MotionVectors;
			}
			if (this.hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading && this.aoSettings.perPixelNormals == HBAO.PerPixelNormals.GBuffer)
			{
				this.SetAoPerPixelNormals(HBAO.PerPixelNormals.Camera);
			}
			if (this.generalSettings.deinterleaving != HBAO.Deinterleaving.Disabled && SystemInfo.supportedRenderTargetCount < 4)
			{
				this.SetDeinterleaving(HBAO.Deinterleaving.Disabled);
			}
			if (this.generalSettings.pipelineStage != HBAO.PipelineStage.BeforeImageEffectsOpaque && this.hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading)
			{
				this.SetPipelineStage(HBAO.PipelineStage.BeforeImageEffectsOpaque);
			}
			if (this.generalSettings.pipelineStage != HBAO.PipelineStage.BeforeImageEffectsOpaque && this.aoSettings.perPixelNormals == HBAO.PerPixelNormals.Camera)
			{
				this.SetAoPerPixelNormals(HBAO.PerPixelNormals.GBuffer);
			}
			if (this.stereoActive && this.hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading && this.aoSettings.perPixelNormals != HBAO.PerPixelNormals.Reconstruct)
			{
				this.SetAoPerPixelNormals(HBAO.PerPixelNormals.Reconstruct);
			}
			if (this.temporalFilterSettings.enabled && !this.motionVectorsSupported)
			{
				this.EnableTemporalFilter(false);
			}
			if (this.colorBleedingSettings.enabled && this.temporalFilterSettings.enabled && SystemInfo.supportedRenderTargetCount < 2)
			{
				this.EnableTemporalFilter(false);
			}
			if (!(this.noiseTex == null))
			{
				HBAO.NoiseType? previousNoiseType = this.m_PreviousNoiseType;
				HBAO.NoiseType noiseType = this.generalSettings.noiseType;
				if (previousNoiseType.GetValueOrDefault() == noiseType & previousNoiseType != null)
				{
					return;
				}
			}
			if (this.noiseTex != null)
			{
				UnityEngine.Object.DestroyImmediate(this.noiseTex);
			}
			this.CreateNoiseTexture();
			this.m_PreviousNoiseType = new HBAO.NoiseType?(this.generalSettings.noiseType);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000044DC File Offset: 0x000026DC
		private RenderTextureDescriptor GetDefaultDescriptor(int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default)
		{
			RenderTextureDescriptor result = new RenderTextureDescriptor(this.m_sourceDescriptor.width, this.m_sourceDescriptor.height, this.m_sourceDescriptor.colorFormat, depthBufferBits);
			result.dimension = this.m_sourceDescriptor.dimension;
			result.volumeDepth = this.m_sourceDescriptor.volumeDepth;
			result.vrUsage = this.m_sourceDescriptor.vrUsage;
			result.msaaSamples = this.m_sourceDescriptor.msaaSamples;
			result.memoryless = this.m_sourceDescriptor.memoryless;
			result.useMipMap = this.m_sourceDescriptor.useMipMap;
			result.autoGenerateMips = this.m_sourceDescriptor.autoGenerateMips;
			result.enableRandomWrite = this.m_sourceDescriptor.enableRandomWrite;
			result.shadowSamplingMode = this.m_sourceDescriptor.shadowSamplingMode;
			if (this.hbaoCamera.allowDynamicResolution)
			{
				result.useDynamicScale = true;
			}
			if (colorFormat != RenderTextureFormat.Default)
			{
				result.colorFormat = colorFormat;
			}
			if (readWrite == RenderTextureReadWrite.sRGB)
			{
				result.sRGB = true;
			}
			else if (readWrite == RenderTextureReadWrite.Linear)
			{
				result.sRGB = false;
			}
			else if (readWrite == RenderTextureReadWrite.Default)
			{
				result.sRGB = HBAO.isLinearColorSpace;
			}
			return result;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004604 File Offset: 0x00002804
		private RenderTexture GetScreenSpaceRT(int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, FilterMode filter = FilterMode.Bilinear, int widthOverride = 0, int heightOverride = 0)
		{
			RenderTextureDescriptor defaultDescriptor = this.GetDefaultDescriptor(depthBufferBits, colorFormat, readWrite);
			if (widthOverride > 0)
			{
				defaultDescriptor.width = widthOverride;
			}
			if (heightOverride > 0)
			{
				defaultDescriptor.height = heightOverride;
			}
			return new RenderTexture(defaultDescriptor)
			{
				filterMode = filter
			};
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004648 File Offset: 0x00002848
		private void GetScreenSpaceTemporaryRT(CommandBuffer cmd, int nameID, int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, FilterMode filter = FilterMode.Bilinear, int widthOverride = 0, int heightOverride = 0)
		{
			RenderTextureDescriptor defaultDescriptor = this.GetDefaultDescriptor(depthBufferBits, colorFormat, readWrite);
			if (widthOverride > 0)
			{
				defaultDescriptor.width = widthOverride;
			}
			if (heightOverride > 0)
			{
				defaultDescriptor.height = heightOverride;
			}
			cmd.GetTemporaryRT(nameID, defaultDescriptor, filter);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004687 File Offset: 0x00002887
		private void ReleaseTemporaryRT(CommandBuffer cmd, int nameID)
		{
			cmd.ReleaseTemporaryRT(nameID);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004690 File Offset: 0x00002890
		private void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int pass = 0)
		{
			cmd.SetGlobalTexture(HBAO.ShaderProperties.mainTex, source);
			cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
			cmd.DrawMesh(this.fullscreenTriangle, Matrix4x4.identity, material, 0, pass);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000046BE File Offset: 0x000028BE
		private void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier[] destinations, Material material, int pass = 0)
		{
			cmd.SetGlobalTexture(HBAO.ShaderProperties.mainTex, source);
			cmd.SetRenderTarget(destinations, destinations[0], 0, CubemapFace.Unknown, -1);
			cmd.DrawMesh(this.fullscreenTriangle, Matrix4x4.identity, material, 0, pass);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000046F3 File Offset: 0x000028F3
		private void BlitFullscreenTriangleWithClear(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, Color clearColor, int pass = 0)
		{
			cmd.SetGlobalTexture(HBAO.ShaderProperties.mainTex, source);
			cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
			cmd.ClearRenderTarget(false, true, clearColor);
			cmd.DrawMesh(this.fullscreenTriangle, Matrix4x4.identity, material, 0, pass);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000472C File Offset: 0x0000292C
		private static void ApplyFlip(CommandBuffer cmd, bool flip = true)
		{
			if (flip)
			{
				cmd.SetGlobalVector(HBAO.ShaderProperties.uvTransform, new Vector4(1f, -1f, 0f, 1f));
				return;
			}
			cmd.SetGlobalVector(HBAO.ShaderProperties.uvTransform, new Vector4(1f, 1f, 0f, 0f));
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004785 File Offset: 0x00002985
		private static Vector2 AdjustBrightnessMaskToGammaSpace(Vector2 v)
		{
			if (!HBAO.isLinearColorSpace)
			{
				return HBAO.ToGammaSpace(v);
			}
			return v;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004796 File Offset: 0x00002996
		private static float ToGammaSpace(float v)
		{
			return Mathf.Pow(v, 0.45454547f);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000047A3 File Offset: 0x000029A3
		private static Vector2 ToGammaSpace(Vector2 v)
		{
			return new Vector2(HBAO.ToGammaSpace(v.x), HBAO.ToGammaSpace(v.y));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000047C0 File Offset: 0x000029C0
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
					float r = (this.generalSettings.noiseType != HBAO.NoiseType.Dither) ? (0.25f * (0.0625f * (float)((i + j & 3) << 2) + (float)(i & 3))) : HBAO.MersenneTwister.Numbers[num++];
					float g = (this.generalSettings.noiseType != HBAO.NoiseType.Dither) ? (0.25f * (float)(j - i & 3)) : HBAO.MersenneTwister.Numbers[num++];
					Color color = new Color(r, g, 0f);
					this.noiseTex.SetPixel(i, j, color);
				}
			}
			this.noiseTex.Apply();
			int k = 0;
			int num2 = 0;
			while (k < HBAO.s_jitter.Length)
			{
				float x = HBAO.MersenneTwister.Numbers[num2++];
				float y = HBAO.MersenneTwister.Numbers[num2++];
				HBAO.s_jitter[k] = new Vector2(x, y);
				k++;
			}
		}

		// Token: 0x04000001 RID: 1
		public Shader hbaoShader;

		// Token: 0x04000002 RID: 2
		[SerializeField]
		[HBAO.SettingsGroup]
		private HBAO.Presets m_Presets = HBAO.Presets.defaults;

		// Token: 0x04000003 RID: 3
		[SerializeField]
		[HBAO.SettingsGroup]
		private HBAO.GeneralSettings m_GeneralSettings = HBAO.GeneralSettings.defaults;

		// Token: 0x04000004 RID: 4
		[SerializeField]
		[HBAO.SettingsGroup]
		private HBAO.AOSettings m_AOSettings = HBAO.AOSettings.defaults;

		// Token: 0x04000005 RID: 5
		[SerializeField]
		[HBAO.SettingsGroup]
		private HBAO.TemporalFilterSettings m_TemporalFilterSettings = HBAO.TemporalFilterSettings.defaults;

		// Token: 0x04000006 RID: 6
		[SerializeField]
		[HBAO.SettingsGroup]
		private HBAO.BlurSettings m_BlurSettings = HBAO.BlurSettings.defaults;

		// Token: 0x04000007 RID: 7
		[SerializeField]
		[HBAO.SettingsGroup]
		private HBAO.ColorBleedingSettings m_ColorBleedingSettings = HBAO.ColorBleedingSettings.defaults;

		// Token: 0x04000008 RID: 8
		private static readonly Vector2[] s_jitter = new Vector2[16];

		// Token: 0x04000009 RID: 9
		private static readonly float[] s_temporalRotations = new float[]
		{
			60f,
			300f,
			180f,
			240f,
			120f,
			0f
		};

		// Token: 0x0400000A RID: 10
		private static readonly float[] s_temporalOffsets = new float[]
		{
			0f,
			0.5f,
			0.25f,
			0.75f
		};

		// Token: 0x04000020 RID: 32
		private RenderTextureDescriptor m_sourceDescriptor;

		// Token: 0x04000021 RID: 33
		private string[] m_ShaderKeywords;

		// Token: 0x04000022 RID: 34
		private Vector4[] m_UVToViewPerEye = new Vector4[2];

		// Token: 0x04000023 RID: 35
		private float[] m_RadiusPerEye = new float[2];

		// Token: 0x04000024 RID: 36
		private bool m_IsCommandBufferDirty;

		// Token: 0x04000025 RID: 37
		private Mesh m_FullscreenTriangle;

		// Token: 0x04000026 RID: 38
		private HBAO.PipelineStage? m_PreviousPipelineStage;

		// Token: 0x04000027 RID: 39
		private HBAO.Resolution? m_PreviousResolution;

		// Token: 0x04000028 RID: 40
		private HBAO.Deinterleaving? m_PreviousDeinterleaving;

		// Token: 0x04000029 RID: 41
		private HBAO.DebugMode? m_PreviousDebugMode;

		// Token: 0x0400002A RID: 42
		private HBAO.NoiseType? m_PreviousNoiseType;

		// Token: 0x0400002B RID: 43
		private HBAO.BlurType? m_PreviousBlurAmount;

		// Token: 0x0400002C RID: 44
		private int m_PreviousWidth;

		// Token: 0x0400002D RID: 45
		private int m_PreviousHeight;

		// Token: 0x0400002E RID: 46
		private bool m_PreviousAllowHDR;

		// Token: 0x0400002F RID: 47
		private bool m_PreviousUseMultibounce;

		// Token: 0x04000030 RID: 48
		private bool m_PreviousColorBleedingEnabled;

		// Token: 0x04000031 RID: 49
		private bool m_PreviousTemporalFilterEnabled;

		// Token: 0x04000032 RID: 50
		private RenderingPath m_PreviousRenderingPath;

		// Token: 0x04000033 RID: 51
		private HBAO.StereoRenderingMode m_PrevStereoRenderingMode;

		// Token: 0x02000006 RID: 6
		public enum Preset
		{
			// Token: 0x0400003F RID: 63
			FastestPerformance,
			// Token: 0x04000040 RID: 64
			FastPerformance,
			// Token: 0x04000041 RID: 65
			Normal,
			// Token: 0x04000042 RID: 66
			HighQuality,
			// Token: 0x04000043 RID: 67
			HighestQuality,
			// Token: 0x04000044 RID: 68
			Custom
		}

		// Token: 0x02000007 RID: 7
		public enum PipelineStage
		{
			// Token: 0x04000046 RID: 70
			BeforeImageEffectsOpaque,
			// Token: 0x04000047 RID: 71
			AfterLighting,
			// Token: 0x04000048 RID: 72
			BeforeReflections
		}

		// Token: 0x02000008 RID: 8
		public enum Quality
		{
			// Token: 0x0400004A RID: 74
			Lowest,
			// Token: 0x0400004B RID: 75
			Low,
			// Token: 0x0400004C RID: 76
			Medium,
			// Token: 0x0400004D RID: 77
			High,
			// Token: 0x0400004E RID: 78
			Highest
		}

		// Token: 0x02000009 RID: 9
		public enum Resolution
		{
			// Token: 0x04000050 RID: 80
			Full,
			// Token: 0x04000051 RID: 81
			Half
		}

		// Token: 0x0200000A RID: 10
		public enum NoiseType
		{
			// Token: 0x04000053 RID: 83
			Dither,
			// Token: 0x04000054 RID: 84
			InterleavedGradientNoise,
			// Token: 0x04000055 RID: 85
			SpatialDistribution
		}

		// Token: 0x0200000B RID: 11
		public enum Deinterleaving
		{
			// Token: 0x04000057 RID: 87
			Disabled,
			// Token: 0x04000058 RID: 88
			x4
		}

		// Token: 0x0200000C RID: 12
		public enum DebugMode
		{
			// Token: 0x0400005A RID: 90
			Disabled,
			// Token: 0x0400005B RID: 91
			AOOnly,
			// Token: 0x0400005C RID: 92
			ColorBleedingOnly,
			// Token: 0x0400005D RID: 93
			SplitWithoutAOAndWithAO,
			// Token: 0x0400005E RID: 94
			SplitWithAOAndAOOnly,
			// Token: 0x0400005F RID: 95
			SplitWithoutAOAndAOOnly,
			// Token: 0x04000060 RID: 96
			ViewNormals
		}

		// Token: 0x0200000D RID: 13
		public enum BlurType
		{
			// Token: 0x04000062 RID: 98
			None,
			// Token: 0x04000063 RID: 99
			Narrow,
			// Token: 0x04000064 RID: 100
			Medium,
			// Token: 0x04000065 RID: 101
			Wide,
			// Token: 0x04000066 RID: 102
			ExtraWide
		}

		// Token: 0x0200000E RID: 14
		public enum PerPixelNormals
		{
			// Token: 0x04000068 RID: 104
			GBuffer,
			// Token: 0x04000069 RID: 105
			Camera,
			// Token: 0x0400006A RID: 106
			Reconstruct
		}

		// Token: 0x0200000F RID: 15
		public enum VarianceClipping
		{
			// Token: 0x0400006C RID: 108
			Disabled,
			// Token: 0x0400006D RID: 109
			_4Tap,
			// Token: 0x0400006E RID: 110
			_8Tap
		}

		// Token: 0x02000010 RID: 16
		[Serializable]
		public struct Presets
		{
			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000A0 RID: 160 RVA: 0x000049A0 File Offset: 0x00002BA0
			[SerializeField]
			public static HBAO.Presets defaults
			{
				get
				{
					return new HBAO.Presets
					{
						preset = HBAO.Preset.Normal
					};
				}
			}

			// Token: 0x0400006F RID: 111
			public HBAO.Preset preset;
		}

		// Token: 0x02000011 RID: 17
		[Serializable]
		public struct GeneralSettings
		{
			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000A1 RID: 161 RVA: 0x000049C0 File Offset: 0x00002BC0
			[SerializeField]
			public static HBAO.GeneralSettings defaults
			{
				get
				{
					return new HBAO.GeneralSettings
					{
						pipelineStage = HBAO.PipelineStage.BeforeImageEffectsOpaque,
						quality = HBAO.Quality.Medium,
						deinterleaving = HBAO.Deinterleaving.Disabled,
						resolution = HBAO.Resolution.Full,
						noiseType = HBAO.NoiseType.Dither,
						debugMode = HBAO.DebugMode.Disabled
					};
				}
			}

			// Token: 0x04000070 RID: 112
			[Tooltip("The stage the AO is injected into the rendering pipeline.")]
			[Space(6f)]
			public HBAO.PipelineStage pipelineStage;

			// Token: 0x04000071 RID: 113
			[Tooltip("The quality of the AO.")]
			[Space(10f)]
			public HBAO.Quality quality;

			// Token: 0x04000072 RID: 114
			[Tooltip("The deinterleaving factor.")]
			public HBAO.Deinterleaving deinterleaving;

			// Token: 0x04000073 RID: 115
			[Tooltip("The resolution at which the AO is calculated.")]
			public HBAO.Resolution resolution;

			// Token: 0x04000074 RID: 116
			[Tooltip("The type of noise to use.")]
			[Space(10f)]
			public HBAO.NoiseType noiseType;

			// Token: 0x04000075 RID: 117
			[Tooltip("The debug mode actually displayed on screen.")]
			[Space(10f)]
			public HBAO.DebugMode debugMode;
		}

		// Token: 0x02000012 RID: 18
		[Serializable]
		public struct AOSettings
		{
			// Token: 0x17000029 RID: 41
			// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004A08 File Offset: 0x00002C08
			[SerializeField]
			public static HBAO.AOSettings defaults
			{
				get
				{
					return new HBAO.AOSettings
					{
						radius = 0.8f,
						maxRadiusPixels = 128f,
						bias = 0.05f,
						intensity = 1f,
						useMultiBounce = false,
						multiBounceInfluence = 1f,
						offscreenSamplesContribution = 0f,
						maxDistance = 150f,
						distanceFalloff = 50f,
						perPixelNormals = HBAO.PerPixelNormals.GBuffer,
						baseColor = Color.black
					};
				}
			}

			// Token: 0x04000076 RID: 118
			[Tooltip("AO radius: this is the distance outside which occluders are ignored.")]
			[Space(6f)]
			[Range(0.25f, 5f)]
			public float radius;

			// Token: 0x04000077 RID: 119
			[Tooltip("Maximum radius in pixels: this prevents the radius to grow too much with close-up object and impact on performances.")]
			[Range(16f, 256f)]
			public float maxRadiusPixels;

			// Token: 0x04000078 RID: 120
			[Tooltip("For low-tessellated geometry, occlusion variations tend to appear at creases and ridges, which betray the underlying tessellation. To remove these artifacts, we use an angle bias parameter which restricts the hemisphere.")]
			[Range(0f, 0.5f)]
			public float bias;

			// Token: 0x04000079 RID: 121
			[Tooltip("This value allows to scale up the ambient occlusion values.")]
			[Range(0f, 4f)]
			public float intensity;

			// Token: 0x0400007A RID: 122
			[Tooltip("Enable/disable MultiBounce approximation.")]
			public bool useMultiBounce;

			// Token: 0x0400007B RID: 123
			[Tooltip("MultiBounce approximation influence.")]
			[Range(0f, 1f)]
			public float multiBounceInfluence;

			// Token: 0x0400007C RID: 124
			[Tooltip("The amount of AO offscreen samples are contributing.")]
			[Range(0f, 1f)]
			public float offscreenSamplesContribution;

			// Token: 0x0400007D RID: 125
			[Tooltip("The max distance to display AO.")]
			[Space(10f)]
			public float maxDistance;

			// Token: 0x0400007E RID: 126
			[Tooltip("The distance before max distance at which AO start to decrease.")]
			public float distanceFalloff;

			// Token: 0x0400007F RID: 127
			[Tooltip("The type of per pixel normals to use.")]
			[Space(10f)]
			public HBAO.PerPixelNormals perPixelNormals;

			// Token: 0x04000080 RID: 128
			[Tooltip("This setting allow you to set the base color if the AO, the alpha channel value is unused.")]
			[Space(10f)]
			public Color baseColor;
		}

		// Token: 0x02000013 RID: 19
		[Serializable]
		public struct TemporalFilterSettings
		{
			// Token: 0x1700002A RID: 42
			// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004A9C File Offset: 0x00002C9C
			[SerializeField]
			public static HBAO.TemporalFilterSettings defaults
			{
				get
				{
					return new HBAO.TemporalFilterSettings
					{
						enabled = false,
						varianceClipping = HBAO.VarianceClipping._4Tap
					};
				}
			}

			// Token: 0x04000081 RID: 129
			[Space(6f)]
			public bool enabled;

			// Token: 0x04000082 RID: 130
			[Tooltip("The type of variance clipping to use.")]
			public HBAO.VarianceClipping varianceClipping;
		}

		// Token: 0x02000014 RID: 20
		[Serializable]
		public struct BlurSettings
		{
			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004AC4 File Offset: 0x00002CC4
			[SerializeField]
			public static HBAO.BlurSettings defaults
			{
				get
				{
					return new HBAO.BlurSettings
					{
						type = HBAO.BlurType.Medium,
						sharpness = 8f
					};
				}
			}

			// Token: 0x04000083 RID: 131
			[Tooltip("The type of blur to use.")]
			[Space(6f)]
			public HBAO.BlurType type;

			// Token: 0x04000084 RID: 132
			[Tooltip("This parameter controls the depth-dependent weight of the bilateral filter, to avoid bleeding across edges. A zero sharpness is a pure Gaussian blur. Increasing the blur sharpness removes bleeding by using lower weights for samples with large depth delta from the current pixel.")]
			[Space(10f)]
			[Range(0f, 16f)]
			public float sharpness;
		}

		// Token: 0x02000015 RID: 21
		[Serializable]
		public struct ColorBleedingSettings
		{
			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004AF0 File Offset: 0x00002CF0
			[SerializeField]
			public static HBAO.ColorBleedingSettings defaults
			{
				get
				{
					return new HBAO.ColorBleedingSettings
					{
						enabled = false,
						saturation = 1f,
						albedoMultiplier = 4f,
						brightnessMask = 1f,
						brightnessMaskRange = new Vector2(0f, 0.5f)
					};
				}
			}

			// Token: 0x04000085 RID: 133
			[Space(6f)]
			public bool enabled;

			// Token: 0x04000086 RID: 134
			[Tooltip("This value allows to control the saturation of the color bleeding.")]
			[Space(10f)]
			[Range(0f, 4f)]
			public float saturation;

			// Token: 0x04000087 RID: 135
			[Tooltip("This value allows to scale the contribution of the color bleeding samples.")]
			[Range(0f, 32f)]
			public float albedoMultiplier;

			// Token: 0x04000088 RID: 136
			[Tooltip("Use masking on emissive pixels")]
			[Range(0f, 1f)]
			public float brightnessMask;

			// Token: 0x04000089 RID: 137
			[Tooltip("Brightness level where masking starts/ends")]
			[HBAO.MinMaxSliderAttribute(0f, 2f)]
			public Vector2 brightnessMaskRange;
		}

		// Token: 0x02000016 RID: 22
		[AttributeUsage(AttributeTargets.Field)]
		public class SettingsGroup : Attribute
		{
		}

		// Token: 0x02000017 RID: 23
		public class MinMaxSliderAttribute : PropertyAttribute
		{
			// Token: 0x060000A7 RID: 167 RVA: 0x00004B50 File Offset: 0x00002D50
			public MinMaxSliderAttribute(float min, float max)
			{
				this.min = min;
				this.max = max;
			}

			// Token: 0x0400008A RID: 138
			public readonly float max;

			// Token: 0x0400008B RID: 139
			public readonly float min;
		}

		// Token: 0x02000018 RID: 24
		private static class Pass
		{
			// Token: 0x0400008C RID: 140
			public const int AO = 0;

			// Token: 0x0400008D RID: 141
			public const int AO_Deinterleaved = 1;

			// Token: 0x0400008E RID: 142
			public const int Deinterleave_Depth = 2;

			// Token: 0x0400008F RID: 143
			public const int Deinterleave_Normals = 3;

			// Token: 0x04000090 RID: 144
			public const int Atlas_AO_Deinterleaved = 4;

			// Token: 0x04000091 RID: 145
			public const int Reinterleave_AO = 5;

			// Token: 0x04000092 RID: 146
			public const int Blur = 6;

			// Token: 0x04000093 RID: 147
			public const int Temporal_Filter = 7;

			// Token: 0x04000094 RID: 148
			public const int Copy = 8;

			// Token: 0x04000095 RID: 149
			public const int Composite = 9;

			// Token: 0x04000096 RID: 150
			public const int Composite_AfterLighting = 10;

			// Token: 0x04000097 RID: 151
			public const int Composite_BeforeReflections = 11;

			// Token: 0x04000098 RID: 152
			public const int Composite_BlendAO = 12;

			// Token: 0x04000099 RID: 153
			public const int Composite_BlendCB = 13;

			// Token: 0x0400009A RID: 154
			public const int Debug_ViewNormals = 14;
		}

		// Token: 0x02000019 RID: 25
		private static class ShaderProperties
		{
			// Token: 0x060000A8 RID: 168 RVA: 0x00004B68 File Offset: 0x00002D68
			static ShaderProperties()
			{
				for (int i = 0; i < 16; i++)
				{
					HBAO.ShaderProperties.depthSliceTex[i] = Shader.PropertyToID("_DepthSliceTex" + i.ToString());
					HBAO.ShaderProperties.normalsSliceTex[i] = Shader.PropertyToID("_NormalsSliceTex" + i.ToString());
					HBAO.ShaderProperties.aoSliceTex[i] = Shader.PropertyToID("_AOSliceTex" + i.ToString());
				}
				HBAO.ShaderProperties.deinterleaveOffset = new int[]
				{
					Shader.PropertyToID("_Deinterleave_Offset00"),
					Shader.PropertyToID("_Deinterleave_Offset10"),
					Shader.PropertyToID("_Deinterleave_Offset01"),
					Shader.PropertyToID("_Deinterleave_Offset11")
				};
				HBAO.ShaderProperties.atlasOffset = Shader.PropertyToID("_AtlasOffset");
				HBAO.ShaderProperties.jitter = Shader.PropertyToID("_Jitter");
				HBAO.ShaderProperties.uvTransform = Shader.PropertyToID("_UVTransform");
				HBAO.ShaderProperties.inputTexelSize = Shader.PropertyToID("_Input_TexelSize");
				HBAO.ShaderProperties.aoTexelSize = Shader.PropertyToID("_AO_TexelSize");
				HBAO.ShaderProperties.deinterleavedAOTexelSize = Shader.PropertyToID("_DeinterleavedAO_TexelSize");
				HBAO.ShaderProperties.reinterleavedAOTexelSize = Shader.PropertyToID("_ReinterleavedAO_TexelSize");
				HBAO.ShaderProperties.uvToView = Shader.PropertyToID("_UVToView");
				HBAO.ShaderProperties.targetScale = Shader.PropertyToID("_TargetScale");
				HBAO.ShaderProperties.radius = Shader.PropertyToID("_Radius");
				HBAO.ShaderProperties.maxRadiusPixels = Shader.PropertyToID("_MaxRadiusPixels");
				HBAO.ShaderProperties.negInvRadius2 = Shader.PropertyToID("_NegInvRadius2");
				HBAO.ShaderProperties.angleBias = Shader.PropertyToID("_AngleBias");
				HBAO.ShaderProperties.aoMultiplier = Shader.PropertyToID("_AOmultiplier");
				HBAO.ShaderProperties.intensity = Shader.PropertyToID("_Intensity");
				HBAO.ShaderProperties.multiBounceInfluence = Shader.PropertyToID("_MultiBounceInfluence");
				HBAO.ShaderProperties.offscreenSamplesContrib = Shader.PropertyToID("_OffscreenSamplesContrib");
				HBAO.ShaderProperties.maxDistance = Shader.PropertyToID("_MaxDistance");
				HBAO.ShaderProperties.distanceFalloff = Shader.PropertyToID("_DistanceFalloff");
				HBAO.ShaderProperties.baseColor = Shader.PropertyToID("_BaseColor");
				HBAO.ShaderProperties.colorBleedSaturation = Shader.PropertyToID("_ColorBleedSaturation");
				HBAO.ShaderProperties.albedoMultiplier = Shader.PropertyToID("_AlbedoMultiplier");
				HBAO.ShaderProperties.colorBleedBrightnessMask = Shader.PropertyToID("_ColorBleedBrightnessMask");
				HBAO.ShaderProperties.colorBleedBrightnessMaskRange = Shader.PropertyToID("_ColorBleedBrightnessMaskRange");
				HBAO.ShaderProperties.blurDeltaUV = Shader.PropertyToID("_BlurDeltaUV");
				HBAO.ShaderProperties.blurSharpness = Shader.PropertyToID("_BlurSharpness");
				HBAO.ShaderProperties.temporalParams = Shader.PropertyToID("_TemporalParams");
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00004E3A File Offset: 0x0000303A
			public static string GetOrthographicOrDeferredKeyword(bool orthographic, HBAO.GeneralSettings settings)
			{
				if (orthographic)
				{
					return "ORTHOGRAPHIC_PROJECTION";
				}
				if (settings.pipelineStage == HBAO.PipelineStage.BeforeImageEffectsOpaque)
				{
					return "__";
				}
				return "DEFERRED_SHADING";
			}

			// Token: 0x060000AA RID: 170 RVA: 0x00004E58 File Offset: 0x00003058
			public static string GetQualityKeyword(HBAO.GeneralSettings settings)
			{
				switch (settings.quality)
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

			// Token: 0x060000AB RID: 171 RVA: 0x00004EAC File Offset: 0x000030AC
			public static string GetNoiseKeyword(HBAO.GeneralSettings settings)
			{
				switch (settings.noiseType)
				{
				case HBAO.NoiseType.InterleavedGradientNoise:
					return "INTERLEAVED_GRADIENT_NOISE";
				}
				return "__";
			}

			// Token: 0x060000AC RID: 172 RVA: 0x00004EE0 File Offset: 0x000030E0
			public static string GetDeinterleavingKeyword(HBAO.GeneralSettings settings)
			{
				HBAO.Deinterleaving deinterleaving = settings.deinterleaving;
				if (deinterleaving != HBAO.Deinterleaving.Disabled && deinterleaving == HBAO.Deinterleaving.x4)
				{
					return "DEINTERLEAVED";
				}
				return "__";
			}

			// Token: 0x060000AD RID: 173 RVA: 0x00004F08 File Offset: 0x00003108
			public static string GetDebugKeyword(HBAO.GeneralSettings settings)
			{
				switch (settings.debugMode)
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

			// Token: 0x060000AE RID: 174 RVA: 0x00004F5F File Offset: 0x0000315F
			public static string GetMultibounceKeyword(HBAO.AOSettings settings)
			{
				if (!settings.useMultiBounce)
				{
					return "__";
				}
				return "MULTIBOUNCE";
			}

			// Token: 0x060000AF RID: 175 RVA: 0x00004F74 File Offset: 0x00003174
			public static string GetOffscreenSamplesContributionKeyword(HBAO.AOSettings settings)
			{
				if (settings.offscreenSamplesContribution <= 0f)
				{
					return "__";
				}
				return "OFFSCREEN_SAMPLES_CONTRIBUTION";
			}

			// Token: 0x060000B0 RID: 176 RVA: 0x00004F90 File Offset: 0x00003190
			public static string GetPerPixelNormalsKeyword(HBAO.AOSettings settings)
			{
				switch (settings.perPixelNormals)
				{
				case HBAO.PerPixelNormals.Camera:
					return "NORMALS_CAMERA";
				case HBAO.PerPixelNormals.Reconstruct:
					return "NORMALS_RECONSTRUCT";
				}
				return "__";
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x00004FCC File Offset: 0x000031CC
			public static string GetBlurRadiusKeyword(HBAO.BlurSettings settings)
			{
				switch (settings.type)
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

			// Token: 0x060000B2 RID: 178 RVA: 0x0000501C File Offset: 0x0000321C
			public static string GetVarianceClippingKeyword(HBAO.TemporalFilterSettings settings)
			{
				switch (settings.varianceClipping)
				{
				case HBAO.VarianceClipping._4Tap:
					return "VARIANCE_CLIPPING_4TAP";
				case HBAO.VarianceClipping._8Tap:
					return "VARIANCE_CLIPPING_8TAP";
				}
				return "__";
			}

			// Token: 0x060000B3 RID: 179 RVA: 0x00005055 File Offset: 0x00003255
			public static string GetColorBleedingKeyword(HBAO.ColorBleedingSettings settings)
			{
				if (!settings.enabled)
				{
					return "__";
				}
				return "COLOR_BLEEDING";
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x0000506A File Offset: 0x0000326A
			public static string GetLightingLogEncodedKeyword(bool hdr)
			{
				if (!hdr)
				{
					return "LIGHTING_LOG_ENCODED";
				}
				return "__";
			}

			// Token: 0x0400009B RID: 155
			public static int mainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x0400009C RID: 156
			public static int hbaoTex = Shader.PropertyToID("_HBAOTex");

			// Token: 0x0400009D RID: 157
			public static int tempTex = Shader.PropertyToID("_TempTex");

			// Token: 0x0400009E RID: 158
			public static int tempTex2 = Shader.PropertyToID("_TempTex2");

			// Token: 0x0400009F RID: 159
			public static int noiseTex = Shader.PropertyToID("_NoiseTex");

			// Token: 0x040000A0 RID: 160
			public static int depthTex = Shader.PropertyToID("_DepthTex");

			// Token: 0x040000A1 RID: 161
			public static int normalsTex = Shader.PropertyToID("_NormalsTex");

			// Token: 0x040000A2 RID: 162
			public static int[] depthSliceTex = new int[16];

			// Token: 0x040000A3 RID: 163
			public static int[] normalsSliceTex = new int[16];

			// Token: 0x040000A4 RID: 164
			public static int[] aoSliceTex = new int[16];

			// Token: 0x040000A5 RID: 165
			public static int[] deinterleaveOffset;

			// Token: 0x040000A6 RID: 166
			public static int atlasOffset;

			// Token: 0x040000A7 RID: 167
			public static int jitter;

			// Token: 0x040000A8 RID: 168
			public static int uvTransform;

			// Token: 0x040000A9 RID: 169
			public static int inputTexelSize;

			// Token: 0x040000AA RID: 170
			public static int aoTexelSize;

			// Token: 0x040000AB RID: 171
			public static int deinterleavedAOTexelSize;

			// Token: 0x040000AC RID: 172
			public static int reinterleavedAOTexelSize;

			// Token: 0x040000AD RID: 173
			public static int uvToView;

			// Token: 0x040000AE RID: 174
			public static int targetScale;

			// Token: 0x040000AF RID: 175
			public static int radius;

			// Token: 0x040000B0 RID: 176
			public static int maxRadiusPixels;

			// Token: 0x040000B1 RID: 177
			public static int negInvRadius2;

			// Token: 0x040000B2 RID: 178
			public static int angleBias;

			// Token: 0x040000B3 RID: 179
			public static int aoMultiplier;

			// Token: 0x040000B4 RID: 180
			public static int intensity;

			// Token: 0x040000B5 RID: 181
			public static int multiBounceInfluence;

			// Token: 0x040000B6 RID: 182
			public static int offscreenSamplesContrib;

			// Token: 0x040000B7 RID: 183
			public static int maxDistance;

			// Token: 0x040000B8 RID: 184
			public static int distanceFalloff;

			// Token: 0x040000B9 RID: 185
			public static int baseColor;

			// Token: 0x040000BA RID: 186
			public static int colorBleedSaturation;

			// Token: 0x040000BB RID: 187
			public static int albedoMultiplier;

			// Token: 0x040000BC RID: 188
			public static int colorBleedBrightnessMask;

			// Token: 0x040000BD RID: 189
			public static int colorBleedBrightnessMaskRange;

			// Token: 0x040000BE RID: 190
			public static int blurDeltaUV;

			// Token: 0x040000BF RID: 191
			public static int blurSharpness;

			// Token: 0x040000C0 RID: 192
			public static int temporalParams;
		}

		// Token: 0x0200001A RID: 26
		public enum StereoRenderingMode
		{
			// Token: 0x040000C2 RID: 194
			MultiPass,
			// Token: 0x040000C3 RID: 195
			SinglePassInstanced
		}

		// Token: 0x0200001B RID: 27
		private static class MersenneTwister
		{
			// Token: 0x040000C4 RID: 196
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
	}
}
