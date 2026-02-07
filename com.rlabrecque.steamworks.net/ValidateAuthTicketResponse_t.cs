using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E2 RID: 226
	[CallbackIdentity(143)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ValidateAuthTicketResponse_t
	{
		// Token: 0x040002B8 RID: 696
		public const int k_iCallback = 143;

		// Token: 0x040002B9 RID: 697
		public CSteamID m_SteamID;

		// Token: 0x040002BA RID: 698
		public EAuthSessionResponse m_eAuthSessionResponse;

		// Token: 0x040002BB RID: 699
		public CSteamID m_OwnerSteamID;
	}
}
