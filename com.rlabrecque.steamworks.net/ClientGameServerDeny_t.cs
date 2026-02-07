using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DF RID: 223
	[CallbackIdentity(113)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ClientGameServerDeny_t
	{
		// Token: 0x040002AF RID: 687
		public const int k_iCallback = 113;

		// Token: 0x040002B0 RID: 688
		public uint m_uAppID;

		// Token: 0x040002B1 RID: 689
		public uint m_unGameServerIP;

		// Token: 0x040002B2 RID: 690
		public ushort m_usGameServerPort;

		// Token: 0x040002B3 RID: 691
		public ushort m_bSecure;

		// Token: 0x040002B4 RID: 692
		public uint m_uReason;
	}
}
