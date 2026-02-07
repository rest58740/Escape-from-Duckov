using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D2 RID: 210
	[CallbackIdentity(3410)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StartPlaytimeTrackingResult_t
	{
		// Token: 0x04000283 RID: 643
		public const int k_iCallback = 3410;

		// Token: 0x04000284 RID: 644
		public EResult m_eResult;
	}
}
