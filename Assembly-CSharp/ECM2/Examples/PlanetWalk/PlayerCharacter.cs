using System;
using UnityEngine;

namespace ECM2.Examples.PlanetWalk
{
	// Token: 0x02000084 RID: 132
	public class PlayerCharacter : Character
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x000113DC File Offset: 0x0000F5DC
		protected override void UpdateRotation(float deltaTime)
		{
			base.UpdateRotation(deltaTime);
			base.SetGravityVector((this.planetTransform.position - base.GetPosition()).normalized * base.GetGravityMagnitude());
			Vector3 toDirection = base.GetGravityDirection() * -1f;
			Quaternion rotation = Quaternion.FromToRotation(this.GetUpVector(), toDirection) * base.GetRotation();
			base.SetRotation(rotation);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00011450 File Offset: 0x0000F650
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
			vector2 = vector2.relativeTo(base.cameraTransform, this.GetUpVector(), true);
			base.SetMovementDirection(vector2);
			if (Input.GetButton("Jump"))
			{
				this.Jump();
				return;
			}
			if (Input.GetButtonUp("Jump"))
			{
				this.StopJumping();
			}
		}

		// Token: 0x040002A1 RID: 673
		[Space(15f)]
		public Transform planetTransform;
	}
}
