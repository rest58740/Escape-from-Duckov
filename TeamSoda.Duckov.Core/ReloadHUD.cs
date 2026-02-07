using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000CC RID: 204
public class ReloadHUD : MonoBehaviour
{
	// Token: 0x0600067E RID: 1662 RVA: 0x0001D724 File Offset: 0x0001B924
	private void Update()
	{
		if (this.characterMainControl == null)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl == null)
			{
				return;
			}
			this.button.onClick.AddListener(new UnityAction(this.Reload));
		}
		this.reloadable = this.characterMainControl.GetGunReloadable();
		if (this.reloadable != this.button.interactable)
		{
			this.button.interactable = this.reloadable;
			if (this.reloadable)
			{
				UnityEvent onShowEvent = this.OnShowEvent;
				if (onShowEvent != null)
				{
					onShowEvent.Invoke();
				}
			}
			else
			{
				UnityEvent onHideEvent = this.OnHideEvent;
				if (onHideEvent != null)
				{
					onHideEvent.Invoke();
				}
			}
		}
		this.frame++;
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x0001D7E9 File Offset: 0x0001B9E9
	private void OnDestroy()
	{
		this.button.onClick.RemoveAllListeners();
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0001D7FB File Offset: 0x0001B9FB
	private void Reload()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.TryToReload(null);
		}
	}

	// Token: 0x0400064B RID: 1611
	private CharacterMainControl characterMainControl;

	// Token: 0x0400064C RID: 1612
	public Button button;

	// Token: 0x0400064D RID: 1613
	private bool reloadable;

	// Token: 0x0400064E RID: 1614
	public UnityEvent OnShowEvent;

	// Token: 0x0400064F RID: 1615
	public UnityEvent OnHideEvent;

	// Token: 0x04000650 RID: 1616
	private int frame;
}
