using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000059 RID: 89
	[CallbackIdentity(4506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FinishedRequest_t
	{
		// Token: 0x040000B8 RID: 184
		public const int k_iCallback = 4506;

		// Token: 0x040000B9 RID: 185
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000BA RID: 186
		public string pchURL;

		// Token: 0x040000BB RID: 187
		public string pchPageTitle;
	}
}
