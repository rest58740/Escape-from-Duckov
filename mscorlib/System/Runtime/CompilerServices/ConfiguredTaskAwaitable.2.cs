using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000816 RID: 2070
	public readonly struct ConfiguredTaskAwaitable<TResult>
	{
		// Token: 0x06004651 RID: 18001 RVA: 0x000E5E68 File Offset: 0x000E4068
		internal ConfiguredTaskAwaitable(Task<TResult> task, bool continueOnCapturedContext)
		{
			this.m_configuredTaskAwaiter = new ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter(task, continueOnCapturedContext);
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x000E5E77 File Offset: 0x000E4077
		public ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter GetAwaiter()
		{
			return this.m_configuredTaskAwaiter;
		}

		// Token: 0x04002D50 RID: 11600
		private readonly ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter m_configuredTaskAwaiter;

		// Token: 0x02000817 RID: 2071
		public readonly struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion, IConfiguredTaskAwaiter
		{
			// Token: 0x06004653 RID: 18003 RVA: 0x000E5E7F File Offset: 0x000E407F
			internal ConfiguredTaskAwaiter(Task<TResult> task, bool continueOnCapturedContext)
			{
				this.m_task = task;
				this.m_continueOnCapturedContext = continueOnCapturedContext;
			}

			// Token: 0x17000AD3 RID: 2771
			// (get) Token: 0x06004654 RID: 18004 RVA: 0x000E5E8F File Offset: 0x000E408F
			public bool IsCompleted
			{
				get
				{
					return this.m_task.IsCompleted;
				}
			}

			// Token: 0x06004655 RID: 18005 RVA: 0x000E5E9C File Offset: 0x000E409C
			[SecuritySafeCritical]
			public void OnCompleted(Action continuation)
			{
				TaskAwaiter.OnCompletedInternal(this.m_task, continuation, this.m_continueOnCapturedContext, true);
			}

			// Token: 0x06004656 RID: 18006 RVA: 0x000E5EB1 File Offset: 0x000E40B1
			[SecurityCritical]
			public void UnsafeOnCompleted(Action continuation)
			{
				TaskAwaiter.OnCompletedInternal(this.m_task, continuation, this.m_continueOnCapturedContext, false);
			}

			// Token: 0x06004657 RID: 18007 RVA: 0x000E5EC6 File Offset: 0x000E40C6
			[StackTraceHidden]
			public TResult GetResult()
			{
				TaskAwaiter.ValidateEnd(this.m_task);
				return this.m_task.ResultOnSuccess;
			}

			// Token: 0x04002D51 RID: 11601
			private readonly Task<TResult> m_task;

			// Token: 0x04002D52 RID: 11602
			private readonly bool m_continueOnCapturedContext;
		}
	}
}
