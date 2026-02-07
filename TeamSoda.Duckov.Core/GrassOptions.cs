using System;
using Duckov.Options;
using SodaCraft.Localizations;
using SymmetryBreakStudio.TastyGrassShader;
using UnityEngine.Rendering.Universal;

// Token: 0x020001DC RID: 476
public class GrassOptions : OptionsProviderBase
{
	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0003B19C File Offset: 0x0003939C
	public override string Key
	{
		get
		{
			return "GrassSettings";
		}
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0003B1A3 File Offset: 0x000393A3
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.offKey),
			LocalizationManager.ToPlainText(this.onKey)
		};
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0003B1C8 File Offset: 0x000393C8
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return LocalizationManager.ToPlainText(this.offKey);
		}
		if (num != 1)
		{
			return LocalizationManager.ToPlainText(this.offKey);
		}
		return LocalizationManager.ToPlainText(this.onKey);
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x0003B20E File Offset: 0x0003940E
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x0003B221 File Offset: 0x00039421
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x0003B234 File Offset: 0x00039434
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x0003B258 File Offset: 0x00039458
	public override void Set(int index)
	{
		ScriptableRendererFeature scriptableRendererFeature = this.rendererData.rendererFeatures.Find((ScriptableRendererFeature e) => e is TastyGrassShaderGlobalSettings);
		if (scriptableRendererFeature != null)
		{
			TastyGrassShaderGlobalSettings tastyGrassShaderGlobalSettings = scriptableRendererFeature as TastyGrassShaderGlobalSettings;
			if (index != 0)
			{
				if (index == 1)
				{
					tastyGrassShaderGlobalSettings.SetActive(true);
					TgsManager.Enable = true;
				}
			}
			else
			{
				tastyGrassShaderGlobalSettings.SetActive(false);
				TgsManager.Enable = false;
			}
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x04000C1A RID: 3098
	[LocalizationKey("Default")]
	public string offKey = "GrassOptions_Off";

	// Token: 0x04000C1B RID: 3099
	[LocalizationKey("Default")]
	public string onKey = "GrassOptions_On";

	// Token: 0x04000C1C RID: 3100
	public UniversalRendererData rendererData;
}
