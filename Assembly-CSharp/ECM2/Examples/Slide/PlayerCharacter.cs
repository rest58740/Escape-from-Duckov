using System;
using UnityEngine;

namespace ECM2.Examples.Slide
{
	// Token: 0x02000082 RID: 130
	public class PlayerCharacter : Character
	{
		// Token: 0x060003F8 RID: 1016 RVA: 0x00011054 File Offset: 0x0000F254
		public override float GetMaxSpeed()
		{
			if (!this.IsSliding())
			{
				return base.GetMaxSpeed();
			}
			return base.maxWalkSpeed;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001106B File Offset: 0x0000F26B
		public override float GetMaxAcceleration()
		{
			if (!this.IsSliding())
			{
				return base.GetMaxAcceleration();
			}
			return base.maxAcceleration * 0.1f;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00011088 File Offset: 0x0000F288
		public override bool IsWalking()
		{
			return this.IsSliding() || base.IsWalking();
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001109A File Offset: 0x0000F29A
		public bool IsSliding()
		{
			return base.movementMode == Character.MovementMode.Custom && base.customMovementMode == 1;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000110B0 File Offset: 0x0000F2B0
		protected virtual bool CanSlide()
		{
			if (!base.IsGrounded())
			{
				return false;
			}
			float sqrMagnitude = base.velocity.sqrMagnitude;
			float num = base.maxWalkSpeedCrouched * base.maxWalkSpeedCrouched;
			return sqrMagnitude >= num * 1.02f;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x000110F0 File Offset: 0x0000F2F0
		protected virtual Vector3 CalcSlideDirection()
		{
			Vector3 vector = base.GetMovementDirection();
			if (vector.isZero())
			{
				vector = base.GetVelocity();
			}
			else if (vector.isZero())
			{
				vector = this.GetForwardVector();
			}
			return this.ConstrainInputVector(vector).normalized;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00011134 File Offset: 0x0000F334
		protected virtual void CheckSlideInput()
		{
			bool flag = this.IsSliding();
			bool crouchInputPressed = base.crouchInputPressed;
			if (!flag && crouchInputPressed && this.CanSlide())
			{
				base.SetMovementMode(Character.MovementMode.Custom, 1);
				return;
			}
			if (flag && (!crouchInputPressed || !this.CanSlide()))
			{
				base.SetMovementMode(Character.MovementMode.Walking, 0);
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00011180 File Offset: 0x0000F380
		protected unsafe override void OnMovementModeChanged(Character.MovementMode prevMovementMode, int prevCustomMode)
		{
			base.OnMovementModeChanged(prevMovementMode, prevCustomMode);
			if (this.IsSliding())
			{
				Vector3 a = this.CalcSlideDirection();
				*base.characterMovement.velocity += a * this.slideImpulse;
				base.SetRotationMode(Character.RotationMode.None);
			}
			if (prevMovementMode == Character.MovementMode.Custom && prevCustomMode == 1)
			{
				base.SetRotationMode(Character.RotationMode.OrientRotationToMovement);
				if (this.IsFalling())
				{
					Vector3 onNormal = -base.GetGravityDirection();
					Vector3 b = Vector3.Project(base.velocity, onNormal);
					Vector3 a2 = Vector3.ClampMagnitude(base.velocity - b, base.maxWalkSpeed);
					*base.characterMovement.velocity = a2 + b;
				}
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00011236 File Offset: 0x0000F436
		protected override void OnBeforeSimulationUpdate(float deltaTime)
		{
			base.OnBeforeSimulationUpdate(deltaTime);
			this.CheckSlideInput();
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00011248 File Offset: 0x0000F448
		protected unsafe virtual void SlidingMovementMode(float deltaTime)
		{
			Vector3 desiredVelocity = Vector3.Project(this.GetDesiredVelocity(), this.GetRightVector());
			*base.characterMovement.velocity = this.CalcVelocity(*base.characterMovement.velocity, desiredVelocity, base.groundFriction * 0.2f, false, deltaTime);
			Vector3 normalized = Vector3.ProjectOnPlane(base.GetGravityDirection(), base.characterMovement.groundNormal).normalized;
			*base.characterMovement.velocity += this.slideDownAcceleration * deltaTime * normalized;
			if (base.applyStandingDownwardForce)
			{
				this.ApplyDownwardsForce();
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x000112F2 File Offset: 0x0000F4F2
		protected override void CustomMovementMode(float deltaTime)
		{
			base.CustomMovementMode(deltaTime);
			if (base.customMovementMode == 1)
			{
				this.SlidingMovementMode(deltaTime);
			}
		}

		// Token: 0x0400029F RID: 671
		[Space(15f)]
		public float slideImpulse = 20f;

		// Token: 0x040002A0 RID: 672
		public float slideDownAcceleration = 20f;

		// Token: 0x020000D0 RID: 208
		private enum ECustomMovementMode
		{
			// Token: 0x04000429 RID: 1065
			Sliding = 1
		}
	}
}
