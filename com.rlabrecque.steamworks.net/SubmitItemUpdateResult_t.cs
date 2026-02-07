using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CC RID: 204
	[CallbackIdentity(3404)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SubmitItemUpdateResult_t
	{
		// Token: 0x04000268 RID: 616
		public const int k_iCallback = 3404;

		// Token: 0x04000269 RID: 617
		public EResult m_eResult;

		// Token: 0x0400026A RID: 618
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;

		// Token: 0x0400026B RID: 619
		public PublishedFileId_t m_nPublishedFileId;
	}
}
