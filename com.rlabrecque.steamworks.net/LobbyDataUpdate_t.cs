using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007B RID: 123
	[CallbackIdentity(505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyDataUpdate_t
	{
		// Token: 0x04000145 RID: 325
		public const int k_iCallback = 505;

		// Token: 0x04000146 RID: 326
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000147 RID: 327
		public ulong m_ulSteamIDMember;

		// Token: 0x04000148 RID: 328
		public byte m_bSuccess;
	}
}
