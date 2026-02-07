using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005E RID: 94
	[CallbackIdentity(4511)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HorizontalScroll_t
	{
		// Token: 0x040000CA RID: 202
		public const int k_iCallback = 4511;

		// Token: 0x040000CB RID: 203
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000CC RID: 204
		public uint unScrollMax;

		// Token: 0x040000CD RID: 205
		public uint unScrollCurrent;

		// Token: 0x040000CE RID: 206
		public float flPageScale;

		// Token: 0x040000CF RID: 207
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x040000D0 RID: 208
		public uint unPageSize;
	}
}
