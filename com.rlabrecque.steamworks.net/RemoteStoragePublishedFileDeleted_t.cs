using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BA RID: 186
	[CallbackIdentity(1323)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileDeleted_t
	{
		// Token: 0x04000224 RID: 548
		public const int k_iCallback = 1323;

		// Token: 0x04000225 RID: 549
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000226 RID: 550
		public AppId_t m_nAppID;
	}
}
