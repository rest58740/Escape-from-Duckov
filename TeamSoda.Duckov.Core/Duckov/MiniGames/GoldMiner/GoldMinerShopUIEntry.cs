using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AF RID: 687
	public class GoldMinerShopUIEntry : MonoBehaviour
	{
		// Token: 0x060016A6 RID: 5798 RVA: 0x0005441C File Offset: 0x0005261C
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			this.VCT = base.GetComponent<VirtualCursorTarget>();
			if (this.VCT)
			{
				this.VCT.onEnter.AddListener(new UnityAction(this.OnVCTEnter));
			}
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x0005449E File Offset: 0x0005269E
		private void OnVCTEnter()
		{
			this.master.hoveringEntry = this;
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x000544AC File Offset: 0x000526AC
		private void OnInteract(NavEntry entry)
		{
			this.master.target.Buy(this.target);
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x000544C8 File Offset: 0x000526C8
		internal void Setup(GoldMinerShopUI master, ShopEntity target)
		{
			this.master = master;
			this.target = target;
			if (target == null || target.artifact == null)
			{
				this.SetupEmpty();
				return;
			}
			this.mainLayout.SetActive(true);
			this.nameText.text = target.artifact.DisplayName;
			this.icon.sprite = target.artifact.Icon;
			this.Refresh();
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0005453C File Offset: 0x0005273C
		private void Refresh()
		{
			bool flag;
			int num = this.master.target.CalculateDealPrice(this.target, out flag);
			this.priceText.text = num.ToString(this.priceFormat);
			this.priceIndicator.SetActive(num > 0);
			this.freeIndicator.SetActive(num <= 0);
			this.soldIndicator.SetActive(this.target.sold);
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x000545B1 File Offset: 0x000527B1
		private void SetupEmpty()
		{
			this.mainLayout.SetActive(false);
		}

		// Token: 0x040010C0 RID: 4288
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x040010C1 RID: 4289
		[SerializeField]
		private VirtualCursorTarget VCT;

		// Token: 0x040010C2 RID: 4290
		[SerializeField]
		private GameObject mainLayout;

		// Token: 0x040010C3 RID: 4291
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x040010C4 RID: 4292
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x040010C5 RID: 4293
		[SerializeField]
		private string priceFormat = "0";

		// Token: 0x040010C6 RID: 4294
		[SerializeField]
		private GameObject priceIndicator;

		// Token: 0x040010C7 RID: 4295
		[SerializeField]
		private GameObject freeIndicator;

		// Token: 0x040010C8 RID: 4296
		[SerializeField]
		private Image icon;

		// Token: 0x040010C9 RID: 4297
		[SerializeField]
		private GameObject soldIndicator;

		// Token: 0x040010CA RID: 4298
		private GoldMinerShopUI master;

		// Token: 0x040010CB RID: 4299
		public ShopEntity target;
	}
}
