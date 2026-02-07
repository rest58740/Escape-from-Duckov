using System;
using UnityEngine;

namespace ECM2.Examples.Teleport
{
	// Token: 0x02000080 RID: 128
	public class Teleporter : MonoBehaviour
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00010EEB File Offset: 0x0000F0EB
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x00010EF3 File Offset: 0x0000F0F3
		public bool isTeleporterEnabled { get; set; } = true;

		// Token: 0x060003F2 RID: 1010 RVA: 0x00010EFC File Offset: 0x0000F0FC
		private void OnTriggerEnter(Collider other)
		{
			if (this.destination == null || !this.isTeleporterEnabled)
			{
				return;
			}
			Character character;
			if (other.TryGetComponent<Character>(out character))
			{
				character.TeleportPosition(this.destination.transform.position, true, false);
				this.destination.isTeleporterEnabled = false;
				if (this.OrientWithDestination)
				{
					Vector3 upVector = character.GetUpVector();
					Vector3 vector = this.destination.transform.forward.projectedOnPlane(upVector);
					Quaternion newRotation = Quaternion.LookRotation(vector, character.GetUpVector());
					character.TeleportRotation(newRotation, true);
					character.LaunchCharacter(vector * character.GetSpeed(), false, true);
				}
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00010F9D File Offset: 0x0000F19D
		private void OnTriggerExit(Collider other)
		{
			this.isTeleporterEnabled = true;
		}

		// Token: 0x0400029C RID: 668
		[Tooltip("The destination teleporter.")]
		public Teleporter destination;

		// Token: 0x0400029D RID: 669
		[Tooltip("If true, the character will orient towards the destination Teleporter forward (yaw only)")]
		public bool OrientWithDestination;
	}
}
