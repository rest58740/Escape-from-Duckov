using System;

namespace System.Runtime
{
	// Token: 0x02000551 RID: 1361
	[Serializable]
	public enum GCLatencyMode
	{
		// Token: 0x04002509 RID: 9481
		Batch,
		// Token: 0x0400250A RID: 9482
		Interactive,
		// Token: 0x0400250B RID: 9483
		LowLatency,
		// Token: 0x0400250C RID: 9484
		SustainedLowLatency,
		// Token: 0x0400250D RID: 9485
		NoGCRegion
	}
}
