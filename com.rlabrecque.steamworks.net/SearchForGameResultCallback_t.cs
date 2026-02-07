using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000084 RID: 132
	[CallbackIdentity(5202)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SearchForGameResultCallback_t
	{
		// Token: 0x0400016A RID: 362
		public const int k_iCallback = 5202;

		// Token: 0x0400016B RID: 363
		public ulong m_ullSearchID;

		// Token: 0x0400016C RID: 364
		public EResult m_eResult;

		// Token: 0x0400016D RID: 365
		public int m_nCountPlayersInGame;

		// Token: 0x0400016E RID: 366
		public int m_nCountAcceptedGame;

		// Token: 0x0400016F RID: 367
		public CSteamID m_steamIDHost;

		// Token: 0x04000170 RID: 368
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bFinalCallback;
	}
}
