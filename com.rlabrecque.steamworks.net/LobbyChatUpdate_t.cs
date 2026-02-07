using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007C RID: 124
	[CallbackIdentity(506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatUpdate_t
	{
		// Token: 0x04000149 RID: 329
		public const int k_iCallback = 506;

		// Token: 0x0400014A RID: 330
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400014B RID: 331
		public ulong m_ulSteamIDUserChanged;

		// Token: 0x0400014C RID: 332
		public ulong m_ulSteamIDMakingChange;

		// Token: 0x0400014D RID: 333
		public uint m_rgfChatMemberStateChange;
	}
}
