using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000064 RID: 100
	[CallbackIdentity(4521)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NewWindow_t
	{
		// Token: 0x040000E9 RID: 233
		public const int k_iCallback = 4521;

		// Token: 0x040000EA RID: 234
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000EB RID: 235
		public string pchURL;

		// Token: 0x040000EC RID: 236
		public uint unX;

		// Token: 0x040000ED RID: 237
		public uint unY;

		// Token: 0x040000EE RID: 238
		public uint unWide;

		// Token: 0x040000EF RID: 239
		public uint unTall;

		// Token: 0x040000F0 RID: 240
		public HHTMLBrowser unNewWindow_BrowserHandle_IGNORE;
	}
}
