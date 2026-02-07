using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000082 RID: 130
	internal sealed class EveryValueChangedStandardObject<TTarget, TProperty> : IUniTaskAsyncEnumerable<TProperty> where TTarget : class
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x0000DE6C File Offset: 0x0000C06C
		public EveryValueChangedStandardObject(TTarget target, Func<TTarget, TProperty> propertySelector, IEqualityComparer<TProperty> equalityComparer, PlayerLoopTiming monitorTiming, bool cancelImmediately)
		{
			this.target = new WeakReference<TTarget>(target, false);
			this.propertySelector = propertySelector;
			this.equalityComparer = equalityComparer;
			this.monitorTiming = monitorTiming;
			this.cancelImmediately = cancelImmediately;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000DE9F File Offset: 0x0000C09F
		public IUniTaskAsyncEnumerator<TProperty> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new EveryValueChangedStandardObject<TTarget, TProperty>._EveryValueChanged(this.target, this.propertySelector, this.equalityComparer, this.monitorTiming, cancellationToken, this.cancelImmediately);
		}

		// Token: 0x04000183 RID: 387
		private readonly WeakReference<TTarget> target;

		// Token: 0x04000184 RID: 388
		private readonly Func<TTarget, TProperty> propertySelector;

		// Token: 0x04000185 RID: 389
		private readonly IEqualityComparer<TProperty> equalityComparer;

		// Token: 0x04000186 RID: 390
		private readonly PlayerLoopTiming monitorTiming;

		// Token: 0x04000187 RID: 391
		private readonly bool cancelImmediately;

		// Token: 0x020001F6 RID: 502
		private sealed class _EveryValueChanged : MoveNextSource, IUniTaskAsyncEnumerator<TProperty>, IUniTaskAsyncDisposable, IPlayerLoopItem
		{
			// Token: 0x060008D0 RID: 2256 RVA: 0x0004D580 File Offset: 0x0004B780
			public _EveryValueChanged(WeakReference<TTarget> target, Func<TTarget, TProperty> propertySelector, IEqualityComparer<TProperty> equalityComparer, PlayerLoopTiming monitorTiming, CancellationToken cancellationToken, bool cancelImmediately)
			{
				this.target = target;
				this.propertySelector = propertySelector;
				this.equalityComparer = equalityComparer;
				this.cancellationToken = cancellationToken;
				this.first = true;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					this.cancellationTokenRegistration = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken, delegate(object state)
					{
						EveryValueChangedStandardObject<TTarget, TProperty>._EveryValueChanged everyValueChanged = (EveryValueChangedStandardObject<TTarget, TProperty>._EveryValueChanged)state;
						everyValueChanged.completionSource.TrySetCanceled(everyValueChanged.cancellationToken);
					}, this);
				}
				PlayerLoopHelper.AddAction(monitorTiming, this);
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0004D5F9 File Offset: 0x0004B7F9
			public TProperty Current
			{
				get
				{
					return this.currentValue;
				}
			}

			// Token: 0x060008D2 RID: 2258 RVA: 0x0004D604 File Offset: 0x0004B804
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
					return new UniTask<bool>(this, this.completionSource.Version);
				}
				if (!this.first)
				{
					return new UniTask<bool>(this, this.completionSource.Version);
				}
				this.first = false;
				TTarget arg;
				if (!this.target.TryGetTarget(out arg))
				{
					return CompletedTasks.False;
				}
				this.currentValue = this.propertySelector(arg);
				return CompletedTasks.True;
			}

			// Token: 0x060008D3 RID: 2259 RVA: 0x0004D6AC File Offset: 0x0004B8AC
			public UniTask DisposeAsync()
			{
				if (!this.disposed)
				{
					this.cancellationTokenRegistration.Dispose();
					this.disposed = true;
				}
				return default(UniTask);
			}

			// Token: 0x060008D4 RID: 2260 RVA: 0x0004D6E0 File Offset: 0x0004B8E0
			public bool MoveNext()
			{
				TTarget arg;
				if (this.disposed || !this.target.TryGetTarget(out arg))
				{
					this.completionSource.TrySetResult(false);
					UniTaskExtensions.Forget(this.DisposeAsync());
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.completionSource.TrySetCanceled(this.cancellationToken);
					return false;
				}
				TProperty y = default(TProperty);
				try
				{
					y = this.propertySelector(arg);
					if (this.equalityComparer.Equals(this.currentValue, y))
					{
						return true;
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
					UniTaskExtensions.Forget(this.DisposeAsync());
					return false;
				}
				this.currentValue = y;
				this.completionSource.TrySetResult(true);
				return true;
			}

			// Token: 0x040012FF RID: 4863
			private readonly WeakReference<TTarget> target;

			// Token: 0x04001300 RID: 4864
			private readonly IEqualityComparer<TProperty> equalityComparer;

			// Token: 0x04001301 RID: 4865
			private readonly Func<TTarget, TProperty> propertySelector;

			// Token: 0x04001302 RID: 4866
			private readonly CancellationToken cancellationToken;

			// Token: 0x04001303 RID: 4867
			private readonly CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04001304 RID: 4868
			private bool first;

			// Token: 0x04001305 RID: 4869
			private TProperty currentValue;

			// Token: 0x04001306 RID: 4870
			private bool disposed;
		}
	}
}
