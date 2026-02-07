using System;
using UnityEngine;

namespace ECM2.Examples.FirstPersonFly
{
	// Token: 0x02000093 RID: 147
	public class FlyAbility : MonoBehaviour
	{
		// Token: 0x0600046F RID: 1135 RVA: 0x00012E8A File Offset: 0x0001108A
		private bool IsFlyAllowed()
		{
			return this.canEverFly && this._character.IsFalling();
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00012EA4 File Offset: 0x000110A4
		protected virtual bool CanFly()
		{
			bool flag = this.IsFlyAllowed();
			if (flag)
			{
				Vector3 rhs = -this._character.GetGravityDirection();
				flag = (Vector3.Dot(this._character.GetVelocity(), rhs) < 0f);
			}
			return flag;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00012EE6 File Offset: 0x000110E6
		private void OnCollided(ref CollisionResult collisionResult)
		{
			if (this._character.IsFlying() && collisionResult.isWalkable)
			{
				this._character.SetMovementMode(Character.MovementMode.Falling, 0);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00012F0C File Offset: 0x0001110C
		private void OnBeforeSimulationUpdated(float deltaTime)
		{
			int num = this._character.IsFlying() ? 1 : 0;
			bool jumpInputPressed = this._character.jumpInputPressed;
			if (num == 0 && jumpInputPressed && this.CanFly())
			{
				this._character.SetMovementMode(Character.MovementMode.Flying, 0);
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00012F4C File Offset: 0x0001114C
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00012F5A File Offset: 0x0001115A
		private void OnEnable()
		{
			this._character.Collided += this.OnCollided;
			this._character.BeforeSimulationUpdated += this.OnBeforeSimulationUpdated;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00012F8A File Offset: 0x0001118A
		private void OnDisable()
		{
			this._character.Collided -= this.OnCollided;
			this._character.BeforeSimulationUpdated -= this.OnBeforeSimulationUpdated;
		}

		// Token: 0x040002DD RID: 733
		public bool canEverFly = true;

		// Token: 0x040002DE RID: 734
		private Character _character;
	}
}
