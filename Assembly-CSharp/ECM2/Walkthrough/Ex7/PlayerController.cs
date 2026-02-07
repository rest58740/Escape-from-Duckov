using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex7
{
	// Token: 0x02000067 RID: 103
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x06000361 RID: 865 RVA: 0x0000F040 File Offset: 0x0000D240
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000F050 File Offset: 0x0000D250
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

		// Token: 0x0400025A RID: 602
		private Character _character;
	}
}
