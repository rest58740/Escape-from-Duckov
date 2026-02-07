using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x02000123 RID: 291
	[StructLayout(LayoutKind.Auto)]
	public struct AsyncUniTaskMethodBuilder
	{
		// Token: 0x060006A4 RID: 1700 RVA: 0x0000FA24 File Offset: 0x0000DC24
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AsyncUniTaskMethodBuilder Create()
		{
			return default(AsyncUniTaskMethodBuilder);
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0000FA3A File Offset: 0x0000DC3A
		public UniTask Task
		{
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (this.runnerPromise != null)
				{
					return this.runnerPromise.Task;
				}
				if (this.ex != null)
				{
					return UniTask.FromException(this.ex);
				}
				return UniTask.CompletedTask;
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0000FA69 File Offset: 0x0000DC69
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetException(Exception exception)
		{
			if (this.runnerPromise == null)
			{
				this.ex = exception;
				return;
			}
			this.runnerPromise.SetException(exception);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0000FA87 File Offset: 0x0000DC87
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetResult()
		{
			if (this.runnerPromise != null)
			{
				this.runnerPromise.SetResult();
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0000FA9C File Offset: 0x0000DC9C
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
		{
			if (this.runnerPromise == null)
			{
				AsyncUniTask<TStateMachine>.SetStateMachine(ref stateMachine, ref this.runnerPromise);
			}
			awaiter.OnCompleted(this.runnerPromise.MoveNext);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0000FAC9 File Offset: 0x0000DCC9
		[DebuggerHidden]
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
		{
			if (this.runnerPromise == null)
			{
				AsyncUniTask<TStateMachine>.SetStateMachine(ref stateMachine, ref this.runnerPromise);
			}
			awaiter.UnsafeOnCompleted(this.runnerPromise.MoveNext);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0000FAF6 File Offset: 0x0000DCF6
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			stateMachine.MoveNext();
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0000FB04 File Offset: 0x0000DD04
		[DebuggerHidden]
		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		// Token: 0x0400015B RID: 347
		private IStateMachineRunnerPromise runnerPromise;

		// Token: 0x0400015C RID: 348
		private Exception ex;
	}
}
