using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000080 RID: 128
	[CallbackIdentity(512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyKicked_t
	{
		// Token: 0x0400015A RID: 346
		public const int k_iCallback = 512;

		// Token: 0x0400015B RID: 347
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400015C RID: 348
		public ulong m_ulSteamIDAdmin;

		// Token: 0x0400015D RID: 349
		public byte m_bKickedDueToDisconnect;
	}
}
