using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BC RID: 188
	[CallbackIdentity(1325)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUserVoteDetails_t
	{
		// Token: 0x0400022A RID: 554
		public const int k_iCallback = 1325;

		// Token: 0x0400022B RID: 555
		public EResult m_eResult;

		// Token: 0x0400022C RID: 556
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400022D RID: 557
		public EWorkshopVote m_eVote;
	}
}
