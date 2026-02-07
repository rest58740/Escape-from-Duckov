using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000067 RID: 103
	[CallbackIdentity(4524)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ShowToolTip_t
	{
		// Token: 0x040000F7 RID: 247
		public const int k_iCallback = 4524;

		// Token: 0x040000F8 RID: 248
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000F9 RID: 249
		public string pchMsg;
	}
}
