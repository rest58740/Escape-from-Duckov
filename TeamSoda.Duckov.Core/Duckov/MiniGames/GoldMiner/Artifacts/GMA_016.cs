using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CB RID: 715
	public class GMA_016 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600173E RID: 5950 RVA: 0x000560B1 File Offset: 0x000542B1
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.rockValueFactor.AddModifier(new Modifier(0, 1f, this));
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x000560D8 File Offset: 0x000542D8
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.rockValueFactor.RemoveAllModifiersFromSource(this);
		}
	}
}
