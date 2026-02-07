using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B0 RID: 176
	[CallbackIdentity(1313)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSubscribePublishedFileResult_t
	{
		// Token: 0x040001E2 RID: 482
		public const int k_iCallback = 1313;

		// Token: 0x040001E3 RID: 483
		public EResult m_eResult;

		// Token: 0x040001E4 RID: 484
		public PublishedFileId_t m_nPublishedFileId;
	}
}
