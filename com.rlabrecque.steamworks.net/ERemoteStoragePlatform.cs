using System;

namespace Steamworks
{
	// Token: 0x0200012E RID: 302
	[Flags]
	public enum ERemoteStoragePlatform
	{
		// Token: 0x040006C4 RID: 1732
		k_ERemoteStoragePlatformNone = 0,
		// Token: 0x040006C5 RID: 1733
		k_ERemoteStoragePlatformWindows = 1,
		// Token: 0x040006C6 RID: 1734
		k_ERemoteStoragePlatformOSX = 2,
		// Token: 0x040006C7 RID: 1735
		k_ERemoteStoragePlatformPS3 = 4,
		// Token: 0x040006C8 RID: 1736
		k_ERemoteStoragePlatformLinux = 8,
		// Token: 0x040006C9 RID: 1737
		k_ERemoteStoragePlatformSwitch = 16,
		// Token: 0x040006CA RID: 1738
		k_ERemoteStoragePlatformAndroid = 32,
		// Token: 0x040006CB RID: 1739
		k_ERemoteStoragePlatformIOS = 64,
		// Token: 0x040006CC RID: 1740
		k_ERemoteStoragePlatformAll = -1
	}
}
