using System;
using System.Threading;
using UnityEngine.Events;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000067 RID: 103
	public class UnityEventHandlerAsyncEnumerable : IUniTaskAsyncEnumerable<AsyncUnit>
	{
		// Token: 0x060002C4 RID: 708 RVA: 0x0000A378 File Offset: 0x00008578
		public UnityEventHandlerAsyncEnumerable(UnityEvent unityEvent, CancellationToken cancellationToken)
		{
			this.unityEvent = unityEvent;
			this.cancellationToken1 = cancellationToken;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000A38E File Offset: 0x0000858E
		public IUniTaskAsyncEnumerator<AsyncUnit> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (this.cancellationToken1 == cancellationToken)
			{
				return new UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator(this.unityEvent, this.cancellationToken1, CancellationToken.None);
			}
			return new UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator(this.unityEvent, this.cancellationToken1, cancellationToken);
		}

		// Token: 0x040000E1 RID: 225
		private readonly UnityEvent unityEvent;

		// Token: 0x040000E2 RID: 226
		private readonly CancellationToken cancellationToken1;

		// Token: 0x020001FF RID: 511
		private class UnityEventHandlerAsyncEnumerator : MoveNextSource, IUniTaskAsyncEnumerator<AsyncUnit>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000B96 RID: 2966 RVA: 0x00029974 File Offset: 0x00027B74
			public UnityEventHandlerAsyncEnumerator(UnityEvent unityEvent, CancellationToken cancellationToken1, CancellationToken cancellationToken2)
			{
				this.unityEvent = unityEvent;
				this.cancellationToken1 = cancellationToken1;
				this.cancellationToken2 = cancellationToken2;
			}

			// Token: 0x17000086 RID: 134
			// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00029994 File Offset: 0x00027B94
			public AsyncUnit Current
			{
				get
				{
					return default(AsyncUnit);
				}
			}

			// Token: 0x06000B98 RID: 2968 RVA: 0x000299AC File Offset: 0x00027BAC
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken1.ThrowIfCancellationRequested();
				this.cancellationToken2.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (this.unityAction == null)
				{
					this.unityAction = new UnityAction(this.Invoke);
					this.unityEvent.AddListener(this.unityAction);
					if (this.cancellationToken1.CanBeCanceled)
					{
						this.registration1 = this.cancellationToken1.RegisterWithoutCaptureExecutionContext(UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator.cancel1, this);
					}
					if (this.cancellationToken2.CanBeCanceled)
					{
						this.registration2 = this.cancellationToken2.RegisterWithoutCaptureExecutionContext(UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator.cancel2, this);
					}
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000B99 RID: 2969 RVA: 0x00029A5E File Offset: 0x00027C5E
			private void Invoke()
			{
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000B9A RID: 2970 RVA: 0x00029A70 File Offset: 0x00027C70
			private static void OnCanceled1(object state)
			{
				UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator unityEventHandlerAsyncEnumerator = (UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator)state;
				try
				{
					unityEventHandlerAsyncEnumerator.completionSource.TrySetCanceled(unityEventHandlerAsyncEnumerator.cancellationToken1);
				}
				finally
				{
					unityEventHandlerAsyncEnumerator.DisposeAsync().Forget();
				}
			}

			// Token: 0x06000B9B RID: 2971 RVA: 0x00029AB4 File Offset: 0x00027CB4
			private static void OnCanceled2(object state)
			{
				UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator unityEventHandlerAsyncEnumerator = (UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator)state;
				try
				{
					unityEventHandlerAsyncEnumerator.completionSource.TrySetCanceled(unityEventHandlerAsyncEnumerator.cancellationToken2);
				}
				finally
				{
					unityEventHandlerAsyncEnumerator.DisposeAsync().Forget();
				}
			}

			// Token: 0x06000B9C RID: 2972 RVA: 0x00029AF8 File Offset: 0x00027CF8
			public UniTask DisposeAsync()
			{
				if (!this.isDisposed)
				{
					this.isDisposed = true;
					this.registration1.Dispose();
					this.registration2.Dispose();
					this.unityEvent.RemoveListener(this.unityAction);
					this.completionSource.TrySetCanceled(default(CancellationToken));
				}
				return default(UniTask);
			}

			// Token: 0x040004F2 RID: 1266
			private static readonly Action<object> cancel1 = new Action<object>(UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator.OnCanceled1);

			// Token: 0x040004F3 RID: 1267
			private static readonly Action<object> cancel2 = new Action<object>(UnityEventHandlerAsyncEnumerable.UnityEventHandlerAsyncEnumerator.OnCanceled2);

			// Token: 0x040004F4 RID: 1268
			private readonly UnityEvent unityEvent;

			// Token: 0x040004F5 RID: 1269
			private CancellationToken cancellationToken1;

			// Token: 0x040004F6 RID: 1270
			private CancellationToken cancellationToken2;

			// Token: 0x040004F7 RID: 1271
			private UnityAction unityAction;

			// Token: 0x040004F8 RID: 1272
			private CancellationTokenRegistration registration1;

			// Token: 0x040004F9 RID: 1273
			private CancellationTokenRegistration registration2;

			// Token: 0x040004FA RID: 1274
			private bool isDisposed;
		}
	}
}
