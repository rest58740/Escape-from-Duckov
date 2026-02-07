using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DD RID: 221
	[CallbackIdentity(102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServerConnectFailure_t
	{
		// Token: 0x040002AA RID: 682
		public const int k_iCallback = 102;

		// Token: 0x040002AB RID: 683
		public EResult m_eResult;

		// Token: 0x040002AC RID: 684
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bStillRetrying;
	}
}
