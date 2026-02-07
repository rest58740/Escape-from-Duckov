using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000061 RID: 97
	[CallbackIdentity(4514)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSAlert_t
	{
		// Token: 0x040000DF RID: 223
		public const int k_iCallback = 4514;

		// Token: 0x040000E0 RID: 224
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000E1 RID: 225
		public string pchMessage;
	}
}
