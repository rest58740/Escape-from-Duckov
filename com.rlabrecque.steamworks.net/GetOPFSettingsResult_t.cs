using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000101 RID: 257
	[CallbackIdentity(4624)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetOPFSettingsResult_t
	{
		// Token: 0x04000321 RID: 801
		public const int k_iCallback = 4624;

		// Token: 0x04000322 RID: 802
		public EResult m_eResult;

		// Token: 0x04000323 RID: 803
		public AppId_t m_unVideoAppID;
	}
}
