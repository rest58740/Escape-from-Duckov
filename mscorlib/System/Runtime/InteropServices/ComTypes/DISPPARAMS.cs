using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007C2 RID: 1986
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DISPPARAMS
	{
		// Token: 0x04002CC4 RID: 11460
		public IntPtr rgvarg;

		// Token: 0x04002CC5 RID: 11461
		public IntPtr rgdispidNamedArgs;

		// Token: 0x04002CC6 RID: 11462
		public int cArgs;

		// Token: 0x04002CC7 RID: 11463
		public int cNamedArgs;
	}
}
