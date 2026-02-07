using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004D RID: 77
	[CallbackIdentity(208)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GSClientGroupStatus_t
	{
		// Token: 0x0400007D RID: 125
		public const int k_iCallback = 208;

		// Token: 0x0400007E RID: 126
		public CSteamID m_SteamIDUser;

		// Token: 0x0400007F RID: 127
		public CSteamID m_SteamIDGroup;

		// Token: 0x04000080 RID: 128
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bMember;

		// Token: 0x04000081 RID: 129
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bOfficer;
	}
}
