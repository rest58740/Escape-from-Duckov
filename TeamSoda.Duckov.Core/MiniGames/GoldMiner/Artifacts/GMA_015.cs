using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CA RID: 714
	public class GMA_015 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600173A RID: 5946 RVA: 0x00056004 File Offset: 0x00054204
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0005603C File Offset: 0x0005423C
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00056074 File Offset: 0x00054274
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (base.Run == null)
			{
				return;
			}
			if (!base.Run.IsPig(entity))
			{
				return;
			}
			entity.Value += this.amount;
		}

		// Token: 0x0400111D RID: 4381
		[SerializeField]
		private int amount = 20;
	}
}
