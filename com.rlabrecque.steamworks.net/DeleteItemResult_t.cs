using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D9 RID: 217
	[CallbackIdentity(3417)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DeleteItemResult_t
	{
		// Token: 0x0400029D RID: 669
		public const int k_iCallback = 3417;

		// Token: 0x0400029E RID: 670
		public EResult m_eResult;

		// Token: 0x0400029F RID: 671
		public PublishedFileId_t m_nPublishedFileId;
	}
}
