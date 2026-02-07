using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A6 RID: 678
	public class AddEagleEyePotionOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x06001681 RID: 5761 RVA: 0x00053CC4 File Offset: 0x00051EC4
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00053D12 File Offset: 0x00051F12
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.eagleEyePotion += this.amount;
		}

		// Token: 0x040010A3 RID: 4259
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x040010A4 RID: 4260
		[SerializeField]
		private int amount = 1;
	}
}
