using System;

namespace Steamworks
{
	// Token: 0x0200016D RID: 365
	public enum ESteamNetConnectionEnd
	{
		// Token: 0x0400095F RID: 2399
		k_ESteamNetConnectionEnd_Invalid,
		// Token: 0x04000960 RID: 2400
		k_ESteamNetConnectionEnd_App_Min = 1000,
		// Token: 0x04000961 RID: 2401
		k_ESteamNetConnectionEnd_App_Generic = 1000,
		// Token: 0x04000962 RID: 2402
		k_ESteamNetConnectionEnd_App_Max = 1999,
		// Token: 0x04000963 RID: 2403
		k_ESteamNetConnectionEnd_AppException_Min,
		// Token: 0x04000964 RID: 2404
		k_ESteamNetConnectionEnd_AppException_Generic = 2000,
		// Token: 0x04000965 RID: 2405
		k_ESteamNetConnectionEnd_AppException_Max = 2999,
		// Token: 0x04000966 RID: 2406
		k_ESteamNetConnectionEnd_Local_Min,
		// Token: 0x04000967 RID: 2407
		k_ESteamNetConnectionEnd_Local_OfflineMode,
		// Token: 0x04000968 RID: 2408
		k_ESteamNetConnectionEnd_Local_ManyRelayConnectivity,
		// Token: 0x04000969 RID: 2409
		k_ESteamNetConnectionEnd_Local_HostedServerPrimaryRelay,
		// Token: 0x0400096A RID: 2410
		k_ESteamNetConnectionEnd_Local_NetworkConfig,
		// Token: 0x0400096B RID: 2411
		k_ESteamNetConnectionEnd_Local_Rights,
		// Token: 0x0400096C RID: 2412
		k_ESteamNetConnectionEnd_Local_P2P_ICE_NoPublicAddresses,
		// Token: 0x0400096D RID: 2413
		k_ESteamNetConnectionEnd_Local_Max = 3999,
		// Token: 0x0400096E RID: 2414
		k_ESteamNetConnectionEnd_Remote_Min,
		// Token: 0x0400096F RID: 2415
		k_ESteamNetConnectionEnd_Remote_Timeout,
		// Token: 0x04000970 RID: 2416
		k_ESteamNetConnectionEnd_Remote_BadCrypt,
		// Token: 0x04000971 RID: 2417
		k_ESteamNetConnectionEnd_Remote_BadCert,
		// Token: 0x04000972 RID: 2418
		k_ESteamNetConnectionEnd_Remote_BadProtocolVersion = 4006,
		// Token: 0x04000973 RID: 2419
		k_ESteamNetConnectionEnd_Remote_P2P_ICE_NoPublicAddresses,
		// Token: 0x04000974 RID: 2420
		k_ESteamNetConnectionEnd_Remote_Max = 4999,
		// Token: 0x04000975 RID: 2421
		k_ESteamNetConnectionEnd_Misc_Min,
		// Token: 0x04000976 RID: 2422
		k_ESteamNetConnectionEnd_Misc_Generic,
		// Token: 0x04000977 RID: 2423
		k_ESteamNetConnectionEnd_Misc_InternalError,
		// Token: 0x04000978 RID: 2424
		k_ESteamNetConnectionEnd_Misc_Timeout,
		// Token: 0x04000979 RID: 2425
		k_ESteamNetConnectionEnd_Misc_SteamConnectivity = 5005,
		// Token: 0x0400097A RID: 2426
		k_ESteamNetConnectionEnd_Misc_NoRelaySessionsToClient,
		// Token: 0x0400097B RID: 2427
		k_ESteamNetConnectionEnd_Misc_P2P_Rendezvous = 5008,
		// Token: 0x0400097C RID: 2428
		k_ESteamNetConnectionEnd_Misc_P2P_NAT_Firewall,
		// Token: 0x0400097D RID: 2429
		k_ESteamNetConnectionEnd_Misc_PeerSentNoConnection,
		// Token: 0x0400097E RID: 2430
		k_ESteamNetConnectionEnd_Misc_Max = 5999,
		// Token: 0x0400097F RID: 2431
		k_ESteamNetConnectionEnd__Force32Bit = 2147483647
	}
}
