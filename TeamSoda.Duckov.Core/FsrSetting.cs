using System;
using Duckov.MiniGames;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001DB RID: 475
public class FsrSetting : OptionsProviderBase
{
	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0003AF24 File Offset: 0x00039124
	public override string Key
	{
		get
		{
			return "FsrSetting";
		}
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x0003AF2C File Offset: 0x0003912C
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.offKey),
			LocalizationManager.ToPlainText(this.qualityKey),
			LocalizationManager.ToPlainText(this.balancedKey),
			LocalizationManager.ToPlainText(this.performanceKey),
			LocalizationManager.ToPlainText(this.ultraPerformanceKey)
		};
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x0003AF88 File Offset: 0x00039188
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 0))
		{
		case 0:
			return LocalizationManager.ToPlainText(this.offKey);
		case 1:
			return LocalizationManager.ToPlainText(this.qualityKey);
		case 2:
			return LocalizationManager.ToPlainText(this.balancedKey);
		case 3:
			return LocalizationManager.ToPlainText(this.performanceKey);
		case 4:
			return LocalizationManager.ToPlainText(this.ultraPerformanceKey);
		default:
			return LocalizationManager.ToPlainText(this.offKey);
		}
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x0003B008 File Offset: 0x00039208
	public override void Set(int index)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
		int num = index;
		if (FsrSetting.gameOn)
		{
			num = 0;
		}
		switch (num)
		{
		case 0:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 1f;
				universalRenderPipelineAsset.upscalingFilter = 1;
			}
			break;
		case 1:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.67f;
				universalRenderPipelineAsset.upscalingFilter = 3;
			}
			break;
		case 2:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.58f;
				universalRenderPipelineAsset.upscalingFilter = 3;
			}
			break;
		case 3:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.5f;
				universalRenderPipelineAsset.upscalingFilter = 3;
			}
			break;
		case 4:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.33f;
				universalRenderPipelineAsset.upscalingFilter = 3;
			}
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x0003B0E8 File Offset: 0x000392E8
	private void Awake()
	{
		this.RefreshOnLevelInited();
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
		GamingConsole.OnGamingConsoleInteractChanged += this.OnGamingConsoleInteractChanged;
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x0003B112 File Offset: 0x00039312
	private void OnGamingConsoleInteractChanged(bool _gameOn)
	{
		FsrSetting.gameOn = _gameOn;
		this.SyncSetting();
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0003B120 File Offset: 0x00039320
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x0003B134 File Offset: 0x00039334
	private void SyncSetting()
	{
		int index = OptionsManager.Load<int>(this.Key, 0);
		this.Set(index);
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x0003B155 File Offset: 0x00039355
	private void RefreshOnLevelInited()
	{
		this.SyncSetting();
	}

	// Token: 0x04000C14 RID: 3092
	[LocalizationKey("Default")]
	public string offKey = "fsr_Off";

	// Token: 0x04000C15 RID: 3093
	[LocalizationKey("Default")]
	public string qualityKey = "fsr_Quality";

	// Token: 0x04000C16 RID: 3094
	[LocalizationKey("Default")]
	public string balancedKey = "fsr_Balanced";

	// Token: 0x04000C17 RID: 3095
	[LocalizationKey("Default")]
	public string performanceKey = "fsr_Performance";

	// Token: 0x04000C18 RID: 3096
	[LocalizationKey("Default")]
	public string ultraPerformanceKey = "fsr_UltraPerformance";

	// Token: 0x04000C19 RID: 3097
	private static bool gameOn;
}
