using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CC RID: 716
	public class GMA_017 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001741 RID: 5953 RVA: 0x000560FD File Offset: 0x000542FD
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.goldValueFactor.AddModifier(new Modifier(0, 0.2f, this));
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x00056124 File Offset: 0x00054324
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.goldValueFactor.RemoveAllModifiersFromSource(this);
		}
	}
}
