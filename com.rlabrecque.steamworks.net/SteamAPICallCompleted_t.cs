using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F9 RID: 249
	[CallbackIdentity(703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAPICallCompleted_t
	{
		// Token: 0x0400030E RID: 782
		public const int k_iCallback = 703;

		// Token: 0x0400030F RID: 783
		public SteamAPICall_t m_hAsyncCall;

		// Token: 0x04000310 RID: 784
		public int m_iCallback;

		// Token: 0x04000311 RID: 785
		public uint m_cubParam;
	}
}
