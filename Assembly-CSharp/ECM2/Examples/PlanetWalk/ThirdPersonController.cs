using System;
using ECM2.Examples.ThirdPerson;
using UnityEngine;

namespace ECM2.Examples.PlanetWalk
{
	// Token: 0x02000085 RID: 133
	public class ThirdPersonController : ThirdPersonController
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x00011508 File Offset: 0x0000F708
		public override void AddControlYawInput(float value)
		{
			Vector3 up = this.followTarget.transform.up;
			this._cameraForward = Quaternion.Euler(up * value) * this._cameraForward;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00011544 File Offset: 0x0000F744
		protected override void UpdateCameraRotation()
		{
			Vector3 up = this.followTarget.transform.up;
			Vector3.OrthoNormalize(ref up, ref this._cameraForward);
			this._character.cameraTransform.rotation = Quaternion.LookRotation(this._cameraForward, up) * Quaternion.Euler(this._cameraPitch, 0f, 0f);
		}

		// Token: 0x040002A2 RID: 674
		private Vector3 _cameraForward = Vector3.forward;
	}
}
