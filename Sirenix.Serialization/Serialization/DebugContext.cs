using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000076 RID: 118
	public sealed class DebugContext
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0001A708 File Offset: 0x00018908
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0001A76C File Offset: 0x0001896C
		public ILogger Logger
		{
			get
			{
				if (this.logger == null)
				{
					object @lock = this.LOCK;
					lock (@lock)
					{
						if (this.logger == null)
						{
							this.logger = DefaultLoggers.UnityLogger;
						}
					}
				}
				return this.logger;
			}
			set
			{
				object @lock = this.LOCK;
				lock (@lock)
				{
					this.logger = value;
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0001A7B0 File Offset: 0x000189B0
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x0001A7BA File Offset: 0x000189BA
		public LoggingPolicy LoggingPolicy
		{
			get
			{
				return this.loggingPolicy;
			}
			set
			{
				this.loggingPolicy = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0001A7C5 File Offset: 0x000189C5
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x0001A7CF File Offset: 0x000189CF
		public ErrorHandlingPolicy ErrorHandlingPolicy
		{
			get
			{
				return this.errorHandlingPolicy;
			}
			set
			{
				this.errorHandlingPolicy = value;
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001A7DA File Offset: 0x000189DA
		public void LogWarning(string message)
		{
			if (this.errorHandlingPolicy == 2)
			{
				throw new SerializationAbortException("The following warning was logged during serialization or deserialization: " + (message ?? "EMPTY EXCEPTION MESSAGE"));
			}
			if (this.loggingPolicy == 1)
			{
				this.Logger.LogWarning(message);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001A818 File Offset: 0x00018A18
		public void LogError(string message)
		{
			if (this.errorHandlingPolicy != null)
			{
				throw new SerializationAbortException("The following error was logged during serialization or deserialization: " + (message ?? "EMPTY EXCEPTION MESSAGE"));
			}
			if (this.loggingPolicy != 2)
			{
				this.Logger.LogError(message);
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001A858 File Offset: 0x00018A58
		public void LogException(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			if (exception is SerializationAbortException)
			{
				throw exception;
			}
			ErrorHandlingPolicy errorHandlingPolicy = this.errorHandlingPolicy;
			if (errorHandlingPolicy != null)
			{
				throw new SerializationAbortException("An exception of type " + exception.GetType().Name + " occurred during serialization or deserialization.", exception);
			}
			if (this.loggingPolicy != 2)
			{
				this.Logger.LogException(exception);
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001A8C4 File Offset: 0x00018AC4
		public void ResetToDefault()
		{
			object @lock = this.LOCK;
			lock (@lock)
			{
				this.logger = null;
				this.loggingPolicy = 0;
				this.errorHandlingPolicy = 0;
			}
		}

		// Token: 0x04000154 RID: 340
		private readonly object LOCK = new object();

		// Token: 0x04000155 RID: 341
		private volatile ILogger logger;

		// Token: 0x04000156 RID: 342
		private volatile LoggingPolicy loggingPolicy;

		// Token: 0x04000157 RID: 343
		private volatile ErrorHandlingPolicy errorHandlingPolicy;
	}
}
