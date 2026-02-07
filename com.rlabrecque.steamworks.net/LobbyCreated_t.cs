using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000081 RID: 129
	[CallbackIdentity(513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyCreated_t
	{
		// Token: 0x0400015E RID: 350
		public const int k_iCallback = 513;

		// Token: 0x0400015F RID: 351
		public EResult m_eResult;

		// Token: 0x04000160 RID: 352
		public ulong m_ulSteamIDLobby;
	}
}
