using System;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class SetActiveByInputDevice : MonoBehaviour
{
	// Token: 0x06000B77 RID: 2935 RVA: 0x0003140D File Offset: 0x0002F60D
	private void Awake()
	{
		this.OnInputDeviceChanged();
		InputManager.OnInputDeviceChanged += this.OnInputDeviceChanged;
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x00031426 File Offset: 0x0002F626
	private void OnDestroy()
	{
		InputManager.OnInputDeviceChanged -= this.OnInputDeviceChanged;
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x00031439 File Offset: 0x0002F639
	private void OnInputDeviceChanged()
	{
		if (InputManager.InputDevice == this.activeIfDeviceIs)
		{
			base.gameObject.SetActive(true);
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x040009E8 RID: 2536
	public InputManager.InputDevices activeIfDeviceIs;
}
