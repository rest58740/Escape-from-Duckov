using System;
using UnityEngine;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x0200005D RID: 93
	public class MagicBlendState : StateMachineBehaviour
	{
		// Token: 0x06000333 RID: 819 RVA: 0x0000DDE4 File Offset: 0x0000BFE4
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!this._isInitialized)
			{
				this._magicBlending = animator.gameObject.GetComponent<MagicBlending>();
				if (this._magicBlending == null)
				{
					return;
				}
				this._isInitialized = true;
			}
			float duration = animator.GetAnimatorTransitionInfo(layerIndex).duration;
			this._magicBlending.UpdateMagicBlendAsset(this.magicBlendAsset, true, duration, false);
		}

		// Token: 0x0400021B RID: 539
		[SerializeField]
		private MagicBlendAsset magicBlendAsset;

		// Token: 0x0400021C RID: 540
		private bool _isInitialized;

		// Token: 0x0400021D RID: 541
		private MagicBlending _magicBlending;
	}
}
