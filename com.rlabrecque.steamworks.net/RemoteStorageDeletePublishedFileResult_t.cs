using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AE RID: 174
	[CallbackIdentity(1311)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDeletePublishedFileResult_t
	{
		// Token: 0x040001DA RID: 474
		public const int k_iCallback = 1311;

		// Token: 0x040001DB RID: 475
		public EResult m_eResult;

		// Token: 0x040001DC RID: 476
		public PublishedFileId_t m_nPublishedFileId;
	}
}
