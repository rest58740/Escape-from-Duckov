using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex21
{
	// Token: 0x02000079 RID: 121
	public class PlayerCharacter : Character
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00010610 File Offset: 0x0000E810
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
				return;
			}
			if (Input.GetButtonUp("Jump"))
			{
				this.StopJumping();
			}
		}
	}
}
