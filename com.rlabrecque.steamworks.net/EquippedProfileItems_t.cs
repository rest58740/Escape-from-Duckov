using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000044 RID: 68
	[CallbackIdentity(351)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct EquippedProfileItems_t
	{
		// Token: 0x0400005C RID: 92
		public const int k_iCallback = 351;

		// Token: 0x0400005D RID: 93
		public EResult m_eResult;

		// Token: 0x0400005E RID: 94
		public CSteamID m_steamID;

		// Token: 0x0400005F RID: 95
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasAnimatedAvatar;

		// Token: 0x04000060 RID: 96
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasAvatarFrame;

		// Token: 0x04000061 RID: 97
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasProfileModifier;

		// Token: 0x04000062 RID: 98
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasProfileBackground;

		// Token: 0x04000063 RID: 99
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasMiniProfileBackground;

		// Token: 0x04000064 RID: 100
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bFromCache;
	}
}
