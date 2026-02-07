using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BE RID: 702
	public class GMA_003 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600170E RID: 5902 RVA: 0x000558A6 File Offset: 0x00053AA6
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x000558DE File Offset: 0x00053ADE
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00055918 File Offset: 0x00053B18
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity))
			{
				Debug.Log("Enity is Rock ", entity);
				this.streak++;
			}
			else
			{
				this.streak = 0;
			}
			if (this.streak > 1)
			{
				base.Run.levelScoreFactor += 0.1f;
			}
		}

		// Token: 0x04001119 RID: 4377
		private int streak;
	}
}
