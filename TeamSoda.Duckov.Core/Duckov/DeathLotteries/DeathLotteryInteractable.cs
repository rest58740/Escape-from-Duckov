using System;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000315 RID: 789
	public class DeathLotteryInteractable : InteractableBase
	{
		// Token: 0x060019E1 RID: 6625 RVA: 0x0005EB83 File Offset: 0x0005CD83
		protected override bool IsInteractable()
		{
			return !(this.deathLottery == null) && this.deathLottery.CurrentStatus.valid && !this.deathLottery.Loading;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0005EBB9 File Offset: 0x0005CDB9
		protected override void OnInteractFinished()
		{
			this.deathLottery.RequestUI();
		}

		// Token: 0x040012CE RID: 4814
		[SerializeField]
		private DeathLottery deathLottery;
	}
}
