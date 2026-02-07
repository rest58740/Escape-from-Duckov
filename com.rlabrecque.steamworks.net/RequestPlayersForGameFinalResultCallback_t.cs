using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000087 RID: 135
	[CallbackIdentity(5213)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RequestPlayersForGameFinalResultCallback_t
	{
		// Token: 0x0400017F RID: 383
		public const int k_iCallback = 5213;

		// Token: 0x04000180 RID: 384
		public EResult m_eResult;

		// Token: 0x04000181 RID: 385
		public ulong m_ullSearchID;

		// Token: 0x04000182 RID: 386
		public ulong m_ullUniqueGameID;
	}
}
