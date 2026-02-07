using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000056 RID: 86
	public static class UniTaskObservableExtensions
	{
		// Token: 0x0600022D RID: 557 RVA: 0x00009138 File Offset: 0x00007338
		public static UniTask<T> ToUniTask<T>(this IObservable<T> source, bool useFirstValue = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTaskCompletionSource<T> uniTaskCompletionSource = new UniTaskCompletionSource<T>();
			SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
			IObserver<T> observer2;
			if (!useFirstValue)
			{
				IObserver<T> observer = new UniTaskObservableExtensions.ToUniTaskObserver<T>(uniTaskCompletionSource, singleAssignmentDisposable, cancellationToken);
				observer2 = observer;
			}
			else
			{
				IObserver<T> observer = new UniTaskObservableExtensions.FirstValueToUniTaskObserver<T>(uniTaskCompletionSource, singleAssignmentDisposable, cancellationToken);
				observer2 = observer;
			}
			IObserver<T> observer3 = observer2;
			try
			{
				singleAssignmentDisposable.Disposable = source.Subscribe(observer3);
			}
			catch (Exception exception)
			{
				uniTaskCompletionSource.TrySetException(exception);
			}
			return uniTaskCompletionSource.Task;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000091A0 File Offset: 0x000073A0
		public static IObservable<T> ToObservable<T>(this UniTask<T> task)
		{
			if (task.Status.IsCompleted())
			{
				try
				{
					return new UniTaskObservableExtensions.ReturnObservable<T>(task.GetAwaiter().GetResult());
				}
				catch (Exception value)
				{
					return new UniTaskObservableExtensions.ThrowObservable<T>(value);
				}
			}
			AsyncSubject<T> asyncSubject = new AsyncSubject<T>();
			UniTaskObservableExtensions.Fire<T>(asyncSubject, task).Forget();
			return asyncSubject;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00009200 File Offset: 0x00007400
		public static IObservable<AsyncUnit> ToObservable(this UniTask task)
		{
			if (task.Status.IsCompleted())
			{
				try
				{
					task.GetAwaiter().GetResult();
					return new UniTaskObservableExtensions.ReturnObservable<AsyncUnit>(AsyncUnit.Default);
				}
				catch (Exception value)
				{
					return new UniTaskObservableExtensions.ThrowObservable<AsyncUnit>(value);
				}
			}
			AsyncSubject<AsyncUnit> asyncSubject = new AsyncSubject<AsyncUnit>();
			UniTaskObservableExtensions.Fire(asyncSubject, task).Forget();
			return asyncSubject;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009268 File Offset: 0x00007468
		private static UniTaskVoid Fire<T>(AsyncSubject<T> subject, UniTask<T> task)
		{
			UniTaskObservableExtensions.<Fire>d__3<T> <Fire>d__;
			<Fire>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<Fire>d__.subject = subject;
			<Fire>d__.task = task;
			<Fire>d__.<>1__state = -1;
			<Fire>d__.<>t__builder.Start<UniTaskObservableExtensions.<Fire>d__3<T>>(ref <Fire>d__);
			return <Fire>d__.<>t__builder.Task;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000092B4 File Offset: 0x000074B4
		private static UniTaskVoid Fire(AsyncSubject<AsyncUnit> subject, UniTask task)
		{
			UniTaskObservableExtensions.<Fire>d__4 <Fire>d__;
			<Fire>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<Fire>d__.subject = subject;
			<Fire>d__.task = task;
			<Fire>d__.<>1__state = -1;
			<Fire>d__.<>t__builder.Start<UniTaskObservableExtensions.<Fire>d__4>(ref <Fire>d__);
			return <Fire>d__.<>t__builder.Task;
		}

		// Token: 0x020001E7 RID: 487
		private class ToUniTaskObserver<T> : IObserver<T>
		{
			// Token: 0x06000AEE RID: 2798 RVA: 0x0002769C File Offset: 0x0002589C
			public ToUniTaskObserver(UniTaskCompletionSource<T> promise, SingleAssignmentDisposable disposable, CancellationToken cancellationToken)
			{
				this.promise = promise;
				this.disposable = disposable;
				this.cancellationToken = cancellationToken;
				if (this.cancellationToken.CanBeCanceled)
				{
					this.registration = this.cancellationToken.RegisterWithoutCaptureExecutionContext(UniTaskObservableExtensions.ToUniTaskObserver<T>.callback, this);
				}
			}

			// Token: 0x06000AEF RID: 2799 RVA: 0x000276E8 File Offset: 0x000258E8
			private static void OnCanceled(object state)
			{
				UniTaskObservableExtensions.ToUniTaskObserver<T> toUniTaskObserver = (UniTaskObservableExtensions.ToUniTaskObserver<T>)state;
				toUniTaskObserver.disposable.Dispose();
				toUniTaskObserver.promise.TrySetCanceled(toUniTaskObserver.cancellationToken);
			}

			// Token: 0x06000AF0 RID: 2800 RVA: 0x00027719 File Offset: 0x00025919
			public void OnNext(T value)
			{
				this.hasValue = true;
				this.latestValue = value;
			}

			// Token: 0x06000AF1 RID: 2801 RVA: 0x0002772C File Offset: 0x0002592C
			public void OnError(Exception error)
			{
				try
				{
					this.promise.TrySetException(error);
				}
				finally
				{
					this.registration.Dispose();
					this.disposable.Dispose();
				}
			}

			// Token: 0x06000AF2 RID: 2802 RVA: 0x00027774 File Offset: 0x00025974
			public void OnCompleted()
			{
				try
				{
					if (this.hasValue)
					{
						this.promise.TrySetResult(this.latestValue);
					}
					else
					{
						this.promise.TrySetException(new InvalidOperationException("Sequence has no elements"));
					}
				}
				finally
				{
					this.registration.Dispose();
					this.disposable.Dispose();
				}
			}

			// Token: 0x0400046C RID: 1132
			private static readonly Action<object> callback = new Action<object>(UniTaskObservableExtensions.ToUniTaskObserver<T>.OnCanceled);

			// Token: 0x0400046D RID: 1133
			private readonly UniTaskCompletionSource<T> promise;

			// Token: 0x0400046E RID: 1134
			private readonly SingleAssignmentDisposable disposable;

			// Token: 0x0400046F RID: 1135
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000470 RID: 1136
			private readonly CancellationTokenRegistration registration;

			// Token: 0x04000471 RID: 1137
			private bool hasValue;

			// Token: 0x04000472 RID: 1138
			private T latestValue;
		}

		// Token: 0x020001E8 RID: 488
		private class FirstValueToUniTaskObserver<T> : IObserver<T>
		{
			// Token: 0x06000AF4 RID: 2804 RVA: 0x000277F4 File Offset: 0x000259F4
			public FirstValueToUniTaskObserver(UniTaskCompletionSource<T> promise, SingleAssignmentDisposable disposable, CancellationToken cancellationToken)
			{
				this.promise = promise;
				this.disposable = disposable;
				this.cancellationToken = cancellationToken;
				if (this.cancellationToken.CanBeCanceled)
				{
					this.registration = this.cancellationToken.RegisterWithoutCaptureExecutionContext(UniTaskObservableExtensions.FirstValueToUniTaskObserver<T>.callback, this);
				}
			}

			// Token: 0x06000AF5 RID: 2805 RVA: 0x00027840 File Offset: 0x00025A40
			private static void OnCanceled(object state)
			{
				UniTaskObservableExtensions.FirstValueToUniTaskObserver<T> firstValueToUniTaskObserver = (UniTaskObservableExtensions.FirstValueToUniTaskObserver<T>)state;
				firstValueToUniTaskObserver.disposable.Dispose();
				firstValueToUniTaskObserver.promise.TrySetCanceled(firstValueToUniTaskObserver.cancellationToken);
			}

			// Token: 0x06000AF6 RID: 2806 RVA: 0x00027874 File Offset: 0x00025A74
			public void OnNext(T value)
			{
				this.hasValue = true;
				try
				{
					this.promise.TrySetResult(value);
				}
				finally
				{
					this.registration.Dispose();
					this.disposable.Dispose();
				}
			}

			// Token: 0x06000AF7 RID: 2807 RVA: 0x000278C4 File Offset: 0x00025AC4
			public void OnError(Exception error)
			{
				try
				{
					this.promise.TrySetException(error);
				}
				finally
				{
					this.registration.Dispose();
					this.disposable.Dispose();
				}
			}

			// Token: 0x06000AF8 RID: 2808 RVA: 0x0002790C File Offset: 0x00025B0C
			public void OnCompleted()
			{
				try
				{
					if (!this.hasValue)
					{
						this.promise.TrySetException(new InvalidOperationException("Sequence has no elements"));
					}
				}
				finally
				{
					this.registration.Dispose();
					this.disposable.Dispose();
				}
			}

			// Token: 0x04000473 RID: 1139
			private static readonly Action<object> callback = new Action<object>(UniTaskObservableExtensions.FirstValueToUniTaskObserver<T>.OnCanceled);

			// Token: 0x04000474 RID: 1140
			private readonly UniTaskCompletionSource<T> promise;

			// Token: 0x04000475 RID: 1141
			private readonly SingleAssignmentDisposable disposable;

			// Token: 0x04000476 RID: 1142
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000477 RID: 1143
			private readonly CancellationTokenRegistration registration;

			// Token: 0x04000478 RID: 1144
			private bool hasValue;
		}

		// Token: 0x020001E9 RID: 489
		private class ReturnObservable<T> : IObservable<T>
		{
			// Token: 0x06000AFA RID: 2810 RVA: 0x00027977 File Offset: 0x00025B77
			public ReturnObservable(T value)
			{
				this.value = value;
			}

			// Token: 0x06000AFB RID: 2811 RVA: 0x00027986 File Offset: 0x00025B86
			public IDisposable Subscribe(IObserver<T> observer)
			{
				observer.OnNext(this.value);
				observer.OnCompleted();
				return EmptyDisposable.Instance;
			}

			// Token: 0x04000479 RID: 1145
			private readonly T value;
		}

		// Token: 0x020001EA RID: 490
		private class ThrowObservable<T> : IObservable<T>
		{
			// Token: 0x06000AFC RID: 2812 RVA: 0x0002799F File Offset: 0x00025B9F
			public ThrowObservable(Exception value)
			{
				this.value = value;
			}

			// Token: 0x06000AFD RID: 2813 RVA: 0x000279AE File Offset: 0x00025BAE
			public IDisposable Subscribe(IObserver<T> observer)
			{
				observer.OnError(this.value);
				return EmptyDisposable.Instance;
			}

			// Token: 0x0400047A RID: 1146
			private readonly Exception value;
		}
	}
}
