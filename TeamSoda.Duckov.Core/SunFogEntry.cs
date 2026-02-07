using System;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class SunFogEntry : MonoBehaviour
{
	// Token: 0x14000070 RID: 112
	// (add) Token: 0x06000E8F RID: 3727 RVA: 0x0003BB18 File Offset: 0x00039D18
	// (remove) Token: 0x06000E90 RID: 3728 RVA: 0x0003BB4C File Offset: 0x00039D4C
	private static event Action OnSettingChangedEvent;

	// Token: 0x06000E91 RID: 3729 RVA: 0x0003BB7F File Offset: 0x00039D7F
	public static void SetEnabled(bool enabled)
	{
		SunFogEntry.settingEnabled = enabled;
		Action onSettingChangedEvent = SunFogEntry.OnSettingChangedEvent;
		if (onSettingChangedEvent == null)
		{
			return;
		}
		onSettingChangedEvent();
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0003BB96 File Offset: 0x00039D96
	private void Awake()
	{
		SunFogEntry.OnSettingChangedEvent += this.OnSettingChanged;
		base.gameObject.SetActive(SunFogEntry.settingEnabled);
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0003BBB9 File Offset: 0x00039DB9
	private void OnDestroy()
	{
		SunFogEntry.OnSettingChangedEvent -= this.OnSettingChanged;
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0003BBCC File Offset: 0x00039DCC
	private void OnSettingChanged()
	{
		base.gameObject.SetActive(SunFogEntry.settingEnabled);
	}

	// Token: 0x04000C31 RID: 3121
	private static bool settingEnabled = true;
}
