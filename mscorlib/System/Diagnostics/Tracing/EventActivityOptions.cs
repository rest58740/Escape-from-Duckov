using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009E3 RID: 2531
	[Flags]
	public enum EventActivityOptions
	{
		// Token: 0x040037D7 RID: 14295
		None = 0,
		// Token: 0x040037D8 RID: 14296
		Disable = 2,
		// Token: 0x040037D9 RID: 14297
		Recursive = 4,
		// Token: 0x040037DA RID: 14298
		Detachable = 8
	}
}
