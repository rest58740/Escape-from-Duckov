using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200072A RID: 1834
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEDESC
	{
		// Token: 0x04002B5E RID: 11102
		public IntPtr lpValue;

		// Token: 0x04002B5F RID: 11103
		public short vt;
	}
}
