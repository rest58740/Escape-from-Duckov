using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C2 RID: 194
	[CallbackIdentity(1331)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileWriteAsyncComplete_t
	{
		// Token: 0x04000245 RID: 581
		public const int k_iCallback = 1331;

		// Token: 0x04000246 RID: 582
		public EResult m_eResult;
	}
}
