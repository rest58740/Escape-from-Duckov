using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200002D RID: 45
	internal sealed class DeltaTimePlayerLoopTimer : PlayerLoopTimer
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00003A3C File Offset: 0x00001C3C
		public DeltaTimePlayerLoopTimer(TimeSpan interval, bool periodic, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state) : base(periodic, playerLoopTiming, cancellationToken, timerCallback, state)
		{
			this.ResetCore(new TimeSpan?(interval));
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003A58 File Offset: 0x00001C58
		protected override bool MoveNextCore()
		{
			if (this.elapsed == 0f && this.initialFrame == Time.frameCount)
			{
				return true;
			}
			this.elapsed += Time.deltaTime;
			return this.elapsed < this.interval;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003AA4 File Offset: 0x00001CA4
		protected override void ResetCore(TimeSpan? interval)
		{
			this.elapsed = 0f;
			this.initialFrame = (PlayerLoopHelper.IsMainThread ? Time.frameCount : -1);
			if (interval != null)
			{
				this.interval = (float)interval.Value.TotalSeconds;
			}
		}

		// Token: 0x04000063 RID: 99
		private int initialFrame;

		// Token: 0x04000064 RID: 100
		private float elapsed;

		// Token: 0x04000065 RID: 101
		private float interval;
	}
}
