using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009F6 RID: 2550
	[Flags]
	public enum EventManifestOptions
	{
		// Token: 0x0400382A RID: 14378
		AllCultures = 2,
		// Token: 0x0400382B RID: 14379
		AllowEventSourceOverride = 8,
		// Token: 0x0400382C RID: 14380
		None = 0,
		// Token: 0x0400382D RID: 14381
		OnlyIfNeededForRegistration = 4,
		// Token: 0x0400382E RID: 14382
		Strict = 1
	}
}
