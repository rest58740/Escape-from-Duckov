using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B3 RID: 179
	[CallbackIdentity(1316)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdatePublishedFileResult_t
	{
		// Token: 0x040001EE RID: 494
		public const int k_iCallback = 1316;

		// Token: 0x040001EF RID: 495
		public EResult m_eResult;

		// Token: 0x040001F0 RID: 496
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001F1 RID: 497
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
