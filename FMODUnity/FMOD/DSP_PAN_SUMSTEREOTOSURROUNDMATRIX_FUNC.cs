using System;

namespace FMOD
{
	// Token: 0x0200007C RID: 124
	// (Invoke) Token: 0x06000483 RID: 1155
	public delegate RESULT DSP_PAN_SUMSTEREOTOSURROUNDMATRIX_FUNC(ref DSP_STATE dsp_state, int targetSpeakerMode, float direction, float extent, float rotation, float lowFrequencyGain, float overallGain, int matrixHop, IntPtr matrix);
}
