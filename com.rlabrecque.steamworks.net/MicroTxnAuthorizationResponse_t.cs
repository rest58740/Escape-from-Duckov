using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E3 RID: 227
	[CallbackIdentity(152)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MicroTxnAuthorizationResponse_t
	{
		// Token: 0x040002BC RID: 700
		public const int k_iCallback = 152;

		// Token: 0x040002BD RID: 701
		public uint m_unAppID;

		// Token: 0x040002BE RID: 702
		public ulong m_ulOrderID;

		// Token: 0x040002BF RID: 703
		public byte m_bAuthorized;
	}
}
