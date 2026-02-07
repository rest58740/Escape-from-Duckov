using System;
using UnityEngine;

namespace ECM2.Examples.Ladders
{
	// Token: 0x02000087 RID: 135
	public class CharacterInput : MonoBehaviour
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x0001185F File Offset: 0x0000FA5F
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
			this._ladderClimbAbility = base.GetComponent<LadderClimbAbility>();
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001187C File Offset: 0x0000FA7C
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
				this._ladderClimbAbility.Climb();
				return;
			}
			if (Input.GetKeyUp(KeyCode.E))
			{
				this._ladderClimbAbility.StopClimbing();
			}
		}

		// Token: 0x040002AA RID: 682
		private Character _character;

		// Token: 0x040002AB RID: 683
		private LadderClimbAbility _ladderClimbAbility;
	}
}
