using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D9 RID: 729
	public class GMA_030 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001765 RID: 5989 RVA: 0x00056513 File Offset: 0x00054713
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraRocks = Mathf.MoveTowards(base.Run.extraRocks, 5f, 1f);
		}
	}
}
