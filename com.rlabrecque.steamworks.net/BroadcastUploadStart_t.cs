using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000102 RID: 258
	[CallbackIdentity(4604)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct BroadcastUploadStart_t
	{
		// Token: 0x04000324 RID: 804
		public const int k_iCallback = 4604;

		// Token: 0x04000325 RID: 805
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bIsRTMP;
	}
}
