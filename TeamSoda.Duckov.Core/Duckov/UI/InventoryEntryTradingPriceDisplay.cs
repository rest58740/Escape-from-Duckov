using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003BE RID: 958
	public class InventoryEntryTradingPriceDisplay : MonoBehaviour
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x00077E06 File Offset: 0x00076006
		// (set) Token: 0x0600223E RID: 8766 RVA: 0x00077E0E File Offset: 0x0007600E
		public bool Selling
		{
			get
			{
				return this.selling;
			}
			set
			{
				this.selling = value;
			}
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00077E17 File Offset: 0x00076017
		private void Awake()
		{
			this.master.onRefresh += this.OnRefresh;
			TradingUIUtilities.OnActiveMerchantChanged += this.OnActiveMerchantChanged;
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00077E41 File Offset: 0x00076041
		private void OnActiveMerchantChanged(IMerchant merchant)
		{
			this.Refresh();
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00077E49 File Offset: 0x00076049
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00077E51 File Offset: 0x00076051
		private void OnDestroy()
		{
			if (this.master != null)
			{
				this.master.onRefresh -= this.OnRefresh;
			}
			TradingUIUtilities.OnActiveMerchantChanged -= this.OnActiveMerchantChanged;
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x00077E89 File Offset: 0x00076089
		private void OnRefresh(InventoryEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x00077E94 File Offset: 0x00076094
		private void Refresh()
		{
			InventoryEntry inventoryEntry = this.master;
			Item item = (inventoryEntry != null) ? inventoryEntry.Content : null;
			if (item != null)
			{
				this.canvasGroup.alpha = 1f;
				string text = this.GetPrice(item).ToString(this.moneyFormat);
				this.priceText.text = text;
				return;
			}
			this.canvasGroup.alpha = 0f;
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x00077F00 File Offset: 0x00076100
		private int GetPrice(Item content)
		{
			if (content == null)
			{
				return 0;
			}
			int value = content.Value;
			if (TradingUIUtilities.ActiveMerchant == null)
			{
				return value;
			}
			return TradingUIUtilities.ActiveMerchant.ConvertPrice(content, this.selling);
		}

		// Token: 0x0400173C RID: 5948
		[SerializeField]
		private InventoryEntry master;

		// Token: 0x0400173D RID: 5949
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x0400173E RID: 5950
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x0400173F RID: 5951
		[SerializeField]
		private bool selling = true;

		// Token: 0x04001740 RID: 5952
		[SerializeField]
		private string moneyFormat = "n0";
	}
}
