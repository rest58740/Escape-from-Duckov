using System;
using UnityEngine;

namespace ECM2.Examples.ToggleGravityDirection
{
	// Token: 0x0200007E RID: 126
	public class ToggleCharacterGravityDirection : MonoBehaviour
	{
		// Token: 0x060003DE RID: 990 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		private void RotateCharacterToGravity(float deltaTime)
		{
			Vector3 toDirection = -1f * this._character.GetGravityDirection();
			Vector3 upVector = this._character.GetUpVector();
			Quaternion quaternion = this._character.GetRotation();
			Quaternion to = Quaternion.FromToRotation(upVector, toDirection) * quaternion;
			quaternion = Quaternion.RotateTowards(quaternion, to, this._character.rotationRate * deltaTime);
			this._character.SetRotation(quaternion);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00010B1C File Offset: 0x0000ED1C
		private void OnAfterSimulationUpdated(float deltaTime)
		{
			this.RotateCharacterToGravity(deltaTime);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00010B25 File Offset: 0x0000ED25
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00010B33 File Offset: 0x0000ED33
		private void OnEnable()
		{
			this._character.AfterSimulationUpdated += this.OnAfterSimulationUpdated;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00010B4C File Offset: 0x0000ED4C
		private void OnDisable()
		{
			this._character.AfterSimulationUpdated -= this.OnAfterSimulationUpdated;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00010B65 File Offset: 0x0000ED65
		private void Update()
		{
			if (this._character.IsFalling() && Input.GetKeyDown(KeyCode.E))
			{
				this._character.gravityScale *= -1f;
			}
		}

		// Token: 0x0400028E RID: 654
		private Character _character;
	}
}
