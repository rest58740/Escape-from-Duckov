using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000FB RID: 251
	[CallbackIdentity(705)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CheckFileSignature_t
	{
		// Token: 0x04000313 RID: 787
		public const int k_iCallback = 705;

		// Token: 0x04000314 RID: 788
		public ECheckFileSignature m_eCheckFileSignature;
	}
}
