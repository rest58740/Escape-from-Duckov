using System;

namespace Steamworks
{
	// Token: 0x0200016C RID: 364
	public enum ESteamNetworkingConnectionState
	{
		// Token: 0x04000954 RID: 2388
		k_ESteamNetworkingConnectionState_None,
		// Token: 0x04000955 RID: 2389
		k_ESteamNetworkingConnectionState_Connecting,
		// Token: 0x04000956 RID: 2390
		k_ESteamNetworkingConnectionState_FindingRoute,
		// Token: 0x04000957 RID: 2391
		k_ESteamNetworkingConnectionState_Connected,
		// Token: 0x04000958 RID: 2392
		k_ESteamNetworkingConnectionState_ClosedByPeer,
		// Token: 0x04000959 RID: 2393
		k_ESteamNetworkingConnectionState_ProblemDetectedLocally,
		// Token: 0x0400095A RID: 2394
		k_ESteamNetworkingConnectionState_FinWait = -1,
		// Token: 0x0400095B RID: 2395
		k_ESteamNetworkingConnectionState_Linger = -2,
		// Token: 0x0400095C RID: 2396
		k_ESteamNetworkingConnectionState_Dead = -3,
		// Token: 0x0400095D RID: 2397
		k_ESteamNetworkingConnectionState__Force32Bit = 2147483647
	}
}
