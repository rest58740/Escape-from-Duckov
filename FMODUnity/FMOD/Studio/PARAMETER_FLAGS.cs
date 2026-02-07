using System;

namespace FMOD.Studio
{
	// Token: 0x020000D6 RID: 214
	[Flags]
	public enum PARAMETER_FLAGS : uint
	{
		// Token: 0x040004D4 RID: 1236
		READONLY = 1U,
		// Token: 0x040004D5 RID: 1237
		AUTOMATIC = 2U,
		// Token: 0x040004D6 RID: 1238
		GLOBAL = 4U,
		// Token: 0x040004D7 RID: 1239
		DISCRETE = 8U,
		// Token: 0x040004D8 RID: 1240
		LABELED = 16U
	}
}
