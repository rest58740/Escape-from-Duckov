using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CF RID: 207
	[CallbackIdentity(3407)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserFavoriteItemsListChanged_t
	{
		// Token: 0x04000275 RID: 629
		public const int k_iCallback = 3407;

		// Token: 0x04000276 RID: 630
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000277 RID: 631
		public EResult m_eResult;

		// Token: 0x04000278 RID: 632
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bWasAddRequest;
	}
}
