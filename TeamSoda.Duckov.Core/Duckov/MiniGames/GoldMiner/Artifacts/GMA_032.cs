using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002DB RID: 731
	public class GMA_032 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001769 RID: 5993 RVA: 0x00056583 File Offset: 0x00054783
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraDiamond = Mathf.MoveTowards(base.Run.extraDiamond, 5f, 0.5f);
		}
	}
}
