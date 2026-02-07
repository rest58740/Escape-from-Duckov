using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D6 RID: 214
	[CallbackIdentity(3414)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AddAppDependencyResult_t
	{
		// Token: 0x0400028F RID: 655
		public const int k_iCallback = 3414;

		// Token: 0x04000290 RID: 656
		public EResult m_eResult;

		// Token: 0x04000291 RID: 657
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000292 RID: 658
		public AppId_t m_nAppID;
	}
}
