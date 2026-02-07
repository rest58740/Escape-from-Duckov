using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000091 RID: 145
	[CallbackIdentity(4002)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct VolumeHasChanged_t
	{
		// Token: 0x0400019A RID: 410
		public const int k_iCallback = 4002;

		// Token: 0x0400019B RID: 411
		public float m_flNewVolume;
	}
}
