using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CE RID: 206
	[CallbackIdentity(3406)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadItemResult_t
	{
		// Token: 0x04000271 RID: 625
		public const int k_iCallback = 3406;

		// Token: 0x04000272 RID: 626
		public AppId_t m_unAppID;

		// Token: 0x04000273 RID: 627
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000274 RID: 628
		public EResult m_eResult;
	}
}
