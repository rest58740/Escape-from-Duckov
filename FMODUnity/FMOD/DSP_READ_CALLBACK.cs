using System;

namespace FMOD
{
	// Token: 0x0200005E RID: 94
	// (Invoke) Token: 0x0600040B RID: 1035
	public delegate RESULT DSP_READ_CALLBACK(ref DSP_STATE dsp_state, IntPtr inbuffer, IntPtr outbuffer, uint length, int inchannels, ref int outchannels);
}
