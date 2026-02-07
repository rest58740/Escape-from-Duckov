using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200071F RID: 1823
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.CONNECTDATA instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CONNECTDATA
	{
		// Token: 0x04002B0B RID: 11019
		[MarshalAs(UnmanagedType.Interface)]
		public object pUnk;

		// Token: 0x04002B0C RID: 11020
		public int dwCookie;
	}
}
