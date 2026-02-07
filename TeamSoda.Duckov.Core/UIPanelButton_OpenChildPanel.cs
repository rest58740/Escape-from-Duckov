using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200015C RID: 348
public class UIPanelButton_OpenChildPanel : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000AE4 RID: 2788 RVA: 0x0003004F File Offset: 0x0002E24F
	private void Awake()
	{
		if (this.master == null)
		{
			this.master = base.GetComponentInParent<UIPanel>();
		}
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0003006B File Offset: 0x0002E26B
	public void OnPointerClick(PointerEventData eventData)
	{
		UIPanel uipanel = this.master;
		if (uipanel != null)
		{
			uipanel.OpenChild(this.target);
		}
		eventData.Use();
	}

	// Token: 0x0400098F RID: 2447
	[SerializeField]
	private UIPanel master;

	// Token: 0x04000990 RID: 2448
	[SerializeField]
	private UIPanel target;
}
