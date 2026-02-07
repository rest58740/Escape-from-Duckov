using System;

namespace Steamworks
{
	// Token: 0x02000123 RID: 291
	[Flags]
	public enum EChatMemberStateChange
	{
		// Token: 0x04000676 RID: 1654
		k_EChatMemberStateChangeEntered = 1,
		// Token: 0x04000677 RID: 1655
		k_EChatMemberStateChangeLeft = 2,
		// Token: 0x04000678 RID: 1656
		k_EChatMemberStateChangeDisconnected = 4,
		// Token: 0x04000679 RID: 1657
		k_EChatMemberStateChangeKicked = 8,
		// Token: 0x0400067A RID: 1658
		k_EChatMemberStateChangeBanned = 16
	}
}
