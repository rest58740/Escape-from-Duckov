using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex92
{
	// Token: 0x02000063 RID: 99
	public class AnimationController : MonoBehaviour
	{
		// Token: 0x06000347 RID: 839 RVA: 0x0000E839 File Offset: 0x0000CA39
		private void Awake()
		{
			this._character = base.GetComponentInParent<Character>();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000E848 File Offset: 0x0000CA48
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

		// Token: 0x04000239 RID: 569
		private static readonly int Forward = Animator.StringToHash("Forward");

		// Token: 0x0400023A RID: 570
		private static readonly int Turn = Animator.StringToHash("Turn");

		// Token: 0x0400023B RID: 571
		private static readonly int Ground = Animator.StringToHash("OnGround");

		// Token: 0x0400023C RID: 572
		private static readonly int Crouch = Animator.StringToHash("Crouch");

		// Token: 0x0400023D RID: 573
		private static readonly int Jump = Animator.StringToHash("Jump");

		// Token: 0x0400023E RID: 574
		private static readonly int JumpLeg = Animator.StringToHash("JumpLeg");

		// Token: 0x0400023F RID: 575
		private Character _character;
	}
}
