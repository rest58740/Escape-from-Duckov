using System;

namespace FMOD
{
	// Token: 0x0200005F RID: 95
	// (Invoke) Token: 0x0600040F RID: 1039
	public delegate RESULT DSP_SHOULDIPROCESS_CALLBACK(ref DSP_STATE dsp_state, bool inputsidle, uint length, CHANNELMASK inmask, int inchannels, SPEAKERMODE speakermode);
}
