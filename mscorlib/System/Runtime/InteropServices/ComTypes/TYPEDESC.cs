using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007BC RID: 1980
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEDESC
	{
		// Token: 0x04002CB1 RID: 11441
		public IntPtr lpValue;

		// Token: 0x04002CB2 RID: 11442
		public short vt;
	}
}
