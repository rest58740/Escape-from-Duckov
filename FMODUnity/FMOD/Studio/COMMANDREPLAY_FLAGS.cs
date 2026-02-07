using System;

namespace FMOD.Studio
{
	// Token: 0x020000E2 RID: 226
	[Flags]
	public enum COMMANDREPLAY_FLAGS : uint
	{
		// Token: 0x0400050A RID: 1290
		NORMAL = 0U,
		// Token: 0x0400050B RID: 1291
		SKIP_CLEANUP = 1U,
		// Token: 0x0400050C RID: 1292
		FAST_FORWARD = 2U,
		// Token: 0x0400050D RID: 1293
		SKIP_BANK_LOAD = 4U
	}
}
