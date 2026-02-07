using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009FB RID: 2555
	[Flags]
	public enum EventSourceSettings
	{
		// Token: 0x04003839 RID: 14393
		Default = 0,
		// Token: 0x0400383A RID: 14394
		ThrowOnEventWriteErrors = 1,
		// Token: 0x0400383B RID: 14395
		EtwManifestEventFormat = 4,
		// Token: 0x0400383C RID: 14396
		EtwSelfDescribingEventFormat = 8
	}
}
