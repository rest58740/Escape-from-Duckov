using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E4 RID: 228
	[CallbackIdentity(154)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct EncryptedAppTicketResponse_t
	{
		// Token: 0x040002C0 RID: 704
		public const int k_iCallback = 154;

		// Token: 0x040002C1 RID: 705
		public EResult m_eResult;
	}
}
