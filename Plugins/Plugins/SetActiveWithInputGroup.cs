using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000005 RID: 5
public class SetActiveWithInputGroup : MonoBehaviour
{
	// Token: 0x0600001E RID: 30 RVA: 0x0000258A File Offset: 0x0000078A
	private void Awake()
	{
		InputSystem.onDeviceChange += new Action<InputDevice, InputDeviceChange>(this.OnDeviceChanged);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x0000259D File Offset: 0x0000079D
	private void OnDestroy()
	{
		InputSystem.onDeviceChange -= new Action<InputDevice, InputDeviceChange>(this.OnDeviceChanged);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000025B0 File Offset: 0x000007B0
	private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
	{
		this.Refresh();
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Refresh()
	{
		string currentControlScheme = PlayerInput.all[0].currentControlScheme;
		base.gameObject.SetActive(currentControlScheme == this.group);
	}

	// Token: 0x0400000D RID: 13
	[SerializeField]
	private string group = "KeyAndMouse";
}
