using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001DF RID: 479
public class MoveDirectionOptions : OptionsProviderBase
{
	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0003B5E3 File Offset: 0x000397E3
	public override string Key
	{
		get
		{
			return "MoveDirModeSettings";
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0003B5EA File Offset: 0x000397EA
	public static bool MoveViaCharacterDirection
	{
		get
		{
			return MoveDirectionOptions.moveViaCharacterDirection;
		}
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x0003B5F1 File Offset: 0x000397F1
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText(this.cameraModeKey),
			LocalizationManager.ToPlainText(this.aimModeKey)
		};
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x0003B618 File Offset: 0x00039818
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 0);
		if (num == 0)
		{
			return LocalizationManager.ToPlainText(this.cameraModeKey);
		}
		if (num != 1)
		{
			return LocalizationManager.ToPlainText(this.cameraModeKey);
		}
		return LocalizationManager.ToPlainText(this.aimModeKey);
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x0003B65E File Offset: 0x0003985E
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				MoveDirectionOptions.moveViaCharacterDirection = true;
			}
		}
		else
		{
			MoveDirectionOptions.moveViaCharacterDirection = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x0003B683 File Offset: 0x00039883
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x0003B696 File Offset: 0x00039896
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x0003B6AC File Offset: 0x000398AC
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 0);
		this.Set(index);
	}

	// Token: 0x04000C24 RID: 3108
	[LocalizationKey("Default")]
	public string cameraModeKey = "MoveDirectionMode_Camera";

	// Token: 0x04000C25 RID: 3109
	[LocalizationKey("Default")]
	public string aimModeKey = "MoveDirectionMode_Aim";

	// Token: 0x04000C26 RID: 3110
	private static bool moveViaCharacterDirection;
}
