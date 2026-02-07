using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C5 RID: 709
	public class GMA_010 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600172B RID: 5931 RVA: 0x00055E57 File Offset: 0x00054057
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.minMoneySum = 1000;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00055E72 File Offset: 0x00054072
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.minMoneySum = 0;
		}
	}
}
