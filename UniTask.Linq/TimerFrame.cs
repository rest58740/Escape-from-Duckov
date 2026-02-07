using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000084 RID: 132
	internal class TimerFrame : IUniTaskAsyncEnumerable<AsyncUnit>
	{
		// Token: 0x060003C8 RID: 968 RVA: 0x0000DF18 File Offset: 0x0000C118
		public TimerFrame(int dueTimeFrameCount, int? periodFrameCount, PlayerLoopTiming updateTiming, bool cancelImmediately)
		{
			this.updateTiming = updateTiming;
			this.dueTimeFrameCount = dueTimeFrameCount;
			this.periodFrameCount = periodFrameCount;
			this.cancelImmediately = cancelImmediately;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000DF3D File Offset: 0x0000C13D
		public IUniTaskAsyncEnumerator<AsyncUnit> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TimerFrame._TimerFrame(this.dueTimeFrameCount, this.periodFrameCount, this.updateTiming, cancellationToken, this.cancelImmediately);
		}

		// Token: 0x0400018D RID: 397
		private readonly PlayerLoopTiming updateTiming;

		// Token: 0x0400018E RID: 398
		private readonly int dueTimeFrameCount;

		// Token: 0x0400018F RID: 399
		private readonly int? periodFrameCount;

		// Token: 0x04000190 RID: 400
		private readonly bool cancelImmediately;

		// Token: 0x020001F8 RID: 504
		private class _TimerFrame : MoveNextSource, IUniTaskAsyncEnumerator<AsyncUnit>, IUniTaskAsyncDisposable, IPlayerLoopItem
		{
			// Token: 0x060008DA RID: 2266 RVA: 0x0004DAB8 File Offset: 0x0004BCB8
			public _TimerFrame(int dueTimeFrameCount, int? periodFrameCount, PlayerLoopTiming updateTiming, CancellationToken cancellationToken, bool cancelImmediately)
			{
				if (dueTimeFrameCount <= 0)
				{
					dueTimeFrameCount = 0;
				}
				if (periodFrameCount != null)
				{
					int? num = periodFrameCount;
					int num2 = 0;
					if (num.GetValueOrDefault() <= num2 & num != null)
					{
						periodFrameCount = new int?(1);
					}
				}
				this.initialFrame = (PlayerLoopHelper.IsMainThread ? Time.frameCount : -1);
				this.dueTimePhase = true;
				this.dueTimeFrameCount = dueTimeFrameCount;
				this.periodFrameCount = periodFrameCount;
				this.cancellationToken = cancellationToken;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					this.cancellationTokenRegistration = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken, delegate(object state)
					{
						TimerFrame._TimerFrame timerFrame = (TimerFrame._TimerFrame)state;
						timerFrame.completionSource.TrySetCanceled(timerFrame.cancellationToken);
					}, this);
				}
				PlayerLoopHelper.AddAction(updateTiming, this);
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x060008DB RID: 2267 RVA: 0x0004DB74 File Offset: 0x0004BD74
			public AsyncUnit Current
			{
				get
				{
					return default(AsyncUnit);
				}
			}

			// Token: 0x060008DC RID: 2268 RVA: 0x0004DB8C File Offset: 0x0004BD8C
			public UniTask<bool> MoveNextAsync()
			{
				if (this.disposed || this.completed)
				{
					return CompletedTasks.False;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.completionSource.TrySetCanceled(this.cancellationToken);
				}
				this.currentFrame = 0;
				this.completionSource.Reset();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060008DD RID: 2269 RVA: 0x0004DBF4 File Offset: 0x0004BDF4
			public UniTask DisposeAsync()
			{
				if (!this.disposed)
				{
					this.cancellationTokenRegistration.Dispose();
					this.disposed = true;
				}
				return default(UniTask);
			}

			// Token: 0x060008DE RID: 2270 RVA: 0x0004DC28 File Offset: 0x0004BE28
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.completionSource.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.disposed)
				{
					this.completionSource.TrySetResult(false);
					return false;
				}
				if (this.dueTimePhase)
				{
					if (this.currentFrame == 0)
					{
						if (this.dueTimeFrameCount == 0)
						{
							this.dueTimePhase = false;
							this.completionSource.TrySetResult(true);
							return true;
						}
						if (this.initialFrame == Time.frameCount)
						{
							return true;
						}
					}
					int num = this.currentFrame + 1;
					this.currentFrame = num;
					if (num >= this.dueTimeFrameCount)
					{
						this.dueTimePhase = false;
						this.completionSource.TrySetResult(true);
					}
				}
				else
				{
					if (this.periodFrameCount == null)
					{
						this.completed = true;
						this.completionSource.TrySetResult(false);
						return false;
					}
					int num = this.currentFrame + 1;
					this.currentFrame = num;
					int num2 = num;
					int? num3 = this.periodFrameCount;
					if (num2 >= num3.GetValueOrDefault() & num3 != null)
					{
						this.completionSource.TrySetResult(true);
					}
				}
				return true;
			}

			// Token: 0x04001312 RID: 4882
			private readonly int dueTimeFrameCount;

			// Token: 0x04001313 RID: 4883
			private readonly int? periodFrameCount;

			// Token: 0x04001314 RID: 4884
			private readonly CancellationToken cancellationToken;

			// Token: 0x04001315 RID: 4885
			private readonly CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04001316 RID: 4886
			private int initialFrame;

			// Token: 0x04001317 RID: 4887
			private int currentFrame;

			// Token: 0x04001318 RID: 4888
			private bool dueTimePhase;

			// Token: 0x04001319 RID: 4889
			private bool completed;

			// Token: 0x0400131A RID: 4890
			private bool disposed;
		}
	}
}
