using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EB RID: 235
	[CallbackIdentity(1101)]
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	public struct UserStatsReceived_t
	{
		// Token: 0x040002DD RID: 733
		public const int k_iCallback = 1101;

		// Token: 0x040002DE RID: 734
		[FieldOffset(0)]
		public ulong m_nGameID;

		// Token: 0x040002DF RID: 735
		[FieldOffset(8)]
		public EResult m_eResult;

		// Token: 0x040002E0 RID: 736
		[FieldOffset(12)]
		public CSteamID m_steamIDUser;
	}
}
