using System;

namespace Steamworks
{
	// Token: 0x02000128 RID: 296
	public enum EP2PSessionError
	{
		// Token: 0x0400068F RID: 1679
		k_EP2PSessionErrorNone,
		// Token: 0x04000690 RID: 1680
		k_EP2PSessionErrorNoRightsToApp = 2,
		// Token: 0x04000691 RID: 1681
		k_EP2PSessionErrorTimeout = 4,
		// Token: 0x04000692 RID: 1682
		k_EP2PSessionErrorNotRunningApp_DELETED = 1,
		// Token: 0x04000693 RID: 1683
		k_EP2PSessionErrorDestinationNotLoggedIn_DELETED = 3,
		// Token: 0x04000694 RID: 1684
		k_EP2PSessionErrorMax = 5
	}
}
