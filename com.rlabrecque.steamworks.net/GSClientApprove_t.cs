using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000047 RID: 71
	[CallbackIdentity(201)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientApprove_t
	{
		// Token: 0x04000068 RID: 104
		public const int k_iCallback = 201;

		// Token: 0x04000069 RID: 105
		public CSteamID m_SteamID;

		// Token: 0x0400006A RID: 106
		public CSteamID m_OwnerSteamID;
	}
}
