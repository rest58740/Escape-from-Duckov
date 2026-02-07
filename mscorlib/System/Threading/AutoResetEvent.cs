using System;

namespace System.Threading
{
	// Token: 0x0200028C RID: 652
	public sealed class AutoResetEvent : EventWaitHandle
	{
		// Token: 0x06001D7A RID: 7546 RVA: 0x0006E428 File Offset: 0x0006C628
		public AutoResetEvent(bool initialState) : base(initialState, EventResetMode.AutoReset)
		{
		}
	}
}
