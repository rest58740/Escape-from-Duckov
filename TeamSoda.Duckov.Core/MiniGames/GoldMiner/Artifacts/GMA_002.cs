using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BD RID: 701
	public class GMA_002 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001708 RID: 5896 RVA: 0x000556EC File Offset: 0x000538EC
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (this.master == null)
			{
				return;
			}
			if (base.GoldMiner == null)
			{
				return;
			}
			this.modifer = new Modifier(200, -0.5f, this);
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onHookBeginRetrieve = (Action<GoldMiner, Hook>)Delegate.Combine(goldMiner2.onHookBeginRetrieve, new Action<GoldMiner, Hook>(this.OnBeginRetrieve));
			GoldMiner goldMiner3 = base.GoldMiner;
			goldMiner3.onHookEndRetrieve = (Action<GoldMiner, Hook>)Delegate.Combine(goldMiner3.onHookEndRetrieve, new Action<GoldMiner, Hook>(this.OnEndRetrieve));
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x000557A4 File Offset: 0x000539A4
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onHookBeginRetrieve = (Action<GoldMiner, Hook>)Delegate.Remove(goldMiner2.onHookBeginRetrieve, new Action<GoldMiner, Hook>(this.OnBeginRetrieve));
			GoldMiner goldMiner3 = base.GoldMiner;
			goldMiner3.onHookEndRetrieve = (Action<GoldMiner, Hook>)Delegate.Remove(goldMiner3.onHookEndRetrieve, new Action<GoldMiner, Hook>(this.OnEndRetrieve));
			if (base.Run != null)
			{
				base.Run.staminaDrain.RemoveModifier(this.modifer);
			}
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x00055854 File Offset: 0x00053A54
		private void OnBeginRetrieve(GoldMiner miner, Hook hook)
		{
			if (!this.effectActive)
			{
				return;
			}
			base.Run.staminaDrain.AddModifier(this.modifer);
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00055875 File Offset: 0x00053A75
		private void OnEndRetrieve(GoldMiner miner, Hook hook)
		{
			base.Run.staminaDrain.RemoveModifier(this.modifer);
			this.effectActive = false;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00055895 File Offset: 0x00053A95
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			this.effectActive = true;
		}

		// Token: 0x04001117 RID: 4375
		private Modifier modifer;

		// Token: 0x04001118 RID: 4376
		private bool effectActive;
	}
}
