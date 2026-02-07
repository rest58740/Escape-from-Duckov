using System;

namespace Steamworks
{
	// Token: 0x02000140 RID: 320
	[Flags]
	public enum EItemState
	{
		// Token: 0x04000756 RID: 1878
		k_EItemStateNone = 0,
		// Token: 0x04000757 RID: 1879
		k_EItemStateSubscribed = 1,
		// Token: 0x04000758 RID: 1880
		k_EItemStateLegacyItem = 2,
		// Token: 0x04000759 RID: 1881
		k_EItemStateInstalled = 4,
		// Token: 0x0400075A RID: 1882
		k_EItemStateNeedsUpdate = 8,
		// Token: 0x0400075B RID: 1883
		k_EItemStateDownloading = 16,
		// Token: 0x0400075C RID: 1884
		k_EItemStateDownloadPending = 32,
		// Token: 0x0400075D RID: 1885
		k_EItemStateDisabledLocally = 64
	}
}
