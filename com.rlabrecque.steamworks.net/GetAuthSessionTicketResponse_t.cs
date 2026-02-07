using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E5 RID: 229
	[CallbackIdentity(163)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetAuthSessionTicketResponse_t
	{
		// Token: 0x040002C2 RID: 706
		public const int k_iCallback = 163;

		// Token: 0x040002C3 RID: 707
		public HAuthTicket m_hAuthTicket;

		// Token: 0x040002C4 RID: 708
		public EResult m_eResult;
	}
}
