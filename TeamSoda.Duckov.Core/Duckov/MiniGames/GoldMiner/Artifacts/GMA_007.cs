using System;
using System.Linq;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C2 RID: 706
	public class GMA_007 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600171E RID: 5918 RVA: 0x00055B86 File Offset: 0x00053D86
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.additionalFactorFuncs.Add(new Func<float>(this.AddFactorIfResolved3DifferentKindsOfGold));
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x00055BAD File Offset: 0x00053DAD
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.additionalFactorFuncs.Remove(new Func<float>(this.AddFactorIfResolved3DifferentKindsOfGold));
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x00055BD8 File Offset: 0x00053DD8
		private float AddFactorIfResolved3DifferentKindsOfGold()
		{
			if ((from e in base.GoldMiner.resolvedEntities
			where e != null && e.tags.Contains(GoldMinerEntity.Tag.Gold)
			group e by e.size).Count<IGrouping<GoldMinerEntity.Size, GoldMinerEntity>>() >= 3)
			{
				return 0.5f;
			}
			return 0f;
		}
	}
}
