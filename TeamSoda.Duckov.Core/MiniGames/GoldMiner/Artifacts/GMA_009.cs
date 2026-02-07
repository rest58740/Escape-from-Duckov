using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C4 RID: 708
	public class GMA_009 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001727 RID: 5927 RVA: 0x00055D88 File Offset: 0x00053F88
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x00055DC0 File Offset: 0x00053FC0
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x00055DF8 File Offset: 0x00053FF8
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity))
			{
				this.effectActive = true;
			}
			if (this.effectActive && base.Run.IsGold(entity))
			{
				this.effectActive = false;
				entity.Value *= 2;
			}
		}

		// Token: 0x0400111C RID: 4380
		private bool effectActive;
	}
}
