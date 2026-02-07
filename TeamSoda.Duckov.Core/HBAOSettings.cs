using System;
using Duckov.Options;
using HorizonBasedAmbientOcclusion.Universal;
using SodaCraft.Localizations;
using UnityEngine.Rendering;

// Token: 0x020001DD RID: 477
public class HBAOSettings : OptionsProviderBase
{
	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06000E5D RID: 3677 RVA: 0x0003B2F4 File Offset: 0x000394F4
	public override string Key
	{
		get
		{
			return "HBAOSettings";
		}
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x0003B2FB File Offset: 0x000394FB
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.offKey),
			LocalizationManager.ToPlainText(this.lowKey),
			LocalizationManager.ToPlainText(this.normalKey),
			LocalizationManager.ToPlainText(this.highKey)
		};
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x0003B33C File Offset: 0x0003953C
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 2))
		{
		case 0:
			return LocalizationManager.ToPlainText(this.offKey);
		case 1:
			return LocalizationManager.ToPlainText(this.lowKey);
		case 2:
			return LocalizationManager.ToPlainText(this.normalKey);
		case 3:
			return LocalizationManager.ToPlainText(this.highKey);
		default:
			return LocalizationManager.ToPlainText(this.offKey);
		}
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x0003B3AC File Offset: 0x000395AC
	public override void Set(int index)
	{
		HBAO hbao;
		if (this.GlobalVolumePorfile.TryGet<HBAO>(ref hbao))
		{
			switch (index)
			{
			case 0:
				hbao.EnableHBAO(false);
				break;
			case 1:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(1, false);
				hbao.bias.value = 64f;
				break;
			case 2:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(1, false);
				hbao.bias.value = 128f;
				break;
			case 3:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(0, false);
				hbao.bias.value = 128f;
				break;
			}
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x0003B468 File Offset: 0x00039668
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0003B47B File Offset: 0x0003967B
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x0003B490 File Offset: 0x00039690
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000C1D RID: 3101
	[LocalizationKey("Default")]
	public string offKey = "HBAOSettings_Off";

	// Token: 0x04000C1E RID: 3102
	[LocalizationKey("Default")]
	public string lowKey = "HBAOSettings_Low";

	// Token: 0x04000C1F RID: 3103
	[LocalizationKey("Default")]
	public string normalKey = "HBAOSettings_Normal";

	// Token: 0x04000C20 RID: 3104
	[LocalizationKey("Default")]
	public string highKey = "HBAOSettings_High";

	// Token: 0x04000C21 RID: 3105
	public VolumeProfile GlobalVolumePorfile;
}
