using System;
using UnityEngine;

namespace ECM2.Examples.FirstPerson
{
	// Token: 0x0200008E RID: 142
	public class FirstPersonCharacter : Character
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x000128BB File Offset: 0x00010ABB
		public virtual void AddControlYawInput(float value)
		{
			if (value != 0f)
			{
				this.AddYawInput(value);
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000128CC File Offset: 0x00010ACC
		public virtual void AddControlPitchInput(float value, float minPitch = -80f, float maxPitch = 80f)
		{
			if (value != 0f)
			{
				this._cameraPitch = MathLib.ClampAngle(this._cameraPitch + value, minPitch, maxPitch);
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000128EB File Offset: 0x00010AEB
		protected virtual void UpdateCameraParentRotation()
		{
			this.cameraParent.transform.localRotation = Quaternion.Euler(this._cameraPitch, 0f, 0f);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00012912 File Offset: 0x00010B12
		protected virtual void LateUpdate()
		{
			this.UpdateCameraParentRotation();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001291A File Offset: 0x00010B1A
		protected override void Reset()
		{
			base.Reset();
			base.SetRotationMode(Character.RotationMode.None);
		}

		// Token: 0x040002D3 RID: 723
		[Tooltip("The first person camera parent.")]
		public GameObject cameraParent;

		// Token: 0x040002D4 RID: 724
		private float _cameraPitch;
	}
}
