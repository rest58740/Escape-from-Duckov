using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CB RID: 203
	[CallbackIdentity(3403)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CreateItemResult_t
	{
		// Token: 0x04000264 RID: 612
		public const int k_iCallback = 3403;

		// Token: 0x04000265 RID: 613
		public EResult m_eResult;

		// Token: 0x04000266 RID: 614
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000267 RID: 615
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
