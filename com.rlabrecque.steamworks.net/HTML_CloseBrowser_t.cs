using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000057 RID: 87
	[CallbackIdentity(4504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CloseBrowser_t
	{
		// Token: 0x040000AF RID: 175
		public const int k_iCallback = 4504;

		// Token: 0x040000B0 RID: 176
		public HHTMLBrowser unBrowserHandle;
	}
}
