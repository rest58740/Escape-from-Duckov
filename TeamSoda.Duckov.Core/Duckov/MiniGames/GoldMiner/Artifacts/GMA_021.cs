using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D0 RID: 720
	public class GMA_021 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600174D RID: 5965 RVA: 0x000562AD File Offset: 0x000544AD
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.eagleEyePotion += 3;
		}
	}
}
