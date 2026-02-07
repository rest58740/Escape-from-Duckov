using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C0 RID: 704
	public class GMA_005 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001716 RID: 5910 RVA: 0x00055A39 File Offset: 0x00053C39
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00055A71 File Offset: 0x00053C71
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x00055AAC File Offset: 0x00053CAC
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (this.remaining < 1)
			{
				return;
			}
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity) && entity.size < GoldMinerEntity.Size.M)
			{
				entity.Value += 500;
				this.remaining--;
			}
		}

		// Token: 0x0400111A RID: 4378
		private int remaining = 3;
	}
}
