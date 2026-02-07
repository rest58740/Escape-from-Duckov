using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007B1 RID: 1969
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	public struct BINDPTR
	{
		// Token: 0x04002C5D RID: 11357
		[FieldOffset(0)]
		public IntPtr lpfuncdesc;

		// Token: 0x04002C5E RID: 11358
		[FieldOffset(0)]
		public IntPtr lpvardesc;

		// Token: 0x04002C5F RID: 11359
		[FieldOffset(0)]
		public IntPtr lptcomp;
	}
}
