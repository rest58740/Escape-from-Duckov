using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CD RID: 205
	[CallbackIdentity(3405)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ItemInstalled_t
	{
		// Token: 0x0400026C RID: 620
		public const int k_iCallback = 3405;

		// Token: 0x0400026D RID: 621
		public AppId_t m_unAppID;

		// Token: 0x0400026E RID: 622
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400026F RID: 623
		public UGCHandle_t m_hLegacyContent;

		// Token: 0x04000270 RID: 624
		public ulong m_unManifestID;
	}
}
