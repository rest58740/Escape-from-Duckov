using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000080 RID: 128
	internal class EveryUpdate : IUniTaskAsyncEnumerable<AsyncUnit>
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x0000DDEF File Offset: 0x0000BFEF
		public EveryUpdate(PlayerLoopTiming updateTiming, bool cancelImmediately)
		{
			this.updateTiming = updateTiming;
			this.cancelImmediately = cancelImmediately;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000DE05 File Offset: 0x0000C005
		public IUniTaskAsyncEnumerator<AsyncUnit> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new EveryUpdate._EveryUpdate(this.updateTiming, cancellationToken, this.cancelImmediately);
		}

		// Token: 0x0400017C RID: 380
		private readonly PlayerLoopTiming updateTiming;

		// Token: 0x0400017D RID: 381
		private readonly bool cancelImmediately;

		// Token: 0x020001F4 RID: 500
		private class _EveryUpdate : MoveNextSource, IUniTaskAsyncEnumerator<AsyncUnit>, IUniTaskAsyncDisposable, IPlayerLoopItem
		{
			// Token: 0x060008C6 RID: 2246 RVA: 0x0004D1D0 File Offset: 0x0004B3D0
			public _EveryUpdate(PlayerLoopTiming updateTiming, CancellationToken cancellationToken, bool cancelImmediately)
			{
				this.updateTiming = updateTiming;
				this.cancellationToken = cancellationToken;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					this.cancellationTokenRegistration = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken, delegate(object state)
					{
						EveryUpdate._EveryUpdate everyUpdate = (EveryUpdate._EveryUpdate)state;
						everyUpdate.completionSource.TrySetCanceled(everyUpdate.cancellationToken);
					}, this);
				}
				PlayerLoopHelper.AddAction(updateTiming, this);
			}

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0004D230 File Offset: 0x0004B430
			public AsyncUnit Current
			{
				get
				{
					return default(AsyncUnit);
				}
			}

			// Token: 0x060008C8 RID: 2248 RVA: 0x0004D248 File Offset: 0x0004B448
			public UniTask<bool> MoveNextAsync()
			{
				if (this.disposed)
				{
					return CompletedTasks.False;
				}
				this.completionSource.Reset();
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.completionSource.TrySetCanceled(this.cancellationToken);
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060008C9 RID: 2249 RVA: 0x0004D2A4 File Offset: 0x0004B4A4
			public UniTask DisposeAsync()
			{
				if (!this.disposed)
				{
					this.cancellationTokenRegistration.Dispose();
					this.disposed = true;
				}
				return default(UniTask);
			}

			// Token: 0x060008CA RID: 2250 RVA: 0x0004D2D8 File Offset: 0x0004B4D8
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
				this.completionSource.TrySetResult(true);
				return true;
			}

			// Token: 0x040012F2 RID: 4850
			private readonly PlayerLoopTiming updateTiming;

			// Token: 0x040012F3 RID: 4851
			private readonly CancellationToken cancellationToken;

			// Token: 0x040012F4 RID: 4852
			private readonly CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040012F5 RID: 4853
			private bool disposed;
		}
	}
}
