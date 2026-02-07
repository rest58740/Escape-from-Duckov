using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000073 RID: 115
	[CallbackIdentity(4701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryFullUpdate_t
	{
		// Token: 0x04000125 RID: 293
		public const int k_iCallback = 4701;

		// Token: 0x04000126 RID: 294
		public SteamInventoryResult_t m_handle;
	}
}
