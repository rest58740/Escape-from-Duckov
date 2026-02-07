using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000065 RID: 101
	[CallbackIdentity(4522)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SetCursor_t
	{
		// Token: 0x040000F1 RID: 241
		public const int k_iCallback = 4522;

		// Token: 0x040000F2 RID: 242
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000F3 RID: 243
		public uint eMouseCursor;
	}
}
