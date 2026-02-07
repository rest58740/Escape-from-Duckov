using System;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class UIPanel : MonoBehaviour
{
	// Token: 0x06000ADB RID: 2779 RVA: 0x0002FF4D File Offset: 0x0002E14D
	protected virtual void OnOpen()
	{
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0002FF4F File Offset: 0x0002E14F
	protected virtual void OnClose()
	{
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0002FF51 File Offset: 0x0002E151
	protected virtual void OnChildOpened(UIPanel child)
	{
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0002FF53 File Offset: 0x0002E153
	protected virtual void OnChildClosed(UIPanel child)
	{
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0002FF55 File Offset: 0x0002E155
	internal void Open(UIPanel parent = null, bool controlFadeGroup = true)
	{
		this.parent = parent;
		this.OnOpen();
		if (controlFadeGroup)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0002FF78 File Offset: 0x0002E178
	public void Close()
	{
		if (this.activeChild != null)
		{
			this.activeChild.Close();
		}
		this.OnClose();
		UIPanel uipanel = this.parent;
		if (uipanel != null)
		{
			uipanel.NotifyChildClosed(this);
		}
		FadeGroup fadeGroup = this.fadeGroup;
		if (fadeGroup == null)
		{
			return;
		}
		fadeGroup.Hide();
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0002FFC8 File Offset: 0x0002E1C8
	public void OpenChild(UIPanel childPanel)
	{
		if (childPanel == null)
		{
			return;
		}
		if (this.activeChild != null)
		{
			this.activeChild.Close();
		}
		this.activeChild = childPanel;
		childPanel.Open(this, true);
		this.OnChildOpened(childPanel);
		if (this.hideWhenChildActive)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Hide();
		}
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x00030026 File Offset: 0x0002E226
	private void NotifyChildClosed(UIPanel child)
	{
		this.OnChildClosed(child);
		if (this.hideWhenChildActive)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}
	}

	// Token: 0x0400098B RID: 2443
	[SerializeField]
	protected FadeGroup fadeGroup;

	// Token: 0x0400098C RID: 2444
	[SerializeField]
	private bool hideWhenChildActive;

	// Token: 0x0400098D RID: 2445
	private UIPanel parent;

	// Token: 0x0400098E RID: 2446
	private UIPanel activeChild;
}
