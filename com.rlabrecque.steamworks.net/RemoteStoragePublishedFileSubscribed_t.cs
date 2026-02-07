using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B8 RID: 184
	[CallbackIdentity(1321)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileSubscribed_t
	{
		// Token: 0x0400021E RID: 542
		public const int k_iCallback = 1321;

		// Token: 0x0400021F RID: 543
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000220 RID: 544
		public AppId_t m_nAppID;
	}
}
