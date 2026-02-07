using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000076 RID: 118
	[CallbackIdentity(4704)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryStartPurchaseResult_t
	{
		// Token: 0x0400012D RID: 301
		public const int k_iCallback = 4704;

		// Token: 0x0400012E RID: 302
		public EResult m_result;

		// Token: 0x0400012F RID: 303
		public ulong m_ulOrderID;

		// Token: 0x04000130 RID: 304
		public ulong m_ulTransID;
	}
}
