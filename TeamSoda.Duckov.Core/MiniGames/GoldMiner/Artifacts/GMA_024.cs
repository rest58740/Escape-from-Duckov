using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D3 RID: 723
	public class GMA_024 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001755 RID: 5973 RVA: 0x00056377 File Offset: 0x00054577
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.maxStamina.AddModifier(new Modifier(0, 1.5f, this));
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0005639E File Offset: 0x0005459E
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.maxStamina.RemoveAllModifiersFromSource(this);
		}
	}
}
