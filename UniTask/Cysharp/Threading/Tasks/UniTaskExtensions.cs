using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000055 RID: 85
	public static class UniTaskExtensions
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x000077EC File Offset: 0x000059EC
		public static UniTask<T> AsUniTask<T>(this Task<T> task, bool useCurrentSynchronizationContext = true)
		{
			UniTaskCompletionSource<T> uniTaskCompletionSource = new UniTaskCompletionSource<T>();
			task.ContinueWith(delegate(Task<T> x, object state)
			{
				UniTaskCompletionSource<T> uniTaskCompletionSource2 = (UniTaskCompletionSource<T>)state;
				switch (x.Status)
				{
				case TaskStatus.RanToCompletion:
					uniTaskCompletionSource2.TrySetResult(x.Result);
					return;
				case TaskStatus.Canceled:
					uniTaskCompletionSource2.TrySetCanceled(default(CancellationToken));
					return;
				case TaskStatus.Faulted:
					uniTaskCompletionSource2.TrySetException(x.Exception.InnerException ?? x.Exception);
					return;
				default:
					throw new NotSupportedException();
				}
			}, uniTaskCompletionSource, useCurrentSynchronizationContext ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Current);
			return uniTaskCompletionSource.Task;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000783C File Offset: 0x00005A3C
		public static UniTask AsUniTask(this Task task, bool useCurrentSynchronizationContext = true)
		{
			UniTaskCompletionSource uniTaskCompletionSource = new UniTaskCompletionSource();
			task.ContinueWith(delegate(Task x, object state)
			{
				UniTaskCompletionSource uniTaskCompletionSource2 = (UniTaskCompletionSource)state;
				switch (x.Status)
				{
				case TaskStatus.RanToCompletion:
					uniTaskCompletionSource2.TrySetResult();
					return;
				case TaskStatus.Canceled:
					uniTaskCompletionSource2.TrySetCanceled(default(CancellationToken));
					return;
				case TaskStatus.Faulted:
					uniTaskCompletionSource2.TrySetException(x.Exception.InnerException ?? x.Exception);
					return;
				default:
					throw new NotSupportedException();
				}
			}, uniTaskCompletionSource, useCurrentSynchronizationContext ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Current);
			return uniTaskCompletionSource.Task;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000788C File Offset: 0x00005A8C
		public static Task<T> AsTask<T>(this UniTask<T> task)
		{
			Task<T> result;
			try
			{
				UniTask<T>.Awaiter awaiter;
				try
				{
					awaiter = task.GetAwaiter();
				}
				catch (Exception exception)
				{
					return Task.FromException<T>(exception);
				}
				if (awaiter.IsCompleted)
				{
					try
					{
						return Task.FromResult<T>(awaiter.GetResult());
					}
					catch (Exception exception2)
					{
						return Task.FromException<T>(exception2);
					}
				}
				TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
				awaiter.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<TaskCompletionSource<T>, UniTask<T>.Awaiter> stateTuple = (StateTuple<TaskCompletionSource<T>, UniTask<T>.Awaiter>)state)
					{
						TaskCompletionSource<T> taskCompletionSource2;
						UniTask<T>.Awaiter awaiter2;
						stateTuple.Deconstruct(out taskCompletionSource2, out awaiter2);
						TaskCompletionSource<T> taskCompletionSource3 = taskCompletionSource2;
						UniTask<T>.Awaiter awaiter3 = awaiter2;
						try
						{
							T result2 = awaiter3.GetResult();
							taskCompletionSource3.SetResult(result2);
						}
						catch (Exception exception4)
						{
							taskCompletionSource3.SetException(exception4);
						}
					}
				}, StateTuple.Create<TaskCompletionSource<T>, UniTask<T>.Awaiter>(taskCompletionSource, awaiter));
				result = taskCompletionSource.Task;
			}
			catch (Exception exception3)
			{
				result = Task.FromException<T>(exception3);
			}
			return result;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00007938 File Offset: 0x00005B38
		public static Task AsTask(this UniTask task)
		{
			Task result;
			try
			{
				UniTask.Awaiter awaiter;
				try
				{
					awaiter = task.GetAwaiter();
				}
				catch (Exception exception)
				{
					return Task.FromException(exception);
				}
				if (awaiter.IsCompleted)
				{
					try
					{
						awaiter.GetResult();
						return Task.CompletedTask;
					}
					catch (Exception exception2)
					{
						return Task.FromException(exception2);
					}
				}
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				awaiter.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<TaskCompletionSource<object>, UniTask.Awaiter> stateTuple = (StateTuple<TaskCompletionSource<object>, UniTask.Awaiter>)state)
					{
						TaskCompletionSource<object> taskCompletionSource2;
						UniTask.Awaiter awaiter2;
						stateTuple.Deconstruct(out taskCompletionSource2, out awaiter2);
						TaskCompletionSource<object> taskCompletionSource3 = taskCompletionSource2;
						UniTask.Awaiter awaiter3 = awaiter2;
						try
						{
							awaiter3.GetResult();
							taskCompletionSource3.SetResult(null);
						}
						catch (Exception exception4)
						{
							taskCompletionSource3.SetException(exception4);
						}
					}
				}, StateTuple.Create<TaskCompletionSource<object>, UniTask.Awaiter>(taskCompletionSource, awaiter));
				result = taskCompletionSource.Task;
			}
			catch (Exception exception3)
			{
				result = Task.FromException(exception3);
			}
			return result;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000079E4 File Offset: 0x00005BE4
		public static AsyncLazy ToAsyncLazy(this UniTask task)
		{
			return new AsyncLazy(task);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000079EC File Offset: 0x00005BEC
		public static AsyncLazy<T> ToAsyncLazy<T>(this UniTask<T> task)
		{
			return new AsyncLazy<T>(task);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000079F4 File Offset: 0x00005BF4
		public static UniTask AttachExternalCancellation(this UniTask task, CancellationToken cancellationToken)
		{
			if (!cancellationToken.CanBeCanceled)
			{
				return task;
			}
			if (cancellationToken.IsCancellationRequested)
			{
				task.Forget();
				return UniTask.FromCanceled(cancellationToken);
			}
			if (task.Status.IsCompleted())
			{
				return task;
			}
			return new UniTask(new UniTaskExtensions.AttachExternalCancellationSource(task, cancellationToken), 0);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007A34 File Offset: 0x00005C34
		public static UniTask<T> AttachExternalCancellation<T>(this UniTask<T> task, CancellationToken cancellationToken)
		{
			if (!cancellationToken.CanBeCanceled)
			{
				return task;
			}
			if (cancellationToken.IsCancellationRequested)
			{
				task.Forget<T>();
				return UniTask.FromCanceled<T>(cancellationToken);
			}
			if (task.Status.IsCompleted())
			{
				return task;
			}
			return new UniTask<T>(new UniTaskExtensions.AttachExternalCancellationSource<T>(task, cancellationToken), 0);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007A74 File Offset: 0x00005C74
		public static IEnumerator ToCoroutine<T>(this UniTask<T> task, Action<T> resultHandler = null, Action<Exception> exceptionHandler = null)
		{
			return new UniTaskExtensions.ToCoroutineEnumerator<T>(task, resultHandler, exceptionHandler);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007A7E File Offset: 0x00005C7E
		public static IEnumerator ToCoroutine(this UniTask task, Action<Exception> exceptionHandler = null)
		{
			return new UniTaskExtensions.ToCoroutineEnumerator(task, exceptionHandler);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007A88 File Offset: 0x00005C88
		public static UniTask Timeout(this UniTask task, TimeSpan timeout, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming timeoutCheckTiming = PlayerLoopTiming.Update, CancellationTokenSource taskCancellationTokenSource = null)
		{
			UniTaskExtensions.<Timeout>d__12 <Timeout>d__;
			<Timeout>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Timeout>d__.task = task;
			<Timeout>d__.timeout = timeout;
			<Timeout>d__.delayType = delayType;
			<Timeout>d__.timeoutCheckTiming = timeoutCheckTiming;
			<Timeout>d__.taskCancellationTokenSource = taskCancellationTokenSource;
			<Timeout>d__.<>1__state = -1;
			<Timeout>d__.<>t__builder.Start<UniTaskExtensions.<Timeout>d__12>(ref <Timeout>d__);
			return <Timeout>d__.<>t__builder.Task;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007AEC File Offset: 0x00005CEC
		public static UniTask<T> Timeout<T>(this UniTask<T> task, TimeSpan timeout, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming timeoutCheckTiming = PlayerLoopTiming.Update, CancellationTokenSource taskCancellationTokenSource = null)
		{
			UniTaskExtensions.<Timeout>d__13<T> <Timeout>d__;
			<Timeout>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<Timeout>d__.task = task;
			<Timeout>d__.timeout = timeout;
			<Timeout>d__.delayType = delayType;
			<Timeout>d__.timeoutCheckTiming = timeoutCheckTiming;
			<Timeout>d__.taskCancellationTokenSource = taskCancellationTokenSource;
			<Timeout>d__.<>1__state = -1;
			<Timeout>d__.<>t__builder.Start<UniTaskExtensions.<Timeout>d__13<T>>(ref <Timeout>d__);
			return <Timeout>d__.<>t__builder.Task;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007B50 File Offset: 0x00005D50
		public static UniTask<bool> TimeoutWithoutException(this UniTask task, TimeSpan timeout, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming timeoutCheckTiming = PlayerLoopTiming.Update, CancellationTokenSource taskCancellationTokenSource = null)
		{
			UniTaskExtensions.<TimeoutWithoutException>d__14 <TimeoutWithoutException>d__;
			<TimeoutWithoutException>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<TimeoutWithoutException>d__.task = task;
			<TimeoutWithoutException>d__.timeout = timeout;
			<TimeoutWithoutException>d__.delayType = delayType;
			<TimeoutWithoutException>d__.timeoutCheckTiming = timeoutCheckTiming;
			<TimeoutWithoutException>d__.taskCancellationTokenSource = taskCancellationTokenSource;
			<TimeoutWithoutException>d__.<>1__state = -1;
			<TimeoutWithoutException>d__.<>t__builder.Start<UniTaskExtensions.<TimeoutWithoutException>d__14>(ref <TimeoutWithoutException>d__);
			return <TimeoutWithoutException>d__.<>t__builder.Task;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007BB4 File Offset: 0x00005DB4
		[return: TupleElementNames(new string[]
		{
			"IsTimeout",
			"Result"
		})]
		public static UniTask<ValueTuple<bool, T>> TimeoutWithoutException<T>(this UniTask<T> task, TimeSpan timeout, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming timeoutCheckTiming = PlayerLoopTiming.Update, CancellationTokenSource taskCancellationTokenSource = null)
		{
			UniTaskExtensions.<TimeoutWithoutException>d__15<T> <TimeoutWithoutException>d__;
			<TimeoutWithoutException>d__.<>t__builder = AsyncUniTaskMethodBuilder<ValueTuple<bool, T>>.Create();
			<TimeoutWithoutException>d__.task = task;
			<TimeoutWithoutException>d__.timeout = timeout;
			<TimeoutWithoutException>d__.delayType = delayType;
			<TimeoutWithoutException>d__.timeoutCheckTiming = timeoutCheckTiming;
			<TimeoutWithoutException>d__.taskCancellationTokenSource = taskCancellationTokenSource;
			<TimeoutWithoutException>d__.<>1__state = -1;
			<TimeoutWithoutException>d__.<>t__builder.Start<UniTaskExtensions.<TimeoutWithoutException>d__15<T>>(ref <TimeoutWithoutException>d__);
			return <TimeoutWithoutException>d__.<>t__builder.Task;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007C18 File Offset: 0x00005E18
		public static void Forget(this UniTask task)
		{
			UniTask.Awaiter awaiter = task.GetAwaiter();
			if (awaiter.IsCompleted)
			{
				try
				{
					awaiter.GetResult();
					return;
				}
				catch (Exception ex)
				{
					UniTaskScheduler.PublishUnobservedTaskException(ex);
					return;
				}
			}
			awaiter.SourceOnCompleted(delegate(object state)
			{
				using (StateTuple<UniTask.Awaiter> stateTuple = (StateTuple<UniTask.Awaiter>)state)
				{
					try
					{
						stateTuple.Item1.GetResult();
					}
					catch (Exception ex2)
					{
						UniTaskScheduler.PublishUnobservedTaskException(ex2);
					}
				}
			}, StateTuple.Create<UniTask.Awaiter>(awaiter));
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007C84 File Offset: 0x00005E84
		public static void Forget(this UniTask task, Action<Exception> exceptionHandler, bool handleExceptionOnMainThread = true)
		{
			if (exceptionHandler == null)
			{
				task.Forget();
				return;
			}
			UniTaskExtensions.ForgetCoreWithCatch(task, exceptionHandler, handleExceptionOnMainThread).Forget();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007CAC File Offset: 0x00005EAC
		private static UniTaskVoid ForgetCoreWithCatch(UniTask task, Action<Exception> exceptionHandler, bool handleExceptionOnMainThread)
		{
			UniTaskExtensions.<ForgetCoreWithCatch>d__18 <ForgetCoreWithCatch>d__;
			<ForgetCoreWithCatch>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<ForgetCoreWithCatch>d__.task = task;
			<ForgetCoreWithCatch>d__.exceptionHandler = exceptionHandler;
			<ForgetCoreWithCatch>d__.handleExceptionOnMainThread = handleExceptionOnMainThread;
			<ForgetCoreWithCatch>d__.<>1__state = -1;
			<ForgetCoreWithCatch>d__.<>t__builder.Start<UniTaskExtensions.<ForgetCoreWithCatch>d__18>(ref <ForgetCoreWithCatch>d__);
			return <ForgetCoreWithCatch>d__.<>t__builder.Task;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007D00 File Offset: 0x00005F00
		public static void Forget<T>(this UniTask<T> task)
		{
			UniTask<T>.Awaiter awaiter = task.GetAwaiter();
			if (awaiter.IsCompleted)
			{
				try
				{
					awaiter.GetResult();
					return;
				}
				catch (Exception ex)
				{
					UniTaskScheduler.PublishUnobservedTaskException(ex);
					return;
				}
			}
			awaiter.SourceOnCompleted(delegate(object state)
			{
				using (StateTuple<UniTask<T>.Awaiter> stateTuple = (StateTuple<UniTask<T>.Awaiter>)state)
				{
					try
					{
						stateTuple.Item1.GetResult();
					}
					catch (Exception ex2)
					{
						UniTaskScheduler.PublishUnobservedTaskException(ex2);
					}
				}
			}, StateTuple.Create<UniTask<T>.Awaiter>(awaiter));
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007D6C File Offset: 0x00005F6C
		public static void Forget<T>(this UniTask<T> task, Action<Exception> exceptionHandler, bool handleExceptionOnMainThread = true)
		{
			if (exceptionHandler == null)
			{
				task.Forget<T>();
				return;
			}
			UniTaskExtensions.ForgetCoreWithCatch<T>(task, exceptionHandler, handleExceptionOnMainThread).Forget();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00007D94 File Offset: 0x00005F94
		private static UniTaskVoid ForgetCoreWithCatch<T>(UniTask<T> task, Action<Exception> exceptionHandler, bool handleExceptionOnMainThread)
		{
			UniTaskExtensions.<ForgetCoreWithCatch>d__21<T> <ForgetCoreWithCatch>d__;
			<ForgetCoreWithCatch>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<ForgetCoreWithCatch>d__.task = task;
			<ForgetCoreWithCatch>d__.exceptionHandler = exceptionHandler;
			<ForgetCoreWithCatch>d__.handleExceptionOnMainThread = handleExceptionOnMainThread;
			<ForgetCoreWithCatch>d__.<>1__state = -1;
			<ForgetCoreWithCatch>d__.<>t__builder.Start<UniTaskExtensions.<ForgetCoreWithCatch>d__21<T>>(ref <ForgetCoreWithCatch>d__);
			return <ForgetCoreWithCatch>d__.<>t__builder.Task;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007DE8 File Offset: 0x00005FE8
		public static UniTask ContinueWith<T>(this UniTask<T> task, Action<T> continuationFunction)
		{
			UniTaskExtensions.<ContinueWith>d__22<T> <ContinueWith>d__;
			<ContinueWith>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ContinueWith>d__.task = task;
			<ContinueWith>d__.continuationFunction = continuationFunction;
			<ContinueWith>d__.<>1__state = -1;
			<ContinueWith>d__.<>t__builder.Start<UniTaskExtensions.<ContinueWith>d__22<T>>(ref <ContinueWith>d__);
			return <ContinueWith>d__.<>t__builder.Task;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00007E34 File Offset: 0x00006034
		public static UniTask ContinueWith<T>(this UniTask<T> task, Func<T, UniTask> continuationFunction)
		{
			UniTaskExtensions.<ContinueWith>d__23<T> <ContinueWith>d__;
			<ContinueWith>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ContinueWith>d__.task = task;
			<ContinueWith>d__.continuationFunction = continuationFunction;
			<ContinueWith>d__.<>1__state = -1;
			<ContinueWith>d__.<>t__builder.Start<UniTaskExtensions.<ContinueWith>d__23<T>>(ref <ContinueWith>d__);
			return <ContinueWith>d__.<>t__builder.Task;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00007E80 File Offset: 0x00006080
		public static UniTask<TR> ContinueWith<T, TR>(this UniTask<T> task, Func<T, TR> continuationFunction)
		{
			UniTaskExtensions.<ContinueWith>d__24<T, TR> <ContinueWith>d__;
			<ContinueWith>d__.<>t__builder = AsyncUniTaskMethodBuilder<TR>.Create();
			<ContinueWith>d__.task = task;
			<ContinueWith>d__.continuationFunction = continuationFunction;
			<ContinueWith>d__.<>1__state = -1;
			<ContinueWith>d__.<>t__builder.Start<UniTaskExtensions.<ContinueWith>d__24<T, TR>>(ref <ContinueWith>d__);
			return <ContinueWith>d__.<>t__builder.Task;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00007ECC File Offset: 0x000060CC
		public static UniTask<TR> ContinueWith<T, TR>(this UniTask<T> task, Func<T, UniTask<TR>> continuationFunction)
		{
			UniTaskExtensions.<ContinueWith>d__25<T, TR> <ContinueWith>d__;
			<ContinueWith>d__.<>t__builder = AsyncUniTaskMethodBuilder<TR>.Create();
			<ContinueWith>d__.task = task;
			<ContinueWith>d__.continuationFunction = continuationFunction;
			<ContinueWith>d__.<>1__state = -1;
			<ContinueWith>d__.<>t__builder.Start<UniTaskExtensions.<ContinueWith>d__25<T, TR>>(ref <ContinueWith>d__);
			return <ContinueWith>d__.<>t__builder.Task;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00007F18 File Offset: 0x00006118
		public static UniTask ContinueWith(this UniTask task, Action continuationFunction)
		{
			UniTaskExtensions.<ContinueWith>d__26 <ContinueWith>d__;
			<ContinueWith>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ContinueWith>d__.task = task;
			<ContinueWith>d__.continuationFunction = continuationFunction;
			<ContinueWith>d__.<>1__state = -1;
			<ContinueWith>d__.<>t__builder.Start<UniTaskExtensions.<ContinueWith>d__26>(ref <ContinueWith>d__);
			return <ContinueWith>d__.<>t__builder.Task;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00007F64 File Offset: 0x00006164
		public static UniTask ContinueWith(this UniTask task, Func<UniTask> continuationFunction)
		{
			UniTaskExtensions.<ContinueWith>d__27 <ContinueWith>d__;
			<ContinueWith>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ContinueWith>d__.task = task;
			<ContinueWith>d__.continuationFunction = continuationFunction;
			<ContinueWith>d__.<>1__state = -1;
			<ContinueWith>d__.<>t__builder.Start<UniTaskExtensions.<ContinueWith>d__27>(ref <ContinueWith>d__);
			return <ContinueWith>d__.<>t__builder.Task;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00007FB0 File Offset: 0x000061B0
		public static UniTask<T> ContinueWith<T>(this UniTask task, Func<T> continuationFunction)
		{
			UniTaskExtensions.<ContinueWith>d__28<T> <ContinueWith>d__;
			<ContinueWith>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<ContinueWith>d__.task = task;
			<ContinueWith>d__.continuationFunction = continuationFunction;
			<ContinueWith>d__.<>1__state = -1;
			<ContinueWith>d__.<>t__builder.Start<UniTaskExtensions.<ContinueWith>d__28<T>>(ref <ContinueWith>d__);
			return <ContinueWith>d__.<>t__builder.Task;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00007FFC File Offset: 0x000061FC
		public static UniTask<T> ContinueWith<T>(this UniTask task, Func<UniTask<T>> continuationFunction)
		{
			UniTaskExtensions.<ContinueWith>d__29<T> <ContinueWith>d__;
			<ContinueWith>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<ContinueWith>d__.task = task;
			<ContinueWith>d__.continuationFunction = continuationFunction;
			<ContinueWith>d__.<>1__state = -1;
			<ContinueWith>d__.<>t__builder.Start<UniTaskExtensions.<ContinueWith>d__29<T>>(ref <ContinueWith>d__);
			return <ContinueWith>d__.<>t__builder.Task;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00008048 File Offset: 0x00006248
		public static UniTask<T> Unwrap<T>(this UniTask<UniTask<T>> task)
		{
			UniTaskExtensions.<Unwrap>d__30<T> <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__30<T>>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000808C File Offset: 0x0000628C
		public static UniTask Unwrap(this UniTask<UniTask> task)
		{
			UniTaskExtensions.<Unwrap>d__31 <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__31>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000080D0 File Offset: 0x000062D0
		public static UniTask<T> Unwrap<T>(this Task<UniTask<T>> task)
		{
			UniTaskExtensions.<Unwrap>d__32<T> <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__32<T>>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00008114 File Offset: 0x00006314
		public static UniTask<T> Unwrap<T>(this Task<UniTask<T>> task, bool continueOnCapturedContext)
		{
			UniTaskExtensions.<Unwrap>d__33<T> <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.continueOnCapturedContext = continueOnCapturedContext;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__33<T>>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00008160 File Offset: 0x00006360
		public static UniTask Unwrap(this Task<UniTask> task)
		{
			UniTaskExtensions.<Unwrap>d__34 <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__34>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000081A4 File Offset: 0x000063A4
		public static UniTask Unwrap(this Task<UniTask> task, bool continueOnCapturedContext)
		{
			UniTaskExtensions.<Unwrap>d__35 <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.continueOnCapturedContext = continueOnCapturedContext;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__35>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000081F0 File Offset: 0x000063F0
		public static UniTask<T> Unwrap<T>(this UniTask<Task<T>> task)
		{
			UniTaskExtensions.<Unwrap>d__36<T> <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__36<T>>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00008234 File Offset: 0x00006434
		public static UniTask<T> Unwrap<T>(this UniTask<Task<T>> task, bool continueOnCapturedContext)
		{
			UniTaskExtensions.<Unwrap>d__37<T> <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.continueOnCapturedContext = continueOnCapturedContext;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__37<T>>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008280 File Offset: 0x00006480
		public static UniTask Unwrap(this UniTask<Task> task)
		{
			UniTaskExtensions.<Unwrap>d__38 <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__38>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000082C4 File Offset: 0x000064C4
		public static UniTask Unwrap(this UniTask<Task> task, bool continueOnCapturedContext)
		{
			UniTaskExtensions.<Unwrap>d__39 <Unwrap>d__;
			<Unwrap>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Unwrap>d__.task = task;
			<Unwrap>d__.continueOnCapturedContext = continueOnCapturedContext;
			<Unwrap>d__.<>1__state = -1;
			<Unwrap>d__.<>t__builder.Start<UniTaskExtensions.<Unwrap>d__39>(ref <Unwrap>d__);
			return <Unwrap>d__.<>t__builder.Task;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00008310 File Offset: 0x00006510
		public static UniTask.Awaiter GetAwaiter(this UniTask[] tasks)
		{
			return UniTask.WhenAll(tasks).GetAwaiter();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000832C File Offset: 0x0000652C
		public static UniTask.Awaiter GetAwaiter(this IEnumerable<UniTask> tasks)
		{
			return UniTask.WhenAll(tasks).GetAwaiter();
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00008348 File Offset: 0x00006548
		public static UniTask<T[]>.Awaiter GetAwaiter<T>(this UniTask<T>[] tasks)
		{
			return UniTask.WhenAll<T>(tasks).GetAwaiter();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008364 File Offset: 0x00006564
		public static UniTask<T[]>.Awaiter GetAwaiter<T>(this IEnumerable<UniTask<T>> tasks)
		{
			return UniTask.WhenAll<T>(tasks).GetAwaiter();
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00008380 File Offset: 0x00006580
		public static UniTask<ValueTuple<T1, T2>>.Awaiter GetAwaiter<T1, T2>([TupleElementNames(new string[]
		{
			"task1",
			"task2"
		})] this ValueTuple<UniTask<T1>, UniTask<T2>> tasks)
		{
			return UniTask.WhenAll<T1, T2>(tasks.Item1, tasks.Item2).GetAwaiter();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000083A8 File Offset: 0x000065A8
		public static UniTask<ValueTuple<T1, T2, T3>>.Awaiter GetAwaiter<T1, T2, T3>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3"
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3>(tasks.Item1, tasks.Item2, tasks.Item3).GetAwaiter();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000083D4 File Offset: 0x000065D4
		public static UniTask<ValueTuple<T1, T2, T3, T4>>.Awaiter GetAwaiter<T1, T2, T3, T4>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4"
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4).GetAwaiter();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008408 File Offset: 0x00006608
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5"
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5).GetAwaiter();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00008440 File Offset: 0x00006640
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6"
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6).GetAwaiter();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00008480 File Offset: 0x00006680
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7"
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7).GetAwaiter();
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000084C4 File Offset: 0x000066C4
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			null
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>, ValueTuple<UniTask<T8>>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7, T8>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Rest.Item1).GetAwaiter();
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00008514 File Offset: 0x00006714
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			null,
			null
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>, ValueTuple<UniTask<T8>, UniTask<T9>>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Rest.Item1, tasks.Rest.Item2).GetAwaiter();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00008570 File Offset: 0x00006770
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			null,
			null,
			null
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>, ValueTuple<UniTask<T8>, UniTask<T9>, UniTask<T10>>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Rest.Item1, tasks.Rest.Item2, tasks.Rest.Item3).GetAwaiter();
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000085D8 File Offset: 0x000067D8
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>, ValueTuple<UniTask<T8>, UniTask<T9>, UniTask<T10>, UniTask<T11>>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Rest.Item1, tasks.Rest.Item2, tasks.Rest.Item3, tasks.Rest.Item4).GetAwaiter();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00008648 File Offset: 0x00006848
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			"task12",
			null,
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>, ValueTuple<UniTask<T8>, UniTask<T9>, UniTask<T10>, UniTask<T11>, UniTask<T12>>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Rest.Item1, tasks.Rest.Item2, tasks.Rest.Item3, tasks.Rest.Item4, tasks.Rest.Item5).GetAwaiter();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000086C4 File Offset: 0x000068C4
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			"task12",
			"task13",
			null,
			null,
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>, ValueTuple<UniTask<T8>, UniTask<T9>, UniTask<T10>, UniTask<T11>, UniTask<T12>, UniTask<T13>>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Rest.Item1, tasks.Rest.Item2, tasks.Rest.Item3, tasks.Rest.Item4, tasks.Rest.Item5, tasks.Rest.Item6).GetAwaiter();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000874C File Offset: 0x0000694C
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			"task12",
			"task13",
			"task14",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>, ValueTuple<UniTask<T8>, UniTask<T9>, UniTask<T10>, UniTask<T11>, UniTask<T12>, UniTask<T13>, UniTask<T14>>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Rest.Item1, tasks.Rest.Item2, tasks.Rest.Item3, tasks.Rest.Item4, tasks.Rest.Item5, tasks.Rest.Item6, tasks.Rest.Item7).GetAwaiter();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000087E0 File Offset: 0x000069E0
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			"task12",
			"task13",
			"task14",
			"task15",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask<T1>, UniTask<T2>, UniTask<T3>, UniTask<T4>, UniTask<T5>, UniTask<T6>, UniTask<T7>, ValueTuple<UniTask<T8>, UniTask<T9>, UniTask<T10>, UniTask<T11>, UniTask<T12>, UniTask<T13>, UniTask<T14>, ValueTuple<UniTask<T15>>>> tasks)
		{
			return UniTask.WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Rest.Item1, tasks.Rest.Item2, tasks.Rest.Item3, tasks.Rest.Item4, tasks.Rest.Item5, tasks.Rest.Item6, tasks.Rest.Item7, tasks.Rest.Rest.Item1).GetAwaiter();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00008884 File Offset: 0x00006A84
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2"
		})] this ValueTuple<UniTask, UniTask> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2
			}).GetAwaiter();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000088C0 File Offset: 0x00006AC0
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3"
		})] this ValueTuple<UniTask, UniTask, UniTask> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3
			}).GetAwaiter();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00008908 File Offset: 0x00006B08
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4"
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4
			}).GetAwaiter();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000895C File Offset: 0x00006B5C
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5"
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5
			}).GetAwaiter();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000089C0 File Offset: 0x00006BC0
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6"
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6
			}).GetAwaiter();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008A30 File Offset: 0x00006C30
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7"
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7
			}).GetAwaiter();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008AAC File Offset: 0x00006CAC
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			null
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask>> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7,
				tasks.Rest.Item1
			}).GetAwaiter();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008B3C File Offset: 0x00006D3C
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			null,
			null
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask, UniTask>> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7,
				tasks.Rest.Item1,
				tasks.Rest.Item2
			}).GetAwaiter();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00008BDC File Offset: 0x00006DDC
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			null,
			null,
			null
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask, UniTask, UniTask>> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7,
				tasks.Rest.Item1,
				tasks.Rest.Item2,
				tasks.Rest.Item3
			}).GetAwaiter();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008C90 File Offset: 0x00006E90
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask, UniTask, UniTask, UniTask>> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7,
				tasks.Rest.Item1,
				tasks.Rest.Item2,
				tasks.Rest.Item3,
				tasks.Rest.Item4
			}).GetAwaiter();
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00008D58 File Offset: 0x00006F58
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			"task12",
			null,
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask>> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7,
				tasks.Rest.Item1,
				tasks.Rest.Item2,
				tasks.Rest.Item3,
				tasks.Rest.Item4,
				tasks.Rest.Item5
			}).GetAwaiter();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00008E34 File Offset: 0x00007034
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			"task12",
			"task13",
			null,
			null,
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask>> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7,
				tasks.Rest.Item1,
				tasks.Rest.Item2,
				tasks.Rest.Item3,
				tasks.Rest.Item4,
				tasks.Rest.Item5,
				tasks.Rest.Item6
			}).GetAwaiter();
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00008F20 File Offset: 0x00007120
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			"task12",
			"task13",
			"task14",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask>> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7,
				tasks.Rest.Item1,
				tasks.Rest.Item2,
				tasks.Rest.Item3,
				tasks.Rest.Item4,
				tasks.Rest.Item5,
				tasks.Rest.Item6,
				tasks.Rest.Item7
			}).GetAwaiter();
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00009020 File Offset: 0x00007220
		public static UniTask.Awaiter GetAwaiter([TupleElementNames(new string[]
		{
			"task1",
			"task2",
			"task3",
			"task4",
			"task5",
			"task6",
			"task7",
			"task8",
			"task9",
			"task10",
			"task11",
			"task12",
			"task13",
			"task14",
			"task15",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		})] this ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, UniTask, ValueTuple<UniTask>>> tasks)
		{
			return UniTask.WhenAll(new UniTask[]
			{
				tasks.Item1,
				tasks.Item2,
				tasks.Item3,
				tasks.Item4,
				tasks.Item5,
				tasks.Item6,
				tasks.Item7,
				tasks.Rest.Item1,
				tasks.Rest.Item2,
				tasks.Rest.Item3,
				tasks.Rest.Item4,
				tasks.Rest.Item5,
				tasks.Rest.Item6,
				tasks.Rest.Item7,
				tasks.Rest.Rest.Item1
			}).GetAwaiter();
		}

		// Token: 0x020001C7 RID: 455
		private sealed class AttachExternalCancellationSource : IUniTaskSource, IValueTaskSource
		{
			// Token: 0x06000A95 RID: 2709 RVA: 0x000251D4 File Offset: 0x000233D4
			public AttachExternalCancellationSource(UniTask task, CancellationToken cancellationToken)
			{
				this.cancellationToken = cancellationToken;
				this.tokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(UniTaskExtensions.AttachExternalCancellationSource.cancellationCallbackDelegate, this);
				this.RunTask(task).Forget();
			}

			// Token: 0x06000A96 RID: 2710 RVA: 0x00025210 File Offset: 0x00023410
			private UniTaskVoid RunTask(UniTask task)
			{
				UniTaskExtensions.AttachExternalCancellationSource.<RunTask>d__5 <RunTask>d__;
				<RunTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunTask>d__.<>4__this = this;
				<RunTask>d__.task = task;
				<RunTask>d__.<>1__state = -1;
				<RunTask>d__.<>t__builder.Start<UniTaskExtensions.AttachExternalCancellationSource.<RunTask>d__5>(ref <RunTask>d__);
				return <RunTask>d__.<>t__builder.Task;
			}

			// Token: 0x06000A97 RID: 2711 RVA: 0x0002525C File Offset: 0x0002345C
			private static void CancellationCallback(object state)
			{
				UniTaskExtensions.AttachExternalCancellationSource attachExternalCancellationSource = (UniTaskExtensions.AttachExternalCancellationSource)state;
				attachExternalCancellationSource.core.TrySetCanceled(attachExternalCancellationSource.cancellationToken);
			}

			// Token: 0x06000A98 RID: 2712 RVA: 0x00025282 File Offset: 0x00023482
			public void GetResult(short token)
			{
				this.core.GetResult(token);
			}

			// Token: 0x06000A99 RID: 2713 RVA: 0x00025291 File Offset: 0x00023491
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000A9A RID: 2714 RVA: 0x0002529F File Offset: 0x0002349F
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000A9B RID: 2715 RVA: 0x000252AF File Offset: 0x000234AF
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x040003B1 RID: 945
			private static readonly Action<object> cancellationCallbackDelegate = new Action<object>(UniTaskExtensions.AttachExternalCancellationSource.CancellationCallback);

			// Token: 0x040003B2 RID: 946
			private CancellationToken cancellationToken;

			// Token: 0x040003B3 RID: 947
			private CancellationTokenRegistration tokenRegistration;

			// Token: 0x040003B4 RID: 948
			private UniTaskCompletionSourceCore<AsyncUnit> core;
		}

		// Token: 0x020001C8 RID: 456
		private sealed class AttachExternalCancellationSource<T> : IUniTaskSource<!0>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>
		{
			// Token: 0x06000A9D RID: 2717 RVA: 0x000252D0 File Offset: 0x000234D0
			public AttachExternalCancellationSource(UniTask<T> task, CancellationToken cancellationToken)
			{
				this.cancellationToken = cancellationToken;
				this.tokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(UniTaskExtensions.AttachExternalCancellationSource<T>.cancellationCallbackDelegate, this);
				this.RunTask(task).Forget();
			}

			// Token: 0x06000A9E RID: 2718 RVA: 0x0002530C File Offset: 0x0002350C
			private UniTaskVoid RunTask(UniTask<T> task)
			{
				UniTaskExtensions.AttachExternalCancellationSource<T>.<RunTask>d__5 <RunTask>d__;
				<RunTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunTask>d__.<>4__this = this;
				<RunTask>d__.task = task;
				<RunTask>d__.<>1__state = -1;
				<RunTask>d__.<>t__builder.Start<UniTaskExtensions.AttachExternalCancellationSource<T>.<RunTask>d__5>(ref <RunTask>d__);
				return <RunTask>d__.<>t__builder.Task;
			}

			// Token: 0x06000A9F RID: 2719 RVA: 0x00025358 File Offset: 0x00023558
			private static void CancellationCallback(object state)
			{
				UniTaskExtensions.AttachExternalCancellationSource<T> attachExternalCancellationSource = (UniTaskExtensions.AttachExternalCancellationSource<T>)state;
				attachExternalCancellationSource.core.TrySetCanceled(attachExternalCancellationSource.cancellationToken);
			}

			// Token: 0x06000AA0 RID: 2720 RVA: 0x0002537E File Offset: 0x0002357E
			void IUniTaskSource.GetResult(short token)
			{
				this.core.GetResult(token);
			}

			// Token: 0x06000AA1 RID: 2721 RVA: 0x0002538D File Offset: 0x0002358D
			public T GetResult(short token)
			{
				return this.core.GetResult(token);
			}

			// Token: 0x06000AA2 RID: 2722 RVA: 0x0002539B File Offset: 0x0002359B
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000AA3 RID: 2723 RVA: 0x000253A9 File Offset: 0x000235A9
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000AA4 RID: 2724 RVA: 0x000253B9 File Offset: 0x000235B9
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x040003B5 RID: 949
			private static readonly Action<object> cancellationCallbackDelegate = new Action<object>(UniTaskExtensions.AttachExternalCancellationSource<T>.CancellationCallback);

			// Token: 0x040003B6 RID: 950
			private CancellationToken cancellationToken;

			// Token: 0x040003B7 RID: 951
			private CancellationTokenRegistration tokenRegistration;

			// Token: 0x040003B8 RID: 952
			private UniTaskCompletionSourceCore<T> core;
		}

		// Token: 0x020001C9 RID: 457
		private sealed class ToCoroutineEnumerator : IEnumerator
		{
			// Token: 0x06000AA6 RID: 2726 RVA: 0x000253D9 File Offset: 0x000235D9
			public ToCoroutineEnumerator(UniTask task, Action<Exception> exceptionHandler)
			{
				this.completed = false;
				this.exceptionHandler = exceptionHandler;
				this.task = task;
			}

			// Token: 0x06000AA7 RID: 2727 RVA: 0x000253F8 File Offset: 0x000235F8
			private UniTaskVoid RunTask(UniTask task)
			{
				UniTaskExtensions.ToCoroutineEnumerator.<RunTask>d__6 <RunTask>d__;
				<RunTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunTask>d__.<>4__this = this;
				<RunTask>d__.task = task;
				<RunTask>d__.<>1__state = -1;
				<RunTask>d__.<>t__builder.Start<UniTaskExtensions.ToCoroutineEnumerator.<RunTask>d__6>(ref <RunTask>d__);
				return <RunTask>d__.<>t__builder.Task;
			}

			// Token: 0x17000075 RID: 117
			// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00025443 File Offset: 0x00023643
			public object Current
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06000AA9 RID: 2729 RVA: 0x00025448 File Offset: 0x00023648
			public bool MoveNext()
			{
				if (!this.isStarted)
				{
					this.isStarted = true;
					this.RunTask(this.task).Forget();
				}
				if (this.exception != null)
				{
					this.exception.Throw();
					return false;
				}
				return !this.completed;
			}

			// Token: 0x06000AAA RID: 2730 RVA: 0x00025496 File Offset: 0x00023696
			void IEnumerator.Reset()
			{
			}

			// Token: 0x040003B9 RID: 953
			private bool completed;

			// Token: 0x040003BA RID: 954
			private UniTask task;

			// Token: 0x040003BB RID: 955
			private Action<Exception> exceptionHandler;

			// Token: 0x040003BC RID: 956
			private bool isStarted;

			// Token: 0x040003BD RID: 957
			private ExceptionDispatchInfo exception;
		}

		// Token: 0x020001CA RID: 458
		private sealed class ToCoroutineEnumerator<T> : IEnumerator
		{
			// Token: 0x06000AAB RID: 2731 RVA: 0x00025498 File Offset: 0x00023698
			public ToCoroutineEnumerator(UniTask<T> task, Action<T> resultHandler, Action<Exception> exceptionHandler)
			{
				this.completed = false;
				this.task = task;
				this.resultHandler = resultHandler;
				this.exceptionHandler = exceptionHandler;
			}

			// Token: 0x06000AAC RID: 2732 RVA: 0x000254BC File Offset: 0x000236BC
			private UniTaskVoid RunTask(UniTask<T> task)
			{
				UniTaskExtensions.ToCoroutineEnumerator<T>.<RunTask>d__8 <RunTask>d__;
				<RunTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunTask>d__.<>4__this = this;
				<RunTask>d__.task = task;
				<RunTask>d__.<>1__state = -1;
				<RunTask>d__.<>t__builder.Start<UniTaskExtensions.ToCoroutineEnumerator<T>.<RunTask>d__8>(ref <RunTask>d__);
				return <RunTask>d__.<>t__builder.Task;
			}

			// Token: 0x17000076 RID: 118
			// (get) Token: 0x06000AAD RID: 2733 RVA: 0x00025507 File Offset: 0x00023707
			public object Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x06000AAE RID: 2734 RVA: 0x00025510 File Offset: 0x00023710
			public bool MoveNext()
			{
				if (!this.isStarted)
				{
					this.isStarted = true;
					this.RunTask(this.task).Forget();
				}
				if (this.exception != null)
				{
					this.exception.Throw();
					return false;
				}
				return !this.completed;
			}

			// Token: 0x06000AAF RID: 2735 RVA: 0x0002555E File Offset: 0x0002375E
			void IEnumerator.Reset()
			{
			}

			// Token: 0x040003BE RID: 958
			private bool completed;

			// Token: 0x040003BF RID: 959
			private Action<T> resultHandler;

			// Token: 0x040003C0 RID: 960
			private Action<Exception> exceptionHandler;

			// Token: 0x040003C1 RID: 961
			private bool isStarted;

			// Token: 0x040003C2 RID: 962
			private UniTask<T> task;

			// Token: 0x040003C3 RID: 963
			private object current;

			// Token: 0x040003C4 RID: 964
			private ExceptionDispatchInfo exception;
		}
	}
}
