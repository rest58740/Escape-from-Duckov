using System;

namespace FMOD.Studio
{
	// Token: 0x020000DF RID: 223
	[Flags]
	public enum INITFLAGS : uint
	{
		// Token: 0x040004F9 RID: 1273
		NORMAL = 0U,
		// Token: 0x040004FA RID: 1274
		LIVEUPDATE = 1U,
		// Token: 0x040004FB RID: 1275
		ALLOW_MISSING_PLUGINS = 2U,
		// Token: 0x040004FC RID: 1276
		SYNCHRONOUS_UPDATE = 4U,
		// Token: 0x040004FD RID: 1277
		DEFERRED_CALLBACKS = 8U,
		// Token: 0x040004FE RID: 1278
		LOAD_FROM_UPDATE = 16U,
		// Token: 0x040004FF RID: 1279
		MEMORY_TRACKING = 32U
	}
}
