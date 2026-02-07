using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000356 RID: 854
	public enum TaskStatus
	{
		// Token: 0x04001CC1 RID: 7361
		Created,
		// Token: 0x04001CC2 RID: 7362
		WaitingForActivation,
		// Token: 0x04001CC3 RID: 7363
		WaitingToRun,
		// Token: 0x04001CC4 RID: 7364
		Running,
		// Token: 0x04001CC5 RID: 7365
		WaitingForChildrenToComplete,
		// Token: 0x04001CC6 RID: 7366
		RanToCompletion,
		// Token: 0x04001CC7 RID: 7367
		Canceled,
		// Token: 0x04001CC8 RID: 7368
		Faulted
	}
}
