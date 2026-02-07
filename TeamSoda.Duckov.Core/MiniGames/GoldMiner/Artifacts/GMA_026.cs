using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D5 RID: 725
	public class GMA_026 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600175B RID: 5979 RVA: 0x0005641C File Offset: 0x0005461C
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshPrice.AddModifier(new Modifier(200, -1f, this));
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00056447 File Offset: 0x00054647
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshPrice.RemoveAllModifiersFromSource(this);
		}
	}
}
