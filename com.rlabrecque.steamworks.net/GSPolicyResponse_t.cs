using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004B RID: 75
	[CallbackIdentity(115)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSPolicyResponse_t
	{
		// Token: 0x04000076 RID: 118
		public const int k_iCallback = 115;

		// Token: 0x04000077 RID: 119
		public byte m_bSecure;
	}
}
