using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200037D RID: 893
	public class UnobservedTaskExceptionEventArgs : EventArgs
	{
		// Token: 0x06002532 RID: 9522 RVA: 0x000844B7 File Offset: 0x000826B7
		public UnobservedTaskExceptionEventArgs(AggregateException exception)
		{
			this.m_exception = exception;
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000844C6 File Offset: 0x000826C6
		public void SetObserved()
		{
			this.m_observed = true;
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x000844CF File Offset: 0x000826CF
		public bool Observed
		{
			get
			{
				return this.m_observed;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x000844D7 File Offset: 0x000826D7
		public AggregateException Exception
		{
			get
			{
				return this.m_exception;
			}
		}

		// Token: 0x04001D57 RID: 7511
		private AggregateException m_exception;

		// Token: 0x04001D58 RID: 7512
		internal bool m_observed;
	}
}
