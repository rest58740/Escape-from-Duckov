using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BC RID: 700
	public class GMA_001 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001705 RID: 5893 RVA: 0x00055638 File Offset: 0x00053838
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			this.cachedRun = base.Run;
			this.staminaModifier = new Modifier(0, 1f, this);
			this.scoreFactorModifier = new Modifier(0, 1f, this);
			this.cachedRun.staminaDrain.AddModifier(this.staminaModifier);
			this.cachedRun.scoreFactorBase.AddModifier(this.scoreFactorModifier);
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000556AA File Offset: 0x000538AA
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (this.cachedRun == null)
			{
				return;
			}
			this.cachedRun.staminaDrain.RemoveModifier(this.staminaModifier);
			this.cachedRun.scoreFactorBase.RemoveModifier(this.scoreFactorModifier);
		}

		// Token: 0x04001114 RID: 4372
		private Modifier staminaModifier;

		// Token: 0x04001115 RID: 4373
		private Modifier scoreFactorModifier;

		// Token: 0x04001116 RID: 4374
		private GoldMinerRunData cachedRun;
	}
}
