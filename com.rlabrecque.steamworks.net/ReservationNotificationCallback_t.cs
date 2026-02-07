using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008C RID: 140
	[CallbackIdentity(5303)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ReservationNotificationCallback_t
	{
		// Token: 0x04000192 RID: 402
		public const int k_iCallback = 5303;

		// Token: 0x04000193 RID: 403
		public PartyBeaconID_t m_ulBeaconID;

		// Token: 0x04000194 RID: 404
		public CSteamID m_steamIDJoiner;
	}
}
