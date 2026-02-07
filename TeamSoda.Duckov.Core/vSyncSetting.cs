using System;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class vSyncSetting : OptionsProviderBase
{
	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0003BD7F File Offset: 0x00039F7F
	public override string Key
	{
		get
		{
			return "GSyncSetting";
		}
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0003BD86 File Offset: 0x00039F86
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.onKey),
			LocalizationManager.ToPlainText(this.offKey)
		};
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x0003BDAC File Offset: 0x00039FAC
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			this.SyncObjectActive(true);
			return LocalizationManager.ToPlainText(this.onKey);
		}
		if (num != 1)
		{
			return LocalizationManager.ToPlainText(this.offKey);
		}
		this.SyncObjectActive(false);
		return LocalizationManager.ToPlainText(this.offKey);
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0003BE00 File Offset: 0x0003A000
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				QualitySettings.vSyncCount = 0;
				this.SyncObjectActive(false);
			}
		}
		else
		{
			QualitySettings.vSyncCount = 1;
			this.SyncObjectActive(true);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x0003BE33 File Offset: 0x0003A033
	private void SyncObjectActive(bool active)
	{
		if (this.setActiveIfOn)
		{
			this.setActiveIfOn.SetActive(active);
		}
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x0003BE4E File Offset: 0x0003A04E
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x0003BE61 File Offset: 0x0003A061
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0003BE74 File Offset: 0x0003A074
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000C36 RID: 3126
	[LocalizationKey("Default")]
	public string onKey = "gSync_On";

	// Token: 0x04000C37 RID: 3127
	[LocalizationKey("Default")]
	public string offKey = "gSync_Off";

	// Token: 0x04000C38 RID: 3128
	public GameObject setActiveIfOn;
}
