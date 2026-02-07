using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D7 RID: 215
	[CallbackIdentity(3415)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoveAppDependencyResult_t
	{
		// Token: 0x04000293 RID: 659
		public const int k_iCallback = 3415;

		// Token: 0x04000294 RID: 660
		public EResult m_eResult;

		// Token: 0x04000295 RID: 661
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000296 RID: 662
		public AppId_t m_nAppID;
	}
}
