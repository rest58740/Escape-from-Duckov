using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200008F RID: 143
	public struct DSP_PARAMETER_DYNAMIC_RESPONSE
	{
		// Token: 0x040002D3 RID: 723
		public int numchannels;

		// Token: 0x040002D4 RID: 724
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public float[] rms;
	}
}
