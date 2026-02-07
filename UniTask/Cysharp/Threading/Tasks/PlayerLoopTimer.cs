using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200002C RID: 44
	public abstract class PlayerLoopTimer : IDisposable, IPlayerLoopItem
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00003884 File Offset: 0x00001A84
		protected PlayerLoopTimer(bool periodic, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state)
		{
			this.periodic = periodic;
			this.playerLoopTiming = playerLoopTiming;
			this.cancellationToken = cancellationToken;
			this.timerCallback = timerCallback;
			this.state = state;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000038B4 File Offset: 0x00001AB4
		public static PlayerLoopTimer Create(TimeSpan interval, bool periodic, DelayType delayType, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state)
		{
			switch (delayType)
			{
			case DelayType.UnscaledDeltaTime:
				return new IgnoreTimeScalePlayerLoopTimer(interval, periodic, playerLoopTiming, cancellationToken, timerCallback, state);
			case DelayType.Realtime:
				return new RealtimePlayerLoopTimer(interval, periodic, playerLoopTiming, cancellationToken, timerCallback, state);
			}
			return new DeltaTimePlayerLoopTimer(interval, periodic, playerLoopTiming, cancellationToken, timerCallback, state);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003901 File Offset: 0x00001B01
		public static PlayerLoopTimer StartNew(TimeSpan interval, bool periodic, DelayType delayType, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state)
		{
			PlayerLoopTimer playerLoopTimer = PlayerLoopTimer.Create(interval, periodic, delayType, playerLoopTiming, cancellationToken, timerCallback, state);
			playerLoopTimer.Restart();
			return playerLoopTimer;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003918 File Offset: 0x00001B18
		public void Restart()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException(null);
			}
			this.ResetCore(null);
			if (!this.isRunning)
			{
				this.isRunning = true;
				PlayerLoopHelper.AddAction(this.playerLoopTiming, this);
			}
			this.tryStop = false;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003965 File Offset: 0x00001B65
		public void Restart(TimeSpan interval)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException(null);
			}
			this.ResetCore(new TimeSpan?(interval));
			if (!this.isRunning)
			{
				this.isRunning = true;
				PlayerLoopHelper.AddAction(this.playerLoopTiming, this);
			}
			this.tryStop = false;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000039A4 File Offset: 0x00001BA4
		public void Stop()
		{
			this.tryStop = true;
		}

		// Token: 0x060000B7 RID: 183
		protected abstract void ResetCore(TimeSpan? newInterval);

		// Token: 0x060000B8 RID: 184 RVA: 0x000039AD File Offset: 0x00001BAD
		public void Dispose()
		{
			this.isDisposed = true;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000039B8 File Offset: 0x00001BB8
		bool IPlayerLoopItem.MoveNext()
		{
			if (this.isDisposed)
			{
				this.isRunning = false;
				return false;
			}
			if (this.tryStop)
			{
				this.isRunning = false;
				return false;
			}
			if (this.cancellationToken.IsCancellationRequested)
			{
				this.isRunning = false;
				return false;
			}
			if (this.MoveNextCore())
			{
				return true;
			}
			this.timerCallback(this.state);
			if (this.periodic)
			{
				this.ResetCore(null);
				return true;
			}
			this.isRunning = false;
			return false;
		}

		// Token: 0x060000BA RID: 186
		protected abstract bool MoveNextCore();

		// Token: 0x0400005B RID: 91
		private readonly CancellationToken cancellationToken;

		// Token: 0x0400005C RID: 92
		private readonly Action<object> timerCallback;

		// Token: 0x0400005D RID: 93
		private readonly object state;

		// Token: 0x0400005E RID: 94
		private readonly PlayerLoopTiming playerLoopTiming;

		// Token: 0x0400005F RID: 95
		private readonly bool periodic;

		// Token: 0x04000060 RID: 96
		private bool isRunning;

		// Token: 0x04000061 RID: 97
		private bool tryStop;

		// Token: 0x04000062 RID: 98
		private bool isDisposed;
	}
}
