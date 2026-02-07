using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001E0 RID: 480
public class RunInputOptions : OptionsProviderBase
{
	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0003B6EB File Offset: 0x000398EB
	public override string Key
	{
		get
		{
			return "RunInputModeSettings";
		}
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0003B6F2 File Offset: 0x000398F2
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.holdModeKey),
			LocalizationManager.ToPlainText(this.switchModeKey)
		};
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0003B718 File Offset: 0x00039918
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 0);
		if (num == 0)
		{
			return LocalizationManager.ToPlainText(this.holdModeKey);
		}
		if (num != 1)
		{
			return LocalizationManager.ToPlainText(this.holdModeKey);
		}
		return LocalizationManager.ToPlainText(this.switchModeKey);
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0003B75E File Offset: 0x0003995E
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				InputManager.useRunInputBuffer = true;
			}
		}
		else
		{
			InputManager.useRunInputBuffer = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0003B783 File Offset: 0x00039983
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0003B796 File Offset: 0x00039996
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x0003B7AC File Offset: 0x000399AC
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000C27 RID: 3111
	[LocalizationKey("Default")]
	public string holdModeKey = "RunInputMode_Hold";

	// Token: 0x04000C28 RID: 3112
	[LocalizationKey("Default")]
	public string switchModeKey = "RunInputMode_Switch";
}
