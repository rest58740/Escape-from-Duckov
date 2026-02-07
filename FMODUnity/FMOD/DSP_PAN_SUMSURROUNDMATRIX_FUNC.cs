using System;

namespace FMOD
{
	// Token: 0x0200007A RID: 122
	// (Invoke) Token: 0x0600047B RID: 1147
	public delegate RESULT DSP_PAN_SUMSURROUNDMATRIX_FUNC(ref DSP_STATE dsp_state, int sourceSpeakerMode, int targetSpeakerMode, float direction, float extent, float rotation, float lowFrequencyGain, float overallGain, int matrixHop, IntPtr matrix, DSP_PAN_SURROUND_FLAGS flags);
}
