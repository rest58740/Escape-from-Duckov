using System;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x0200019B RID: 411
[Serializable]
public struct TimeOfDayPhase
{
	// Token: 0x04000AC5 RID: 2757
	[FormerlySerializedAs("phaseTag")]
	public TimePhaseTags timePhaseTag;

	// Token: 0x04000AC6 RID: 2758
	public VolumeProfile volumeProfile;
}
