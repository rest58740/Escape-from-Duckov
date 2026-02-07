using System;

namespace Steamworks
{
	// Token: 0x02000163 RID: 355
	public enum EGameSearchErrorCode_t
	{
		// Token: 0x040008E9 RID: 2281
		k_EGameSearchErrorCode_OK = 1,
		// Token: 0x040008EA RID: 2282
		k_EGameSearchErrorCode_Failed_Search_Already_In_Progress,
		// Token: 0x040008EB RID: 2283
		k_EGameSearchErrorCode_Failed_No_Search_In_Progress,
		// Token: 0x040008EC RID: 2284
		k_EGameSearchErrorCode_Failed_Not_Lobby_Leader,
		// Token: 0x040008ED RID: 2285
		k_EGameSearchErrorCode_Failed_No_Host_Available,
		// Token: 0x040008EE RID: 2286
		k_EGameSearchErrorCode_Failed_Search_Params_Invalid,
		// Token: 0x040008EF RID: 2287
		k_EGameSearchErrorCode_Failed_Offline,
		// Token: 0x040008F0 RID: 2288
		k_EGameSearchErrorCode_Failed_NotAuthorized,
		// Token: 0x040008F1 RID: 2289
		k_EGameSearchErrorCode_Failed_Unknown_Error
	}
}
