using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000091 RID: 145
	public struct DSP_LOUDNESS_METER_WEIGHTING_TYPE
	{
		// Token: 0x040002DD RID: 733
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public float[] channelweight;
	}
}
