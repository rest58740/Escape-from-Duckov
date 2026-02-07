using System;

namespace System.Threading
{
	// Token: 0x02000291 RID: 657
	public sealed class ManualResetEvent : EventWaitHandle
	{
		// Token: 0x06001D8A RID: 7562 RVA: 0x0006E69A File Offset: 0x0006C89A
		public ManualResetEvent(bool initialState) : base(initialState, EventResetMode.ManualReset)
		{
		}
	}
}
