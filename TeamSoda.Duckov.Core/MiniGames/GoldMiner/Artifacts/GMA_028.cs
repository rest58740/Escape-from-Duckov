using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D7 RID: 727
	public class GMA_028 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001761 RID: 5985 RVA: 0x000564B8 File Offset: 0x000546B8
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopTicket++;
		}
	}
}
