using System;
using UnityEngine;

namespace ECM2.Examples.FirstPerson
{
	// Token: 0x0200008F RID: 143
	public class FirstPersonCharacterInput : MonoBehaviour
	{
		// Token: 0x06000462 RID: 1122 RVA: 0x00012931 File Offset: 0x00010B31
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00012940 File Offset: 0x00010B40
		private void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = Input.GetAxisRaw("Vertical")
			};
			Vector3 vector2 = Vector3.zero;
			vector2 += this._character.GetRightVector() * vector.x;
			vector2 += this._character.GetForwardVector() * vector.y;
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

		// Token: 0x040002D5 RID: 725
		private Character _character;
	}
}
