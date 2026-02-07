using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AD RID: 173
	[CallbackIdentity(1309)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileResult_t
	{
		// Token: 0x040001D6 RID: 470
		public const int k_iCallback = 1309;

		// Token: 0x040001D7 RID: 471
		public EResult m_eResult;

		// Token: 0x040001D8 RID: 472
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001D9 RID: 473
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
