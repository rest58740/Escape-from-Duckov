using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F1 RID: 241
	[CallbackIdentity(1107)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct NumberOfCurrentPlayers_t
	{
		// Token: 0x040002F8 RID: 760
		public const int k_iCallback = 1107;

		// Token: 0x040002F9 RID: 761
		public byte m_bSuccess;

		// Token: 0x040002FA RID: 762
		public int m_cPlayers;
	}
}
