using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D1 RID: 209
	[CallbackIdentity(3409)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetUserItemVoteResult_t
	{
		// Token: 0x0400027D RID: 637
		public const int k_iCallback = 3409;

		// Token: 0x0400027E RID: 638
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400027F RID: 639
		public EResult m_eResult;

		// Token: 0x04000280 RID: 640
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedUp;

		// Token: 0x04000281 RID: 641
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedDown;

		// Token: 0x04000282 RID: 642
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteSkipped;
	}
}
