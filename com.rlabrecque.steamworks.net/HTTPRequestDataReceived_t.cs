using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006D RID: 109
	[CallbackIdentity(2103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestDataReceived_t
	{
		// Token: 0x0400010B RID: 267
		public const int k_iCallback = 2103;

		// Token: 0x0400010C RID: 268
		public HTTPRequestHandle m_hRequest;

		// Token: 0x0400010D RID: 269
		public ulong m_ulContextValue;

		// Token: 0x0400010E RID: 270
		public uint m_cOffset;

		// Token: 0x0400010F RID: 271
		public uint m_cBytesReceived;
	}
}
