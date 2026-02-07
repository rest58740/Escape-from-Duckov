using System;

namespace Steamworks
{
	// Token: 0x02000156 RID: 342
	public enum EAuthSessionResponse
	{
		// Token: 0x04000860 RID: 2144
		k_EAuthSessionResponseOK,
		// Token: 0x04000861 RID: 2145
		k_EAuthSessionResponseUserNotConnectedToSteam,
		// Token: 0x04000862 RID: 2146
		k_EAuthSessionResponseNoLicenseOrExpired,
		// Token: 0x04000863 RID: 2147
		k_EAuthSessionResponseVACBanned,
		// Token: 0x04000864 RID: 2148
		k_EAuthSessionResponseLoggedInElseWhere,
		// Token: 0x04000865 RID: 2149
		k_EAuthSessionResponseVACCheckTimedOut,
		// Token: 0x04000866 RID: 2150
		k_EAuthSessionResponseAuthTicketCanceled,
		// Token: 0x04000867 RID: 2151
		k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed,
		// Token: 0x04000868 RID: 2152
		k_EAuthSessionResponseAuthTicketInvalid,
		// Token: 0x04000869 RID: 2153
		k_EAuthSessionResponsePublisherIssuedBan,
		// Token: 0x0400086A RID: 2154
		k_EAuthSessionResponseAuthTicketNetworkIdentityFailure
	}
}
