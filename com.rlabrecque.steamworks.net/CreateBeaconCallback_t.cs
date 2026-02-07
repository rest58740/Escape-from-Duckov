using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008B RID: 139
	[CallbackIdentity(5302)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CreateBeaconCallback_t
	{
		// Token: 0x0400018F RID: 399
		public const int k_iCallback = 5302;

		// Token: 0x04000190 RID: 400
		public EResult m_eResult;

		// Token: 0x04000191 RID: 401
		public PartyBeaconID_t m_ulBeaconID;
	}
}
