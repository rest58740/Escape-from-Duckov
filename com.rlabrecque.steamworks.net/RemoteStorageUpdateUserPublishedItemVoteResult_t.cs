using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BB RID: 187
	[CallbackIdentity(1324)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdateUserPublishedItemVoteResult_t
	{
		// Token: 0x04000227 RID: 551
		public const int k_iCallback = 1324;

		// Token: 0x04000228 RID: 552
		public EResult m_eResult;

		// Token: 0x04000229 RID: 553
		public PublishedFileId_t m_nPublishedFileId;
	}
}
