using System;
using Duckov.MiniGames.GoldMiner.UI;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AE RID: 686
	public class GoldMinerShopUIContinueBtn : MonoBehaviour
	{
		// Token: 0x060016A3 RID: 5795 RVA: 0x000543B8 File Offset: 0x000525B8
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00054405 File Offset: 0x00052605
		private void OnInteract(NavEntry entry)
		{
			this.shop.Continue();
		}

		// Token: 0x040010BE RID: 4286
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x040010BF RID: 4287
		[SerializeField]
		private NavEntry navEntry;
	}
}
