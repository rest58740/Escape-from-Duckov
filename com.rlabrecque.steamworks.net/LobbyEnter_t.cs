using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007A RID: 122
	[CallbackIdentity(504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyEnter_t
	{
		// Token: 0x04000140 RID: 320
		public const int k_iCallback = 504;

		// Token: 0x04000141 RID: 321
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000142 RID: 322
		public uint m_rgfChatPermissions;

		// Token: 0x04000143 RID: 323
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocked;

		// Token: 0x04000144 RID: 324
		public uint m_EChatRoomEnterResponse;
	}
}
