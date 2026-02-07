using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000090 RID: 144
	public struct DSP_LOUDNESS_METER_INFO_TYPE
	{
		// Token: 0x040002D5 RID: 725
		public float momentaryloudness;

		// Token: 0x040002D6 RID: 726
		public float shorttermloudness;

		// Token: 0x040002D7 RID: 727
		public float integratedloudness;

		// Token: 0x040002D8 RID: 728
		public float loudness10thpercentile;

		// Token: 0x040002D9 RID: 729
		public float loudness95thpercentile;

		// Token: 0x040002DA RID: 730
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
		public float[] loudnesshistogram;

		// Token: 0x040002DB RID: 731
		public float maxtruepeak;

		// Token: 0x040002DC RID: 732
		public float maxmomentaryloudness;
	}
}
