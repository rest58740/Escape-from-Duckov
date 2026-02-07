using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200003F RID: 63
	public struct SwitchToMainThreadAwaitable
	{
		// Token: 0x06000185 RID: 389 RVA: 0x000068DB File Offset: 0x00004ADB
		public SwitchToMainThreadAwaitable(PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken)
		{
			this.playerLoopTiming = playerLoopTiming;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000068EB File Offset: 0x00004AEB
		public SwitchToMainThreadAwaitable.Awaiter GetAwaiter()
		{
			return new SwitchToMainThreadAwaitable.Awaiter(this.playerLoopTiming, this.cancellationToken);
		}

		// Token: 0x0400008E RID: 142
		private readonly PlayerLoopTiming playerLoopTiming;

		// Token: 0x0400008F RID: 143
		private readonly CancellationToken cancellationToken;

		// Token: 0x020001BE RID: 446
		public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000A63 RID: 2659 RVA: 0x00024D61 File Offset: 0x00022F61
			public Awaiter(PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken)
			{
				this.playerLoopTiming = playerLoopTiming;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700006E RID: 110
			// (get) Token: 0x06000A64 RID: 2660 RVA: 0x00024D74 File Offset: 0x00022F74
			public bool IsCompleted
			{
				get
				{
					int managedThreadId = Thread.CurrentThread.ManagedThreadId;
					return PlayerLoopHelper.MainThreadId == managedThreadId;
				}
			}

			// Token: 0x06000A65 RID: 2661 RVA: 0x00024D98 File Offset: 0x00022F98
			public void GetResult()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
			}

			// Token: 0x06000A66 RID: 2662 RVA: 0x00024DB3 File Offset: 0x00022FB3
			public void OnCompleted(Action continuation)
			{
				PlayerLoopHelper.AddContinuation(this.playerLoopTiming, continuation);
			}

			// Token: 0x06000A67 RID: 2663 RVA: 0x00024DC1 File Offset: 0x00022FC1
			public void UnsafeOnCompleted(Action continuation)
			{
				PlayerLoopHelper.AddContinuation(this.playerLoopTiming, continuation);
			}

			// Token: 0x0400039C RID: 924
			private readonly PlayerLoopTiming playerLoopTiming;

			// Token: 0x0400039D RID: 925
			private readonly CancellationToken cancellationToken;
		}
	}
}
