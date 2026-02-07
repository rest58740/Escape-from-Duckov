using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C0 RID: 192
	[CallbackIdentity(1329)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileProgress_t
	{
		// Token: 0x0400023E RID: 574
		public const int k_iCallback = 1329;

		// Token: 0x0400023F RID: 575
		public double m_dPercentFile;

		// Token: 0x04000240 RID: 576
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPreview;
	}
}
