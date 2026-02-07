using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000040 RID: 64
	public struct ReturnToMainThread
	{
		// Token: 0x06000187 RID: 391 RVA: 0x000068FE File Offset: 0x00004AFE
		public ReturnToMainThread(PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken)
		{
			this.playerLoopTiming = playerLoopTiming;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000690E File Offset: 0x00004B0E
		public ReturnToMainThread.Awaiter DisposeAsync()
		{
			return new ReturnToMainThread.Awaiter(this.playerLoopTiming, this.cancellationToken);
		}

		// Token: 0x04000090 RID: 144
		private readonly PlayerLoopTiming playerLoopTiming;

		// Token: 0x04000091 RID: 145
		private readonly CancellationToken cancellationToken;

		// Token: 0x020001BF RID: 447
		public readonly struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000A68 RID: 2664 RVA: 0x00024DCF File Offset: 0x00022FCF
			public Awaiter(PlayerLoopTiming timing, CancellationToken cancellationToken)
			{
				this.timing = timing;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x06000A69 RID: 2665 RVA: 0x00024DDF File Offset: 0x00022FDF
			public ReturnToMainThread.Awaiter GetAwaiter()
			{
				return this;
			}

			// Token: 0x1700006F RID: 111
			// (get) Token: 0x06000A6A RID: 2666 RVA: 0x00024DE7 File Offset: 0x00022FE7
			public bool IsCompleted
			{
				get
				{
					return PlayerLoopHelper.MainThreadId == Thread.CurrentThread.ManagedThreadId;
				}
			}

			// Token: 0x06000A6B RID: 2667 RVA: 0x00024DFC File Offset: 0x00022FFC
			public void GetResult()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
			}

			// Token: 0x06000A6C RID: 2668 RVA: 0x00024E17 File Offset: 0x00023017
			public void OnCompleted(Action continuation)
			{
				PlayerLoopHelper.AddContinuation(this.timing, continuation);
			}

			// Token: 0x06000A6D RID: 2669 RVA: 0x00024E25 File Offset: 0x00023025
			public void UnsafeOnCompleted(Action continuation)
			{
				PlayerLoopHelper.AddContinuation(this.timing, continuation);
			}

			// Token: 0x0400039E RID: 926
			private readonly PlayerLoopTiming timing;

			// Token: 0x0400039F RID: 927
			private readonly CancellationToken cancellationToken;
		}
	}
}
