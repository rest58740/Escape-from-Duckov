using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000811 RID: 2065
	public readonly struct TaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion, ITaskAwaiter
	{
		// Token: 0x06004645 RID: 17989 RVA: 0x000E5DAF File Offset: 0x000E3FAF
		internal TaskAwaiter(Task<TResult> task)
		{
			this.m_task = task;
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06004646 RID: 17990 RVA: 0x000E5DB8 File Offset: 0x000E3FB8
		public bool IsCompleted
		{
			get
			{
				return this.m_task.IsCompleted;
			}
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x000E5DC5 File Offset: 0x000E3FC5
		[SecuritySafeCritical]
		public void OnCompleted(Action continuation)
		{
			TaskAwaiter.OnCompletedInternal(this.m_task, continuation, true, true);
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x000E5DD5 File Offset: 0x000E3FD5
		[SecurityCritical]
		public void UnsafeOnCompleted(Action continuation)
		{
			TaskAwaiter.OnCompletedInternal(this.m_task, continuation, true, false);
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x000E5DE5 File Offset: 0x000E3FE5
		[StackTraceHidden]
		public TResult GetResult()
		{
			TaskAwaiter.ValidateEnd(this.m_task);
			return this.m_task.ResultOnSuccess;
		}

		// Token: 0x04002D4C RID: 11596
		private readonly Task<TResult> m_task;
	}
}
