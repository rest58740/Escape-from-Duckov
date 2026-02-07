using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000034 RID: 52
	public sealed class TimeoutController : IDisposable
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00003DEB File Offset: 0x00001FEB
		private static void CancelCancellationTokenSourceState(object state)
		{
			((CancellationTokenSource)state).Cancel();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003DF8 File Offset: 0x00001FF8
		public TimeoutController(DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update)
		{
			this.timeoutSource = new CancellationTokenSource();
			this.originalLinkCancellationTokenSource = null;
			this.linkedSource = null;
			this.delayType = delayType;
			this.delayTiming = delayTiming;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003E28 File Offset: 0x00002028
		public TimeoutController(CancellationTokenSource linkCancellationTokenSource, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update)
		{
			this.timeoutSource = new CancellationTokenSource();
			this.originalLinkCancellationTokenSource = linkCancellationTokenSource;
			this.linkedSource = CancellationTokenSource.CreateLinkedTokenSource(this.timeoutSource.Token, linkCancellationTokenSource.Token);
			this.delayType = delayType;
			this.delayTiming = delayTiming;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003E77 File Offset: 0x00002077
		public CancellationToken Timeout(int millisecondsTimeout)
		{
			return this.Timeout(TimeSpan.FromMilliseconds((double)millisecondsTimeout));
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003E88 File Offset: 0x00002088
		public CancellationToken Timeout(TimeSpan timeout)
		{
			if (this.originalLinkCancellationTokenSource != null && this.originalLinkCancellationTokenSource.IsCancellationRequested)
			{
				return this.originalLinkCancellationTokenSource.Token;
			}
			if (this.timeoutSource.IsCancellationRequested)
			{
				this.timeoutSource.Dispose();
				this.timeoutSource = new CancellationTokenSource();
				if (this.linkedSource != null)
				{
					this.linkedSource.Cancel();
					this.linkedSource.Dispose();
					this.linkedSource = CancellationTokenSource.CreateLinkedTokenSource(this.timeoutSource.Token, this.originalLinkCancellationTokenSource.Token);
				}
				PlayerLoopTimer playerLoopTimer = this.timer;
				if (playerLoopTimer != null)
				{
					playerLoopTimer.Dispose();
				}
				this.timer = null;
			}
			CancellationToken token = ((this.linkedSource != null) ? this.linkedSource : this.timeoutSource).Token;
			if (this.timer == null)
			{
				this.timer = PlayerLoopTimer.StartNew(timeout, false, this.delayType, this.delayTiming, token, TimeoutController.CancelCancellationTokenSourceStateDelegate, this.timeoutSource);
			}
			else
			{
				this.timer.Restart(timeout);
			}
			return token;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003F88 File Offset: 0x00002188
		public bool IsTimeout()
		{
			return this.timeoutSource.IsCancellationRequested;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003F95 File Offset: 0x00002195
		public void Reset()
		{
			PlayerLoopTimer playerLoopTimer = this.timer;
			if (playerLoopTimer == null)
			{
				return;
			}
			playerLoopTimer.Stop();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003FA8 File Offset: 0x000021A8
		public void Dispose()
		{
			if (this.isDisposed)
			{
				return;
			}
			try
			{
				PlayerLoopTimer playerLoopTimer = this.timer;
				if (playerLoopTimer != null)
				{
					playerLoopTimer.Dispose();
				}
				this.timeoutSource.Cancel();
				this.timeoutSource.Dispose();
				if (this.linkedSource != null)
				{
					this.linkedSource.Cancel();
					this.linkedSource.Dispose();
				}
			}
			finally
			{
				this.isDisposed = true;
			}
		}

		// Token: 0x04000070 RID: 112
		private static readonly Action<object> CancelCancellationTokenSourceStateDelegate = new Action<object>(TimeoutController.CancelCancellationTokenSourceState);

		// Token: 0x04000071 RID: 113
		private CancellationTokenSource timeoutSource;

		// Token: 0x04000072 RID: 114
		private CancellationTokenSource linkedSource;

		// Token: 0x04000073 RID: 115
		private PlayerLoopTimer timer;

		// Token: 0x04000074 RID: 116
		private bool isDisposed;

		// Token: 0x04000075 RID: 117
		private readonly DelayType delayType;

		// Token: 0x04000076 RID: 118
		private readonly PlayerLoopTiming delayTiming;

		// Token: 0x04000077 RID: 119
		private readonly CancellationTokenSource originalLinkCancellationTokenSource;
	}
}
