using System;

namespace Steamworks
{
	// Token: 0x02000169 RID: 361
	public enum ESteamNetworkingAvailability
	{
		// Token: 0x04000939 RID: 2361
		k_ESteamNetworkingAvailability_CannotTry = -102,
		// Token: 0x0400093A RID: 2362
		k_ESteamNetworkingAvailability_Failed,
		// Token: 0x0400093B RID: 2363
		k_ESteamNetworkingAvailability_Previously,
		// Token: 0x0400093C RID: 2364
		k_ESteamNetworkingAvailability_Retrying = -10,
		// Token: 0x0400093D RID: 2365
		k_ESteamNetworkingAvailability_NeverTried = 1,
		// Token: 0x0400093E RID: 2366
		k_ESteamNetworkingAvailability_Waiting,
		// Token: 0x0400093F RID: 2367
		k_ESteamNetworkingAvailability_Attempting,
		// Token: 0x04000940 RID: 2368
		k_ESteamNetworkingAvailability_Current = 100,
		// Token: 0x04000941 RID: 2369
		k_ESteamNetworkingAvailability_Unknown = 0,
		// Token: 0x04000942 RID: 2370
		k_ESteamNetworkingAvailability__Force32bit = 2147483647
	}
}
