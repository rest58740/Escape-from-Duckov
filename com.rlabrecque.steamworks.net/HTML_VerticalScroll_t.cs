using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005F RID: 95
	[CallbackIdentity(4512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_VerticalScroll_t
	{
		// Token: 0x040000D1 RID: 209
		public const int k_iCallback = 4512;

		// Token: 0x040000D2 RID: 210
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000D3 RID: 211
		public uint unScrollMax;

		// Token: 0x040000D4 RID: 212
		public uint unScrollCurrent;

		// Token: 0x040000D5 RID: 213
		public float flPageScale;

		// Token: 0x040000D6 RID: 214
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x040000D7 RID: 215
		public uint unPageSize;
	}
}
