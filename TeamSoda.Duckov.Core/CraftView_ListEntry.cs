using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001B5 RID: 437
public class CraftView_ListEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000D24 RID: 3364 RVA: 0x000379FE File Offset: 0x00035BFE
	// (set) Token: 0x06000D25 RID: 3365 RVA: 0x00037A06 File Offset: 0x00035C06
	public CraftView Master { get; private set; }

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00037A0F File Offset: 0x00035C0F
	// (set) Token: 0x06000D27 RID: 3367 RVA: 0x00037A17 File Offset: 0x00035C17
	public CraftingFormula Formula { get; private set; }

	// Token: 0x06000D28 RID: 3368 RVA: 0x00037A20 File Offset: 0x00035C20
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.Refresh;
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x00037A33 File Offset: 0x00035C33
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.Refresh;
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x00037A48 File Offset: 0x00035C48
	public void Setup(CraftView master, CraftingFormula formula)
	{
		this.Master = master;
		this.Formula = formula;
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.Formula.result.id);
		this.icon.sprite = metaData.icon;
		this.nameText.text = string.Format("{0} x{1}", metaData.DisplayName, formula.result.amount);
		this.Refresh();
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x00037ABC File Offset: 0x00035CBC
	public void OnPointerClick(PointerEventData eventData)
	{
		CraftView master = this.Master;
		if (master == null)
		{
			return;
		}
		master.SetSelection(this);
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x00037AD0 File Offset: 0x00035CD0
	internal void NotifyUnselected()
	{
		this.Refresh();
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x00037AD8 File Offset: 0x00035CD8
	internal void NotifySelected()
	{
		this.Refresh();
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x00037AE0 File Offset: 0x00035CE0
	private void Refresh()
	{
		if (this.Master == null)
		{
			return;
		}
		bool active = this.Master.GetSelection() == this;
		Color color = this.normalColor;
		if (this.selectedIndicator != null)
		{
			this.selectedIndicator.SetActive(active);
		}
		if (this.Formula.cost.Enough)
		{
			color = this.normalColor;
		}
		else
		{
			color = this.normalInsufficientColor;
		}
		this.background.color = color;
	}

	// Token: 0x04000B72 RID: 2930
	[SerializeField]
	private Color normalColor;

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private Color normalInsufficientColor;

	// Token: 0x04000B74 RID: 2932
	[SerializeField]
	private Image icon;

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	private Image background;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000B77 RID: 2935
	[SerializeField]
	private GameObject selectedIndicator;
}
