using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D6 RID: 726
	public class GMA_027 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600175E RID: 5982 RVA: 0x0005646C File Offset: 0x0005466C
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshChances.AddModifier(new Modifier(0, 1f, this));
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x00056493 File Offset: 0x00054693
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshChances.RemoveAllModifiersFromSource(this);
		}
	}
}
