using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000054 RID: 84
	[CallbackIdentity(4501)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_BrowserReady_t
	{
		// Token: 0x0400009A RID: 154
		public const int k_iCallback = 4501;

		// Token: 0x0400009B RID: 155
		public HHTMLBrowser unBrowserHandle;
	}
}
