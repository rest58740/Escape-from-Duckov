using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200008C RID: 140
	public struct DSP_PARAMETER_3DATTRIBUTES_MULTI
	{
		// Token: 0x040002CB RID: 715
		public int numlisteners;

		// Token: 0x040002CC RID: 716
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public ATTRIBUTES_3D[] relative;

		// Token: 0x040002CD RID: 717
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public float[] weight;

		// Token: 0x040002CE RID: 718
		public ATTRIBUTES_3D absolute;
	}
}
