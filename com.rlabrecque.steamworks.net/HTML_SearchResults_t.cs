using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005C RID: 92
	[CallbackIdentity(4509)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SearchResults_t
	{
		// Token: 0x040000C2 RID: 194
		public const int k_iCallback = 4509;

		// Token: 0x040000C3 RID: 195
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000C4 RID: 196
		public uint unResults;

		// Token: 0x040000C5 RID: 197
		public uint unCurrentMatch;
	}
}
