using System;

namespace Steamworks
{
	// Token: 0x02000108 RID: 264
	[Flags]
	public enum EFriendFlags
	{
		// Token: 0x040003E5 RID: 997
		k_EFriendFlagNone = 0,
		// Token: 0x040003E6 RID: 998
		k_EFriendFlagBlocked = 1,
		// Token: 0x040003E7 RID: 999
		k_EFriendFlagFriendshipRequested = 2,
		// Token: 0x040003E8 RID: 1000
		k_EFriendFlagImmediate = 4,
		// Token: 0x040003E9 RID: 1001
		k_EFriendFlagClanMember = 8,
		// Token: 0x040003EA RID: 1002
		k_EFriendFlagOnGameServer = 16,
		// Token: 0x040003EB RID: 1003
		k_EFriendFlagRequestingFriendship = 128,
		// Token: 0x040003EC RID: 1004
		k_EFriendFlagRequestingInfo = 256,
		// Token: 0x040003ED RID: 1005
		k_EFriendFlagIgnored = 512,
		// Token: 0x040003EE RID: 1006
		k_EFriendFlagIgnoredFriend = 1024,
		// Token: 0x040003EF RID: 1007
		k_EFriendFlagChatMember = 4096,
		// Token: 0x040003F0 RID: 1008
		k_EFriendFlagAll = 65535
	}
}
