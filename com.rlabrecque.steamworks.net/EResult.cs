using System;

namespace Steamworks
{
	// Token: 0x02000152 RID: 338
	public enum EResult
	{
		// Token: 0x040007BA RID: 1978
		k_EResultNone,
		// Token: 0x040007BB RID: 1979
		k_EResultOK,
		// Token: 0x040007BC RID: 1980
		k_EResultFail,
		// Token: 0x040007BD RID: 1981
		k_EResultNoConnection,
		// Token: 0x040007BE RID: 1982
		k_EResultInvalidPassword = 5,
		// Token: 0x040007BF RID: 1983
		k_EResultLoggedInElsewhere,
		// Token: 0x040007C0 RID: 1984
		k_EResultInvalidProtocolVer,
		// Token: 0x040007C1 RID: 1985
		k_EResultInvalidParam,
		// Token: 0x040007C2 RID: 1986
		k_EResultFileNotFound,
		// Token: 0x040007C3 RID: 1987
		k_EResultBusy,
		// Token: 0x040007C4 RID: 1988
		k_EResultInvalidState,
		// Token: 0x040007C5 RID: 1989
		k_EResultInvalidName,
		// Token: 0x040007C6 RID: 1990
		k_EResultInvalidEmail,
		// Token: 0x040007C7 RID: 1991
		k_EResultDuplicateName,
		// Token: 0x040007C8 RID: 1992
		k_EResultAccessDenied,
		// Token: 0x040007C9 RID: 1993
		k_EResultTimeout,
		// Token: 0x040007CA RID: 1994
		k_EResultBanned,
		// Token: 0x040007CB RID: 1995
		k_EResultAccountNotFound,
		// Token: 0x040007CC RID: 1996
		k_EResultInvalidSteamID,
		// Token: 0x040007CD RID: 1997
		k_EResultServiceUnavailable,
		// Token: 0x040007CE RID: 1998
		k_EResultNotLoggedOn,
		// Token: 0x040007CF RID: 1999
		k_EResultPending,
		// Token: 0x040007D0 RID: 2000
		k_EResultEncryptionFailure,
		// Token: 0x040007D1 RID: 2001
		k_EResultInsufficientPrivilege,
		// Token: 0x040007D2 RID: 2002
		k_EResultLimitExceeded,
		// Token: 0x040007D3 RID: 2003
		k_EResultRevoked,
		// Token: 0x040007D4 RID: 2004
		k_EResultExpired,
		// Token: 0x040007D5 RID: 2005
		k_EResultAlreadyRedeemed,
		// Token: 0x040007D6 RID: 2006
		k_EResultDuplicateRequest,
		// Token: 0x040007D7 RID: 2007
		k_EResultAlreadyOwned,
		// Token: 0x040007D8 RID: 2008
		k_EResultIPNotFound,
		// Token: 0x040007D9 RID: 2009
		k_EResultPersistFailed,
		// Token: 0x040007DA RID: 2010
		k_EResultLockingFailed,
		// Token: 0x040007DB RID: 2011
		k_EResultLogonSessionReplaced,
		// Token: 0x040007DC RID: 2012
		k_EResultConnectFailed,
		// Token: 0x040007DD RID: 2013
		k_EResultHandshakeFailed,
		// Token: 0x040007DE RID: 2014
		k_EResultIOFailure,
		// Token: 0x040007DF RID: 2015
		k_EResultRemoteDisconnect,
		// Token: 0x040007E0 RID: 2016
		k_EResultShoppingCartNotFound,
		// Token: 0x040007E1 RID: 2017
		k_EResultBlocked,
		// Token: 0x040007E2 RID: 2018
		k_EResultIgnored,
		// Token: 0x040007E3 RID: 2019
		k_EResultNoMatch,
		// Token: 0x040007E4 RID: 2020
		k_EResultAccountDisabled,
		// Token: 0x040007E5 RID: 2021
		k_EResultServiceReadOnly,
		// Token: 0x040007E6 RID: 2022
		k_EResultAccountNotFeatured,
		// Token: 0x040007E7 RID: 2023
		k_EResultAdministratorOK,
		// Token: 0x040007E8 RID: 2024
		k_EResultContentVersion,
		// Token: 0x040007E9 RID: 2025
		k_EResultTryAnotherCM,
		// Token: 0x040007EA RID: 2026
		k_EResultPasswordRequiredToKickSession,
		// Token: 0x040007EB RID: 2027
		k_EResultAlreadyLoggedInElsewhere,
		// Token: 0x040007EC RID: 2028
		k_EResultSuspended,
		// Token: 0x040007ED RID: 2029
		k_EResultCancelled,
		// Token: 0x040007EE RID: 2030
		k_EResultDataCorruption,
		// Token: 0x040007EF RID: 2031
		k_EResultDiskFull,
		// Token: 0x040007F0 RID: 2032
		k_EResultRemoteCallFailed,
		// Token: 0x040007F1 RID: 2033
		k_EResultPasswordUnset,
		// Token: 0x040007F2 RID: 2034
		k_EResultExternalAccountUnlinked,
		// Token: 0x040007F3 RID: 2035
		k_EResultPSNTicketInvalid,
		// Token: 0x040007F4 RID: 2036
		k_EResultExternalAccountAlreadyLinked,
		// Token: 0x040007F5 RID: 2037
		k_EResultRemoteFileConflict,
		// Token: 0x040007F6 RID: 2038
		k_EResultIllegalPassword,
		// Token: 0x040007F7 RID: 2039
		k_EResultSameAsPreviousValue,
		// Token: 0x040007F8 RID: 2040
		k_EResultAccountLogonDenied,
		// Token: 0x040007F9 RID: 2041
		k_EResultCannotUseOldPassword,
		// Token: 0x040007FA RID: 2042
		k_EResultInvalidLoginAuthCode,
		// Token: 0x040007FB RID: 2043
		k_EResultAccountLogonDeniedNoMail,
		// Token: 0x040007FC RID: 2044
		k_EResultHardwareNotCapableOfIPT,
		// Token: 0x040007FD RID: 2045
		k_EResultIPTInitError,
		// Token: 0x040007FE RID: 2046
		k_EResultParentalControlRestricted,
		// Token: 0x040007FF RID: 2047
		k_EResultFacebookQueryError,
		// Token: 0x04000800 RID: 2048
		k_EResultExpiredLoginAuthCode,
		// Token: 0x04000801 RID: 2049
		k_EResultIPLoginRestrictionFailed,
		// Token: 0x04000802 RID: 2050
		k_EResultAccountLockedDown,
		// Token: 0x04000803 RID: 2051
		k_EResultAccountLogonDeniedVerifiedEmailRequired,
		// Token: 0x04000804 RID: 2052
		k_EResultNoMatchingURL,
		// Token: 0x04000805 RID: 2053
		k_EResultBadResponse,
		// Token: 0x04000806 RID: 2054
		k_EResultRequirePasswordReEntry,
		// Token: 0x04000807 RID: 2055
		k_EResultValueOutOfRange,
		// Token: 0x04000808 RID: 2056
		k_EResultUnexpectedError,
		// Token: 0x04000809 RID: 2057
		k_EResultDisabled,
		// Token: 0x0400080A RID: 2058
		k_EResultInvalidCEGSubmission,
		// Token: 0x0400080B RID: 2059
		k_EResultRestrictedDevice,
		// Token: 0x0400080C RID: 2060
		k_EResultRegionLocked,
		// Token: 0x0400080D RID: 2061
		k_EResultRateLimitExceeded,
		// Token: 0x0400080E RID: 2062
		k_EResultAccountLoginDeniedNeedTwoFactor,
		// Token: 0x0400080F RID: 2063
		k_EResultItemDeleted,
		// Token: 0x04000810 RID: 2064
		k_EResultAccountLoginDeniedThrottle,
		// Token: 0x04000811 RID: 2065
		k_EResultTwoFactorCodeMismatch,
		// Token: 0x04000812 RID: 2066
		k_EResultTwoFactorActivationCodeMismatch,
		// Token: 0x04000813 RID: 2067
		k_EResultAccountAssociatedToMultiplePartners,
		// Token: 0x04000814 RID: 2068
		k_EResultNotModified,
		// Token: 0x04000815 RID: 2069
		k_EResultNoMobileDevice,
		// Token: 0x04000816 RID: 2070
		k_EResultTimeNotSynced,
		// Token: 0x04000817 RID: 2071
		k_EResultSmsCodeFailed,
		// Token: 0x04000818 RID: 2072
		k_EResultAccountLimitExceeded,
		// Token: 0x04000819 RID: 2073
		k_EResultAccountActivityLimitExceeded,
		// Token: 0x0400081A RID: 2074
		k_EResultPhoneActivityLimitExceeded,
		// Token: 0x0400081B RID: 2075
		k_EResultRefundToWallet,
		// Token: 0x0400081C RID: 2076
		k_EResultEmailSendFailure,
		// Token: 0x0400081D RID: 2077
		k_EResultNotSettled,
		// Token: 0x0400081E RID: 2078
		k_EResultNeedCaptcha,
		// Token: 0x0400081F RID: 2079
		k_EResultGSLTDenied,
		// Token: 0x04000820 RID: 2080
		k_EResultGSOwnerDenied,
		// Token: 0x04000821 RID: 2081
		k_EResultInvalidItemType,
		// Token: 0x04000822 RID: 2082
		k_EResultIPBanned,
		// Token: 0x04000823 RID: 2083
		k_EResultGSLTExpired,
		// Token: 0x04000824 RID: 2084
		k_EResultInsufficientFunds,
		// Token: 0x04000825 RID: 2085
		k_EResultTooManyPending,
		// Token: 0x04000826 RID: 2086
		k_EResultNoSiteLicensesFound,
		// Token: 0x04000827 RID: 2087
		k_EResultWGNetworkSendExceeded,
		// Token: 0x04000828 RID: 2088
		k_EResultAccountNotFriends,
		// Token: 0x04000829 RID: 2089
		k_EResultLimitedUserAccount,
		// Token: 0x0400082A RID: 2090
		k_EResultCantRemoveItem,
		// Token: 0x0400082B RID: 2091
		k_EResultAccountDeleted,
		// Token: 0x0400082C RID: 2092
		k_EResultExistingUserCancelledLicense,
		// Token: 0x0400082D RID: 2093
		k_EResultCommunityCooldown,
		// Token: 0x0400082E RID: 2094
		k_EResultNoLauncherSpecified,
		// Token: 0x0400082F RID: 2095
		k_EResultMustAgreeToSSA,
		// Token: 0x04000830 RID: 2096
		k_EResultLauncherMigrated,
		// Token: 0x04000831 RID: 2097
		k_EResultSteamRealmMismatch,
		// Token: 0x04000832 RID: 2098
		k_EResultInvalidSignature,
		// Token: 0x04000833 RID: 2099
		k_EResultParseFailure,
		// Token: 0x04000834 RID: 2100
		k_EResultNoVerifiedPhone,
		// Token: 0x04000835 RID: 2101
		k_EResultInsufficientBattery,
		// Token: 0x04000836 RID: 2102
		k_EResultChargerRequired,
		// Token: 0x04000837 RID: 2103
		k_EResultCachedCredentialInvalid,
		// Token: 0x04000838 RID: 2104
		K_EResultPhoneNumberIsVOIP,
		// Token: 0x04000839 RID: 2105
		k_EResultNotSupported,
		// Token: 0x0400083A RID: 2106
		k_EResultFamilySizeLimitExceeded,
		// Token: 0x0400083B RID: 2107
		k_EResultOfflineAppCacheInvalid
	}
}
