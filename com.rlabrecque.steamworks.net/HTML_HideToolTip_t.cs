using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000069 RID: 105
	[CallbackIdentity(4526)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HideToolTip_t
	{
		// Token: 0x040000FD RID: 253
		public const int k_iCallback = 4526;

		// Token: 0x040000FE RID: 254
		public HHTMLBrowser unBrowserHandle;
	}
}
