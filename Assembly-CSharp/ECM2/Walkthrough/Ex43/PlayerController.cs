using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex43
{
	// Token: 0x02000071 RID: 113
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x0000FBF0 File Offset: 0x0000DDF0
		private void Awake()
		{
			this._character = base.GetComponent<DashingCharacter>();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000FC00 File Offset: 0x0000DE00
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
			if (this._character.camera)
			{
				vector2 = vector2.relativeTo(this._character.cameraTransform, true);
			}
			this._character.SetMovementDirection(vector2);
			if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
			{
				this._character.Crouch();
			}
			else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
			{
				this._character.UnCrouch();
			}
			if (Input.GetButtonDown("Jump"))
			{
				this._character.Jump();
			}
			else if (Input.GetButtonUp("Jump"))
			{
				this._character.StopJumping();
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				this._character.Dash();
				return;
			}
			if (Input.GetKeyUp(KeyCode.E))
			{
				this._character.StopDashing();
			}
		}

		// Token: 0x04000272 RID: 626
		private DashingCharacter _character;
	}
}
