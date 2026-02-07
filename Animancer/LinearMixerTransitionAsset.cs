using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000063 RID: 99
	[CreateAssetMenu(menuName = "Animancer/Mixer Transition/Linear", order = 413)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/LinearMixerTransitionAsset")]
	public class LinearMixerTransitionAsset : AnimancerTransitionAsset<LinearMixerTransition>
	{
		// Token: 0x020000B1 RID: 177
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<LinearMixerTransitionAsset, LinearMixerTransition, LinearMixerState>, LinearMixerState.ITransition, ITransition<LinearMixerState>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
