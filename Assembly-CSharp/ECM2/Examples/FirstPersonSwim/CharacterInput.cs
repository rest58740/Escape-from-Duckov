using System;
using UnityEngine;

namespace ECM2.Examples.FirstPersonSwim
{
	// Token: 0x02000091 RID: 145
	public class CharacterInput : MonoBehaviour
	{
		// Token: 0x06000469 RID: 1129 RVA: 0x00012B11 File Offset: 0x00010D11
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00012B20 File Offset: 0x00010D20
		private void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = Input.GetAxisRaw("Vertical")
			};
			Vector3 vector2 = Vector3.zero;
			if (this._character.IsSwimming())
			{
				vector2 += this._character.GetRightVector() * vector.x;
				Vector3 a = this._character.camera ? this._character.cameraTransform.forward : this._character.GetForwardVector();
				vector2 += a * vector.y;
				if (this._character.jumpInputPressed)
				{
					if (this._character.CalcImmersionDepth() > 0.65f)
					{
						vector2 += this._character.GetUpVector();
					}
					else
					{
						this._character.SetMovementMode(Character.MovementMode.Falling, 0);
						this._character.LaunchCharacter(this._character.GetUpVector() * 9f, true, false);
					}
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

		// Token: 0x040002DB RID: 731
		private Character _character;
	}
}
