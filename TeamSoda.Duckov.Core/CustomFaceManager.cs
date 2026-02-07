using System;
using Duckov.Utilities;
using Saves;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class CustomFaceManager : MonoBehaviour
{
	// Token: 0x060008C2 RID: 2242 RVA: 0x00027815 File Offset: 0x00025A15
	public void SaveSettingToMainCharacter(CustomFaceSettingData setting)
	{
		this.SaveSetting("CustomFace_MainCharacter", setting);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00027823 File Offset: 0x00025A23
	public CustomFaceSettingData LoadMainCharacterSetting()
	{
		return this.LoadSetting("CustomFace_MainCharacter");
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00027830 File Offset: 0x00025A30
	private void SaveSetting(string key, CustomFaceSettingData setting)
	{
		setting.savedSetting = true;
		SavesSystem.Save<CustomFaceSettingData>(key, setting);
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00027844 File Offset: 0x00025A44
	private CustomFaceSettingData LoadSetting(string key)
	{
		CustomFaceSettingData customFaceSettingData = SavesSystem.Load<CustomFaceSettingData>(key);
		if (!customFaceSettingData.savedSetting)
		{
			customFaceSettingData = GameplayDataSettings.CustomFaceData.DefaultPreset.settings;
		}
		return customFaceSettingData;
	}
}
