using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C6 RID: 710
	public class GMA_011 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600172E RID: 5934 RVA: 0x00055E91 File Offset: 0x00054091
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.forceLevelSuccessFuncs.Add(new Func<bool>(this.ForceAndDetach));
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x00055EB8 File Offset: 0x000540B8
		private bool ForceAndDetach()
		{
			base.Run.DetachArtifact(this.master);
			return true;
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x00055ECD File Offset: 0x000540CD
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.forceLevelSuccessFuncs.Remove(new Func<bool>(this.ForceAndDetach));
		}
	}
}
