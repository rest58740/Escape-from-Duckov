using System;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000823 RID: 2083
	public readonly struct YieldAwaitable
	{
		// Token: 0x0600469C RID: 18076 RVA: 0x000E7000 File Offset: 0x000E5200
		public YieldAwaitable.YieldAwaiter GetAwaiter()
		{
			return default(YieldAwaitable.YieldAwaiter);
		}

		// Token: 0x02000824 RID: 2084
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public readonly struct YieldAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x17000ADA RID: 2778
			// (get) Token: 0x0600469D RID: 18077 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			public bool IsCompleted
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600469E RID: 18078 RVA: 0x000E7016 File Offset: 0x000E5216
			[SecuritySafeCritical]
			public void OnCompleted(Action continuation)
			{
				YieldAwaitable.YieldAwaiter.QueueContinuation(continuation, true);
			}

			// Token: 0x0600469F RID: 18079 RVA: 0x000E701F File Offset: 0x000E521F
			[SecurityCritical]
			public void UnsafeOnCompleted(Action continuation)
			{
				YieldAwaitable.YieldAwaiter.QueueContinuation(continuation, false);
			}

			// Token: 0x060046A0 RID: 18080 RVA: 0x000E7028 File Offset: 0x000E5228
			[SecurityCritical]
			private static void QueueContinuation(Action continuation, bool flowContext)
			{
				if (continuation == null)
				{
					throw new ArgumentNullException("continuation");
				}
				SynchronizationContext currentNoFlow = SynchronizationContext.CurrentNoFlow;
				if (currentNoFlow != null && currentNoFlow.GetType() != typeof(SynchronizationContext))
				{
					currentNoFlow.Post(YieldAwaitable.YieldAwaiter.s_sendOrPostCallbackRunAction, continuation);
					return;
				}
				TaskScheduler taskScheduler = TaskScheduler.Current;
				if (taskScheduler != TaskScheduler.Default)
				{
					Task.Factory.StartNew(continuation, default(CancellationToken), TaskCreationOptions.PreferFairness, taskScheduler);
					return;
				}
				if (flowContext)
				{
					ThreadPool.QueueUserWorkItem(YieldAwaitable.YieldAwaiter.s_waitCallbackRunAction, continuation);
					return;
				}
				ThreadPool.UnsafeQueueUserWorkItem(YieldAwaitable.YieldAwaiter.s_waitCallbackRunAction, continuation);
			}

			// Token: 0x060046A1 RID: 18081 RVA: 0x00082EFE File Offset: 0x000810FE
			private static void RunAction(object state)
			{
				((Action)state)();
			}

			// Token: 0x060046A2 RID: 18082 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void GetResult()
			{
			}

			// Token: 0x04002D70 RID: 11632
			private static readonly WaitCallback s_waitCallbackRunAction = new WaitCallback(YieldAwaitable.YieldAwaiter.RunAction);

			// Token: 0x04002D71 RID: 11633
			private static readonly SendOrPostCallback s_sendOrPostCallbackRunAction = new SendOrPostCallback(YieldAwaitable.YieldAwaiter.RunAction);
		}
	}
}
