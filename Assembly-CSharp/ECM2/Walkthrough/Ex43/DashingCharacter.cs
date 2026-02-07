using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex43
{
	// Token: 0x02000070 RID: 112
	public class DashingCharacter : Character
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000FA7C File Offset: 0x0000DC7C
		protected bool dashInputPressed
		{
			get
			{
				return this._dashInputPressed;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000FA84 File Offset: 0x0000DC84
		public bool IsDashing()
		{
			return base.movementMode == Character.MovementMode.Custom && base.customMovementMode == 1;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000FA9A File Offset: 0x0000DC9A
		public void Dash()
		{
			this._dashInputPressed = true;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000FAA3 File Offset: 0x0000DCA3
		public void StopDashing()
		{
			this._dashInputPressed = false;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		public bool IsDashAllowed()
		{
			return !this.IsCrouched() && this.canEverDash && (this.IsWalking() || this.IsFalling());
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000FAD4 File Offset: 0x0000DCD4
		protected virtual void DoDash()
		{
			Vector3 vector = base.GetMovementDirection();
			if (vector.isZero())
			{
				vector = this.GetForwardVector();
			}
			Vector3 normalized = vector.onlyXZ().normalized;
			base.SetVelocity(normalized * this.dashImpulse);
			base.SetMovementMode(Character.MovementMode.Custom, 1);
			if (base.rotationMode == Character.RotationMode.OrientRotationToMovement)
			{
				base.SetRotation(Quaternion.LookRotation(normalized));
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000FB35 File Offset: 0x0000DD35
		protected virtual void ResetDashState()
		{
			this._dashingTime = 0f;
			this._dashInputPressed = false;
			base.SetVelocity(Vector3.zero);
			base.SetMovementMode(Character.MovementMode.Falling, 0);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000FB5C File Offset: 0x0000DD5C
		protected virtual void DashingMovementMode(float deltaTime)
		{
			base.SetMovementDirection(Vector3.zero);
			this._dashingTime += deltaTime;
			if (this._dashingTime >= this.dashDuration)
			{
				this.ResetDashState();
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000FB8B File Offset: 0x0000DD8B
		protected override void OnBeforeSimulationUpdate(float deltaTime)
		{
			base.OnBeforeSimulationUpdate(deltaTime);
			if (!this.IsDashing() && this.dashInputPressed && this.IsDashAllowed())
			{
				this.DoDash();
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000FBB2 File Offset: 0x0000DDB2
		protected override void CustomMovementMode(float deltaTime)
		{
			base.CustomMovementMode(deltaTime);
			if (base.customMovementMode == 1)
			{
				this.DashingMovementMode(deltaTime);
			}
		}

		// Token: 0x0400026D RID: 621
		[Space(15f)]
		[Tooltip("Is the character able to Dash?")]
		public bool canEverDash = true;

		// Token: 0x0400026E RID: 622
		[Tooltip("Dash initial impulse.")]
		public float dashImpulse = 20f;

		// Token: 0x0400026F RID: 623
		[Tooltip("Dash duration in seconds.")]
		public float dashDuration = 0.15f;

		// Token: 0x04000270 RID: 624
		protected float _dashingTime;

		// Token: 0x04000271 RID: 625
		protected bool _dashInputPressed;

		// Token: 0x020000CF RID: 207
		public enum ECustomMovementMode
		{
			// Token: 0x04000426 RID: 1062
			None,
			// Token: 0x04000427 RID: 1063
			Dashing
		}
	}
}
