using System;
using UnityEngine;

namespace ECM2.Examples.FirstPersonFly
{
	// Token: 0x02000092 RID: 146
	public class CharacterInput : MonoBehaviour
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x00012CF5 File Offset: 0x00010EF5
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00012D04 File Offset: 0x00010F04
		private void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = Input.GetAxisRaw("Vertical")
			};
			Vector3 vector2 = Vector3.zero;
			if (this._character.IsFlying())
			{
				vector2 += this._character.GetRightVector() * vector.x;
				Vector3 a = this._character.camera ? this._character.cameraTransform.forward : this._character.GetForwardVector();
				vector2 += a * vector.y;
				if (this._character.jumpInputPressed)
				{
					vector2 += Vector3.up;
				}
			}
			else
			{
				vector2 += this._character.GetRightVector() * vector.x;
				vector2 += this._character.GetForwardVector() * vector.y;
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

		// Token: 0x040002DC RID: 732
		private Character _character;
	}
}
