using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C9 RID: 713
	public class GMA_014 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001736 RID: 5942 RVA: 0x00055F51 File Offset: 0x00054151
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00055F89 File Offset: 0x00054189
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00055FC1 File Offset: 0x000541C1
		private void OnAfterResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run == null)
			{
				return;
			}
			if (!base.Run.IsPig(entity))
			{
				return;
			}
			base.Run.stamina += 2f;
		}
	}
}
