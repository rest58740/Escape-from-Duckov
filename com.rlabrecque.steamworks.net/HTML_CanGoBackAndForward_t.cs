using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005D RID: 93
	[CallbackIdentity(4510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CanGoBackAndForward_t
	{
		// Token: 0x040000C6 RID: 198
		public const int k_iCallback = 4510;

		// Token: 0x040000C7 RID: 199
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000C8 RID: 200
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoBack;

		// Token: 0x040000C9 RID: 201
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoForward;
	}
}
