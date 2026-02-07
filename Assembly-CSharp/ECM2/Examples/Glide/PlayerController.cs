using System;
using UnityEngine;

namespace ECM2.Examples.Glide
{
	// Token: 0x0200008D RID: 141
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x00012757 File Offset: 0x00010957
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
			this._glideAbility = base.GetComponent<GlideAbility>();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00012774 File Offset: 0x00010974
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
			if (Input.GetButtonDown("Jump"))
			{
				this._glideAbility.Glide();
				return;
			}
			if (Input.GetButtonUp("Jump"))
			{
				this._glideAbility.StopGliding();
			}
		}

		// Token: 0x040002D1 RID: 721
		private Character _character;

		// Token: 0x040002D2 RID: 722
		private GlideAbility _glideAbility;
	}
}
