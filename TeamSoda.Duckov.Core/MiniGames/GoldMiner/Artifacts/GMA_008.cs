using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C3 RID: 707
	public class GMA_008 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001722 RID: 5922 RVA: 0x00055C54 File Offset: 0x00053E54
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x00055CC0 File Offset: 0x00053EC0
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Remove(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00055D2A File Offset: 0x00053F2A
		private void OnLevelBegin(GoldMiner miner)
		{
			this.triggered = false;
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00055D34 File Offset: 0x00053F34
		private void OnAfterResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (this.triggered)
			{
				return;
			}
			if (base.GoldMiner.activeEntities.Count <= 0)
			{
				this.triggered = true;
				base.Run.charm.BaseValue += 0.5f;
			}
		}

		// Token: 0x0400111B RID: 4379
		private bool triggered;
	}
}
