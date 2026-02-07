using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D9 RID: 473
public class EdgeLightSettings : OptionsProviderBase
{
	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000E3B RID: 3643 RVA: 0x0003ACA2 File Offset: 0x00038EA2
	public override string Key
	{
		get
		{
			return "EdgeLightSetting";
		}
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x0003ACA9 File Offset: 0x00038EA9
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.offKey),
			LocalizationManager.ToPlainText(this.onKey)
		};
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0003ACD0 File Offset: 0x00038ED0
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

	// Token: 0x06000E3E RID: 3646 RVA: 0x0003AD16 File Offset: 0x00038F16
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				EdgeLightEntry.SetEnabled(true);
			}
		}
		else
		{
			EdgeLightEntry.SetEnabled(false);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x0003AD3B File Offset: 0x00038F3B
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x0003AD4E File Offset: 0x00038F4E
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x0003AD64 File Offset: 0x00038F64
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000C11 RID: 3089
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000C12 RID: 3090
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
