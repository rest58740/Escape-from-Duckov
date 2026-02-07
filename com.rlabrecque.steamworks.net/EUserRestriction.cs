using System;

namespace Steamworks
{
	// Token: 0x02000109 RID: 265
	public enum EUserRestriction
	{
		// Token: 0x040003F2 RID: 1010
		k_nUserRestrictionNone,
		// Token: 0x040003F3 RID: 1011
		k_nUserRestrictionUnknown,
		// Token: 0x040003F4 RID: 1012
		k_nUserRestrictionAnyChat,
		// Token: 0x040003F5 RID: 1013
		k_nUserRestrictionVoiceChat = 4,
		// Token: 0x040003F6 RID: 1014
		k_nUserRestrictionGroupChat = 8,
		// Token: 0x040003F7 RID: 1015
		k_nUserRestrictionRating = 16,
		// Token: 0x040003F8 RID: 1016
		k_nUserRestrictionGameInvites = 32,
		// Token: 0x040003F9 RID: 1017
		k_nUserRestrictionTrading = 64
	}
}
