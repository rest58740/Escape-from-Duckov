using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007F RID: 127
	[CallbackIdentity(510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyMatchList_t
	{
		// Token: 0x04000158 RID: 344
		public const int k_iCallback = 510;

		// Token: 0x04000159 RID: 345
		public uint m_nLobbiesMatching;
	}
}
