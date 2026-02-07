using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D8 RID: 728
	public class GMA_029 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001763 RID: 5987 RVA: 0x000564DE File Offset: 0x000546DE
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			if (base.Run.shopCapacity >= 6)
			{
				return;
			}
			base.Run.shopCapacity++;
		}
	}
}
