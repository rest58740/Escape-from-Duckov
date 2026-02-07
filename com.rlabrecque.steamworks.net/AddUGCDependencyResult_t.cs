using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D4 RID: 212
	[CallbackIdentity(3412)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AddUGCDependencyResult_t
	{
		// Token: 0x04000287 RID: 647
		public const int k_iCallback = 3412;

		// Token: 0x04000288 RID: 648
		public EResult m_eResult;

		// Token: 0x04000289 RID: 649
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400028A RID: 650
		public PublishedFileId_t m_nChildPublishedFileId;
	}
}
