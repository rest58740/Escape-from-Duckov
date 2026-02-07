using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A7 RID: 679
	public class AddStrengthPotionOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x06001684 RID: 5764 RVA: 0x00053D3C File Offset: 0x00051F3C
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00053D8A File Offset: 0x00051F8A
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.strengthPotion += this.amount;
		}

		// Token: 0x040010A5 RID: 4261
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x040010A6 RID: 4262
		[SerializeField]
		private int amount = 1;
	}
}
