using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D8 RID: 216
	[CallbackIdentity(3416)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetAppDependenciesResult_t
	{
		// Token: 0x04000297 RID: 663
		public const int k_iCallback = 3416;

		// Token: 0x04000298 RID: 664
		public EResult m_eResult;

		// Token: 0x04000299 RID: 665
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400029A RID: 666
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public AppId_t[] m_rgAppIDs;

		// Token: 0x0400029B RID: 667
		public uint m_nNumAppDependencies;

		// Token: 0x0400029C RID: 668
		public uint m_nTotalNumAppDependencies;
	}
}
