using System;
using UnityEngine;

namespace ECM2.Examples.Jump
{
	// Token: 0x0200008A RID: 138
	public class CharacterInput : MonoBehaviour
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x0001202C File Offset: 0x0001022C
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
			this._jumpAbility = base.GetComponent<JumpAbility>();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00012048 File Offset: 0x00010248
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
				this._jumpAbility.Jump();
				return;
			}
			if (Input.GetButtonUp("Jump"))
			{
				this._jumpAbility.StopJumping();
			}
		}

		// Token: 0x040002BD RID: 701
		private Character _character;

		// Token: 0x040002BE RID: 702
		private JumpAbility _jumpAbility;
	}
}
