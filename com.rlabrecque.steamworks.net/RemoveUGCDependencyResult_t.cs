using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D5 RID: 213
	[CallbackIdentity(3413)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoveUGCDependencyResult_t
	{
		// Token: 0x0400028B RID: 651
		public const int k_iCallback = 3413;

		// Token: 0x0400028C RID: 652
		public EResult m_eResult;

		// Token: 0x0400028D RID: 653
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400028E RID: 654
		public PublishedFileId_t m_nChildPublishedFileId;
	}
}
