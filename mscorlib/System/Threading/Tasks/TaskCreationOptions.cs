using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000361 RID: 865
	[Flags]
	public enum TaskCreationOptions
	{
		// Token: 0x04001D06 RID: 7430
		None = 0,
		// Token: 0x04001D07 RID: 7431
		PreferFairness = 1,
		// Token: 0x04001D08 RID: 7432
		LongRunning = 2,
		// Token: 0x04001D09 RID: 7433
		AttachedToParent = 4,
		// Token: 0x04001D0A RID: 7434
		DenyChildAttach = 8,
		// Token: 0x04001D0B RID: 7435
		HideScheduler = 16,
		// Token: 0x04001D0C RID: 7436
		RunContinuationsAsynchronously = 64
	}
}
