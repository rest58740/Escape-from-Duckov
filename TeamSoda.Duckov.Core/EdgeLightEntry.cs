using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class EdgeLightEntry : MonoBehaviour
{
	// Token: 0x1400006F RID: 111
	// (add) Token: 0x06000E33 RID: 3635 RVA: 0x0003ABCC File Offset: 0x00038DCC
	// (remove) Token: 0x06000E34 RID: 3636 RVA: 0x0003AC00 File Offset: 0x00038E00
	private static event Action OnSettingChangedEvent;

	// Token: 0x06000E35 RID: 3637 RVA: 0x0003AC33 File Offset: 0x00038E33
	public static void SetEnabled(bool enabled)
	{
		EdgeLightEntry.settingEnabled = enabled;
		Action onSettingChangedEvent = EdgeLightEntry.OnSettingChangedEvent;
		if (onSettingChangedEvent == null)
		{
			return;
		}
		onSettingChangedEvent();
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0003AC4A File Offset: 0x00038E4A
	private void Awake()
	{
		EdgeLightEntry.OnSettingChangedEvent += this.OnSettingChanged;
		base.gameObject.SetActive(EdgeLightEntry.settingEnabled);
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0003AC6D File Offset: 0x00038E6D
	private void OnDestroy()
	{
		EdgeLightEntry.OnSettingChangedEvent -= this.OnSettingChanged;
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0003AC80 File Offset: 0x00038E80
	private void OnSettingChanged()
	{
		base.gameObject.SetActive(EdgeLightEntry.settingEnabled);
	}

	// Token: 0x04000C0F RID: 3087
	private static bool settingEnabled = true;
}
