using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D0 RID: 208
	[CallbackIdentity(3408)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetUserItemVoteResult_t
	{
		// Token: 0x04000279 RID: 633
		public const int k_iCallback = 3408;

		// Token: 0x0400027A RID: 634
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400027B RID: 635
		public EResult m_eResult;

		// Token: 0x0400027C RID: 636
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteUp;
	}
}
