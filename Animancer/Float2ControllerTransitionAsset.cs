using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200005F RID: 95
	[CreateAssetMenu(menuName = "Animancer/Controller Transition/Float 2", order = 417)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/Float2ControllerTransitionAsset")]
	public class Float2ControllerTransitionAsset : AnimancerTransitionAsset<Float2ControllerTransition>
	{
		// Token: 0x020000AF RID: 175
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<Float2ControllerTransitionAsset, Float2ControllerTransition, Float2ControllerState>, Float2ControllerState.ITransition, ITransition<Float2ControllerState>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
