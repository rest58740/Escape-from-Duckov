using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CD RID: 717
	public class GMA_018 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001744 RID: 5956 RVA: 0x0005614C File Offset: 0x0005434C
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x000561B8 File Offset: 0x000543B8
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Remove(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner2.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00056222 File Offset: 0x00054422
		private void OnLevelBegin(GoldMiner miner)
		{
			this.remaining = 5;
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0005622B File Offset: 0x0005442B
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (!entity)
			{
				return;
			}
			if (this.remaining < 1)
			{
				return;
			}
			this.remaining--;
			entity.Value = 200;
		}

		// Token: 0x0400111E RID: 4382
		private int remaining;
	}
}
