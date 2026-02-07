using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001DE RID: 478
public class HurtVisualSettings : OptionsProviderBase
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0003B4E5 File Offset: 0x000396E5
	public override string Key
	{
		get
		{
			return "HurtVisualSettings";
		}
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x0003B4EC File Offset: 0x000396EC
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.offKey),
			LocalizationManager.ToPlainText(this.onKey)
		};
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x0003B510 File Offset: 0x00039710
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

	// Token: 0x06000E68 RID: 3688 RVA: 0x0003B556 File Offset: 0x00039756
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				PlayerHurtVisual.hurtVisualOn = true;
			}
		}
		else
		{
			PlayerHurtVisual.hurtVisualOn = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x0003B57B File Offset: 0x0003977B
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x0003B58E File Offset: 0x0003978E
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x0003B5A4 File Offset: 0x000397A4
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000C22 RID: 3106
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000C23 RID: 3107
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
