using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200002E RID: 46
	internal sealed class IgnoreTimeScalePlayerLoopTimer : PlayerLoopTimer
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00003AF0 File Offset: 0x00001CF0
		public IgnoreTimeScalePlayerLoopTimer(TimeSpan interval, bool periodic, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state) : base(periodic, playerLoopTiming, cancellationToken, timerCallback, state)
		{
			this.ResetCore(new TimeSpan?(interval));
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003B0C File Offset: 0x00001D0C
		protected override bool MoveNextCore()
		{
			if (this.elapsed == 0f && this.initialFrame == Time.frameCount)
			{
				return true;
			}
			this.elapsed += Time.unscaledDeltaTime;
			return this.elapsed < this.interval;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003B58 File Offset: 0x00001D58
		protected override void ResetCore(TimeSpan? interval)
		{
			this.elapsed = 0f;
			this.initialFrame = (PlayerLoopHelper.IsMainThread ? Time.frameCount : -1);
			if (interval != null)
			{
				this.interval = (float)interval.Value.TotalSeconds;
			}
		}

		// Token: 0x04000066 RID: 102
		private int initialFrame;

		// Token: 0x04000067 RID: 103
		private float elapsed;

		// Token: 0x04000068 RID: 104
		private float interval;
	}
}
