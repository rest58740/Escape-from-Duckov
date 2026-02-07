using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BF RID: 703
	public class GMA_004 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001712 RID: 5906 RVA: 0x00055987 File Offset: 0x00053B87
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x000559BF File Offset: 0x00053BBF
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x000559F7 File Offset: 0x00053BF7
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity) && entity.size > GoldMinerEntity.Size.M)
			{
				base.Run.levelScoreFactor += 0.3f;
			}
		}
	}
}
