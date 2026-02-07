using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CA RID: 202
	[CallbackIdentity(3402)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCRequestUGCDetailsResult_t
	{
		// Token: 0x04000261 RID: 609
		public const int k_iCallback = 3402;

		// Token: 0x04000262 RID: 610
		public SteamUGCDetails_t m_details;

		// Token: 0x04000263 RID: 611
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
