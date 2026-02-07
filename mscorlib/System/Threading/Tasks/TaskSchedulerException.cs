using System;
using System.Runtime.Serialization;

namespace System.Threading.Tasks
{
	// Token: 0x0200030B RID: 779
	[Serializable]
	public class TaskSchedulerException : Exception
	{
		// Token: 0x0600217D RID: 8573 RVA: 0x000786D9 File Offset: 0x000768D9
		public TaskSchedulerException() : base("An exception was thrown by a TaskScheduler.")
		{
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000328A6 File Offset: 0x00030AA6
		public TaskSchedulerException(string message) : base(message)
		{
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000786E6 File Offset: 0x000768E6
		public TaskSchedulerException(Exception innerException) : base("An exception was thrown by a TaskScheduler.", innerException)
		{
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x000328AF File Offset: 0x00030AAF
		public TaskSchedulerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected TaskSchedulerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
