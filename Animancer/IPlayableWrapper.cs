using System;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000030 RID: 48
	public interface IPlayableWrapper
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000348 RID: 840
		IPlayableWrapper Parent { get; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000349 RID: 841
		float Weight { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600034A RID: 842
		Playable Playable { get; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600034B RID: 843
		int ChildCount { get; }

		// Token: 0x0600034C RID: 844
		AnimancerNode GetChild(int index);

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600034D RID: 845
		bool KeepChildrenConnected { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600034E RID: 846
		// (set) Token: 0x0600034F RID: 847
		float Speed { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000350 RID: 848
		// (set) Token: 0x06000351 RID: 849
		bool ApplyAnimatorIK { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000352 RID: 850
		// (set) Token: 0x06000353 RID: 851
		bool ApplyFootIK { get; set; }
	}
}
