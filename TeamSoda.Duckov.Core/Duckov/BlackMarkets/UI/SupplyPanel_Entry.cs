using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x02000323 RID: 803
	public class SupplyPanel_Entry : MonoBehaviour, IPoolable
	{
		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001A86 RID: 6790 RVA: 0x000608BA File Offset: 0x0005EABA
		// (set) Token: 0x06001A87 RID: 6791 RVA: 0x000608C2 File Offset: 0x0005EAC2
		public BlackMarket.DemandSupplyEntry Target { get; private set; }

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06001A88 RID: 6792 RVA: 0x000608CC File Offset: 0x0005EACC
		// (remove) Token: 0x06001A89 RID: 6793 RVA: 0x00060904 File Offset: 0x0005EB04
		public event Action<SupplyPanel_Entry> onDealButtonClicked;

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x00060939 File Offset: 0x0005EB39
		private string TitleFormatKey
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				if (this.Target.priceFactor <= 0.9f)
				{
					return this.titleFormatKey_Low;
				}
				return this.titleFormatKey_Normal;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x00060968 File Offset: 0x0005EB68
		private string TitleText
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				return StringExtensions.Format(LocalizationManager.ToPlainText(this.TitleFormatKey), new
				{
					itemName = this.Target.ItemDisplayName
				});
			}
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x00060998 File Offset: 0x0005EB98
		private bool CanInteract()
		{
			return this.Target != null && this.Target.remaining > 0 && this.Target.BuyCost.Enough;
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x000609D2 File Offset: 0x0005EBD2
		public void NotifyPooled()
		{
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000609D4 File Offset: 0x0005EBD4
		public void NotifyReleased()
		{
			if (this.Target != null)
			{
				this.Target.onChanged -= this.OnChanged;
			}
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000609F5 File Offset: 0x0005EBF5
		private void OnChanged(BlackMarket.DemandSupplyEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00060A00 File Offset: 0x0005EC00
		internal void Setup(BlackMarket.DemandSupplyEntry target)
		{
			if (target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.Target = target;
			this.costDisplay.Setup(target.BuyCost, 1);
			this.resultDisplay.Setup(target.ItemID, (long)target.ItemMetaData.defaultStackCount);
			this.titleDisplay.text = this.TitleText;
			this.Refresh();
			target.onChanged += this.OnChanged;
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00060A80 File Offset: 0x0005EC80
		private void OnEnable()
		{
			ItemUtilities.OnPlayerItemOperation += this.Refresh;
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00060A93 File Offset: 0x0005EC93
		private void OnDisable()
		{
			ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x00060AA6 File Offset: 0x0005ECA6
		private void Awake()
		{
			this.dealButton.onClick.AddListener(new UnityAction(this.OnDealButtonClicked));
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00060AC4 File Offset: 0x0005ECC4
		private void OnDealButtonClicked()
		{
			Action<SupplyPanel_Entry> action = this.onDealButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x00060AD8 File Offset: 0x0005ECD8
		private void Refresh()
		{
			if (this.Target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.remainingAmountDisplay.text = string.Format("{0}", this.Target.Remaining);
			bool active = this.CanInteract();
			this.canInteractIndicator.SetActive(active);
			bool active2 = this.Target.Remaining <= 0;
			this.outOfStockIndicator.SetActive(active2);
			this.remainingInfoContainer.SetActive(this.Target.remaining > 1);
		}

		// Token: 0x0400132C RID: 4908
		[SerializeField]
		private TextMeshProUGUI titleDisplay;

		// Token: 0x0400132D RID: 4909
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x0400132E RID: 4910
		[SerializeField]
		private ItemAmountDisplay resultDisplay;

		// Token: 0x0400132F RID: 4911
		[SerializeField]
		private GameObject remainingInfoContainer;

		// Token: 0x04001330 RID: 4912
		[SerializeField]
		private TextMeshProUGUI remainingAmountDisplay;

		// Token: 0x04001331 RID: 4913
		[SerializeField]
		private GameObject canInteractIndicator;

		// Token: 0x04001332 RID: 4914
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x04001333 RID: 4915
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Normal = "BlackMarket_Supply_Title_Normal";

		// Token: 0x04001334 RID: 4916
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Low = "BlackMarket_Supply_Title_Low";

		// Token: 0x04001335 RID: 4917
		[SerializeField]
		private Button dealButton;
	}
}
