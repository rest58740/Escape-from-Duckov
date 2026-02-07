using System;
using Duckov.BlackMarkets;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x02000264 RID: 612
	public class AddBlackMarketRefreshChance : PerkBehaviour
	{
		// Token: 0x06001357 RID: 4951 RVA: 0x00049532 File Offset: 0x00047732
		protected override void OnAwake()
		{
			base.OnAwake();
			BlackMarket.onRequestMaxRefreshChance += this.HandleEvent;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0004954B File Offset: 0x0004774B
		protected override void OnOnDestroy()
		{
			base.OnOnDestroy();
			BlackMarket.onRequestMaxRefreshChance -= this.HandleEvent;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00049564 File Offset: 0x00047764
		private void HandleEvent(BlackMarket.OnRequestMaxRefreshChanceEventContext context)
		{
			if (base.Master == null)
			{
				return;
			}
			if (!base.Master.Unlocked)
			{
				return;
			}
			context.Add(this.addAmount);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0004958F File Offset: 0x0004778F
		protected override void OnUnlocked()
		{
			BlackMarket.NotifyMaxRefreshChanceChanged();
		}

		// Token: 0x04000EA3 RID: 3747
		[SerializeField]
		private int addAmount = 1;
	}
}
