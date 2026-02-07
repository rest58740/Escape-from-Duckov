using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A0 RID: 928
	public class FormulasIndexEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x00071749 File Offset: 0x0006F949
		public CraftingFormula Formula
		{
			get
			{
				return this.formula;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x00071751 File Offset: 0x0006F951
		private int ItemID
		{
			get
			{
				return this.formula.result.id;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x00071763 File Offset: 0x0006F963
		private ItemMetaData Meta
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.ItemID);
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x00071770 File Offset: 0x0006F970
		private bool Unlocked
		{
			get
			{
				return CraftingManager.IsFormulaUnlocked(this.formula.id);
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x00071782 File Offset: 0x0006F982
		public bool Valid
		{
			get
			{
				return this.ItemID >= 0;
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00071790 File Offset: 0x0006F990
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.OnEntryClicked(this);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x0007179E File Offset: 0x0006F99E
		internal void Setup(FormulasIndexView master, CraftingFormula formula)
		{
			this.master = master;
			this.formula = formula;
			this.Refresh();
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000717B4 File Offset: 0x0006F9B4
		public void Refresh()
		{
			ItemMetaData meta = this.Meta;
			if (!this.Valid)
			{
				this.displayNameText.text = "! " + this.formula.id + " !";
				this.image.sprite = this.lockedImage;
				return;
			}
			if (this.Unlocked)
			{
				this.displayNameText.text = string.Format("{0} x{1}", meta.DisplayName, this.formula.result.amount);
				this.image.sprite = meta.icon;
				return;
			}
			this.displayNameText.text = this.lockedText;
			this.image.sprite = this.lockedImage;
		}

		// Token: 0x0400160C RID: 5644
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x0400160D RID: 5645
		[SerializeField]
		private Image image;

		// Token: 0x0400160E RID: 5646
		[SerializeField]
		private string lockedText = "???";

		// Token: 0x0400160F RID: 5647
		[SerializeField]
		private Sprite lockedImage;

		// Token: 0x04001610 RID: 5648
		private FormulasIndexView master;

		// Token: 0x04001611 RID: 5649
		private CraftingFormula formula;
	}
}
