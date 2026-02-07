using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000002 RID: 2
	public class CustomLogger : ILogger
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public CustomLogger(Action<string> logWarningDelegate, Action<string> logErrorDelegate, Action<Exception> logExceptionDelegate)
		{
			if (logWarningDelegate == null)
			{
				throw new ArgumentNullException("logWarningDelegate");
			}
			if (logErrorDelegate == null)
			{
				throw new ArgumentNullException("logErrorDelegate");
			}
			if (logExceptionDelegate == null)
			{
				throw new ArgumentNullException("logExceptionDelegate");
			}
			this.logWarningDelegate = logWarningDelegate;
			this.logErrorDelegate = logErrorDelegate;
			this.logExceptionDelegate = logExceptionDelegate;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A2 File Offset: 0x000002A2
		public void LogWarning(string warning)
		{
			this.logWarningDelegate.Invoke(warning);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020B0 File Offset: 0x000002B0
		public void LogError(string error)
		{
			this.logErrorDelegate.Invoke(error);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020BE File Offset: 0x000002BE
		public void LogException(Exception exception)
		{
			this.logExceptionDelegate.Invoke(exception);
		}

		// Token: 0x04000001 RID: 1
		private Action<string> logWarningDelegate;

		// Token: 0x04000002 RID: 2
		private Action<string> logErrorDelegate;

		// Token: 0x04000003 RID: 3
		private Action<Exception> logExceptionDelegate;
	}
}
