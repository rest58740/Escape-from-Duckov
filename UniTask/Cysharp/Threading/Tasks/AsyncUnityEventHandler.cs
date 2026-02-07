using System;
using System.Threading;
using System.Threading.Tasks.Sources;
using UnityEngine.Events;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000065 RID: 101
	public class AsyncUnityEventHandler : IUniTaskSource, IValueTaskSource, IDisposable, IAsyncClickEventHandler
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x00009FE4 File Offset: 0x000081E4
		public AsyncUnityEventHandler(UnityEvent unityEvent, CancellationToken cancellationToken, bool callOnce)
		{
			this.cancellationToken = cancellationToken;
			if (cancellationToken.IsCancellationRequested)
			{
				this.isDisposed = true;
				return;
			}
			this.action = new UnityAction(this.Invoke);
			this.unityEvent = unityEvent;
			this.callOnce = callOnce;
			unityEvent.AddListener(this.action);
			if (cancellationToken.CanBeCanceled)
			{
				this.registration = cancellationToken.RegisterWithoutCaptureExecutionContext(AsyncUnityEventHandler.cancellationCallback, this);
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000A056 File Offset: 0x00008256
		public UniTask OnInvokeAsync()
		{
			this.core.Reset();
			if (this.isDisposed)
			{
				this.core.TrySetCanceled(this.cancellationToken);
			}
			return new UniTask(this, this.core.Version);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000A08E File Offset: 0x0000828E
		private void Invoke()
		{
			this.core.TrySetResult(AsyncUnit.Default);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000A0A1 File Offset: 0x000082A1
		private static void CancellationCallback(object state)
		{
			((AsyncUnityEventHandler)state).Dispose();
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000A0B0 File Offset: 0x000082B0
		public void Dispose()
		{
			if (!this.isDisposed)
			{
				this.isDisposed = true;
				this.registration.Dispose();
				if (this.unityEvent != null)
				{
					this.unityEvent.RemoveListener(this.action);
				}
				this.core.TrySetCanceled(this.cancellationToken);
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000A102 File Offset: 0x00008302
		UniTask IAsyncClickEventHandler.OnClickAsync()
		{
			return this.OnInvokeAsync();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000A10C File Offset: 0x0000830C
		void IUniTaskSource.GetResult(short token)
		{
			try
			{
				this.core.GetResult(token);
			}
			finally
			{
				if (this.callOnce)
				{
					this.Dispose();
				}
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000A148 File Offset: 0x00008348
		UniTaskStatus IUniTaskSource.GetStatus(short token)
		{
			return this.core.GetStatus(token);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000A156 File Offset: 0x00008356
		UniTaskStatus IUniTaskSource.UnsafeGetStatus()
		{
			return this.core.UnsafeGetStatus();
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000A163 File Offset: 0x00008363
		void IUniTaskSource.OnCompleted(Action<object> continuation, object state, short token)
		{
			this.core.OnCompleted(continuation, state, token);
		}

		// Token: 0x040000D1 RID: 209
		private static Action<object> cancellationCallback = new Action<object>(AsyncUnityEventHandler.CancellationCallback);

		// Token: 0x040000D2 RID: 210
		private readonly UnityAction action;

		// Token: 0x040000D3 RID: 211
		private readonly UnityEvent unityEvent;

		// Token: 0x040000D4 RID: 212
		private CancellationToken cancellationToken;

		// Token: 0x040000D5 RID: 213
		private CancellationTokenRegistration registration;

		// Token: 0x040000D6 RID: 214
		private bool isDisposed;

		// Token: 0x040000D7 RID: 215
		private bool callOnce;

		// Token: 0x040000D8 RID: 216
		private UniTaskCompletionSourceCore<AsyncUnit> core;
	}
}
