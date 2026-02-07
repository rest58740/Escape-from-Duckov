using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000043 RID: 67
	public struct SwitchToSynchronizationContextAwaitable
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00006952 File Offset: 0x00004B52
		public SwitchToSynchronizationContextAwaitable(SynchronizationContext synchronizationContext, CancellationToken cancellationToken)
		{
			this.synchronizationContext = synchronizationContext;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006962 File Offset: 0x00004B62
		public SwitchToSynchronizationContextAwaitable.Awaiter GetAwaiter()
		{
			return new SwitchToSynchronizationContextAwaitable.Awaiter(this.synchronizationContext, this.cancellationToken);
		}

		// Token: 0x04000092 RID: 146
		private readonly SynchronizationContext synchronizationContext;

		// Token: 0x04000093 RID: 147
		private readonly CancellationToken cancellationToken;

		// Token: 0x020001C2 RID: 450
		public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000A7A RID: 2682 RVA: 0x00024ED5 File Offset: 0x000230D5
			public Awaiter(SynchronizationContext synchronizationContext, CancellationToken cancellationToken)
			{
				this.synchronizationContext = synchronizationContext;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000072 RID: 114
			// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00024EE5 File Offset: 0x000230E5
			public bool IsCompleted
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000A7C RID: 2684 RVA: 0x00024EE8 File Offset: 0x000230E8
			public void GetResult()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
			}

			// Token: 0x06000A7D RID: 2685 RVA: 0x00024F03 File Offset: 0x00023103
			public void OnCompleted(Action continuation)
			{
				this.synchronizationContext.Post(SwitchToSynchronizationContextAwaitable.Awaiter.switchToCallback, continuation);
			}

			// Token: 0x06000A7E RID: 2686 RVA: 0x00024F16 File Offset: 0x00023116
			public void UnsafeOnCompleted(Action continuation)
			{
				this.synchronizationContext.Post(SwitchToSynchronizationContextAwaitable.Awaiter.switchToCallback, continuation);
			}

			// Token: 0x06000A7F RID: 2687 RVA: 0x00024F29 File Offset: 0x00023129
			private static void Callback(object state)
			{
				((Action)state)();
			}

			// Token: 0x040003A2 RID: 930
			private static readonly SendOrPostCallback switchToCallback = new SendOrPostCallback(SwitchToSynchronizationContextAwaitable.Awaiter.Callback);

			// Token: 0x040003A3 RID: 931
			private readonly SynchronizationContext synchronizationContext;

			// Token: 0x040003A4 RID: 932
			private readonly CancellationToken cancellationToken;
		}
	}
}
