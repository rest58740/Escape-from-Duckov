using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C5 RID: 197
	[CallbackIdentity(2301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ScreenshotReady_t
	{
		// Token: 0x0400024D RID: 589
		public const int k_iCallback = 2301;

		// Token: 0x0400024E RID: 590
		public ScreenshotHandle m_hLocal;

		// Token: 0x0400024F RID: 591
		public EResult m_eResult;
	}
}
