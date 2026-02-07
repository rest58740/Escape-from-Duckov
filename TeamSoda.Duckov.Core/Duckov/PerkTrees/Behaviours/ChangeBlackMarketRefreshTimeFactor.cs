using System;
using Duckov.BlackMarkets;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x02000265 RID: 613
	public class ChangeBlackMarketRefreshTimeFactor : PerkBehaviour
	{
		// Token: 0x0600135C RID: 4956 RVA: 0x000495A5 File Offset: 0x000477A5
		protected override void OnAwake()
		{
			base.OnAwake();
			BlackMarket.onRequestRefreshTime += this.HandleEvent;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000495BE File Offset: 0x000477BE
		protected override void OnOnDestroy()
		{
			base.OnOnDestroy();
			BlackMarket.onRequestRefreshTime -= this.HandleEvent;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x000495D7 File Offset: 0x000477D7
		private void HandleEvent(BlackMarket.OnRequestRefreshTimeFactorEventContext context)
		{
			if (base.Master == null)
			{
				return;
			}
			if (!base.Master.Unlocked)
			{
				return;
			}
			context.Add(this.amount);
		}

		// Token: 0x04000EA4 RID: 3748
		[SerializeField]
		private float amount = -0.1f;
	}
}
