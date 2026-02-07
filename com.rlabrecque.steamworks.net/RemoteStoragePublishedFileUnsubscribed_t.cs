using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B9 RID: 185
	[CallbackIdentity(1322)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUnsubscribed_t
	{
		// Token: 0x04000221 RID: 545
		public const int k_iCallback = 1322;

		// Token: 0x04000222 RID: 546
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000223 RID: 547
		public AppId_t m_nAppID;
	}
}
