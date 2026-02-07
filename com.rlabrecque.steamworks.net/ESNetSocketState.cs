using System;

namespace Steamworks
{
	// Token: 0x0200012A RID: 298
	public enum ESNetSocketState
	{
		// Token: 0x0400069B RID: 1691
		k_ESNetSocketStateInvalid,
		// Token: 0x0400069C RID: 1692
		k_ESNetSocketStateConnected,
		// Token: 0x0400069D RID: 1693
		k_ESNetSocketStateInitiated = 10,
		// Token: 0x0400069E RID: 1694
		k_ESNetSocketStateLocalCandidatesFound,
		// Token: 0x0400069F RID: 1695
		k_ESNetSocketStateReceivedRemoteCandidates,
		// Token: 0x040006A0 RID: 1696
		k_ESNetSocketStateChallengeHandshake = 15,
		// Token: 0x040006A1 RID: 1697
		k_ESNetSocketStateDisconnecting = 21,
		// Token: 0x040006A2 RID: 1698
		k_ESNetSocketStateLocalDisconnect,
		// Token: 0x040006A3 RID: 1699
		k_ESNetSocketStateTimeoutDuringConnect,
		// Token: 0x040006A4 RID: 1700
		k_ESNetSocketStateRemoteEndDisconnected,
		// Token: 0x040006A5 RID: 1701
		k_ESNetSocketStateConnectionBroken
	}
}
