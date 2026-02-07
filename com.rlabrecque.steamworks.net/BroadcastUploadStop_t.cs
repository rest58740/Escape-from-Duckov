using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000103 RID: 259
	[CallbackIdentity(4605)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct BroadcastUploadStop_t
	{
		// Token: 0x04000326 RID: 806
		public const int k_iCallback = 4605;

		// Token: 0x04000327 RID: 807
		public EBroadcastUploadResult m_eResult;
	}
}
