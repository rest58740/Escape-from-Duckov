using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HorizonBasedAmbientOcclusion.Universal
{
	// Token: 0x02000003 RID: 3
	[ExecuteInEditMode]
	[VolumeComponentMenu("Lighting/HBAO")]
	public class HBAO : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BF File Offset: 0x000002BF
		public void EnableHBAO(bool enable)
		{
			this.intensity.overrideState = enable;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020CD File Offset: 0x000002CD
		public HBAO.Preset GetCurrentPreset()
		{
			return this.preset.value;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020DC File Offset: 0x000002DC
		public void ApplyPreset(HBAO.Preset preset)
		{
			if (preset == HBAO.Preset.Custom)
			{
				this.preset.Override(preset);
				return;
			}
			HBAO.DebugMode value = this.debugMode.value;
			bool overrideState = this.debugMode.overrideState;
			base.SetAllOverridesTo(false);
			this.debugMode.overrideState = overrideState;
			this.debugMode.value = value;
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
			this.preset.Override(preset);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021E0 File Offset: 0x000003E0
		public HBAO.Mode GetMode()
		{
			return this.mode.value;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021ED File Offset: 0x000003ED
		public void SetMode(HBAO.Mode mode)
		{
			this.mode.Override(mode);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021FB File Offset: 0x000003FB
		public HBAO.RenderingPath GetRenderingPath()
		{
			return this.renderingPath.value;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002208 File Offset: 0x00000408
		public void SetRenderingPath(HBAO.RenderingPath renderingPath)
		{
			this.renderingPath.Override(renderingPath);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002216 File Offset: 0x00000416
		public HBAO.Quality GetQuality()
		{
			return this.quality.value;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002223 File Offset: 0x00000423
		public void SetQuality(HBAO.Quality quality)
		{
			this.quality.Override(quality);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002231 File Offset: 0x00000431
		public HBAO.Deinterleaving GetDeinterleaving()
		{
			return this.deinterleaving.value;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000223E File Offset: 0x0000043E
		public void SetDeinterleaving(HBAO.Deinterleaving deinterleaving)
		{
			this.deinterleaving.Override(deinterleaving);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000224C File Offset: 0x0000044C
		public HBAO.Resolution GetResolution()
		{
			return this.resolution.value;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002259 File Offset: 0x00000459
		public void SetResolution(HBAO.Resolution resolution)
		{
			this.resolution.Override(resolution);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002267 File Offset: 0x00000467
		public HBAO.NoiseType GetNoiseType()
		{
			return this.noiseType.value;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002274 File Offset: 0x00000474
		public void SetNoiseType(HBAO.NoiseType noiseType)
		{
			this.noiseType.Override(noiseType);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002282 File Offset: 0x00000482
		public HBAO.DebugMode GetDebugMode()
		{
			return this.debugMode.value;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000228F File Offset: 0x0000048F
		public void SetDebugMode(HBAO.DebugMode debugMode)
		{
			this.debugMode.Override(debugMode);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000229D File Offset: 0x0000049D
		public float GetAoRadius()
		{
			return this.radius.value;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022AA File Offset: 0x000004AA
		public void SetAoRadius(float radius)
		{
			this.radius.Override(Mathf.Clamp(radius, this.radius.min, this.radius.max));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022D3 File Offset: 0x000004D3
		public float GetAoMaxRadiusPixels()
		{
			return this.maxRadiusPixels.value;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000022E0 File Offset: 0x000004E0
		public void SetAoMaxRadiusPixels(float maxRadiusPixels)
		{
			this.maxRadiusPixels.Override(Mathf.Clamp(maxRadiusPixels, this.maxRadiusPixels.min, this.maxRadiusPixels.max));
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002309 File Offset: 0x00000509
		public float GetAoBias()
		{
			return this.bias.value;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002316 File Offset: 0x00000516
		public void SetAoBias(float bias)
		{
			this.bias.Override(Mathf.Clamp(bias, this.bias.min, this.bias.max));
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000233F File Offset: 0x0000053F
		public float GetAoOffscreenSamplesContribution()
		{
			return this.offscreenSamplesContribution.value;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000234C File Offset: 0x0000054C
		public void SetAoOffscreenSamplesContribution(float offscreenSamplesContribution)
		{
			this.offscreenSamplesContribution.Override(Mathf.Clamp(offscreenSamplesContribution, this.offscreenSamplesContribution.min, this.offscreenSamplesContribution.max));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002375 File Offset: 0x00000575
		public float GetAoMaxDistance()
		{
			return this.maxDistance.value;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002382 File Offset: 0x00000582
		public void SetAoMaxDistance(float maxDistance)
		{
			this.maxDistance.Override(maxDistance);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002390 File Offset: 0x00000590
		public float GetAoDistanceFalloff()
		{
			return this.distanceFalloff.value;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000239D File Offset: 0x0000059D
		public void SetAoDistanceFalloff(float distanceFalloff)
		{
			this.distanceFalloff.Override(distanceFalloff);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000023AB File Offset: 0x000005AB
		public HBAO.PerPixelNormals GetAoPerPixelNormals()
		{
			return this.perPixelNormals.value;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000023B8 File Offset: 0x000005B8
		public void SetAoPerPixelNormals(HBAO.PerPixelNormals perPixelNormals)
		{
			this.perPixelNormals.Override(perPixelNormals);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023C6 File Offset: 0x000005C6
		public Color GetAoColor()
		{
			return this.baseColor.value;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023D3 File Offset: 0x000005D3
		public void SetAoColor(Color baseColor)
		{
			this.baseColor.Override(baseColor);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023E1 File Offset: 0x000005E1
		public float GetAoIntensity()
		{
			return this.intensity.value;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023EE File Offset: 0x000005EE
		public void SetAoIntensity(float intensity)
		{
			this.intensity.Override(Mathf.Clamp(intensity, this.intensity.min, this.intensity.max));
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002417 File Offset: 0x00000617
		public bool UseMultiBounce()
		{
			return this.useMultiBounce.value;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002424 File Offset: 0x00000624
		public void EnableMultiBounce(bool enabled = true)
		{
			this.useMultiBounce.Override(enabled);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002432 File Offset: 0x00000632
		public float GetAoMultiBounceInfluence()
		{
			return this.multiBounceInfluence.value;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000243F File Offset: 0x0000063F
		public void SetAoMultiBounceInfluence(float multiBounceInfluence)
		{
			this.multiBounceInfluence.Override(Mathf.Clamp(multiBounceInfluence, this.multiBounceInfluence.min, this.multiBounceInfluence.max));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002468 File Offset: 0x00000668
		public bool IsTemporalFilterEnabled()
		{
			return this.temporalFilterEnabled.value;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002475 File Offset: 0x00000675
		public void EnableTemporalFilter(bool enabled = true)
		{
			this.temporalFilterEnabled.Override(enabled);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002483 File Offset: 0x00000683
		public HBAO.VarianceClipping GetTemporalFilterVarianceClipping()
		{
			return this.varianceClipping.value;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002490 File Offset: 0x00000690
		public void SetTemporalFilterVarianceClipping(HBAO.VarianceClipping varianceClipping)
		{
			this.varianceClipping.Override(varianceClipping);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000249E File Offset: 0x0000069E
		public HBAO.BlurType GetBlurType()
		{
			return this.blurType.value;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000024AB File Offset: 0x000006AB
		public void SetBlurType(HBAO.BlurType blurType)
		{
			this.blurType.Override(blurType);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000024B9 File Offset: 0x000006B9
		public float GetBlurSharpness()
		{
			return this.sharpness.value;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000024C6 File Offset: 0x000006C6
		public void SetBlurSharpness(float sharpness)
		{
			this.sharpness.Override(Mathf.Clamp(sharpness, this.sharpness.min, this.sharpness.max));
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000024EF File Offset: 0x000006EF
		public bool IsColorBleedingEnabled()
		{
			return this.colorBleedingEnabled.value;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000024FC File Offset: 0x000006FC
		public void EnableColorBleeding(bool enabled = true)
		{
			this.colorBleedingEnabled.Override(enabled);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000250A File Offset: 0x0000070A
		public float GetColorBleedingSaturation()
		{
			return this.saturation.value;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002517 File Offset: 0x00000717
		public void SetColorBleedingSaturation(float saturation)
		{
			this.saturation.Override(Mathf.Clamp(saturation, this.saturation.min, this.saturation.max));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002540 File Offset: 0x00000740
		public float GetColorBleedingBrightnessMask()
		{
			return this.brightnessMask.value;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000254D File Offset: 0x0000074D
		public void SetColorBleedingBrightnessMask(float brightnessMask)
		{
			this.brightnessMask.Override(Mathf.Clamp(brightnessMask, this.brightnessMask.min, this.brightnessMask.max));
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002576 File Offset: 0x00000776
		public Vector2 GetColorBleedingBrightnessMaskRange()
		{
			return this.brightnessMaskRange.value;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002584 File Offset: 0x00000784
		public void SetColorBleedingBrightnessMaskRange(Vector2 brightnessMaskRange)
		{
			brightnessMaskRange.x = Mathf.Clamp(brightnessMaskRange.x, this.brightnessMaskRange.min, this.brightnessMaskRange.max);
			brightnessMaskRange.y = Mathf.Clamp(brightnessMaskRange.y, this.brightnessMaskRange.min, this.brightnessMaskRange.max);
			brightnessMaskRange.x = Mathf.Min(brightnessMaskRange.x, brightnessMaskRange.y);
			this.brightnessMaskRange.Override(brightnessMaskRange);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002605 File Offset: 0x00000805
		public bool IsActive()
		{
			return this.intensity.overrideState && this.intensity.value > 0f;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002628 File Offset: 0x00000828
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x04000001 RID: 1
		[HBAO.Presets]
		public HBAO.PresetParameter preset = new HBAO.PresetParameter(HBAO.Preset.Normal, false);

		// Token: 0x04000002 RID: 2
		[Tooltip("The mode of the AO.")]
		[HBAO.GeneralSettings]
		[Space(6f)]
		public HBAO.ModeParameter mode = new HBAO.ModeParameter(HBAO.Mode.LitAO, false);

		// Token: 0x04000003 RID: 3
		[Tooltip("The rendering path used for AO. Temporary settings as for now rendering path is internal to renderer settings.")]
		[HBAO.GeneralSettings]
		[Space(6f)]
		public HBAO.RenderingPathParameter renderingPath = new HBAO.RenderingPathParameter(HBAO.RenderingPath.Forward, false);

		// Token: 0x04000004 RID: 4
		[Tooltip("The quality of the AO.")]
		[HBAO.GeneralSettings]
		[Space(6f)]
		public HBAO.QualityParameter quality = new HBAO.QualityParameter(HBAO.Quality.Medium, false);

		// Token: 0x04000005 RID: 5
		[Tooltip("The deinterleaving factor.")]
		[HBAO.GeneralSettings]
		public HBAO.DeinterleavingParameter deinterleaving = new HBAO.DeinterleavingParameter(HBAO.Deinterleaving.Disabled, false);

		// Token: 0x04000006 RID: 6
		[Tooltip("The resolution at which the AO is calculated.")]
		[HBAO.GeneralSettings]
		public HBAO.ResolutionParameter resolution = new HBAO.ResolutionParameter(HBAO.Resolution.Full, false);

		// Token: 0x04000007 RID: 7
		[Tooltip("The type of noise to use.")]
		[HBAO.GeneralSettings]
		[Space(10f)]
		public HBAO.NoiseTypeParameter noiseType = new HBAO.NoiseTypeParameter(HBAO.NoiseType.Dither, false);

		// Token: 0x04000008 RID: 8
		[Tooltip("The debug mode actually displayed on screen.")]
		[HBAO.GeneralSettings]
		[Space(10f)]
		public HBAO.DebugModeParameter debugMode = new HBAO.DebugModeParameter(HBAO.DebugMode.Disabled, false);

		// Token: 0x04000009 RID: 9
		[Tooltip("AO radius: this is the distance outside which occluders are ignored.")]
		[HBAO.AOSettings]
		[Space(6f)]
		public ClampedFloatParameter radius = new ClampedFloatParameter(0.8f, 0.25f, 5f, false);

		// Token: 0x0400000A RID: 10
		[Tooltip("Maximum radius in pixels: this prevents the radius to grow too much with close-up object and impact on performances.")]
		[HBAO.AOSettings]
		public ClampedFloatParameter maxRadiusPixels = new ClampedFloatParameter(128f, 16f, 256f, false);

		// Token: 0x0400000B RID: 11
		[Tooltip("For low-tessellated geometry, occlusion variations tend to appear at creases and ridges, which betray the underlying tessellation. To remove these artifacts, we use an angle bias parameter which restricts the hemisphere.")]
		[HBAO.AOSettings]
		public ClampedFloatParameter bias = new ClampedFloatParameter(0.05f, 0f, 0.5f, false);

		// Token: 0x0400000C RID: 12
		[Tooltip("This value allows to scale up the ambient occlusion values.")]
		[HBAO.AOSettings]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 4f, false);

		// Token: 0x0400000D RID: 13
		[Tooltip("Enable/disable MultiBounce approximation.")]
		[HBAO.AOSettings]
		public BoolParameter useMultiBounce = new BoolParameter(false, false);

		// Token: 0x0400000E RID: 14
		[Tooltip("MultiBounce approximation influence.")]
		[HBAO.AOSettings]
		public ClampedFloatParameter multiBounceInfluence = new ClampedFloatParameter(1f, 0f, 1f, false);

		// Token: 0x0400000F RID: 15
		[Tooltip("How much AO affect direct lighting.")]
		[HBAO.AOSettings]
		public ClampedFloatParameter directLightingStrength = new ClampedFloatParameter(0.25f, 0f, 1f, false);

		// Token: 0x04000010 RID: 16
		[Tooltip("The amount of AO offscreen samples are contributing.")]
		[HBAO.AOSettings]
		public ClampedFloatParameter offscreenSamplesContribution = new ClampedFloatParameter(0f, 0f, 1f, false);

		// Token: 0x04000011 RID: 17
		[Tooltip("The max distance to display AO.")]
		[HBAO.AOSettings]
		[Space(10f)]
		public FloatParameter maxDistance = new FloatParameter(150f, false);

		// Token: 0x04000012 RID: 18
		[Tooltip("The distance before max distance at which AO start to decrease.")]
		[HBAO.AOSettings]
		public FloatParameter distanceFalloff = new FloatParameter(50f, false);

		// Token: 0x04000013 RID: 19
		[Tooltip("The type of per pixel normals to use.")]
		[HBAO.AOSettings]
		[Space(10f)]
		public HBAO.PerPixelNormalsParameter perPixelNormals = new HBAO.PerPixelNormalsParameter(HBAO.PerPixelNormals.Camera, false);

		// Token: 0x04000014 RID: 20
		[Tooltip("This setting allow you to set the base color if the AO, the alpha channel value is unused.")]
		[HBAO.AOSettings]
		[Space(10f)]
		public ColorParameter baseColor = new ColorParameter(Color.black, false);

		// Token: 0x04000015 RID: 21
		[HBAO.TemporalFilterSettings]
		[HBAO.ParameterDisplayName("Enabled")]
		[Space(6f)]
		public BoolParameter temporalFilterEnabled = new BoolParameter(false, false);

		// Token: 0x04000016 RID: 22
		[Tooltip("The type of variance clipping to use.")]
		[HBAO.TemporalFilterSettings]
		public HBAO.VarianceClippingParameter varianceClipping = new HBAO.VarianceClippingParameter(HBAO.VarianceClipping._4Tap, false);

		// Token: 0x04000017 RID: 23
		[Tooltip("The type of blur to use.")]
		[HBAO.BlurSettings]
		[HBAO.ParameterDisplayName("Type")]
		[Space(6f)]
		public HBAO.BlurTypeParameter blurType = new HBAO.BlurTypeParameter(HBAO.BlurType.Medium, false);

		// Token: 0x04000018 RID: 24
		[Tooltip("This parameter controls the depth-dependent weight of the bilateral filter, to avoid bleeding across edges. A zero sharpness is a pure Gaussian blur. Increasing the blur sharpness removes bleeding by using lower weights for samples with large depth delta from the current pixel.")]
		[HBAO.BlurSettings]
		[Space(10f)]
		public ClampedFloatParameter sharpness = new ClampedFloatParameter(8f, 0f, 16f, false);

		// Token: 0x04000019 RID: 25
		[HBAO.ColorBleedingSettings]
		[HBAO.ParameterDisplayName("Enabled")]
		[Space(6f)]
		public BoolParameter colorBleedingEnabled = new BoolParameter(false, false);

		// Token: 0x0400001A RID: 26
		[Tooltip("This value allows to control the saturation of the color bleeding.")]
		[HBAO.ColorBleedingSettings]
		[Space(10f)]
		public ClampedFloatParameter saturation = new ClampedFloatParameter(1f, 0f, 4f, false);

		// Token: 0x0400001B RID: 27
		[Tooltip("Use masking on emissive pixels")]
		[HBAO.ColorBleedingSettings]
		public ClampedFloatParameter brightnessMask = new ClampedFloatParameter(1f, 0f, 1f, false);

		// Token: 0x0400001C RID: 28
		[Tooltip("Brightness level where masking starts/ends")]
		[HBAO.ColorBleedingSettings]
		public HBAO.MinMaxFloatParameter brightnessMaskRange = new HBAO.MinMaxFloatParameter(new Vector2(0f, 0.5f), 0f, 2f, false);

		// Token: 0x02000007 RID: 7
		public enum Preset
		{
			// Token: 0x0400002A RID: 42
			FastestPerformance,
			// Token: 0x0400002B RID: 43
			FastPerformance,
			// Token: 0x0400002C RID: 44
			Normal,
			// Token: 0x0400002D RID: 45
			HighQuality,
			// Token: 0x0400002E RID: 46
			HighestQuality,
			// Token: 0x0400002F RID: 47
			Custom
		}

		// Token: 0x02000008 RID: 8
		public enum Mode
		{
			// Token: 0x04000031 RID: 49
			Normal,
			// Token: 0x04000032 RID: 50
			LitAO
		}

		// Token: 0x02000009 RID: 9
		public enum RenderingPath
		{
			// Token: 0x04000034 RID: 52
			Forward,
			// Token: 0x04000035 RID: 53
			Deferred
		}

		// Token: 0x0200000A RID: 10
		public enum Quality
		{
			// Token: 0x04000037 RID: 55
			Lowest,
			// Token: 0x04000038 RID: 56
			Low,
			// Token: 0x04000039 RID: 57
			Medium,
			// Token: 0x0400003A RID: 58
			High,
			// Token: 0x0400003B RID: 59
			Highest
		}

		// Token: 0x0200000B RID: 11
		public enum Resolution
		{
			// Token: 0x0400003D RID: 61
			Full,
			// Token: 0x0400003E RID: 62
			Half
		}

		// Token: 0x0200000C RID: 12
		public enum NoiseType
		{
			// Token: 0x04000040 RID: 64
			Dither,
			// Token: 0x04000041 RID: 65
			InterleavedGradientNoise,
			// Token: 0x04000042 RID: 66
			SpatialDistribution
		}

		// Token: 0x0200000D RID: 13
		public enum Deinterleaving
		{
			// Token: 0x04000044 RID: 68
			Disabled,
			// Token: 0x04000045 RID: 69
			x4
		}

		// Token: 0x0200000E RID: 14
		public enum DebugMode
		{
			// Token: 0x04000047 RID: 71
			Disabled,
			// Token: 0x04000048 RID: 72
			AOOnly,
			// Token: 0x04000049 RID: 73
			ColorBleedingOnly,
			// Token: 0x0400004A RID: 74
			SplitWithoutAOAndWithAO,
			// Token: 0x0400004B RID: 75
			SplitWithAOAndAOOnly,
			// Token: 0x0400004C RID: 76
			SplitWithoutAOAndAOOnly,
			// Token: 0x0400004D RID: 77
			ViewNormals
		}

		// Token: 0x0200000F RID: 15
		public enum BlurType
		{
			// Token: 0x0400004F RID: 79
			None,
			// Token: 0x04000050 RID: 80
			Narrow,
			// Token: 0x04000051 RID: 81
			Medium,
			// Token: 0x04000052 RID: 82
			Wide,
			// Token: 0x04000053 RID: 83
			ExtraWide
		}

		// Token: 0x02000010 RID: 16
		public enum PerPixelNormals
		{
			// Token: 0x04000055 RID: 85
			Reconstruct2Samples,
			// Token: 0x04000056 RID: 86
			Reconstruct4Samples,
			// Token: 0x04000057 RID: 87
			Camera
		}

		// Token: 0x02000011 RID: 17
		public enum VarianceClipping
		{
			// Token: 0x04000059 RID: 89
			Disabled,
			// Token: 0x0400005A RID: 90
			_4Tap,
			// Token: 0x0400005B RID: 91
			_8Tap
		}

		// Token: 0x02000012 RID: 18
		[Serializable]
		public sealed class PresetParameter : VolumeParameter<HBAO.Preset>
		{
			// Token: 0x06000042 RID: 66 RVA: 0x0000294B File Offset: 0x00000B4B
			public PresetParameter(HBAO.Preset value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x02000013 RID: 19
		[Serializable]
		public sealed class ModeParameter : VolumeParameter<HBAO.Mode>
		{
			// Token: 0x06000043 RID: 67 RVA: 0x00002955 File Offset: 0x00000B55
			public ModeParameter(HBAO.Mode value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x02000014 RID: 20
		[Serializable]
		public sealed class RenderingPathParameter : VolumeParameter<HBAO.RenderingPath>
		{
			// Token: 0x06000044 RID: 68 RVA: 0x0000295F File Offset: 0x00000B5F
			public RenderingPathParameter(HBAO.RenderingPath value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x02000015 RID: 21
		[Serializable]
		public sealed class QualityParameter : VolumeParameter<HBAO.Quality>
		{
			// Token: 0x06000045 RID: 69 RVA: 0x00002969 File Offset: 0x00000B69
			public QualityParameter(HBAO.Quality value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x02000016 RID: 22
		[Serializable]
		public sealed class DeinterleavingParameter : VolumeParameter<HBAO.Deinterleaving>
		{
			// Token: 0x06000046 RID: 70 RVA: 0x00002973 File Offset: 0x00000B73
			public DeinterleavingParameter(HBAO.Deinterleaving value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x02000017 RID: 23
		[Serializable]
		public sealed class ResolutionParameter : VolumeParameter<HBAO.Resolution>
		{
			// Token: 0x06000047 RID: 71 RVA: 0x0000297D File Offset: 0x00000B7D
			public ResolutionParameter(HBAO.Resolution value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x02000018 RID: 24
		[Serializable]
		public sealed class NoiseTypeParameter : VolumeParameter<HBAO.NoiseType>
		{
			// Token: 0x06000048 RID: 72 RVA: 0x00002987 File Offset: 0x00000B87
			public NoiseTypeParameter(HBAO.NoiseType value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x02000019 RID: 25
		[Serializable]
		public sealed class DebugModeParameter : VolumeParameter<HBAO.DebugMode>
		{
			// Token: 0x06000049 RID: 73 RVA: 0x00002991 File Offset: 0x00000B91
			public DebugModeParameter(HBAO.DebugMode value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x0200001A RID: 26
		[Serializable]
		public sealed class PerPixelNormalsParameter : VolumeParameter<HBAO.PerPixelNormals>
		{
			// Token: 0x0600004A RID: 74 RVA: 0x0000299B File Offset: 0x00000B9B
			public PerPixelNormalsParameter(HBAO.PerPixelNormals value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x0200001B RID: 27
		[Serializable]
		public sealed class VarianceClippingParameter : VolumeParameter<HBAO.VarianceClipping>
		{
			// Token: 0x0600004B RID: 75 RVA: 0x000029A5 File Offset: 0x00000BA5
			public VarianceClippingParameter(HBAO.VarianceClipping value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x0200001C RID: 28
		[Serializable]
		public sealed class BlurTypeParameter : VolumeParameter<HBAO.BlurType>
		{
			// Token: 0x0600004C RID: 76 RVA: 0x000029AF File Offset: 0x00000BAF
			public BlurTypeParameter(HBAO.BlurType value, bool overrideState = false) : base(value, overrideState)
			{
			}
		}

		// Token: 0x0200001D RID: 29
		[Serializable]
		public sealed class MinMaxFloatParameter : VolumeParameter<Vector2>
		{
			// Token: 0x0600004D RID: 77 RVA: 0x000029B9 File Offset: 0x00000BB9
			public MinMaxFloatParameter(Vector2 value, float min, float max, bool overrideState = false) : base(value, overrideState)
			{
				this.min = min;
				this.max = max;
			}

			// Token: 0x0400005C RID: 92
			public float min;

			// Token: 0x0400005D RID: 93
			public float max;
		}

		// Token: 0x0200001E RID: 30
		[AttributeUsage(AttributeTargets.Field)]
		public class SettingsGroup : Attribute
		{
			// Token: 0x0400005E RID: 94
			public bool isExpanded = true;
		}

		// Token: 0x0200001F RID: 31
		[AttributeUsage(AttributeTargets.Field)]
		public class ParameterDisplayName : Attribute
		{
			// Token: 0x0600004F RID: 79 RVA: 0x000029E1 File Offset: 0x00000BE1
			public ParameterDisplayName(string name)
			{
				this.name = name;
			}

			// Token: 0x0400005F RID: 95
			public string name;
		}

		// Token: 0x02000020 RID: 32
		public class Presets : HBAO.SettingsGroup
		{
		}

		// Token: 0x02000021 RID: 33
		public class GeneralSettings : HBAO.SettingsGroup
		{
		}

		// Token: 0x02000022 RID: 34
		public class AOSettings : HBAO.SettingsGroup
		{
		}

		// Token: 0x02000023 RID: 35
		public class TemporalFilterSettings : HBAO.SettingsGroup
		{
		}

		// Token: 0x02000024 RID: 36
		public class BlurSettings : HBAO.SettingsGroup
		{
		}

		// Token: 0x02000025 RID: 37
		public class ColorBleedingSettings : HBAO.SettingsGroup
		{
		}
	}
}
