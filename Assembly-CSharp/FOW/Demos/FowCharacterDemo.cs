using System;
using UnityEngine;

namespace FOW.Demos
{
	// Token: 0x02000061 RID: 97
	public class FowCharacterDemo : MonoBehaviour
	{
		// Token: 0x0600033E RID: 830 RVA: 0x0000E1C8 File Offset: 0x0000C3C8
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
			this.CursorLocked = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000E1EC File Offset: 0x0000C3EC
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.CursorLocked = !this.CursorLocked;
				if (this.CursorLocked)
				{
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
				else
				{
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
			}
			if (this.CursorLocked)
			{
				base.transform.Rotate(0f, Input.GetAxis("Mouse X"), 0f);
				this.yRot -= Input.GetAxis("Mouse Y");
			}
			this.yRot = Mathf.Clamp(this.yRot, -80f, 80f);
			this.setInput();
			this.move();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000E298 File Offset: 0x0000C498
		public void setInput()
		{
			bool[] array = new bool[]
			{
				Input.GetKey(KeyCode.W),
				Input.GetKey(KeyCode.A),
				Input.GetKey(KeyCode.S),
				Input.GetKey(KeyCode.D),
				Input.GetKey(KeyCode.LeftShift)
			};
			this.speedTarget = 0f;
			this.inputDirection = Vector2.zero;
			if (array[0])
			{
				this.inputDirection.y = this.inputDirection.y + 1f;
				this.speedTarget = this.WalkingSpeed;
			}
			if (array[1])
			{
				this.inputDirection.x = this.inputDirection.x - 1f;
				this.speedTarget = this.WalkingSpeed;
			}
			if (array[2])
			{
				this.inputDirection.y = this.inputDirection.y - 1f;
				this.speedTarget = this.WalkingSpeed;
			}
			if (array[3])
			{
				this.inputDirection.x = this.inputDirection.x + 1f;
				this.speedTarget = this.WalkingSpeed;
			}
			if (array[4])
			{
				this.speedTarget *= this.RunningMultiplier;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		private void move()
		{
			if (this.cc.isGrounded)
			{
				this.velocity.y = 0f;
			}
			Vector2 a = new Vector2(base.transform.forward.x, base.transform.forward.z);
			Vector2 vector = Vector3.Normalize(new Vector2(base.transform.right.x, base.transform.right.z) * this.inputDirection.x + a * this.inputDirection.y);
			this.velocityXZ = Vector2.MoveTowards(this.velocityXZ, vector.normalized * this.speedTarget, Time.deltaTime * this.Acceleration);
			this.velocity.x = this.velocityXZ.x * Time.deltaTime;
			this.velocity.z = this.velocityXZ.y * Time.deltaTime;
			this.velocity.y = this.velocity.y + -9.81f * Time.deltaTime * Time.deltaTime;
			this.cc.enabled = true;
			this.cc.Move(this.velocity);
			this.cc.enabled = false;
		}

		// Token: 0x04000227 RID: 551
		public float WalkingSpeed = 5f;

		// Token: 0x04000228 RID: 552
		public float RunningMultiplier = 1.65f;

		// Token: 0x04000229 RID: 553
		public float Acceleration = 25f;

		// Token: 0x0400022A RID: 554
		private float yRot;

		// Token: 0x0400022B RID: 555
		private CharacterController cc;

		// Token: 0x0400022C RID: 556
		private bool CursorLocked;

		// Token: 0x0400022D RID: 557
		private Vector2 inputDirection = Vector2.zero;

		// Token: 0x0400022E RID: 558
		private Vector2 velocityXZ = Vector2.zero;

		// Token: 0x0400022F RID: 559
		private Vector3 velocity = Vector3.zero;

		// Token: 0x04000230 RID: 560
		private float speedTarget;
	}
}
