using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000179 RID: 377
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamItemDetails_t
	{
		// Token: 0x04000A0C RID: 2572
		public SteamItemInstanceID_t m_itemId;

		// Token: 0x04000A0D RID: 2573
		public SteamItemDef_t m_iDefinition;

		// Token: 0x04000A0E RID: 2574
		public ushort m_unQuantity;

		// Token: 0x04000A0F RID: 2575
		public ushort m_unFlags;
	}
}
