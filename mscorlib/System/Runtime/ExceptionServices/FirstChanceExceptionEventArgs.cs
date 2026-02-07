using System;

namespace System.Runtime.ExceptionServices
{
	// Token: 0x020007D0 RID: 2000
	public class FirstChanceExceptionEventArgs : EventArgs
	{
		// Token: 0x060045A8 RID: 17832 RVA: 0x000E5057 File Offset: 0x000E3257
		public FirstChanceExceptionEventArgs(Exception exception)
		{
			this.Exception = exception;
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060045A9 RID: 17833 RVA: 0x000E5066 File Offset: 0x000E3266
		public Exception Exception { get; }
	}
}
