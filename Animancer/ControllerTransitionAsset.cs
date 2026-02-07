using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200005A RID: 90
	[CreateAssetMenu(menuName = "Animancer/Controller Transition/Base", order = 415)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/ControllerTransitionAsset")]
	public class ControllerTransitionAsset : AnimancerTransitionAsset<ControllerTransition>
	{
		// Token: 0x020000AD RID: 173
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<ControllerTransitionAsset, ControllerTransition, ControllerState>, ControllerState.ITransition, ITransition<ControllerState>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
