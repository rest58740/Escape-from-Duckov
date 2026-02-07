using System;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class FrameRateSetting : OptionsProviderBase
{
	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0003ADA3 File Offset: 0x00038FA3
	public override string Key
	{
		get
		{
			return "FrameRateSetting";
		}
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x0003ADAA File Offset: 0x00038FAA
	public override string[] GetOptions()
	{
		return new string[]
		{
			"60",
			"90",
			"120",
			"144",
			"240",
			LocalizationManager.ToPlainText(this.optionUnlimitKey)
		};
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x0003ADE8 File Offset: 0x00038FE8
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 1))
		{
		case 0:
			return "60";
		case 1:
			return "90";
		case 2:
			return "120";
		case 3:
			return "144";
		case 4:
			return "240";
		case 5:
			return LocalizationManager.ToPlainText(this.optionUnlimitKey);
		default:
			return "60";
		}
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x0003AE54 File Offset: 0x00039054
	public override void Set(int index)
	{
		switch (index)
		{
		case 0:
			Application.targetFrameRate = 60;
			break;
		case 1:
			Application.targetFrameRate = 90;
			break;
		case 2:
			Application.targetFrameRate = 120;
			break;
		case 3:
			Application.targetFrameRate = 144;
			break;
		case 4:
			Application.targetFrameRate = 240;
			break;
		case 5:
			Application.targetFrameRate = 500;
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x0003AECA File Offset: 0x000390CA
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x0003AEDD File Offset: 0x000390DD
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x0003AEF0 File Offset: 0x000390F0
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000C13 RID: 3091
	[LocalizationKey("Default")]
	public string optionUnlimitKey = "FrameRateUnlimit";
}
