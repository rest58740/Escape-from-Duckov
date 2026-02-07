using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006C RID: 108
	[CallbackIdentity(2102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestHeadersReceived_t
	{
		// Token: 0x04000108 RID: 264
		public const int k_iCallback = 2102;

		// Token: 0x04000109 RID: 265
		public HTTPRequestHandle m_hRequest;

		// Token: 0x0400010A RID: 266
		public ulong m_ulContextValue;
	}
}
