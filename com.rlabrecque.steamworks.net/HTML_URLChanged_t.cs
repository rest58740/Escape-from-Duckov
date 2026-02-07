using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000058 RID: 88
	[CallbackIdentity(4505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_URLChanged_t
	{
		// Token: 0x040000B1 RID: 177
		public const int k_iCallback = 4505;

		// Token: 0x040000B2 RID: 178
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000B3 RID: 179
		public string pchURL;

		// Token: 0x040000B4 RID: 180
		public string pchPostData;

		// Token: 0x040000B5 RID: 181
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;

		// Token: 0x040000B6 RID: 182
		public string pchPageTitle;

		// Token: 0x040000B7 RID: 183
		[MarshalAs(UnmanagedType.I1)]
		public bool bNewNavigation;
	}
}
