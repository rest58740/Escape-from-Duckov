using System;
using System.Linq;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C1 RID: 705
	public class GMA_006 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600171A RID: 5914 RVA: 0x00055B13 File Offset: 0x00053D13
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.isGoldPredicators.Add(new Func<GoldMinerEntity, bool>(this.SmallRockIsGold));
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00055B3A File Offset: 0x00053D3A
		private bool SmallRockIsGold(GoldMinerEntity entity)
		{
			return entity.tags.Contains(GoldMinerEntity.Tag.Rock) && entity.size < GoldMinerEntity.Size.M;
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00055B56 File Offset: 0x00053D56
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.isGoldPredicators.Remove(new Func<GoldMinerEntity, bool>(this.SmallRockIsGold));
		}
	}
}
