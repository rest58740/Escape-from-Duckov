using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex31
{
	// Token: 0x02000077 RID: 119
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x060003C3 RID: 963 RVA: 0x000103BF File Offset: 0x0000E5BF
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000103D0 File Offset: 0x0000E5D0
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
				return;
			}
			if (Input.GetButtonUp("Jump"))
			{
				this._character.StopJumping();
			}
		}

		// Token: 0x04000285 RID: 645
		private Character _character;
	}
}
