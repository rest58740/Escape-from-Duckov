using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CF RID: 719
	public class GMA_020 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600174B RID: 5963 RVA: 0x00056287 File Offset: 0x00054487
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strengthPotion += 3;
		}
	}
}
