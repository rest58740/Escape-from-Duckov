using System;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D1 RID: 721
	public class GMA_022 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600174F RID: 5967 RVA: 0x000562D3 File Offset: 0x000544D3
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.charm.AddModifier(new Modifier(0, this.amount, this));
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x000562FB File Offset: 0x000544FB
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.charm.RemoveAllModifiersFromSource(this);
		}

		// Token: 0x0400111F RID: 4383
		[SerializeField]
		private float amount = 0.1f;
	}
}
