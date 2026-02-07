using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000050 RID: 80
	[CallbackIdentity(211)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ComputeNewPlayerCompatibilityResult_t
	{
		// Token: 0x0400008C RID: 140
		public const int k_iCallback = 211;

		// Token: 0x0400008D RID: 141
		public EResult m_eResult;

		// Token: 0x0400008E RID: 142
		public int m_cPlayersThatDontLikeCandidate;

		// Token: 0x0400008F RID: 143
		public int m_cPlayersThatCandidateDoesntLike;

		// Token: 0x04000090 RID: 144
		public int m_cClanPlayersThatDontLikeCandidate;

		// Token: 0x04000091 RID: 145
		public CSteamID m_SteamIDCandidate;
	}
}
