using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000071 RID: 113
	[CallbackIdentity(2804)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInputGamepadSlotChange_t
	{
		// Token: 0x0400011C RID: 284
		public const int k_iCallback = 2804;

		// Token: 0x0400011D RID: 285
		public AppId_t m_unAppID;

		// Token: 0x0400011E RID: 286
		public InputHandle_t m_ulDeviceHandle;

		// Token: 0x0400011F RID: 287
		public ESteamInputType m_eDeviceType;

		// Token: 0x04000120 RID: 288
		public int m_nOldGamepadSlot;

		// Token: 0x04000121 RID: 289
		public int m_nNewGamepadSlot;
	}
}
