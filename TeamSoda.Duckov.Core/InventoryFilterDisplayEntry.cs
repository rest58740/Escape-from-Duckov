using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001FA RID: 506
public class InventoryFilterDisplayEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0003CF6F File Offset: 0x0003B16F
	// (set) Token: 0x06000F0D RID: 3853 RVA: 0x0003CF77 File Offset: 0x0003B177
	public InventoryFilterProvider.FilterEntry Filter { get; private set; }

	// Token: 0x06000F0E RID: 3854 RVA: 0x0003CF80 File Offset: 0x0003B180
	public void OnPointerClick(PointerEventData eventData)
	{
		Action<InventoryFilterDisplayEntry, PointerEventData> action = this.onPointerClick;
		if (action == null)
		{
			return;
		}
		action(this, eventData);
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x0003CF94 File Offset: 0x0003B194
	internal void Setup(Action<InventoryFilterDisplayEntry, PointerEventData> onPointerClick, InventoryFilterProvider.FilterEntry filter)
	{
		this.onPointerClick = onPointerClick;
		this.Filter = filter;
		if (this.icon)
		{
			this.icon.sprite = filter.icon;
		}
		if (this.nameDisplay)
		{
			this.nameDisplay.text = filter.DisplayName;
		}
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0003CFEC File Offset: 0x0003B1EC
	internal void NotifySelectionChanged(bool isThisSelected)
	{
		this.selectedIndicator.SetActive(isThisSelected);
	}

	// Token: 0x04000C88 RID: 3208
	[SerializeField]
	private Image icon;

	// Token: 0x04000C89 RID: 3209
	[SerializeField]
	private TextMeshProUGUI nameDisplay;

	// Token: 0x04000C8A RID: 3210
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000C8C RID: 3212
	private Action<InventoryFilterDisplayEntry, PointerEventData> onPointerClick;
}
