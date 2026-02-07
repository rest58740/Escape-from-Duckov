using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009C RID: 156
	[CallbackIdentity(4011)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsVolume_t
	{
		// Token: 0x040001A8 RID: 424
		public const int k_iCallback = 4011;

		// Token: 0x040001A9 RID: 425
		public float m_flNewVolume;
	}
}
