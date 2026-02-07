using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006F RID: 111
	[CallbackIdentity(2802)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInputDeviceDisconnected_t
	{
		// Token: 0x04000112 RID: 274
		public const int k_iCallback = 2802;

		// Token: 0x04000113 RID: 275
		public InputHandle_t m_ulDisconnectedDeviceHandle;
	}
}
