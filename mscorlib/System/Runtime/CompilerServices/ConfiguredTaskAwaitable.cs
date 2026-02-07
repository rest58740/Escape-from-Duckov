using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000814 RID: 2068
	public readonly struct ConfiguredTaskAwaitable
	{
		// Token: 0x0600464A RID: 17994 RVA: 0x000E5DFD File Offset: 0x000E3FFD
		internal ConfiguredTaskAwaitable(Task task, bool continueOnCapturedContext)
		{
			this.m_configuredTaskAwaiter = new ConfiguredTaskAwaitable.ConfiguredTaskAwaiter(task, continueOnCapturedContext);
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x000E5E0C File Offset: 0x000E400C
		public ConfiguredTaskAwaitable.ConfiguredTaskAwaiter GetAwaiter()
		{
			return this.m_configuredTaskAwaiter;
		}

		// Token: 0x04002D4D RID: 11597
		private readonly ConfiguredTaskAwaitable.ConfiguredTaskAwaiter m_configuredTaskAwaiter;

		// Token: 0x02000815 RID: 2069
		public readonly struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion, IConfiguredTaskAwaiter
		{
			// Token: 0x0600464C RID: 17996 RVA: 0x000E5E14 File Offset: 0x000E4014
			internal ConfiguredTaskAwaiter(Task task, bool continueOnCapturedContext)
			{
				this.m_task = task;
				this.m_continueOnCapturedContext = continueOnCapturedContext;
			}

			// Token: 0x17000AD2 RID: 2770
			// (get) Token: 0x0600464D RID: 17997 RVA: 0x000E5E24 File Offset: 0x000E4024
			public bool IsCompleted
			{
				get
				{
					return this.m_task.IsCompleted;
				}
			}

			// Token: 0x0600464E RID: 17998 RVA: 0x000E5E31 File Offset: 0x000E4031
			[SecuritySafeCritical]
			public void OnCompleted(Action continuation)
			{
				TaskAwaiter.OnCompletedInternal(this.m_task, continuation, this.m_continueOnCapturedContext, true);
			}

			// Token: 0x0600464F RID: 17999 RVA: 0x000E5E46 File Offset: 0x000E4046
			[SecurityCritical]
			public void UnsafeOnCompleted(Action continuation)
			{
				TaskAwaiter.OnCompletedInternal(this.m_task, continuation, this.m_continueOnCapturedContext, false);
			}

			// Token: 0x06004650 RID: 18000 RVA: 0x000E5E5B File Offset: 0x000E405B
			[StackTraceHidden]
			public void GetResult()
			{
				TaskAwaiter.ValidateEnd(this.m_task);
			}

			// Token: 0x04002D4E RID: 11598
			internal readonly Task m_task;

			// Token: 0x04002D4F RID: 11599
			internal readonly bool m_continueOnCapturedContext;
		}
	}
}
