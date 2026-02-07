using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DB RID: 219
	[CallbackIdentity(3420)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct WorkshopEULAStatus_t
	{
		// Token: 0x040002A2 RID: 674
		public const int k_iCallback = 3420;

		// Token: 0x040002A3 RID: 675
		public EResult m_eResult;

		// Token: 0x040002A4 RID: 676
		public AppId_t m_nAppID;

		// Token: 0x040002A5 RID: 677
		public uint m_unVersion;

		// Token: 0x040002A6 RID: 678
		public RTime32 m_rtAction;

		// Token: 0x040002A7 RID: 679
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAccepted;

		// Token: 0x040002A8 RID: 680
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bNeedsAction;
	}
}
