using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D2 RID: 722
	public class GMA_023 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001752 RID: 5970 RVA: 0x0005632B File Offset: 0x0005452B
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strength.AddModifier(new Modifier(0, 10f, this));
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00056352 File Offset: 0x00054552
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strength.RemoveAllModifiersFromSource(this);
		}
	}
}
