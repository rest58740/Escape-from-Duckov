using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex22
{
	// Token: 0x02000078 RID: 120
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x000104E7 File Offset: 0x0000E6E7
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000104F8 File Offset: 0x0000E6F8
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

		// Token: 0x04000286 RID: 646
		private Character _character;
	}
}
