using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000175 RID: 373
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FriendGameInfo_t
	{
		// Token: 0x040009F7 RID: 2551
		public CGameID m_gameID;

		// Token: 0x040009F8 RID: 2552
		public uint m_unGameIP;

		// Token: 0x040009F9 RID: 2553
		public ushort m_usGamePort;

		// Token: 0x040009FA RID: 2554
		public ushort m_usQueryPort;

		// Token: 0x040009FB RID: 2555
		public CSteamID m_steamIDLobby;
	}
}
