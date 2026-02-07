using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000044 RID: 68
	public struct ReturnToSynchronizationContext
	{
		// Token: 0x0600018D RID: 397 RVA: 0x00006975 File Offset: 0x00004B75
		public ReturnToSynchronizationContext(SynchronizationContext syncContext, bool dontPostWhenSameContext, CancellationToken cancellationToken)
		{
			this.syncContext = syncContext;
			this.dontPostWhenSameContext = dontPostWhenSameContext;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000698C File Offset: 0x00004B8C
		public ReturnToSynchronizationContext.Awaiter DisposeAsync()
		{
			return new ReturnToSynchronizationContext.Awaiter(this.syncContext, this.dontPostWhenSameContext, this.cancellationToken);
		}

		// Token: 0x04000094 RID: 148
		private readonly SynchronizationContext syncContext;

		// Token: 0x04000095 RID: 149
		private readonly bool dontPostWhenSameContext;

		// Token: 0x04000096 RID: 150
		private readonly CancellationToken cancellationToken;

		// Token: 0x020001C3 RID: 451
		public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000A81 RID: 2689 RVA: 0x00024F49 File Offset: 0x00023149
			public Awaiter(SynchronizationContext synchronizationContext, bool dontPostWhenSameContext, CancellationToken cancellationToken)
			{
				this.synchronizationContext = synchronizationContext;
				this.dontPostWhenSameContext = dontPostWhenSameContext;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x06000A82 RID: 2690 RVA: 0x00024F60 File Offset: 0x00023160
			public ReturnToSynchronizationContext.Awaiter GetAwaiter()
			{
				return this;
			}

			// Token: 0x17000073 RID: 115
			// (get) Token: 0x06000A83 RID: 2691 RVA: 0x00024F68 File Offset: 0x00023168
			public bool IsCompleted
			{
				get
				{
					return this.dontPostWhenSameContext && SynchronizationContext.Current == this.synchronizationContext;
				}
			}

			// Token: 0x06000A84 RID: 2692 RVA: 0x00024F84 File Offset: 0x00023184
			public void GetResult()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
			}

			// Token: 0x06000A85 RID: 2693 RVA: 0x00024F9F File Offset: 0x0002319F
			public void OnCompleted(Action continuation)
			{
				this.synchronizationContext.Post(ReturnToSynchronizationContext.Awaiter.switchToCallback, continuation);
			}

			// Token: 0x06000A86 RID: 2694 RVA: 0x00024FB2 File Offset: 0x000231B2
			public void UnsafeOnCompleted(Action continuation)
			{
				this.synchronizationContext.Post(ReturnToSynchronizationContext.Awaiter.switchToCallback, continuation);
			}

			// Token: 0x06000A87 RID: 2695 RVA: 0x00024FC5 File Offset: 0x000231C5
			private static void Callback(object state)
			{
				((Action)state)();
			}

			// Token: 0x040003A5 RID: 933
			private static readonly SendOrPostCallback switchToCallback = new SendOrPostCallback(ReturnToSynchronizationContext.Awaiter.Callback);

			// Token: 0x040003A6 RID: 934
			private readonly SynchronizationContext synchronizationContext;

			// Token: 0x040003A7 RID: 935
			private readonly bool dontPostWhenSameContext;

			// Token: 0x040003A8 RID: 936
			private readonly CancellationToken cancellationToken;
		}
	}
}
