using System;

namespace Steamworks
{
	// Token: 0x0200001F RID: 31
	public static class SteamParentalSettings
	{
		// Token: 0x06000389 RID: 905 RVA: 0x000095C4 File Offset: 0x000077C4
		public static bool BIsParentalLockEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsParentalLockEnabled(CSteamAPIContext.GetSteamParentalSettings());
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000095D5 File Offset: 0x000077D5
		public static bool BIsParentalLockLocked()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsParentalLockLocked(CSteamAPIContext.GetSteamParentalSettings());
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000095E6 File Offset: 0x000077E6
		public static bool BIsAppBlocked(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsAppBlocked(CSteamAPIContext.GetSteamParentalSettings(), nAppID);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000095F8 File Offset: 0x000077F8
		public static bool BIsAppInBlockList(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsAppInBlockList(CSteamAPIContext.GetSteamParentalSettings(), nAppID);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000960A File Offset: 0x0000780A
		public static bool BIsFeatureBlocked(EParentalFeature eFeature)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsFeatureBlocked(CSteamAPIContext.GetSteamParentalSettings(), eFeature);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000961C File Offset: 0x0000781C
		public static bool BIsFeatureInBlockList(EParentalFeature eFeature)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsFeatureInBlockList(CSteamAPIContext.GetSteamParentalSettings(), eFeature);
		}
	}
}
