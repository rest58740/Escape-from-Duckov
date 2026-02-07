using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000363 RID: 867
	[Flags]
	public enum TaskContinuationOptions
	{
		// Token: 0x04001D16 RID: 7446
		None = 0,
		// Token: 0x04001D17 RID: 7447
		PreferFairness = 1,
		// Token: 0x04001D18 RID: 7448
		LongRunning = 2,
		// Token: 0x04001D19 RID: 7449
		AttachedToParent = 4,
		// Token: 0x04001D1A RID: 7450
		DenyChildAttach = 8,
		// Token: 0x04001D1B RID: 7451
		HideScheduler = 16,
		// Token: 0x04001D1C RID: 7452
		LazyCancellation = 32,
		// Token: 0x04001D1D RID: 7453
		RunContinuationsAsynchronously = 64,
		// Token: 0x04001D1E RID: 7454
		NotOnRanToCompletion = 65536,
		// Token: 0x04001D1F RID: 7455
		NotOnFaulted = 131072,
		// Token: 0x04001D20 RID: 7456
		NotOnCanceled = 262144,
		// Token: 0x04001D21 RID: 7457
		OnlyOnRanToCompletion = 393216,
		// Token: 0x04001D22 RID: 7458
		OnlyOnFaulted = 327680,
		// Token: 0x04001D23 RID: 7459
		OnlyOnCanceled = 196608,
		// Token: 0x04001D24 RID: 7460
		ExecuteSynchronously = 524288
	}
}
