using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000083 RID: 131
	internal class Timer : IUniTaskAsyncEnumerable<AsyncUnit>
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x0000DEC5 File Offset: 0x0000C0C5
		public Timer(TimeSpan dueTime, TimeSpan? period, PlayerLoopTiming updateTiming, bool ignoreTimeScale, bool cancelImmediately)
		{
			this.updateTiming = updateTiming;
			this.dueTime = dueTime;
			this.period = period;
			this.ignoreTimeScale = ignoreTimeScale;
			this.cancelImmediately = cancelImmediately;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000DEF2 File Offset: 0x0000C0F2
		public IUniTaskAsyncEnumerator<AsyncUnit> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Timer._Timer(this.dueTime, this.period, this.updateTiming, this.ignoreTimeScale, cancellationToken, this.cancelImmediately);
		}

		// Token: 0x04000188 RID: 392
		private readonly PlayerLoopTiming updateTiming;

		// Token: 0x04000189 RID: 393
		private readonly TimeSpan dueTime;

		// Token: 0x0400018A RID: 394
		private readonly TimeSpan? period;

		// Token: 0x0400018B RID: 395
		private readonly bool ignoreTimeScale;

		// Token: 0x0400018C RID: 396
		private readonly bool cancelImmediately;

		// Token: 0x020001F7 RID: 503
		private class _Timer : MoveNextSource, IUniTaskAsyncEnumerator<AsyncUnit>, IUniTaskAsyncDisposable, IPlayerLoopItem
		{
			// Token: 0x060008D5 RID: 2261 RVA: 0x0004D7B8 File Offset: 0x0004B9B8
			public _Timer(TimeSpan dueTime, TimeSpan? period, PlayerLoopTiming updateTiming, bool ignoreTimeScale, CancellationToken cancellationToken, bool cancelImmediately)
			{
				this.dueTime = (float)dueTime.TotalSeconds;
				this.period = ((period == null) ? null : new float?((float)period.Value.TotalSeconds));
				if (this.dueTime <= 0f)
				{
					this.dueTime = 0f;
				}
				if (this.period != null)
				{
					float? num = this.period;
					float num2 = 0f;
					if (num.GetValueOrDefault() <= num2 & num != null)
					{
						this.period = new float?((float)1);
					}
				}
				this.initialFrame = (PlayerLoopHelper.IsMainThread ? Time.frameCount : -1);
				this.dueTimePhase = true;
				this.updateTiming = updateTiming;
				this.ignoreTimeScale = ignoreTimeScale;
				this.cancellationToken = cancellationToken;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					this.cancellationTokenRegistration = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken, delegate(object state)
					{
						Timer._Timer timer = (Timer._Timer)state;
						timer.completionSource.TrySetCanceled(timer.cancellationToken);
					}, this);
				}
				PlayerLoopHelper.AddAction(updateTiming, this);
			}

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x060008D6 RID: 2262 RVA: 0x0004D8D4 File Offset: 0x0004BAD4
			public AsyncUnit Current
			{
				get
				{
					return default(AsyncUnit);
				}
			}

			// Token: 0x060008D7 RID: 2263 RVA: 0x0004D8EC File Offset: 0x0004BAEC
			public UniTask<bool> MoveNextAsync()
			{
				if (this.disposed || this.completed)
				{
					return CompletedTasks.False;
				}
				this.elapsed = 0f;
				this.completionSource.Reset();
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.completionSource.TrySetCanceled(this.cancellationToken);
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060008D8 RID: 2264 RVA: 0x0004D958 File Offset: 0x0004BB58
			public UniTask DisposeAsync()
			{
				if (!this.disposed)
				{
					this.cancellationTokenRegistration.Dispose();
					this.disposed = true;
				}
				return default(UniTask);
			}

			// Token: 0x060008D9 RID: 2265 RVA: 0x0004D98C File Offset: 0x0004BB8C
			public bool MoveNext()
			{
				if (this.disposed)
				{
					this.completionSource.TrySetResult(false);
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.completionSource.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.dueTimePhase)
				{
					if (this.elapsed == 0f && this.initialFrame == Time.frameCount)
					{
						return true;
					}
					this.elapsed += (this.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
					if (this.elapsed >= this.dueTime)
					{
						this.dueTimePhase = false;
						this.completionSource.TrySetResult(true);
					}
				}
				else
				{
					if (this.period == null)
					{
						this.completed = true;
						this.completionSource.TrySetResult(false);
						return false;
					}
					this.elapsed += (this.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
					float num = this.elapsed;
					float? num2 = this.period;
					if (num >= num2.GetValueOrDefault() & num2 != null)
					{
						this.completionSource.TrySetResult(true);
					}
				}
				return true;
			}

			// Token: 0x04001307 RID: 4871
			private readonly float dueTime;

			// Token: 0x04001308 RID: 4872
			private readonly float? period;

			// Token: 0x04001309 RID: 4873
			private readonly PlayerLoopTiming updateTiming;

			// Token: 0x0400130A RID: 4874
			private readonly bool ignoreTimeScale;

			// Token: 0x0400130B RID: 4875
			private readonly CancellationToken cancellationToken;

			// Token: 0x0400130C RID: 4876
			private readonly CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x0400130D RID: 4877
			private int initialFrame;

			// Token: 0x0400130E RID: 4878
			private float elapsed;

			// Token: 0x0400130F RID: 4879
			private bool dueTimePhase;

			// Token: 0x04001310 RID: 4880
			private bool completed;

			// Token: 0x04001311 RID: 4881
			private bool disposed;
		}
	}
}
