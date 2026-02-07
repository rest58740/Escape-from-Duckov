using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B0 RID: 688
	public class GoldMinerShopUIRefreshBtn : MonoBehaviour
	{
		// Token: 0x060016AD RID: 5805 RVA: 0x000545D4 File Offset: 0x000527D4
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMinerShop goldMinerShop = this.shop;
			goldMinerShop.onAfterOperation = (Action)Delegate.Combine(goldMinerShop.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00054648 File Offset: 0x00052848
		private void OnEnable()
		{
			this.RefreshCostText();
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x00054650 File Offset: 0x00052850
		private void OnAfterOperation()
		{
			this.RefreshCostText();
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x00054658 File Offset: 0x00052858
		private void RefreshCostText()
		{
			this.costText.text = string.Format("${0}", this.shop.GetRefreshCost());
			this.refreshChanceText.text = string.Format("{0}", this.shop.refreshChance);
			this.noChanceIndicator.SetActive(this.shop.refreshChance < 1);
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x000546C8 File Offset: 0x000528C8
		private void OnInteract(NavEntry entry)
		{
			this.shop.TryRefresh();
		}

		// Token: 0x040010CC RID: 4300
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x040010CD RID: 4301
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x040010CE RID: 4302
		[SerializeField]
		private TextMeshProUGUI costText;

		// Token: 0x040010CF RID: 4303
		[SerializeField]
		private TextMeshProUGUI refreshChanceText;

		// Token: 0x040010D0 RID: 4304
		[SerializeField]
		private GameObject noChanceIndicator;
	}
}
