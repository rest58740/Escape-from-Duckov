using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000051 RID: 81
	[CallbackIdentity(1800)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsReceived_t
	{
		// Token: 0x04000092 RID: 146
		public const int k_iCallback = 1800;

		// Token: 0x04000093 RID: 147
		public EResult m_eResult;

		// Token: 0x04000094 RID: 148
		public CSteamID m_steamIDUser;
	}
}
