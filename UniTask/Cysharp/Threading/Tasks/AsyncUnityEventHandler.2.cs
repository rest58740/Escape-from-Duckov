using System;
using System.Threading;
using System.Threading.Tasks.Sources;
using UnityEngine.Events;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000066 RID: 102
	public class AsyncUnityEventHandler<T> : IUniTaskSource<T>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>, IDisposable, IAsyncValueChangedEventHandler<T>, IAsyncEndEditEventHandler<T>, IAsyncEndTextSelectionEventHandler<T>, IAsyncTextSelectionEventHandler<T>, IAsyncDeselectEventHandler<T>, IAsyncSelectEventHandler<T>, IAsyncSubmitEventHandler<T>
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0000A188 File Offset: 0x00008388
		public AsyncUnityEventHandler(UnityEvent<T> unityEvent, CancellationToken cancellationToken, bool callOnce)
		{
			this.cancellationToken = cancellationToken;
			if (cancellationToken.IsCancellationRequested)
			{
				this.isDisposed = true;
				return;
			}
			this.action = new UnityAction<T>(this.Invoke);
			this.unityEvent = unityEvent;
			this.callOnce = callOnce;
			unityEvent.AddListener(this.action);
			if (cancellationToken.CanBeCanceled)
			{
				this.registration = cancellationToken.RegisterWithoutCaptureExecutionContext(AsyncUnityEventHandler<T>.cancellationCallback, this);
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000A1FA File Offset: 0x000083FA
		public UniTask<T> OnInvokeAsync()
		{
			this.core.Reset();
			if (this.isDisposed)
			{
				this.core.TrySetCanceled(this.cancellationToken);
			}
			return new UniTask<T>(this, this.core.Version);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000A232 File Offset: 0x00008432
		private void Invoke(T result)
		{
			this.core.TrySetResult(result);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000A241 File Offset: 0x00008441
		private static void CancellationCallback(object state)
		{
			((AsyncUnityEventHandler<T>)state).Dispose();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000A250 File Offset: 0x00008450
		public void Dispose()
		{
			if (!this.isDisposed)
			{
				this.isDisposed = true;
				this.registration.Dispose();
				if (this.unityEvent != null)
				{
					IDisposable disposable = this.unityEvent as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
					this.unityEvent.RemoveListener(this.action);
				}
				this.core.TrySetCanceled(default(CancellationToken));
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000A2BA File Offset: 0x000084BA
		UniTask<T> IAsyncValueChangedEventHandler<!0>.OnValueChangedAsync()
		{
			return this.OnInvokeAsync();
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000A2C2 File Offset: 0x000084C2
		UniTask<T> IAsyncEndEditEventHandler<!0>.OnEndEditAsync()
		{
			return this.OnInvokeAsync();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000A2CA File Offset: 0x000084CA
		UniTask<T> IAsyncEndTextSelectionEventHandler<!0>.OnEndTextSelectionAsync()
		{
			return this.OnInvokeAsync();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000A2D2 File Offset: 0x000084D2
		UniTask<T> IAsyncTextSelectionEventHandler<!0>.OnTextSelectionAsync()
		{
			return this.OnInvokeAsync();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000A2DA File Offset: 0x000084DA
		UniTask<T> IAsyncDeselectEventHandler<!0>.OnDeselectAsync()
		{
			return this.OnInvokeAsync();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A2E2 File Offset: 0x000084E2
		UniTask<T> IAsyncSelectEventHandler<!0>.OnSelectAsync()
		{
			return this.OnInvokeAsync();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000A2EA File Offset: 0x000084EA
		UniTask<T> IAsyncSubmitEventHandler<!0>.OnSubmitAsync()
		{
			return this.OnInvokeAsync();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000A2F4 File Offset: 0x000084F4
		T IUniTaskSource<!0>.GetResult(short token)
		{
			T result;
			try
			{
				result = this.core.GetResult(token);
			}
			finally
			{
				if (this.callOnce)
				{
					this.Dispose();
				}
			}
			return result;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000A330 File Offset: 0x00008530
		void IUniTaskSource.GetResult(short token)
		{
			((IUniTaskSource<!0>)this).GetResult(token);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A33A File Offset: 0x0000853A
		UniTaskStatus IUniTaskSource.GetStatus(short token)
		{
			return this.core.GetStatus(token);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000A348 File Offset: 0x00008548
		UniTaskStatus IUniTaskSource.UnsafeGetStatus()
		{
			return this.core.UnsafeGetStatus();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000A355 File Offset: 0x00008555
		void IUniTaskSource.OnCompleted(Action<object> continuation, object state, short token)
		{
			this.core.OnCompleted(continuation, state, token);
		}

		// Token: 0x040000D9 RID: 217
		private static Action<object> cancellationCallback = new Action<object>(AsyncUnityEventHandler<T>.CancellationCallback);

		// Token: 0x040000DA RID: 218
		private readonly UnityAction<T> action;

		// Token: 0x040000DB RID: 219
		private readonly UnityEvent<T> unityEvent;

		// Token: 0x040000DC RID: 220
		private CancellationToken cancellationToken;

		// Token: 0x040000DD RID: 221
		private CancellationTokenRegistration registration;

		// Token: 0x040000DE RID: 222
		private bool isDisposed;

		// Token: 0x040000DF RID: 223
		private bool callOnce;

		// Token: 0x040000E0 RID: 224
		private UniTaskCompletionSourceCore<T> core;
	}
}
