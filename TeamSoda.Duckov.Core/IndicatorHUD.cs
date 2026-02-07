using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class IndicatorHUD : MonoBehaviour
{
	// Token: 0x0600064B RID: 1611 RVA: 0x0001C714 File Offset: 0x0001A914
	private void Start()
	{
		if ((LevelManager.Instance == null || LevelManager.Instance.IsBaseLevel) && this.mapIndicator)
		{
			this.mapIndicator.SetActive(false);
		}
		this.toggleParent.SetActive(this.startActive);
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0001C764 File Offset: 0x0001A964
	private void Awake()
	{
		UIInputManager.OnToggleIndicatorHUD += this.Toggle;
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0001C777 File Offset: 0x0001A977
	private void OnDestroy()
	{
		UIInputManager.OnToggleIndicatorHUD -= this.Toggle;
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0001C78A File Offset: 0x0001A98A
	private void Toggle(UIInputEventData data)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.toggleParent.SetActive(!this.toggleParent.activeInHierarchy);
	}

	// Token: 0x040005F6 RID: 1526
	public GameObject mapIndicator;

	// Token: 0x040005F7 RID: 1527
	public GameObject toggleParent;

	// Token: 0x040005F8 RID: 1528
	public bool startActive;
}
