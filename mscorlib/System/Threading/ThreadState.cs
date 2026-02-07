using System;

namespace System.Threading
{
	// Token: 0x02000299 RID: 665
	[Flags]
	public enum ThreadState
	{
		// Token: 0x04001A45 RID: 6725
		Running = 0,
		// Token: 0x04001A46 RID: 6726
		StopRequested = 1,
		// Token: 0x04001A47 RID: 6727
		SuspendRequested = 2,
		// Token: 0x04001A48 RID: 6728
		Background = 4,
		// Token: 0x04001A49 RID: 6729
		Unstarted = 8,
		// Token: 0x04001A4A RID: 6730
		Stopped = 16,
		// Token: 0x04001A4B RID: 6731
		WaitSleepJoin = 32,
		// Token: 0x04001A4C RID: 6732
		Suspended = 64,
		// Token: 0x04001A4D RID: 6733
		AbortRequested = 128,
		// Token: 0x04001A4E RID: 6734
		Aborted = 256
	}
}
