using System;
using Duckov.Options;
using SodaCraft.Localizations;
using Umbra;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001E1 RID: 481
public class ShadowSetting : OptionsProviderBase
{
	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0003B7EB File Offset: 0x000399EB
	public override string Key
	{
		get
		{
			return "ShadowSettings";
		}
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x0003B7F2 File Offset: 0x000399F2
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.offKey),
			LocalizationManager.ToPlainText(this.lowKey),
			LocalizationManager.ToPlainText(this.middleKey),
			LocalizationManager.ToPlainText(this.highKey)
		};
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0003B834 File Offset: 0x00039A34
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 2))
		{
		case 0:
			return LocalizationManager.ToPlainText(this.offKey);
		case 1:
			return LocalizationManager.ToPlainText(this.lowKey);
		case 2:
			return LocalizationManager.ToPlainText(this.middleKey);
		case 3:
			return LocalizationManager.ToPlainText(this.highKey);
		default:
			return LocalizationManager.ToPlainText(this.highKey);
		}
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x0003B8A4 File Offset: 0x00039AA4
	private void SetShadow(bool on, int res, float shadowDistance, bool softShadow, bool softShadowDownSample, bool contactShadow, int pointLightCount)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
		if (universalRenderPipelineAsset != null)
		{
			universalRenderPipelineAsset.shadowDistance = (on ? shadowDistance : 0f);
			universalRenderPipelineAsset.mainLightShadowmapResolution = res;
			universalRenderPipelineAsset.additionalLightsShadowmapResolution = res;
			universalRenderPipelineAsset.maxAdditionalLightsCount = pointLightCount;
		}
		if (this.umbraProfile)
		{
			this.umbraProfile.shadowSource = (softShadow ? 1 : 0);
			this.umbraProfile.downsample = softShadowDownSample;
			this.umbraProfile.contactShadows = contactShadow;
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x0003B928 File Offset: 0x00039B28
	public override void Set(int index)
	{
		switch (index)
		{
		case 0:
			this.SetShadow(false, 512, 0f, false, false, false, 0);
			break;
		case 1:
			this.SetShadow(true, 1024, 70f, false, false, false, 0);
			break;
		case 2:
			this.SetShadow(true, 2048, 80f, true, true, true, 5);
			break;
		case 3:
			this.SetShadow(true, 4096, 90f, true, false, true, 6);
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x0003B9B3 File Offset: 0x00039BB3
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x0003B9C6 File Offset: 0x00039BC6
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x0003B9DC File Offset: 0x00039BDC
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 2);
		this.Set(index);
	}

	// Token: 0x04000C29 RID: 3113
	public UmbraProfile umbraProfile;

	// Token: 0x04000C2A RID: 3114
	public float onDistance = 100f;

	// Token: 0x04000C2B RID: 3115
	[LocalizationKey("Default")]
	public string highKey = "Options_High";

	// Token: 0x04000C2C RID: 3116
	[LocalizationKey("Default")]
	public string middleKey = "Options_Middle";

	// Token: 0x04000C2D RID: 3117
	[LocalizationKey("Default")]
	public string lowKey = "Options_Low";

	// Token: 0x04000C2E RID: 3118
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
