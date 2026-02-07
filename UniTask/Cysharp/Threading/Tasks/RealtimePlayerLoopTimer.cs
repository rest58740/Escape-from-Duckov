using System;
using System.Threading;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200002F RID: 47
	internal sealed class RealtimePlayerLoopTimer : PlayerLoopTimer
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public RealtimePlayerLoopTimer(TimeSpan interval, bool periodic, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state) : base(periodic, playerLoopTiming, cancellationToken, timerCallback, state)
		{
			this.ResetCore(new TimeSpan?(interval));
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003BC0 File Offset: 0x00001DC0
		protected override bool MoveNextCore()
		{
			return this.stopwatch.ElapsedTicks < this.intervalTicks;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003BD8 File Offset: 0x00001DD8
		protected override void ResetCore(TimeSpan? interval)
		{
			this.stopwatch = ValueStopwatch.StartNew();
			if (interval != null)
			{
				this.intervalTicks = interval.Value.Ticks;
			}
		}

		// Token: 0x04000069 RID: 105
		private ValueStopwatch stopwatch;

		// Token: 0x0400006A RID: 106
		private long intervalTicks;
	}
}
