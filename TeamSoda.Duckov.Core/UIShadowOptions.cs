using System;
using Duckov.Options;
using LeTai.TrueShadow;
using SodaCraft.Localizations;

// Token: 0x020001E5 RID: 485
public class UIShadowOptions : OptionsProviderBase
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0003BCEF File Offset: 0x00039EEF
	public override string Key
	{
		get
		{
			return "UIShadow";
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0003BCF6 File Offset: 0x00039EF6
	// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x0003BD03 File Offset: 0x00039F03
	public static bool Active
	{
		get
		{
			return OptionsManager.Load<bool>("UIShadow", true);
		}
		set
		{
			OptionsManager.Save<bool>("UIShadow", value);
		}
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0003BD10 File Offset: 0x00039F10
	public static void Apply()
	{
		TrueShadow.ExternalActive = UIShadowOptions.Active;
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x0003BD1C File Offset: 0x00039F1C
	public string ActiveText
	{
		get
		{
			return LocalizationManager.ToPlainText("Options_On");
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0003BD28 File Offset: 0x00039F28
	public string InactiveText
	{
		get
		{
			return LocalizationManager.ToPlainText("Options_Off");
		}
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x0003BD34 File Offset: 0x00039F34
	public override string GetCurrentOption()
	{
		if (UIShadowOptions.Active)
		{
			return this.ActiveText;
		}
		return this.InactiveText;
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x0003BD4A File Offset: 0x00039F4A
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.InactiveText,
			this.ActiveText
		};
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x0003BD64 File Offset: 0x00039F64
	public override void Set(int index)
	{
		if (index <= 0)
		{
			UIShadowOptions.Active = false;
			return;
		}
		UIShadowOptions.Active = true;
	}

	// Token: 0x04000C35 RID: 3125
	private const string key = "UIShadow";
}
