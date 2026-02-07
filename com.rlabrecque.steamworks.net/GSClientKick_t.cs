using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000049 RID: 73
	[CallbackIdentity(203)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientKick_t
	{
		// Token: 0x0400006F RID: 111
		public const int k_iCallback = 203;

		// Token: 0x04000070 RID: 112
		public CSteamID m_SteamID;

		// Token: 0x04000071 RID: 113
		public EDenyReason m_eDenyReason;
	}
}
