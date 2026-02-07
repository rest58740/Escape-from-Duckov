using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C1 RID: 193
	[CallbackIdentity(1330)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUpdated_t
	{
		// Token: 0x04000241 RID: 577
		public const int k_iCallback = 1330;

		// Token: 0x04000242 RID: 578
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000243 RID: 579
		public AppId_t m_nAppID;

		// Token: 0x04000244 RID: 580
		public ulong m_ulUnused;
	}
}
