using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200005D RID: 93
	[CreateAssetMenu(menuName = "Animancer/Controller Transition/Float 1", order = 416)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/Float1ControllerTransitionAsset")]
	public class Float1ControllerTransitionAsset : AnimancerTransitionAsset<Float1ControllerTransition>
	{
		// Token: 0x020000AE RID: 174
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<Float1ControllerTransitionAsset, Float1ControllerTransition, Float1ControllerState>, Float1ControllerState.ITransition, ITransition<Float1ControllerState>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
