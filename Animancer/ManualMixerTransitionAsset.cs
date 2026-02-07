using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000066 RID: 102
	[CreateAssetMenu(menuName = "Animancer/Mixer Transition/Manual", order = 412)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/ManualMixerTransitionAsset")]
	public class ManualMixerTransitionAsset : AnimancerTransitionAsset<ManualMixerTransition>
	{
		// Token: 0x020000B2 RID: 178
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<ManualMixerTransitionAsset, ManualMixerTransition, ManualMixerState>, ManualMixerState.ITransition, ITransition<ManualMixerState>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
