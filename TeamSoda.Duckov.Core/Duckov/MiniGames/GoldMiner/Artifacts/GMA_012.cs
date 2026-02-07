using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C7 RID: 711
	public class GMA_012 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001732 RID: 5938 RVA: 0x00055EFD File Offset: 0x000540FD
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.defuse.AddModifier(new Modifier(0, 1f, this));
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x00055F24 File Offset: 0x00054124
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.defuse.RemoveAllModifiersFromSource(this);
		}
	}
}
