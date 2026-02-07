using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000069 RID: 105
	[CreateAssetMenu(menuName = "Animancer/Mixer Transition/2D", order = 414)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/MixerTransition2DAsset")]
	public class MixerTransition2DAsset : AnimancerTransitionAsset<MixerTransition2D>
	{
		// Token: 0x020000B3 RID: 179
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<MixerTransition2DAsset, MixerTransition2D, MixerState<Vector2>>, ManualMixerState.ITransition2D, ITransition<MixerState<Vector2>>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
