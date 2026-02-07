using System;
using UnityEngine;

namespace ECM2.Examples.TwinStickMovement
{
	// Token: 0x0200007D RID: 125
	public class TwinStickCharacter : Character
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x00010979 File Offset: 0x0000EB79
		public virtual Vector3 GetAimDirection()
		{
			return this._aimDirection;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00010981 File Offset: 0x0000EB81
		public virtual void SetAimDirection(Vector3 worldDirection)
		{
			this._aimDirection = worldDirection;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001098C File Offset: 0x0000EB8C
		protected override void CustomRotationMode(float deltaTime)
		{
			base.CustomRotationMode(deltaTime);
			Vector3 worldDirection = this._aimDirection.isZero() ? base.GetMovementDirection() : this.GetAimDirection();
			this.RotateTowards(worldDirection, deltaTime, true);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000109C8 File Offset: 0x0000EBC8
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
			if (base.camera)
			{
				vector2 = vector2.relativeTo(base.cameraTransform, true);
			}
			base.SetMovementDirection(vector2);
			Vector3 aimDirection = Vector3.zero;
			RaycastHit raycastHit;
			if (Input.GetMouseButton(0) && Physics.Raycast(base.camera.ScreenPointToRay(Input.mousePosition), out raycastHit, float.PositiveInfinity))
			{
				aimDirection = (raycastHit.point - base.GetPosition()).onlyXZ().normalized;
			}
			this.SetAimDirection(aimDirection);
		}

		// Token: 0x0400028D RID: 653
		private Vector3 _aimDirection;
	}
}
