using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007B9 RID: 1977
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct IDLDESC
	{
		// Token: 0x04002CA4 RID: 11428
		public IntPtr dwReserved;

		// Token: 0x04002CA5 RID: 11429
		public IDLFLAG wIDLFlags;
	}
}
