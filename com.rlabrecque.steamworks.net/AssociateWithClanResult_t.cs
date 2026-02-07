using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004F RID: 79
	[CallbackIdentity(210)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AssociateWithClanResult_t
	{
		// Token: 0x0400008A RID: 138
		public const int k_iCallback = 210;

		// Token: 0x0400008B RID: 139
		public EResult m_eResult;
	}
}
