using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex31
{
	// Token: 0x02000076 RID: 118
	public class AnimationController : MonoBehaviour
	{
		// Token: 0x060003BF RID: 959 RVA: 0x000101E1 File Offset: 0x0000E3E1
		private void Awake()
		{
			this._character = base.GetComponentInParent<Character>();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000101F0 File Offset: 0x0000E3F0
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			Animator animator = this._character.GetAnimator();
			Vector3 vector = base.transform.InverseTransformDirection(this._character.GetMovementDirection());
			float num = (this._character.useRootMotion && this._character.GetRootMotionController()) ? vector.z : Mathf.InverseLerp(0f, this._character.GetMaxSpeed(), this._character.GetSpeed());
			animator.SetFloat(AnimationController.Forward, num, 0.1f, deltaTime);
			animator.SetFloat(AnimationController.Turn, Mathf.Atan2(vector.x, vector.z), 0.1f, deltaTime);
			animator.SetBool(AnimationController.Ground, this._character.IsGrounded());
			animator.SetBool(AnimationController.Crouch, this._character.IsCrouched());
			if (this._character.IsFalling())
			{
				animator.SetFloat(AnimationController.Jump, this._character.GetVelocity().y, 0.1f, deltaTime);
			}
			float value = ((Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1f) < 0.5f) ? 1f : -1f) * num;
			if (this._character.IsGrounded())
			{
				animator.SetFloat(AnimationController.JumpLeg, value);
			}
		}

		// Token: 0x0400027E RID: 638
		private static readonly int Forward = Animator.StringToHash("Forward");

		// Token: 0x0400027F RID: 639
		private static readonly int Turn = Animator.StringToHash("Turn");

		// Token: 0x04000280 RID: 640
		private static readonly int Ground = Animator.StringToHash("OnGround");

		// Token: 0x04000281 RID: 641
		private static readonly int Crouch = Animator.StringToHash("Crouch");

		// Token: 0x04000282 RID: 642
		private static readonly int Jump = Animator.StringToHash("Jump");

		// Token: 0x04000283 RID: 643
		private static readonly int JumpLeg = Animator.StringToHash("JumpLeg");

		// Token: 0x04000284 RID: 644
		private Character _character;
	}
}
