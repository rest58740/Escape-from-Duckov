using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007C3 RID: 1987
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EXCEPINFO
	{
		// Token: 0x04002CC8 RID: 11464
		public short wCode;

		// Token: 0x04002CC9 RID: 11465
		public short wReserved;

		// Token: 0x04002CCA RID: 11466
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrSource;

		// Token: 0x04002CCB RID: 11467
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrDescription;

		// Token: 0x04002CCC RID: 11468
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrHelpFile;

		// Token: 0x04002CCD RID: 11469
		public int dwHelpContext;

		// Token: 0x04002CCE RID: 11470
		public IntPtr pvReserved;

		// Token: 0x04002CCF RID: 11471
		public IntPtr pfnDeferredFillIn;

		// Token: 0x04002CD0 RID: 11472
		public int scode;
	}
}
