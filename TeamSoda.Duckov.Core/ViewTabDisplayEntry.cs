using System;
using Duckov.UI;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class ViewTabDisplayEntry : MonoBehaviour
{
	// Token: 0x06000F98 RID: 3992 RVA: 0x0003E438 File Offset: 0x0003C638
	private void Awake()
	{
		ManagedUIElement.onOpen += this.OnViewOpen;
		ManagedUIElement.onClose += this.OnViewClose;
		this.HideIndicator();
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x0003E462 File Offset: 0x0003C662
	private void OnDestroy()
	{
		ManagedUIElement.onOpen -= this.OnViewOpen;
		ManagedUIElement.onClose -= this.OnViewClose;
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x0003E486 File Offset: 0x0003C686
	private void Start()
	{
		if (View.ActiveView != null && View.ActiveView.GetType().Name == this.viewTypeName)
		{
			this.ShowIndicator();
		}
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x0003E4B7 File Offset: 0x0003C6B7
	private void OnViewClose(ManagedUIElement element)
	{
		if (element.GetType().Name == this.viewTypeName)
		{
			this.HideIndicator();
		}
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x0003E4D7 File Offset: 0x0003C6D7
	private void OnViewOpen(ManagedUIElement element)
	{
		if (element.GetType().Name == this.viewTypeName)
		{
			this.ShowIndicator();
		}
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x0003E4F7 File Offset: 0x0003C6F7
	private void ShowIndicator()
	{
		this.indicator.SetActive(true);
		this.punch.Punch();
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x0003E510 File Offset: 0x0003C710
	private void HideIndicator()
	{
		this.indicator.SetActive(false);
	}

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	private string viewTypeName;

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	private GameObject indicator;

	// Token: 0x04000CD8 RID: 3288
	[SerializeField]
	private PunchReceiver punch;
}
