using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D7 RID: 471
public class DisableCameraOffset : OptionsProviderBase
{
	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000E2B RID: 3627 RVA: 0x0003AACD File Offset: 0x00038CCD
	public override string Key
	{
		get
		{
			return "DisableCameraOffset";
		}
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x0003AAD4 File Offset: 0x00038CD4
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.onKey),
			LocalizationManager.ToPlainText(this.offKey)
		};
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x0003AAF8 File Offset: 0x00038CF8
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return LocalizationManager.ToPlainText(this.onKey);
		}
		if (num != 1)
		{
			return LocalizationManager.ToPlainText(this.offKey);
		}
		return LocalizationManager.ToPlainText(this.offKey);
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x0003AB3E File Offset: 0x00038D3E
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				DisableCameraOffset.disableCameraOffset = false;
			}
		}
		else
		{
			DisableCameraOffset.disableCameraOffset = true;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x0003AB63 File Offset: 0x00038D63
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0003AB76 File Offset: 0x00038D76
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0003AB8C File Offset: 0x00038D8C
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000C0C RID: 3084
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000C0D RID: 3085
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";

	// Token: 0x04000C0E RID: 3086
	public static bool disableCameraOffset;
}
