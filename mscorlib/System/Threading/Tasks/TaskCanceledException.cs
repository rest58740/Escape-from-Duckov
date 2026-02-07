using System;
using System.Runtime.Serialization;

namespace System.Threading.Tasks
{
	// Token: 0x02000309 RID: 777
	[Serializable]
	public class TaskCanceledException : OperationCanceledException
	{
		// Token: 0x06002167 RID: 8551 RVA: 0x00078485 File Offset: 0x00076685
		public TaskCanceledException() : base("A task was canceled.")
		{
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x00078492 File Offset: 0x00076692
		public TaskCanceledException(string message) : base(message)
		{
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0007849B File Offset: 0x0007669B
		public TaskCanceledException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000784A5 File Offset: 0x000766A5
		public TaskCanceledException(string message, Exception innerException, CancellationToken token) : base(message, innerException, token)
		{
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000784B0 File Offset: 0x000766B0
		public TaskCanceledException(Task task) : base("A task was canceled.", (task != null) ? task.CancellationToken : default(CancellationToken))
		{
			this._canceledTask = task;
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x000784E3 File Offset: 0x000766E3
		protected TaskCanceledException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x000784ED File Offset: 0x000766ED
		public Task Task
		{
			get
			{
				return this._canceledTask;
			}
		}

		// Token: 0x04001BC7 RID: 7111
		[NonSerialized]
		private readonly Task _canceledTask;
	}
}
