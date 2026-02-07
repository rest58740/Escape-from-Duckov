using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex61
{
	// Token: 0x0200006C RID: 108
	public class Bouncer : MonoBehaviour
	{
		// Token: 0x0600037D RID: 893 RVA: 0x0000F488 File Offset: 0x0000D688
		private void OnTriggerEnter(Collider other)
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
			character.PauseGroundConstraint(0.1f);
			character.LaunchCharacter(base.transform.up * this.launchImpulse, this.overrideVerticalVelocity, this.overrideLateralVelocity);
		}

		// Token: 0x04000267 RID: 615
		public float launchImpulse = 15f;

		// Token: 0x04000268 RID: 616
		public bool overrideVerticalVelocity;

		// Token: 0x04000269 RID: 617
		public bool overrideLateralVelocity;
	}
}
