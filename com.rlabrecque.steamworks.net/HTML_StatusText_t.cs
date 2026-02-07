using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000066 RID: 102
	[CallbackIdentity(4523)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StatusText_t
	{
		// Token: 0x040000F4 RID: 244
		public const int k_iCallback = 4523;

		// Token: 0x040000F5 RID: 245
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000F6 RID: 246
		public string pchMsg;
	}
}
