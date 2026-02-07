using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001E4 RID: 484
public class SunFogSettings : OptionsProviderBase
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0003BBEE File Offset: 0x00039DEE
	public override string Key
	{
		get
		{
			return "SunFogSetting";
		}
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x0003BBF5 File Offset: 0x00039DF5
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.offKey),
			LocalizationManager.ToPlainText(this.onKey)
		};
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x0003BC1C File Offset: 0x00039E1C
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return LocalizationManager.ToPlainText(this.offKey);
		}
		if (num != 1)
		{
			return LocalizationManager.ToPlainText(this.onKey);
		}
		return LocalizationManager.ToPlainText(this.onKey);
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x0003BC62 File Offset: 0x00039E62
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				SunFogEntry.SetEnabled(true);
			}
		}
		else
		{
			SunFogEntry.SetEnabled(false);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x0003BC87 File Offset: 0x00039E87
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x0003BC9A File Offset: 0x00039E9A
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x0003BCB0 File Offset: 0x00039EB0
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000C33 RID: 3123
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000C34 RID: 3124
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
