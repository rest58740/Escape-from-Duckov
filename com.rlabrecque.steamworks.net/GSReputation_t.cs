using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004E RID: 78
	[CallbackIdentity(209)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSReputation_t
	{
		// Token: 0x04000082 RID: 130
		public const int k_iCallback = 209;

		// Token: 0x04000083 RID: 131
		public EResult m_eResult;

		// Token: 0x04000084 RID: 132
		public uint m_unReputationScore;

		// Token: 0x04000085 RID: 133
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000086 RID: 134
		public uint m_unBannedIP;

		// Token: 0x04000087 RID: 135
		public ushort m_usBannedPort;

		// Token: 0x04000088 RID: 136
		public ulong m_ulBannedGameID;

		// Token: 0x04000089 RID: 137
		public uint m_unBanExpires;
	}
}
