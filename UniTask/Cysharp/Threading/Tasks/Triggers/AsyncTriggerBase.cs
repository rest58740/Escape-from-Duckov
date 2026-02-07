using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200006F RID: 111
	public abstract class AsyncTriggerBase<T> : MonoBehaviour, IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x06000388 RID: 904 RVA: 0x0000AFA3 File Offset: 0x000091A3
		private void Awake()
		{
			this.calledAwake = true;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000AFAC File Offset: 0x000091AC
		private void OnDestroy()
		{
			if (this.calledDestroy)
			{
				return;
			}
			this.calledDestroy = true;
			this.triggerEvent.SetCompleted();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000AFC9 File Offset: 0x000091C9
		internal void AddHandler(ITriggerHandler<T> handler)
		{
			if (!this.calledAwake)
			{
				PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, new AsyncTriggerBase<T>.AwakeMonitor(this));
			}
			this.triggerEvent.Add(handler);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000AFEB File Offset: 0x000091EB
		internal void RemoveHandler(ITriggerHandler<T> handler)
		{
			if (!this.calledAwake)
			{
				PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, new AsyncTriggerBase<T>.AwakeMonitor(this));
			}
			this.triggerEvent.Remove(handler);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000B00D File Offset: 0x0000920D
		protected void RaiseEvent(T value)
		{
			this.triggerEvent.SetResult(value);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000B01B File Offset: 0x0000921B
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new AsyncTriggerBase<T>.AsyncTriggerEnumerator(this, cancellationToken);
		}

		// Token: 0x040000F0 RID: 240
		private TriggerEvent<T> triggerEvent;

		// Token: 0x040000F1 RID: 241
		protected internal bool calledAwake;

		// Token: 0x040000F2 RID: 242
		protected internal bool calledDestroy;

		// Token: 0x02000207 RID: 519
		private sealed class AsyncTriggerEnumerator : MoveNextSource, IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable, ITriggerHandler<!0>
		{
			// Token: 0x06000BB4 RID: 2996 RVA: 0x0002A7D1 File Offset: 0x000289D1
			public AsyncTriggerEnumerator(AsyncTriggerBase<T> parent, CancellationToken cancellationToken)
			{
				this.parent = parent;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x06000BB5 RID: 2997 RVA: 0x0002A7E7 File Offset: 0x000289E7
			public void OnCanceled(CancellationToken cancellationToken = default(CancellationToken))
			{
				this.completionSource.TrySetCanceled(cancellationToken);
			}

			// Token: 0x06000BB6 RID: 2998 RVA: 0x0002A7F6 File Offset: 0x000289F6
			public void OnNext(T value)
			{
				this.Current = value;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000BB7 RID: 2999 RVA: 0x0002A80C File Offset: 0x00028A0C
			public void OnCompleted()
			{
				this.completionSource.TrySetResult(false);
			}

			// Token: 0x06000BB8 RID: 3000 RVA: 0x0002A81B File Offset: 0x00028A1B
			public void OnError(Exception ex)
			{
				this.completionSource.TrySetException(ex);
			}

			// Token: 0x06000BB9 RID: 3001 RVA: 0x0002A82C File Offset: 0x00028A2C
			private static void CancellationCallback(object state)
			{
				AsyncTriggerBase<T>.AsyncTriggerEnumerator asyncTriggerEnumerator = (AsyncTriggerBase<T>.AsyncTriggerEnumerator)state;
				asyncTriggerEnumerator.DisposeAsync().Forget();
				asyncTriggerEnumerator.completionSource.TrySetCanceled(asyncTriggerEnumerator.cancellationToken);
			}

			// Token: 0x17000088 RID: 136
			// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0002A85D File Offset: 0x00028A5D
			// (set) Token: 0x06000BBB RID: 3003 RVA: 0x0002A865 File Offset: 0x00028A65
			public T Current { get; private set; }

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0002A86E File Offset: 0x00028A6E
			// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0002A876 File Offset: 0x00028A76
			ITriggerHandler<T> ITriggerHandler<!0>.Prev { get; set; }

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0002A87F File Offset: 0x00028A7F
			// (set) Token: 0x06000BBF RID: 3007 RVA: 0x0002A887 File Offset: 0x00028A87
			ITriggerHandler<T> ITriggerHandler<!0>.Next { get; set; }

			// Token: 0x06000BC0 RID: 3008 RVA: 0x0002A890 File Offset: 0x00028A90
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (!this.called)
				{
					this.called = true;
					this.parent.AddHandler(this);
					if (this.cancellationToken.CanBeCanceled)
					{
						this.registration = this.cancellationToken.RegisterWithoutCaptureExecutionContext(AsyncTriggerBase<T>.AsyncTriggerEnumerator.cancellationCallback, this);
					}
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000BC1 RID: 3009 RVA: 0x0002A904 File Offset: 0x00028B04
			public UniTask DisposeAsync()
			{
				if (!this.isDisposed)
				{
					this.isDisposed = true;
					this.registration.Dispose();
					this.parent.RemoveHandler(this);
				}
				return default(UniTask);
			}

			// Token: 0x04000539 RID: 1337
			private static Action<object> cancellationCallback = new Action<object>(AsyncTriggerBase<T>.AsyncTriggerEnumerator.CancellationCallback);

			// Token: 0x0400053A RID: 1338
			private readonly AsyncTriggerBase<T> parent;

			// Token: 0x0400053B RID: 1339
			private CancellationToken cancellationToken;

			// Token: 0x0400053C RID: 1340
			private CancellationTokenRegistration registration;

			// Token: 0x0400053D RID: 1341
			private bool called;

			// Token: 0x0400053E RID: 1342
			private bool isDisposed;
		}

		// Token: 0x02000208 RID: 520
		private class AwakeMonitor : IPlayerLoopItem
		{
			// Token: 0x06000BC3 RID: 3011 RVA: 0x0002A953 File Offset: 0x00028B53
			public AwakeMonitor(AsyncTriggerBase<T> trigger)
			{
				this.trigger = trigger;
			}

			// Token: 0x06000BC4 RID: 3012 RVA: 0x0002A962 File Offset: 0x00028B62
			public bool MoveNext()
			{
				if (this.trigger.calledAwake)
				{
					return false;
				}
				if (this.trigger == null)
				{
					this.trigger.OnDestroy();
					return false;
				}
				return true;
			}

			// Token: 0x04000542 RID: 1346
			private readonly AsyncTriggerBase<T> trigger;
		}
	}
}
