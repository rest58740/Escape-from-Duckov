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
	// Token: 0x02000321 RID: 801
	public class DemandPanel_Entry : MonoBehaviour, IPoolable
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0006043A File Offset: 0x0005E63A
		// (set) Token: 0x06001A69 RID: 6761 RVA: 0x00060442 File Offset: 0x0005E642
		public BlackMarket.DemandSupplyEntry Target { get; private set; }

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06001A6A RID: 6762 RVA: 0x0006044C File Offset: 0x0005E64C
		// (remove) Token: 0x06001A6B RID: 6763 RVA: 0x00060484 File Offset: 0x0005E684
		public event Action<DemandPanel_Entry> onDealButtonClicked;

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x000604B9 File Offset: 0x0005E6B9
		private string TitleFormatKey
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				if (this.Target.priceFactor >= 1.9f)
				{
					return this.titleFormatKey_High;
				}
				return this.titleFormatKey_Normal;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x000604E8 File Offset: 0x0005E6E8
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

		// Token: 0x06001A6E RID: 6766 RVA: 0x00060518 File Offset: 0x0005E718
		private bool CanInteract()
		{
			return this.Target != null && this.Target.remaining > 0 && this.Target.SellCost.Enough;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00060552 File Offset: 0x0005E752
		public void NotifyPooled()
		{
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00060554 File Offset: 0x0005E754
		public void NotifyReleased()
		{
			if (this.Target != null)
			{
				this.Target.onChanged -= this.OnChanged;
			}
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00060575 File Offset: 0x0005E775
		private void OnChanged(BlackMarket.DemandSupplyEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0006057D File Offset: 0x0005E77D
		public void OnDealButtonClicked()
		{
			Action<DemandPanel_Entry> action = this.onDealButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00060590 File Offset: 0x0005E790
		internal void Setup(BlackMarket.DemandSupplyEntry target)
		{
			if (target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.Target = target;
			this.costDisplay.Setup(target.SellCost, 1);
			this.moneyDisplay.text = string.Format("{0}", target.TotalPrice);
			this.titleDisplay.text = this.TitleText;
			this.Refresh();
			target.onChanged += this.OnChanged;
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00060613 File Offset: 0x0005E813
		private void OnEnable()
		{
			ItemUtilities.OnPlayerItemOperation += this.Refresh;
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00060626 File Offset: 0x0005E826
		private void OnDisable()
		{
			ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00060639 File Offset: 0x0005E839
		private void Awake()
		{
			this.dealButton.onClick.AddListener(new UnityAction(this.OnDealButtonClicked));
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x00060658 File Offset: 0x0005E858
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

		// Token: 0x0400131D RID: 4893
		[SerializeField]
		private TextMeshProUGUI titleDisplay;

		// Token: 0x0400131E RID: 4894
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x0400131F RID: 4895
		[SerializeField]
		private TextMeshProUGUI moneyDisplay;

		// Token: 0x04001320 RID: 4896
		[SerializeField]
		private GameObject remainingInfoContainer;

		// Token: 0x04001321 RID: 4897
		[SerializeField]
		private TextMeshProUGUI remainingAmountDisplay;

		// Token: 0x04001322 RID: 4898
		[SerializeField]
		private GameObject canInteractIndicator;

		// Token: 0x04001323 RID: 4899
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x04001324 RID: 4900
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Normal = "BlackMarket_Demand_Title_Normal";

		// Token: 0x04001325 RID: 4901
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_High = "BlackMarket_Demand_Title_High";

		// Token: 0x04001326 RID: 4902
		[SerializeField]
		private Button dealButton;
	}
}
