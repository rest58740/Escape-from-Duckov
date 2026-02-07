using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007A1 RID: 1953
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CONNECTDATA
	{
		// Token: 0x04002C47 RID: 11335
		[MarshalAs(UnmanagedType.Interface)]
		public object pUnk;

		// Token: 0x04002C48 RID: 11336
		public int dwCookie;
	}
}
