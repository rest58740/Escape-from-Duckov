using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017A RID: 378
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamPartyBeaconLocation_t
	{
		// Token: 0x04000A10 RID: 2576
		public ESteamPartyBeaconLocationType m_eType;

		// Token: 0x04000A11 RID: 2577
		public ulong m_ulLocationID;
	}
}
