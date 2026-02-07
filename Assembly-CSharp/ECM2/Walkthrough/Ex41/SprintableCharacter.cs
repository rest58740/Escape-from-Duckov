using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex41
{
	// Token: 0x02000074 RID: 116
	public class SprintableCharacter : Character
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x0000FFB6 File Offset: 0x0000E1B6
		public void Sprint()
		{
			this._sprintInputPressed = true;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000FFBF File Offset: 0x0000E1BF
		public void StopSprinting()
		{
			this._sprintInputPressed = false;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		public bool IsSprinting()
		{
			return this._isSprinting;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000FFD0 File Offset: 0x0000E1D0
		private bool CanSprint()
		{
			return this.IsWalking() && !this.IsCrouched();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000FFE8 File Offset: 0x0000E1E8
		private void CheckSprintInput()
		{
			if (!this._isSprinting && this._sprintInputPressed && this.CanSprint())
			{
				this._isSprinting = true;
				return;
			}
			if (this._isSprinting && (!this._sprintInputPressed || !this.CanSprint()))
			{
				this._isSprinting = false;
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00010034 File Offset: 0x0000E234
		public override float GetMaxSpeed()
		{
			if (!this._isSprinting)
			{
				return base.GetMaxSpeed();
			}
			return this.maxSprintSpeed;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0001004B File Offset: 0x0000E24B
		protected override void OnBeforeSimulationUpdate(float deltaTime)
		{
			base.OnBeforeSimulationUpdate(deltaTime);
			this.CheckSprintInput();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001005C File Offset: 0x0000E25C
		private void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = Input.GetAxisRaw("Vertical")
			};
			Vector3 vector2 = Vector3.zero;
			vector2 += Vector3.right * vector.x;
			vector2 += Vector3.forward * vector.y;
			if (base.camera)
			{
				vector2 = vector2.relativeTo(base.cameraTransform, true);
			}
			base.SetMovementDirection(vector2);
			if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
			{
				this.Crouch();
			}
			else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
			{
				this.UnCrouch();
			}
			if (Input.GetButtonDown("Jump"))
			{
				this.Jump();
			}
			else if (Input.GetButtonUp("Jump"))
			{
				this.StopJumping();
			}
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				this.Sprint();
				return;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				this.StopSprinting();
			}
		}

		// Token: 0x0400027A RID: 634
		[Space(15f)]
		public float maxSprintSpeed = 10f;

		// Token: 0x0400027B RID: 635
		private bool _isSprinting;

		// Token: 0x0400027C RID: 636
		private bool _sprintInputPressed;
	}
}
