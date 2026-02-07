using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex62
{
	// Token: 0x0200006B RID: 107
	public class ApplyLandingForce : MonoBehaviour
	{
		// Token: 0x06000378 RID: 888 RVA: 0x0000F3CD File Offset: 0x0000D5CD
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000F3DB File Offset: 0x0000D5DB
		private void OnEnable()
		{
			this._character.Landed += this.OnLanded;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F3F4 File Offset: 0x0000D5F4
		private void OnDisable()
		{
			this._character.Landed -= this.OnLanded;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000F410 File Offset: 0x0000D610
		private void OnLanded(Vector3 landingVelocity)
		{
			Rigidbody groundRigidbody = this._character.characterMovement.groundRigidbody;
			if (!groundRigidbody)
			{
				return;
			}
			Vector3 force = this._character.GetGravityVector() * (this._character.mass * landingVelocity.magnitude * this.landingForceScale);
			groundRigidbody.AddForceAtPosition(force, this._character.position);
		}

		// Token: 0x04000265 RID: 613
		public float landingForceScale = 1f;

		// Token: 0x04000266 RID: 614
		private Character _character;
	}
}
