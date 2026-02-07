using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B1 RID: 177
	[CallbackIdentity(1314)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSubscribedFilesResult_t
	{
		// Token: 0x040001E5 RID: 485
		public const int k_iCallback = 1314;

		// Token: 0x040001E6 RID: 486
		public EResult m_eResult;

		// Token: 0x040001E7 RID: 487
		public int m_nResultsReturned;

		// Token: 0x040001E8 RID: 488
		public int m_nTotalResultCount;

		// Token: 0x040001E9 RID: 489
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x040001EA RID: 490
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeSubscribed;
	}
}
