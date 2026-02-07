using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000055 RID: 85
	[CallbackIdentity(4502)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NeedsPaint_t
	{
		// Token: 0x0400009C RID: 156
		public const int k_iCallback = 4502;

		// Token: 0x0400009D RID: 157
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400009E RID: 158
		public IntPtr pBGRA;

		// Token: 0x0400009F RID: 159
		public uint unWide;

		// Token: 0x040000A0 RID: 160
		public uint unTall;

		// Token: 0x040000A1 RID: 161
		public uint unUpdateX;

		// Token: 0x040000A2 RID: 162
		public uint unUpdateY;

		// Token: 0x040000A3 RID: 163
		public uint unUpdateWide;

		// Token: 0x040000A4 RID: 164
		public uint unUpdateTall;

		// Token: 0x040000A5 RID: 165
		public uint unScrollX;

		// Token: 0x040000A6 RID: 166
		public uint unScrollY;

		// Token: 0x040000A7 RID: 167
		public float flPageScale;

		// Token: 0x040000A8 RID: 168
		public uint unPageSerial;
	}
}
