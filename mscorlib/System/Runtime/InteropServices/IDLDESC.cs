using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000727 RID: 1831
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IDLDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct IDLDESC
	{
		// Token: 0x04002B51 RID: 11089
		public int dwReserved;

		// Token: 0x04002B52 RID: 11090
		public IDLFLAG wIDLFlags;
	}
}
