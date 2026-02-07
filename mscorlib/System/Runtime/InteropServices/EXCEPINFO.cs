using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000730 RID: 1840
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.EXCEPINFO instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EXCEPINFO
	{
		// Token: 0x04002B6F RID: 11119
		public short wCode;

		// Token: 0x04002B70 RID: 11120
		public short wReserved;

		// Token: 0x04002B71 RID: 11121
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrSource;

		// Token: 0x04002B72 RID: 11122
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrDescription;

		// Token: 0x04002B73 RID: 11123
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrHelpFile;

		// Token: 0x04002B74 RID: 11124
		public int dwHelpContext;

		// Token: 0x04002B75 RID: 11125
		public IntPtr pvReserved;

		// Token: 0x04002B76 RID: 11126
		public IntPtr pfnDeferredFillIn;
	}
}
