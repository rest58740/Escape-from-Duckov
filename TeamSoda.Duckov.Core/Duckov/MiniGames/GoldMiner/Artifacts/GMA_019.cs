using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CE RID: 718
	public class GMA_019 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001749 RID: 5961 RVA: 0x00056261 File Offset: 0x00054461
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.bomb += 3;
		}
	}
}
