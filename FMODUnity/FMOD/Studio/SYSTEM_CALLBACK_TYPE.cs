using System;

namespace FMOD.Studio
{
	// Token: 0x020000D3 RID: 211
	[Flags]
	public enum SYSTEM_CALLBACK_TYPE : uint
	{
		// Token: 0x040004C1 RID: 1217
		PREUPDATE = 1U,
		// Token: 0x040004C2 RID: 1218
		POSTUPDATE = 2U,
		// Token: 0x040004C3 RID: 1219
		BANK_UNLOAD = 4U,
		// Token: 0x040004C4 RID: 1220
		LIVEUPDATE_CONNECTED = 8U,
		// Token: 0x040004C5 RID: 1221
		LIVEUPDATE_DISCONNECTED = 16U,
		// Token: 0x040004C6 RID: 1222
		ALL = 4294967295U
	}
}
