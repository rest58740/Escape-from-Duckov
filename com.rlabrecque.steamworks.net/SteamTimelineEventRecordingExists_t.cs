using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C8 RID: 200
	[CallbackIdentity(6002)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamTimelineEventRecordingExists_t
	{
		// Token: 0x04000257 RID: 599
		public const int k_iCallback = 6002;

		// Token: 0x04000258 RID: 600
		public ulong m_ulEventID;

		// Token: 0x04000259 RID: 601
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bRecordingExists;
	}
}
