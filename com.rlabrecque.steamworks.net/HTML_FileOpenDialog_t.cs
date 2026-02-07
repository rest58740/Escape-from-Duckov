using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000063 RID: 99
	[CallbackIdentity(4516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FileOpenDialog_t
	{
		// Token: 0x040000E5 RID: 229
		public const int k_iCallback = 4516;

		// Token: 0x040000E6 RID: 230
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000E7 RID: 231
		public string pchTitle;

		// Token: 0x040000E8 RID: 232
		public string pchInitialFile;
	}
}
