using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000081 RID: 129
	internal sealed class EveryValueChangedUnityObject<TTarget, TProperty> : IUniTaskAsyncEnumerable<TProperty>
	{
		// Token: 0x060003C2 RID: 962 RVA: 0x0000DE19 File Offset: 0x0000C019
		public EveryValueChangedUnityObject(TTarget target, Func<TTarget, TProperty> propertySelector, IEqualityComparer<TProperty> equalityComparer, PlayerLoopTiming monitorTiming, bool cancelImmediately)
		{
			this.target = target;
			this.propertySelector = propertySelector;
			this.equalityComparer = equalityComparer;
			this.monitorTiming = monitorTiming;
			this.cancelImmediately = cancelImmediately;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000DE46 File Offset: 0x0000C046
		public IUniTaskAsyncEnumerator<TProperty> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new EveryValueChangedUnityObject<TTarget, TProperty>._EveryValueChanged(this.target, this.propertySelector, this.equalityComparer, this.monitorTiming, cancellationToken, this.cancelImmediately);
		}

		// Token: 0x0400017E RID: 382
		private readonly TTarget target;

		// Token: 0x0400017F RID: 383
		private readonly Func<TTarget, TProperty> propertySelector;

		// Token: 0x04000180 RID: 384
		private readonly IEqualityComparer<TProperty> equalityComparer;

		// Token: 0x04000181 RID: 385
		private readonly PlayerLoopTiming monitorTiming;

		// Token: 0x04000182 RID: 386
		private readonly bool cancelImmediately;

		// Token: 0x020001F5 RID: 501
		private sealed class _EveryValueChanged : MoveNextSource, IUniTaskAsyncEnumerator<TProperty>, IUniTaskAsyncDisposable, IPlayerLoopItem
		{
			// Token: 0x060008CB RID: 2251 RVA: 0x0004D330 File Offset: 0x0004B530
			public _EveryValueChanged(TTarget target, Func<TTarget, TProperty> propertySelector, IEqualityComparer<TProperty> equalityComparer, PlayerLoopTiming monitorTiming, CancellationToken cancellationToken, bool cancelImmediately)
			{
				this.target = target;
				this.targetAsUnityObject = (target as Object);
				this.propertySelector = propertySelector;
				this.equalityComparer = equalityComparer;
				this.cancellationToken = cancellationToken;
				this.first = true;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					this.cancellationTokenRegistration = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken, delegate(object state)
					{
						EveryValueChangedUnityObject<TTarget, TProperty>._EveryValueChanged everyValueChanged = (EveryValueChangedUnityObject<TTarget, TProperty>._EveryValueChanged)state;
						everyValueChanged.completionSource.TrySetCanceled(everyValueChanged.cancellationToken);
					}, this);
				}
				PlayerLoopHelper.AddAction(monitorTiming, this);
			}

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x060008CC RID: 2252 RVA: 0x0004D3BA File Offset: 0x0004B5BA
			public TProperty Current
			{
				get
				{
					return this.currentValue;
				}
			}

			// Token: 0x060008CD RID: 2253 RVA: 0x0004D3C4 File Offset: 0x0004B5C4
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
				if (this.targetAsUnityObject == null)
				{
					return CompletedTasks.False;
				}
				this.currentValue = this.propertySelector(this.target);
				return CompletedTasks.True;
			}

			// Token: 0x060008CE RID: 2254 RVA: 0x0004D470 File Offset: 0x0004B670
			public UniTask DisposeAsync()
			{
				if (!this.disposed)
				{
					this.cancellationTokenRegistration.Dispose();
					this.disposed = true;
				}
				return default(UniTask);
			}

			// Token: 0x060008CF RID: 2255 RVA: 0x0004D4A4 File Offset: 0x0004B6A4
			public bool MoveNext()
			{
				if (this.disposed || this.targetAsUnityObject == null)
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
					y = this.propertySelector(this.target);
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

			// Token: 0x040012F6 RID: 4854
			private readonly TTarget target;

			// Token: 0x040012F7 RID: 4855
			private readonly Object targetAsUnityObject;

			// Token: 0x040012F8 RID: 4856
			private readonly IEqualityComparer<TProperty> equalityComparer;

			// Token: 0x040012F9 RID: 4857
			private readonly Func<TTarget, TProperty> propertySelector;

			// Token: 0x040012FA RID: 4858
			private readonly CancellationToken cancellationToken;

			// Token: 0x040012FB RID: 4859
			private readonly CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040012FC RID: 4860
			private bool first;

			// Token: 0x040012FD RID: 4861
			private TProperty currentValue;

			// Token: 0x040012FE RID: 4862
			private bool disposed;
		}
	}
}
