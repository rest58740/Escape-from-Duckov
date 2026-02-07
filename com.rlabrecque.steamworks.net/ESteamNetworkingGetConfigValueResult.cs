using System;

namespace Steamworks
{
	// Token: 0x02000171 RID: 369
	public enum ESteamNetworkingGetConfigValueResult
	{
		// Token: 0x040009DC RID: 2524
		k_ESteamNetworkingGetConfigValue_BadValue = -1,
		// Token: 0x040009DD RID: 2525
		k_ESteamNetworkingGetConfigValue_BadScopeObj = -2,
		// Token: 0x040009DE RID: 2526
		k_ESteamNetworkingGetConfigValue_BufferTooSmall = -3,
		// Token: 0x040009DF RID: 2527
		k_ESteamNetworkingGetConfigValue_OK = 1,
		// Token: 0x040009E0 RID: 2528
		k_ESteamNetworkingGetConfigValue_OKInherited,
		// Token: 0x040009E1 RID: 2529
		k_ESteamNetworkingGetConfigValueResult__Force32Bit = 2147483647
	}
}
