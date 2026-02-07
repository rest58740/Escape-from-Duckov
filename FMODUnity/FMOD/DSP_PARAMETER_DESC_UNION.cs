using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000087 RID: 135
	[StructLayout(LayoutKind.Explicit)]
	public struct DSP_PARAMETER_DESC_UNION
	{
		// Token: 0x040002B5 RID: 693
		[FieldOffset(0)]
		public DSP_PARAMETER_DESC_FLOAT floatdesc;

		// Token: 0x040002B6 RID: 694
		[FieldOffset(0)]
		public DSP_PARAMETER_DESC_INT intdesc;

		// Token: 0x040002B7 RID: 695
		[FieldOffset(0)]
		public DSP_PARAMETER_DESC_BOOL booldesc;

		// Token: 0x040002B8 RID: 696
		[FieldOffset(0)]
		public DSP_PARAMETER_DESC_DATA datadesc;
	}
}
