using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EA RID: 234
	[CallbackIdentity(168)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetTicketForWebApiResponse_t
	{
		// Token: 0x040002D8 RID: 728
		public const int k_iCallback = 168;

		// Token: 0x040002D9 RID: 729
		public HAuthTicket m_hAuthTicket;

		// Token: 0x040002DA RID: 730
		public EResult m_eResult;

		// Token: 0x040002DB RID: 731
		public int m_cubTicket;

		// Token: 0x040002DC RID: 732
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2560)]
		public byte[] m_rgubTicket;
	}
}
