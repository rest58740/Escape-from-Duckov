using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000098 RID: 152
	public struct DSP_METERING_INFO
	{
		// Token: 0x04000316 RID: 790
		public int numsamples;

		// Token: 0x04000317 RID: 791
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public float[] peaklevel;

		// Token: 0x04000318 RID: 792
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public float[] rmslevel;

		// Token: 0x04000319 RID: 793
		public short numchannels;
	}
}
