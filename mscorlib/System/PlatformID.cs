using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000248 RID: 584
	[ComVisible(true)]
	[Serializable]
	public enum PlatformID
	{
		// Token: 0x04001770 RID: 6000
		Win32S,
		// Token: 0x04001771 RID: 6001
		Win32Windows,
		// Token: 0x04001772 RID: 6002
		Win32NT,
		// Token: 0x04001773 RID: 6003
		WinCE,
		// Token: 0x04001774 RID: 6004
		Unix,
		// Token: 0x04001775 RID: 6005
		Xbox,
		// Token: 0x04001776 RID: 6006
		MacOSX
	}
}
