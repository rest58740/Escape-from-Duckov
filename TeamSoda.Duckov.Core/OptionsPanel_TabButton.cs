using System;
using Duckov.Options.UI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001E9 RID: 489
public class OptionsPanel_TabButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000EBF RID: 3775 RVA: 0x0003C0A3 File Offset: 0x0003A2A3
	public void OnPointerClick(PointerEventData eventData)
	{
		Action<OptionsPanel_TabButton, PointerEventData> action = this.onClicked;
		if (action == null)
		{
			return;
		}
		action(this, eventData);
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x0003C0B8 File Offset: 0x0003A2B8
	internal void NotifySelectionChanged(OptionsPanel optionsPanel, OptionsPanel_TabButton selection)
	{
		bool active = selection == this;
		this.tab.SetActive(active);
		this.selectedIndicator.SetActive(active);
	}

	// Token: 0x04000C42 RID: 3138
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000C43 RID: 3139
	[SerializeField]
	private GameObject tab;

	// Token: 0x04000C44 RID: 3140
	public Action<OptionsPanel_TabButton, PointerEventData> onClicked;
}
