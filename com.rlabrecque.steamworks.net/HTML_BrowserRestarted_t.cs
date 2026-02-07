using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006A RID: 106
	[CallbackIdentity(4527)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_BrowserRestarted_t
	{
		// Token: 0x040000FF RID: 255
		public const int k_iCallback = 4527;

		// Token: 0x04000100 RID: 256
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000101 RID: 257
		public HHTMLBrowser unOldBrowserHandle;
	}
}
