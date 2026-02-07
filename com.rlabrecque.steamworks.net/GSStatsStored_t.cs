using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000052 RID: 82
	[CallbackIdentity(1801)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsStored_t
	{
		// Token: 0x04000095 RID: 149
		public const int k_iCallback = 1801;

		// Token: 0x04000096 RID: 150
		public EResult m_eResult;

		// Token: 0x04000097 RID: 151
		public CSteamID m_steamIDUser;
	}
}
