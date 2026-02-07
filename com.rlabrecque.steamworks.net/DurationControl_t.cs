using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E9 RID: 233
	[CallbackIdentity(167)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DurationControl_t
	{
		// Token: 0x040002CF RID: 719
		public const int k_iCallback = 167;

		// Token: 0x040002D0 RID: 720
		public EResult m_eResult;

		// Token: 0x040002D1 RID: 721
		public AppId_t m_appid;

		// Token: 0x040002D2 RID: 722
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bApplicable;

		// Token: 0x040002D3 RID: 723
		public int m_csecsLast5h;

		// Token: 0x040002D4 RID: 724
		public EDurationControlProgress m_progress;

		// Token: 0x040002D5 RID: 725
		public EDurationControlNotification m_notification;

		// Token: 0x040002D6 RID: 726
		public int m_csecsToday;

		// Token: 0x040002D7 RID: 727
		public int m_csecsRemaining;
	}
}
