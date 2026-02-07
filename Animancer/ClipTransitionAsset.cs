using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000057 RID: 87
	[CreateAssetMenu(menuName = "Animancer/Clip Transition", order = 411)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/ClipTransitionAsset")]
	public class ClipTransitionAsset : AnimancerTransitionAsset<ClipTransition>
	{
		// Token: 0x020000AB RID: 171
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<ClipTransitionAsset, ClipTransition, ClipState>, ClipState.ITransition, ITransition<ClipState>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
