using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005B RID: 91
	[CallbackIdentity(4508)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ChangedTitle_t
	{
		// Token: 0x040000BF RID: 191
		public const int k_iCallback = 4508;

		// Token: 0x040000C0 RID: 192
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000C1 RID: 193
		public string pchTitle;
	}
}
