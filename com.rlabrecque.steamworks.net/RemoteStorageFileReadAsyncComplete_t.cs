using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C3 RID: 195
	[CallbackIdentity(1332)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileReadAsyncComplete_t
	{
		// Token: 0x04000247 RID: 583
		public const int k_iCallback = 1332;

		// Token: 0x04000248 RID: 584
		public SteamAPICall_t m_hFileReadAsync;

		// Token: 0x04000249 RID: 585
		public EResult m_eResult;

		// Token: 0x0400024A RID: 586
		public uint m_nOffset;

		// Token: 0x0400024B RID: 587
		public uint m_cubRead;
	}
}
