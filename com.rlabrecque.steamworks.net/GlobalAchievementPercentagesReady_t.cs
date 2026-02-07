using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F4 RID: 244
	[CallbackIdentity(1110)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalAchievementPercentagesReady_t
	{
		// Token: 0x04000302 RID: 770
		public const int k_iCallback = 1110;

		// Token: 0x04000303 RID: 771
		public ulong m_nGameID;

		// Token: 0x04000304 RID: 772
		public EResult m_eResult;
	}
}
