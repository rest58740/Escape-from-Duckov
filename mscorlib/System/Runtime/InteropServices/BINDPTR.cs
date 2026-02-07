using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000738 RID: 1848
	[Obsolete]
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	public struct BINDPTR
	{
		// Token: 0x04002BAC RID: 11180
		[FieldOffset(0)]
		public IntPtr lpfuncdesc;

		// Token: 0x04002BAD RID: 11181
		[FieldOffset(0)]
		public IntPtr lptcomp;

		// Token: 0x04002BAE RID: 11182
		[FieldOffset(0)]
		public IntPtr lpvardesc;
	}
}
