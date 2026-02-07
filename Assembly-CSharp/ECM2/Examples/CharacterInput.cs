using System;
using UnityEngine;

namespace ECM2.Examples
{
	// Token: 0x0200007A RID: 122
	public class CharacterInput : MonoBehaviour
	{
		// Token: 0x060003CB RID: 971 RVA: 0x00010704 File Offset: 0x0000E904
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00010714 File Offset: 0x0000E914
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

		// Token: 0x04000287 RID: 647
		private Character _character;
	}
}
