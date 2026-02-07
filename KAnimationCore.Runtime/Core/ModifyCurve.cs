using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x02000023 RID: 35
	public class ModifyCurve : StateMachineBehaviour
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003C0A File Offset: 0x00001E0A
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!this._isInitialized)
			{
				this._paramId = Animator.StringToHash(this.paramName);
				this._isInitialized = true;
			}
			this._paramStartValue = animator.GetFloat(this._paramId);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003C40 File Offset: 0x00001E40
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			int fullPathHash = animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash;
			if (fullPathHash != stateInfo.fullPathHash && fullPathHash != 0)
			{
				return;
			}
			float t;
			if (animator.IsInTransition(layerIndex))
			{
				t = animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime;
			}
			else
			{
				t = 1f;
			}
			animator.SetFloat(this._paramId, Mathf.Lerp(this._paramStartValue, this.paramTargetValue, t));
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003CB0 File Offset: 0x00001EB0
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		// Token: 0x04000053 RID: 83
		[SerializeField]
		private string paramName;

		// Token: 0x04000054 RID: 84
		[SerializeField]
		private float paramTargetValue;

		// Token: 0x04000055 RID: 85
		private int _paramId;

		// Token: 0x04000056 RID: 86
		private float _paramStartValue;

		// Token: 0x04000057 RID: 87
		private bool _isInitialized;
	}
}
