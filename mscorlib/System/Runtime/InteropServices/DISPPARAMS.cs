using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200072F RID: 1839
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.DISPPARAMS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DISPPARAMS
	{
		// Token: 0x04002B6B RID: 11115
		public IntPtr rgvarg;

		// Token: 0x04002B6C RID: 11116
		public IntPtr rgdispidNamedArgs;

		// Token: 0x04002B6D RID: 11117
		public int cArgs;

		// Token: 0x04002B6E RID: 11118
		public int cNamedArgs;
	}
}
