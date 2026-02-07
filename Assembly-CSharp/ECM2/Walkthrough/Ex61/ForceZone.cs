using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex61
{
	// Token: 0x0200006D RID: 109
	public class ForceZone : MonoBehaviour
	{
		// Token: 0x0600037F RID: 895 RVA: 0x0000F4F4 File Offset: 0x0000D6F4
		private void OnTriggerStay(Collider other)
		{
			if (!other.CompareTag("Player"))
			{
				return;
			}
			Character character;
			if (!other.TryGetComponent<Character>(out character))
			{
				return;
			}
			Vector3 vector = this.windDirection.normalized * this.windStrength;
			Vector3 rhs = -character.GetGravityDirection();
			float num = Vector3.Dot(vector, rhs);
			if (num > 0f && character.IsWalking() && num - character.GetGravityMagnitude() > 0f)
			{
				character.PauseGroundConstraint(0.1f);
			}
			character.AddForce(vector, ForceMode.Acceleration);
		}

		// Token: 0x0400026A RID: 618
		public Vector3 windDirection = Vector3.up;

		// Token: 0x0400026B RID: 619
		public float windStrength = 20f;
	}
}
