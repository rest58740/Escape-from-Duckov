using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002DA RID: 730
	public class GMA_031 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001767 RID: 5991 RVA: 0x0005654B File Offset: 0x0005474B
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraGold = Mathf.MoveTowards(base.Run.extraGold, 5f, 1f);
		}
	}
}
