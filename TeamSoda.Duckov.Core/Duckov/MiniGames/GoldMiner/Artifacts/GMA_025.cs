using System;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D4 RID: 724
	public class GMA_025 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001758 RID: 5976 RVA: 0x000563C3 File Offset: 0x000545C3
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.emptySpeed.AddModifier(new Modifier(100, this.addAmount, this));
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x000563EC File Offset: 0x000545EC
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.emptySpeed.RemoveAllModifiersFromSource(this);
		}

		// Token: 0x04001120 RID: 4384
		[SerializeField]
		private float addAmount = 1f;
	}
}
