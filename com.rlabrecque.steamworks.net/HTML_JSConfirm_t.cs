using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000062 RID: 98
	[CallbackIdentity(4515)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSConfirm_t
	{
		// Token: 0x040000E2 RID: 226
		public const int k_iCallback = 4515;

		// Token: 0x040000E3 RID: 227
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000E4 RID: 228
		public string pchMessage;
	}
}
