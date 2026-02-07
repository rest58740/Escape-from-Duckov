using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F8 RID: 248
	[CallbackIdentity(702)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LowBatteryPower_t
	{
		// Token: 0x0400030C RID: 780
		public const int k_iCallback = 702;

		// Token: 0x0400030D RID: 781
		public byte m_nMinutesBatteryLeft;
	}
}
