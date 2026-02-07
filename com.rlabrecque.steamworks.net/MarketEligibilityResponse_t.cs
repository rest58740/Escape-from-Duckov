using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E8 RID: 232
	[CallbackIdentity(166)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MarketEligibilityResponse_t
	{
		// Token: 0x040002C9 RID: 713
		public const int k_iCallback = 166;

		// Token: 0x040002CA RID: 714
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAllowed;

		// Token: 0x040002CB RID: 715
		public EMarketNotAllowedReasonFlags m_eNotAllowedReason;

		// Token: 0x040002CC RID: 716
		public RTime32 m_rtAllowedAtTime;

		// Token: 0x040002CD RID: 717
		public int m_cdaySteamGuardRequiredDays;

		// Token: 0x040002CE RID: 718
		public int m_cdayNewDeviceCooldown;
	}
}
