using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200000D RID: 13
	public static class CancellationTokenExtensions
	{
		// Token: 0x0600003B RID: 59 RVA: 0x000027F8 File Offset: 0x000009F8
		public static CancellationToken ToCancellationToken(this UniTask task)
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			CancellationTokenExtensions.ToCancellationTokenCore(task, cancellationTokenSource).Forget();
			return cancellationTokenSource.Token;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002820 File Offset: 0x00000A20
		public static CancellationToken ToCancellationToken(this UniTask task, CancellationToken linkToken)
		{
			if (linkToken.IsCancellationRequested)
			{
				return linkToken;
			}
			if (!linkToken.CanBeCanceled)
			{
				return task.ToCancellationToken();
			}
			CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[]
			{
				linkToken
			});
			CancellationTokenExtensions.ToCancellationTokenCore(task, cancellationTokenSource).Forget();
			return cancellationTokenSource.Token;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002871 File Offset: 0x00000A71
		public static CancellationToken ToCancellationToken<T>(this UniTask<T> task)
		{
			return task.AsUniTask().ToCancellationToken();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000287F File Offset: 0x00000A7F
		public static CancellationToken ToCancellationToken<T>(this UniTask<T> task, CancellationToken linkToken)
		{
			return task.AsUniTask().ToCancellationToken(linkToken);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002890 File Offset: 0x00000A90
		private static UniTaskVoid ToCancellationTokenCore(UniTask task, CancellationTokenSource cts)
		{
			CancellationTokenExtensions.<ToCancellationTokenCore>d__6 <ToCancellationTokenCore>d__;
			<ToCancellationTokenCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<ToCancellationTokenCore>d__.task = task;
			<ToCancellationTokenCore>d__.cts = cts;
			<ToCancellationTokenCore>d__.<>1__state = -1;
			<ToCancellationTokenCore>d__.<>t__builder.Start<CancellationTokenExtensions.<ToCancellationTokenCore>d__6>(ref <ToCancellationTokenCore>d__);
			return <ToCancellationTokenCore>d__.<>t__builder.Task;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000028DC File Offset: 0x00000ADC
		public static ValueTuple<UniTask, CancellationTokenRegistration> ToUniTask(this CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTuple<UniTask, CancellationTokenRegistration>(UniTask.FromCanceled(cancellationToken), default(CancellationTokenRegistration));
			}
			UniTaskCompletionSource uniTaskCompletionSource = new UniTaskCompletionSource();
			return new ValueTuple<UniTask, CancellationTokenRegistration>(uniTaskCompletionSource.Task, cancellationToken.RegisterWithoutCaptureExecutionContext(CancellationTokenExtensions.cancellationTokenCallback, uniTaskCompletionSource));
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002924 File Offset: 0x00000B24
		private static void Callback(object state)
		{
			((UniTaskCompletionSource)state).TrySetResult();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002932 File Offset: 0x00000B32
		public static CancellationTokenAwaitable WaitUntilCanceled(this CancellationToken cancellationToken)
		{
			return new CancellationTokenAwaitable(cancellationToken);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000293C File Offset: 0x00000B3C
		public static CancellationTokenRegistration RegisterWithoutCaptureExecutionContext(this CancellationToken cancellationToken, Action callback)
		{
			bool flag = false;
			if (!ExecutionContext.IsFlowSuppressed())
			{
				ExecutionContext.SuppressFlow();
				flag = true;
			}
			CancellationTokenRegistration result;
			try
			{
				result = cancellationToken.Register(callback, false);
			}
			finally
			{
				if (flag)
				{
					ExecutionContext.RestoreFlow();
				}
			}
			return result;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002984 File Offset: 0x00000B84
		public static CancellationTokenRegistration RegisterWithoutCaptureExecutionContext(this CancellationToken cancellationToken, Action<object> callback, object state)
		{
			bool flag = false;
			if (!ExecutionContext.IsFlowSuppressed())
			{
				ExecutionContext.SuppressFlow();
				flag = true;
			}
			CancellationTokenRegistration result;
			try
			{
				result = cancellationToken.Register(callback, state, false);
			}
			finally
			{
				if (flag)
				{
					ExecutionContext.RestoreFlow();
				}
			}
			return result;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000029CC File Offset: 0x00000BCC
		public static CancellationTokenRegistration AddTo(this IDisposable disposable, CancellationToken cancellationToken)
		{
			return cancellationToken.RegisterWithoutCaptureExecutionContext(CancellationTokenExtensions.disposeCallback, disposable);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000029DA File Offset: 0x00000BDA
		private static void DisposeCallback(object state)
		{
			((IDisposable)state).Dispose();
		}

		// Token: 0x04000017 RID: 23
		private static readonly Action<object> cancellationTokenCallback = new Action<object>(CancellationTokenExtensions.Callback);

		// Token: 0x04000018 RID: 24
		private static readonly Action<object> disposeCallback = new Action<object>(CancellationTokenExtensions.DisposeCallback);
	}
}
