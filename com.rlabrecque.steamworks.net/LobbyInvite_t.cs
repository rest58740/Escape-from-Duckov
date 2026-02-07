using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000079 RID: 121
	[CallbackIdentity(503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyInvite_t
	{
		// Token: 0x0400013C RID: 316
		public const int k_iCallback = 503;

		// Token: 0x0400013D RID: 317
		public ulong m_ulSteamIDUser;

		// Token: 0x0400013E RID: 318
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400013F RID: 319
		public ulong m_ulGameID;
	}
}
