using System;

namespace FMOD.Studio
{
	// Token: 0x020000E0 RID: 224
	[Flags]
	public enum LOAD_BANK_FLAGS : uint
	{
		// Token: 0x04000501 RID: 1281
		NORMAL = 0U,
		// Token: 0x04000502 RID: 1282
		NONBLOCKING = 1U,
		// Token: 0x04000503 RID: 1283
		DECOMPRESS_SAMPLES = 2U,
		// Token: 0x04000504 RID: 1284
		UNENCRYPTED = 4U
	}
}
