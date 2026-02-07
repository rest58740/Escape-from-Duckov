using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001B4 RID: 436
public class CraftViewFilterBtnEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000D21 RID: 3361 RVA: 0x0003797E File Offset: 0x00035B7E
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.master == null)
		{
			return;
		}
		this.master.SetFilter(this.index);
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x000379A0 File Offset: 0x00035BA0
	public void Setup(CraftView master, CraftView.FilterInfo filterInfo, int index, bool selected)
	{
		this.master = master;
		this.info = filterInfo;
		this.index = index;
		this.icon.sprite = filterInfo.icon;
		this.displayNameText.text = LocalizationManager.ToPlainText(filterInfo.displayNameKey);
		this.selectedIndicator.SetActive(selected);
	}

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	private Image icon;

	// Token: 0x04000B6D RID: 2925
	[SerializeField]
	private TextMeshProUGUI displayNameText;

	// Token: 0x04000B6E RID: 2926
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000B6F RID: 2927
	private CraftView.FilterInfo info;

	// Token: 0x04000B70 RID: 2928
	private CraftView master;

	// Token: 0x04000B71 RID: 2929
	private int index;
}
