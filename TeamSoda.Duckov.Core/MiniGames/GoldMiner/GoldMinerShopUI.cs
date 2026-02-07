using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AD RID: 685
	public class GoldMinerShopUI : MiniGameBehaviour
	{
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001699 RID: 5785 RVA: 0x00054220 File Offset: 0x00052420
		// (set) Token: 0x0600169A RID: 5786 RVA: 0x00054228 File Offset: 0x00052428
		public GoldMinerShop target { get; private set; }

		// Token: 0x0600169B RID: 5787 RVA: 0x00054231 File Offset: 0x00052431
		private void UnregisterEvent()
		{
			if (this.target == null)
			{
				return;
			}
			GoldMinerShop target = this.target;
			target.onAfterOperation = (Action)Delegate.Remove(target.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00054269 File Offset: 0x00052469
		private void RegisterEvent()
		{
			if (this.target == null)
			{
				return;
			}
			GoldMinerShop target = this.target;
			target.onAfterOperation = (Action)Delegate.Combine(target.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x000542A1 File Offset: 0x000524A1
		private void OnAfterOperation()
		{
			this.RefreshEntries();
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x000542AC File Offset: 0x000524AC
		private void RefreshEntries()
		{
			for (int i = 0; i < this.entries.Length; i++)
			{
				GoldMinerShopUIEntry goldMinerShopUIEntry = this.entries[i];
				if (i >= this.target.stock.Count)
				{
					goldMinerShopUIEntry.gameObject.SetActive(false);
				}
				else
				{
					goldMinerShopUIEntry.gameObject.SetActive(true);
					ShopEntity target = this.target.stock[i];
					goldMinerShopUIEntry.Setup(this, target);
				}
			}
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x0005431C File Offset: 0x0005251C
		public void Setup(GoldMinerShop shop)
		{
			this.UnregisterEvent();
			this.target = shop;
			this.RegisterEvent();
			this.RefreshEntries();
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00054337 File Offset: 0x00052537
		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.RefreshDescriptionText();
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00054348 File Offset: 0x00052548
		private void RefreshDescriptionText()
		{
			string text = "";
			if (this.hoveringEntry != null && this.hoveringEntry.target != null && this.hoveringEntry.target.artifact != null)
			{
				text = this.hoveringEntry.target.artifact.Description;
			}
			this.descriptionText.text = text;
		}

		// Token: 0x040010B7 RID: 4279
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010B8 RID: 4280
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x040010B9 RID: 4281
		[SerializeField]
		private GoldMinerShopUIEntry[] entries;

		// Token: 0x040010BA RID: 4282
		public int navIndex;

		// Token: 0x040010BC RID: 4284
		public bool enableInput;

		// Token: 0x040010BD RID: 4285
		public GoldMinerShopUIEntry hoveringEntry;
	}
}
