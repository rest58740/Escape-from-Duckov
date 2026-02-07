using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000072 RID: 114
	[CallbackIdentity(4700)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryResultReady_t
	{
		// Token: 0x04000122 RID: 290
		public const int k_iCallback = 4700;

		// Token: 0x04000123 RID: 291
		public SteamInventoryResult_t m_handle;

		// Token: 0x04000124 RID: 292
		public EResult m_result;
	}
}
