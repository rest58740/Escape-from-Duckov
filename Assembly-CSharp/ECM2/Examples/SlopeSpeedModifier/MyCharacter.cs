using System;
using UnityEngine;

namespace ECM2.Examples.SlopeSpeedModifier
{
	// Token: 0x02000081 RID: 129
	public class MyCharacter : Character
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x00010FB8 File Offset: 0x0000F1B8
		public override float GetMaxSpeed()
		{
			float maxSpeed = base.GetMaxSpeed();
			float signedSlopeAngle = base.GetSignedSlopeAngle();
			float num = (signedSlopeAngle > 0f) ? (1f - Mathf.InverseLerp(0f, 90f, signedSlopeAngle)) : (1f + Mathf.InverseLerp(0f, 90f, -signedSlopeAngle));
			return maxSpeed * num;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001100C File Offset: 0x0000F20C
		private void OnGUI()
		{
			GUI.Label(new Rect(10f, 10f, 400f, 20f), string.Format("Slope angle: {0:F2} maxSpeed: {1:F2} ", base.GetSignedSlopeAngle(), this.GetMaxSpeed()));
		}
	}
}
