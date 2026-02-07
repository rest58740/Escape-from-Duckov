using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x0200043E RID: 1086
	[VolumeComponentMenu("SodaCraft/TimeOfDayPost")]
	[Serializable]
	public class TimeOfDayPost : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06002710 RID: 10000 RVA: 0x00087449 File Offset: 0x00085649
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x00087456 File Offset: 0x00085656
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x0008745C File Offset: 0x0008565C
		public override void Override(VolumeComponent state, float interpFactor)
		{
			TimeOfDayPost timeOfDayPost = state as TimeOfDayPost;
			base.Override(state, interpFactor);
			if (timeOfDayPost == null)
			{
				return;
			}
			TimeOfDayController.NightViewAngleFactor = timeOfDayPost.nightViewAngleFactor.value;
			TimeOfDayController.NightViewDistanceFactor = timeOfDayPost.nightViewDistanceFactor.value;
			TimeOfDayController.NightSenseRangeFactor = timeOfDayPost.nightSenseRangeFactor.value;
			TimeOfDayController.coldLevel = timeOfDayPost.coldLevel.value;
			TimeOfDayController.heatLevel = timeOfDayPost.heatLevel.value;
		}

		// Token: 0x04001AB8 RID: 6840
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001AB9 RID: 6841
		public ClampedFloatParameter nightViewAngleFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);

		// Token: 0x04001ABA RID: 6842
		public ClampedFloatParameter nightViewDistanceFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);

		// Token: 0x04001ABB RID: 6843
		public ClampedFloatParameter nightSenseRangeFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);

		// Token: 0x04001ABC RID: 6844
		public ClampedFloatParameter coldLevel = new ClampedFloatParameter(0f, -10f, 10f, false);

		// Token: 0x04001ABD RID: 6845
		public ClampedFloatParameter heatLevel = new ClampedFloatParameter(0f, -10f, 10f, false);
	}
}
