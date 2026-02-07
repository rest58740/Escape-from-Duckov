using System;

namespace Steamworks
{
	// Token: 0x0200015A RID: 346
	public enum EChatRoomEnterResponse
	{
		// Token: 0x0400088A RID: 2186
		k_EChatRoomEnterResponseSuccess = 1,
		// Token: 0x0400088B RID: 2187
		k_EChatRoomEnterResponseDoesntExist,
		// Token: 0x0400088C RID: 2188
		k_EChatRoomEnterResponseNotAllowed,
		// Token: 0x0400088D RID: 2189
		k_EChatRoomEnterResponseFull,
		// Token: 0x0400088E RID: 2190
		k_EChatRoomEnterResponseError,
		// Token: 0x0400088F RID: 2191
		k_EChatRoomEnterResponseBanned,
		// Token: 0x04000890 RID: 2192
		k_EChatRoomEnterResponseLimited,
		// Token: 0x04000891 RID: 2193
		k_EChatRoomEnterResponseClanDisabled,
		// Token: 0x04000892 RID: 2194
		k_EChatRoomEnterResponseCommunityBan,
		// Token: 0x04000893 RID: 2195
		k_EChatRoomEnterResponseMemberBlockedYou,
		// Token: 0x04000894 RID: 2196
		k_EChatRoomEnterResponseYouBlockedMember,
		// Token: 0x04000895 RID: 2197
		k_EChatRoomEnterResponseRatelimitExceeded = 15
	}
}
