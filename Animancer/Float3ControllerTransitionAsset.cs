using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000061 RID: 97
	[CreateAssetMenu(menuName = "Animancer/Controller Transition/Float 3", order = 418)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/Float3ControllerTransitionAsset")]
	public class Float3ControllerTransitionAsset : AnimancerTransitionAsset<Float3ControllerTransition>
	{
		// Token: 0x020000B0 RID: 176
		[Serializable]
		public new class UnShared : AnimancerTransitionAssetBase.UnShared<Float3ControllerTransitionAsset, Float3ControllerTransition, Float3ControllerState>, Float3ControllerState.ITransition, ITransition<Float3ControllerState>, ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
