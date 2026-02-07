using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex42
{
	// Token: 0x02000072 RID: 114
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x060003A4 RID: 932 RVA: 0x0000FD41 File Offset: 0x0000DF41
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
			this._sprintAbility = base.GetComponent<SprintAbility>();
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000FD5C File Offset: 0x0000DF5C
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
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				this._sprintAbility.Sprint();
				return;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				this._sprintAbility.StopSprinting();
			}
		}

		// Token: 0x04000273 RID: 627
		private Character _character;

		// Token: 0x04000274 RID: 628
		private SprintAbility _sprintAbility;
	}
}
