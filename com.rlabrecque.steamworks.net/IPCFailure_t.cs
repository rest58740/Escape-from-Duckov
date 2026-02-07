using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E0 RID: 224
	[CallbackIdentity(117)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct IPCFailure_t
	{
		// Token: 0x040002B5 RID: 693
		public const int k_iCallback = 117;

		// Token: 0x040002B6 RID: 694
		public byte m_eFailureType;
	}
}
