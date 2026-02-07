using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x02000125 RID: 293
	[StructLayout(LayoutKind.Auto)]
	public struct AsyncUniTaskVoidMethodBuilder
	{
		// Token: 0x060006B4 RID: 1716 RVA: 0x0000FBFC File Offset: 0x0000DDFC
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AsyncUniTaskVoidMethodBuilder Create()
		{
			return default(AsyncUniTaskVoidMethodBuilder);
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0000FC14 File Offset: 0x0000DE14
		public UniTaskVoid Task
		{
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return default(UniTaskVoid);
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0000FC2A File Offset: 0x0000DE2A
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetException(Exception exception)
		{
			if (this.runner != null)
			{
				this.runner.Return();
				this.runner = null;
			}
			UniTaskScheduler.PublishUnobservedTaskException(exception);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0000FC4C File Offset: 0x0000DE4C
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetResult()
		{
			if (this.runner != null)
			{
				this.runner.Return();
				this.runner = null;
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0000FC68 File Offset: 0x0000DE68
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
		{
			if (this.runner == null)
			{
				AsyncUniTaskVoid<TStateMachine>.SetStateMachine(ref stateMachine, ref this.runner);
			}
			awaiter.OnCompleted(this.runner.MoveNext);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0000FC95 File Offset: 0x0000DE95
		[DebuggerHidden]
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
		{
			if (this.runner == null)
			{
				AsyncUniTaskVoid<TStateMachine>.SetStateMachine(ref stateMachine, ref this.runner);
			}
			awaiter.UnsafeOnCompleted(this.runner.MoveNext);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0000FCC2 File Offset: 0x0000DEC2
		[DebuggerHidden]
		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			stateMachine.MoveNext();
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0000FCD0 File Offset: 0x0000DED0
		[DebuggerHidden]
		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		// Token: 0x04000160 RID: 352
		private IStateMachineRunner runner;
	}
}
