using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001E2 RID: 482
public class SoftShadowOptions : OptionsProviderBase
{
	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0003BA3C File Offset: 0x00039C3C
	public override string Key
	{
		get
		{
			return "SoftShadowSettings";
		}
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0003BA43 File Offset: 0x00039C43
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.offKey),
			LocalizationManager.ToPlainText(this.onKey)
		};
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0003BA68 File Offset: 0x00039C68
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

	// Token: 0x06000E8A RID: 3722 RVA: 0x0003BAAE File Offset: 0x00039CAE
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0003BAC1 File Offset: 0x00039CC1
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0003BAD4 File Offset: 0x00039CD4
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0003BAF5 File Offset: 0x00039CF5
	public override void Set(int index)
	{
	}

	// Token: 0x04000C2F RID: 3119
	[LocalizationKey("Default")]
	public string offKey = "SoftShadowOptions_Off";

	// Token: 0x04000C30 RID: 3120
	[LocalizationKey("Default")]
	public string onKey = "SoftShadowOptions_On";
}
