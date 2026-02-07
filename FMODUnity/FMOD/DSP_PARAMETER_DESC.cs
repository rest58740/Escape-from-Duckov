using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000088 RID: 136
	public struct DSP_PARAMETER_DESC
	{
		// Token: 0x040002B9 RID: 697
		public DSP_PARAMETER_TYPE type;

		// Token: 0x040002BA RID: 698
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] name;

		// Token: 0x040002BB RID: 699
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] label;

		// Token: 0x040002BC RID: 700
		public string description;

		// Token: 0x040002BD RID: 701
		public DSP_PARAMETER_DESC_UNION desc;
	}
}
