using System;
using UnityEngine;

namespace ECM2.Examples.SideScrolling
{
	// Token: 0x02000083 RID: 131
	public class SideScrollingCharacter : Character
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x00011329 File Offset: 0x0000F529
		protected override void Awake()
		{
			base.Awake();
			base.SetRotationMode(Character.RotationMode.None);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00011338 File Offset: 0x0000F538
		private void Update()
		{
			float axisRaw = Input.GetAxisRaw("Horizontal");
			base.SetMovementDirection(Vector3.right * axisRaw);
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
			if (axisRaw != 0f)
			{
				this.SetYaw(axisRaw * 90f);
			}
		}
	}
}
