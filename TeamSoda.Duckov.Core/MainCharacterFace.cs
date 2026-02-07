using System;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class MainCharacterFace : MonoBehaviour
{
	// Token: 0x06000BF5 RID: 3061 RVA: 0x000330D0 File Offset: 0x000312D0
	private void Start()
	{
		CustomFaceSettingData saveData = this.customFaceManager.LoadMainCharacterSetting();
		this.customFace.LoadFromData(saveData);
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x000330F5 File Offset: 0x000312F5
	private void Update()
	{
	}

	// Token: 0x04000A3E RID: 2622
	public CustomFaceManager customFaceManager;

	// Token: 0x04000A3F RID: 2623
	public CustomFaceInstance customFace;
}
