using System;
using UnityEngine.Rendering;

namespace SodaCraft
{
	// Token: 0x0200043B RID: 1083
	[VolumeComponentMenu("SodaCraft/CameraArmControl")]
	[Serializable]
	public class CameraArmControl : VolumeComponent
	{
		// Token: 0x06002705 RID: 9989 RVA: 0x00086F81 File Offset: 0x00085181
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x00086F8E File Offset: 0x0008518E
		public override void Override(VolumeComponent state, float interpFactor)
		{
			CameraArmControl cameraArmControl = state as CameraArmControl;
			base.Override(state, interpFactor);
			CameraArm.globalPitch = cameraArmControl.pitch.value;
			CameraArm.globalYaw = cameraArmControl.yaw.value;
			CameraArm.globalDistance = cameraArmControl.distance.value;
		}

		// Token: 0x04001A99 RID: 6809
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001A9A RID: 6810
		public MinFloatParameter pitch = new MinFloatParameter(55f, 0f, false);

		// Token: 0x04001A9B RID: 6811
		public FloatParameter yaw = new FloatParameter(-30f, false);

		// Token: 0x04001A9C RID: 6812
		public MinFloatParameter distance = new MinFloatParameter(45f, 2f, false);
	}
}
