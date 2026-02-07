using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A5 RID: 677
	public class AddBombOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x0600167E RID: 5758 RVA: 0x00053C4C File Offset: 0x00051E4C
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00053C9A File Offset: 0x00051E9A
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.bomb += this.amount;
		}

		// Token: 0x040010A1 RID: 4257
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x040010A2 RID: 4258
		[SerializeField]
		private int amount = 1;
	}
}
