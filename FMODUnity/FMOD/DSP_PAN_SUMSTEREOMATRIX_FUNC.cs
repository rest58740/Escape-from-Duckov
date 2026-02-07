using System;

namespace FMOD
{
	// Token: 0x02000079 RID: 121
	// (Invoke) Token: 0x06000477 RID: 1143
	public delegate RESULT DSP_PAN_SUMSTEREOMATRIX_FUNC(ref DSP_STATE dsp_state, int sourceSpeakerMode, float pan, float lowFrequencyGain, float overallGain, int matrixHop, IntPtr matrix);
}
