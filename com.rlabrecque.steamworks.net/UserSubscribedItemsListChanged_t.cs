using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DA RID: 218
	[CallbackIdentity(3418)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserSubscribedItemsListChanged_t
	{
		// Token: 0x040002A0 RID: 672
		public const int k_iCallback = 3418;

		// Token: 0x040002A1 RID: 673
		public AppId_t m_nAppID;
	}
}
