using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006B RID: 107
	[CallbackIdentity(2101)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestCompleted_t
	{
		// Token: 0x04000102 RID: 258
		public const int k_iCallback = 2101;

		// Token: 0x04000103 RID: 259
		public HTTPRequestHandle m_hRequest;

		// Token: 0x04000104 RID: 260
		public ulong m_ulContextValue;

		// Token: 0x04000105 RID: 261
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bRequestSuccessful;

		// Token: 0x04000106 RID: 262
		public EHTTPStatusCode m_eStatusCode;

		// Token: 0x04000107 RID: 263
		public uint m_unBodySize;
	}
}
