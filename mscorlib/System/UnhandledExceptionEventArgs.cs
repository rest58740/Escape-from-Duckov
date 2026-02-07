using System;

namespace System
{
	// Token: 0x020001AA RID: 426
	[Serializable]
	public class UnhandledExceptionEventArgs : EventArgs
	{
		// Token: 0x06001263 RID: 4707 RVA: 0x000484F9 File Offset: 0x000466F9
		public UnhandledExceptionEventArgs(object exception, bool isTerminating)
		{
			this._exception = exception;
			this._isTerminating = isTerminating;
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0004850F File Offset: 0x0004670F
		public object ExceptionObject
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x00048517 File Offset: 0x00046717
		public bool IsTerminating
		{
			get
			{
				return this._isTerminating;
			}
		}

		// Token: 0x04001364 RID: 4964
		private object _exception;

		// Token: 0x04001365 RID: 4965
		private bool _isTerminating;
	}
}
