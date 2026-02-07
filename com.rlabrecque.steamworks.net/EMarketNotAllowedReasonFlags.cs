using System;

namespace Steamworks
{
	// Token: 0x0200015E RID: 350
	[Flags]
	public enum EMarketNotAllowedReasonFlags
	{
		// Token: 0x040008BB RID: 2235
		k_EMarketNotAllowedReason_None = 0,
		// Token: 0x040008BC RID: 2236
		k_EMarketNotAllowedReason_TemporaryFailure = 1,
		// Token: 0x040008BD RID: 2237
		k_EMarketNotAllowedReason_AccountDisabled = 2,
		// Token: 0x040008BE RID: 2238
		k_EMarketNotAllowedReason_AccountLockedDown = 4,
		// Token: 0x040008BF RID: 2239
		k_EMarketNotAllowedReason_AccountLimited = 8,
		// Token: 0x040008C0 RID: 2240
		k_EMarketNotAllowedReason_TradeBanned = 16,
		// Token: 0x040008C1 RID: 2241
		k_EMarketNotAllowedReason_AccountNotTrusted = 32,
		// Token: 0x040008C2 RID: 2242
		k_EMarketNotAllowedReason_SteamGuardNotEnabled = 64,
		// Token: 0x040008C3 RID: 2243
		k_EMarketNotAllowedReason_SteamGuardOnlyRecentlyEnabled = 128,
		// Token: 0x040008C4 RID: 2244
		k_EMarketNotAllowedReason_RecentPasswordReset = 256,
		// Token: 0x040008C5 RID: 2245
		k_EMarketNotAllowedReason_NewPaymentMethod = 512,
		// Token: 0x040008C6 RID: 2246
		k_EMarketNotAllowedReason_InvalidCookie = 1024,
		// Token: 0x040008C7 RID: 2247
		k_EMarketNotAllowedReason_UsingNewDevice = 2048,
		// Token: 0x040008C8 RID: 2248
		k_EMarketNotAllowedReason_RecentSelfRefund = 4096,
		// Token: 0x040008C9 RID: 2249
		k_EMarketNotAllowedReason_NewPaymentMethodCannotBeVerified = 8192,
		// Token: 0x040008CA RID: 2250
		k_EMarketNotAllowedReason_NoRecentPurchases = 16384,
		// Token: 0x040008CB RID: 2251
		k_EMarketNotAllowedReason_AcceptedWalletGift = 32768
	}
}
