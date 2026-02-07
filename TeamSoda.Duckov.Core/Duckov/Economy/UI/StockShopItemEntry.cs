using System;
using DG.Tweening;
using Duckov.UI;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Economy.UI
{
	// Token: 0x0200033E RID: 830
	public class StockShopItemEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x00065FD5 File Offset: 0x000641D5
		private StockShop stockShop
		{
			get
			{
				StockShopView stockShopView = this.master;
				if (stockShopView == null)
				{
					return null;
				}
				return stockShopView.Target;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x00065FE8 File Offset: 0x000641E8
		public StockShop.Entry Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x00065FF0 File Offset: 0x000641F0
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayPointerClick;
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x00066009 File Offset: 0x00064209
		private void OnItemDisplayPointerClick(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x00066012 File Offset: 0x00064212
		public Item GetItem()
		{
			return this.stockShop.GetItemInstanceDirect(this.target.ItemTypeID);
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x0006602C File Offset: 0x0006422C
		internal void Setup(StockShopView master, StockShop.Entry entry)
		{
			this.UnregisterEvents();
			this.master = master;
			this.target = entry;
			Item itemInstanceDirect = this.stockShop.GetItemInstanceDirect(this.target.ItemTypeID);
			this.itemDisplay.Setup(itemInstanceDirect);
			this.itemDisplay.ShowOperationButtons = false;
			this.itemDisplay.IsStockshopSample = true;
			int stackCount = itemInstanceDirect.StackCount;
			int num = this.stockShop.ConvertPrice(itemInstanceDirect, false);
			this.priceText.text = num.ToString(this.moneyFormat);
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000660C4 File Offset: 0x000642C4
		private void RegisterEvents()
		{
			if (this.master != null)
			{
				StockShopView stockShopView = this.master;
				stockShopView.onSelectionChanged = (Action)Delegate.Combine(stockShopView.onSelectionChanged, new Action(this.OnMasterSelectionChanged));
			}
			if (this.target != null)
			{
				this.target.onStockChanged += this.OnTargetStockChanged;
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x00066128 File Offset: 0x00064328
		private void UnregisterEvents()
		{
			if (this.master != null)
			{
				StockShopView stockShopView = this.master;
				stockShopView.onSelectionChanged = (Action)Delegate.Remove(stockShopView.onSelectionChanged, new Action(this.OnMasterSelectionChanged));
			}
			if (this.target != null)
			{
				this.target.onStockChanged -= this.OnTargetStockChanged;
			}
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x00066189 File Offset: 0x00064389
		private void OnMasterSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x00066191 File Offset: 0x00064391
		private void OnTargetStockChanged(StockShop.Entry entry)
		{
			this.Refresh();
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00066199 File Offset: 0x00064399
		public bool IsUnlocked()
		{
			return this.target != null && (this.target.ForceUnlock || EconomyManager.IsUnlocked(this.target.ItemTypeID));
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000661C4 File Offset: 0x000643C4
		private void Refresh()
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			bool active = this.master.GetSelection() == this;
			this.selectionIndicator.SetActive(active);
			bool flag = EconomyManager.IsUnlocked(this.target.ItemTypeID);
			bool flag2 = EconomyManager.IsWaitingForUnlockConfirm(this.target.ItemTypeID);
			if (this.target.ForceUnlock)
			{
				flag = true;
				flag2 = false;
			}
			this.lockedIndicator.SetActive(!flag && !flag2);
			this.waitingForUnlockIndicator.SetActive(!flag && flag2);
			base.gameObject.SetActive(flag || flag2);
			this.outOfStockIndicator.SetActive(this.Target.CurrentStock <= 0);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00066280 File Offset: 0x00064480
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Punch();
			if (this.master == null)
			{
				return;
			}
			eventData.Use();
			if (EconomyManager.IsWaitingForUnlockConfirm(this.target.ItemTypeID))
			{
				EconomyManager.ConfirmUnlock(this.target.ItemTypeID);
			}
			if (this.master.GetSelection() == this)
			{
				this.master.SetSelection(null);
				return;
			}
			this.master.SetSelection(this);
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000662F8 File Offset: 0x000644F8
		public void Punch()
		{
			ShortcutExtensions.DOKill(this.selectionIndicator.transform, false);
			this.selectionIndicator.transform.localScale = Vector3.one;
			ShortcutExtensions.DOPunchScale(this.selectionIndicator.transform, Vector3.one * this.selectionRingPunchScale, this.punchDuration, 10, 1f);
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x0006635A File Offset: 0x0006455A
		private void OnEnable()
		{
			EconomyManager.OnItemUnlockStateChanged += this.OnItemUnlockStateChanged;
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x0006636D File Offset: 0x0006456D
		private void OnDisable()
		{
			EconomyManager.OnItemUnlockStateChanged -= this.OnItemUnlockStateChanged;
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x00066380 File Offset: 0x00064580
		private void OnItemUnlockStateChanged(int itemTypeID)
		{
			if (this.target == null)
			{
				return;
			}
			if (itemTypeID == this.target.ItemTypeID)
			{
				this.Refresh();
			}
		}

		// Token: 0x040013E7 RID: 5095
		[SerializeField]
		private string moneyFormat = "n0";

		// Token: 0x040013E8 RID: 5096
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040013E9 RID: 5097
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x040013EA RID: 5098
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x040013EB RID: 5099
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x040013EC RID: 5100
		[SerializeField]
		private GameObject waitingForUnlockIndicator;

		// Token: 0x040013ED RID: 5101
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x040013EE RID: 5102
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.2f;

		// Token: 0x040013EF RID: 5103
		[SerializeField]
		[Range(-1f, 1f)]
		private float selectionRingPunchScale = 0.1f;

		// Token: 0x040013F0 RID: 5104
		[SerializeField]
		[Range(-1f, 1f)]
		private float iconPunchScale = 0.1f;

		// Token: 0x040013F1 RID: 5105
		private StockShopView master;

		// Token: 0x040013F2 RID: 5106
		private StockShop.Entry target;
	}
}
