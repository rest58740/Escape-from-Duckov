using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007BB RID: 1979
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct PARAMDESC
	{
		// Token: 0x04002CAF RID: 11439
		public IntPtr lpVarValue;

		// Token: 0x04002CB0 RID: 11440
		public PARAMFLAG wParamFlags;
	}
}
