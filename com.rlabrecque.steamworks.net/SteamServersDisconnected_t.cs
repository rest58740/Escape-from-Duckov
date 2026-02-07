using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DE RID: 222
	[CallbackIdentity(103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServersDisconnected_t
	{
		// Token: 0x040002AD RID: 685
		public const int k_iCallback = 103;

		// Token: 0x040002AE RID: 686
		public EResult m_eResult;
	}
}
