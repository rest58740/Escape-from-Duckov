using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200006B RID: 107
	[CreateAssetMenu(menuName = "Animancer/Playable Asset Transition", order = 419)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/PlayableAssetTransitionAsset")]
	public class PlayableAssetTransitionAsset : AnimancerTransitionAsset<PlayableAssetTransition>
	{
		// Token: 0x020000B5 RID: 181
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<PlayableAssetTransitionAsset, PlayableAssetTransition, PlayableAssetState>, PlayableAssetState.ITransition, ITransition<PlayableAssetState>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
