using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x0200012B RID: 299
	internal sealed class AsyncUniTask<TStateMachine> : IStateMachineRunnerPromise, IUniTaskSource, IValueTaskSource, ITaskPoolNode<AsyncUniTask<TStateMachine>> where TStateMachine : IAsyncStateMachine
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0000FDC4 File Offset: 0x0000DFC4
		public Action MoveNext { get; }

		// Token: 0x060006D3 RID: 1747 RVA: 0x0000FDCC File Offset: 0x0000DFCC
		private AsyncUniTask()
		{
			this.MoveNext = new Action(this.Run);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0000FDE8 File Offset: 0x0000DFE8
		public static void SetStateMachine(ref TStateMachine stateMachine, ref IStateMachineRunnerPromise runnerPromiseFieldRef)
		{
			AsyncUniTask<TStateMachine> asyncUniTask;
			if (!AsyncUniTask<TStateMachine>.pool.TryPop(out asyncUniTask))
			{
				asyncUniTask = new AsyncUniTask<TStateMachine>();
			}
			runnerPromiseFieldRef = asyncUniTask;
			asyncUniTask.stateMachine = stateMachine;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0000FE18 File Offset: 0x0000E018
		public ref AsyncUniTask<TStateMachine> NextNode
		{
			get
			{
				return ref this.nextNode;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0000FE20 File Offset: 0x0000E020
		static AsyncUniTask()
		{
			TaskPool.RegisterSizeGetter(typeof(AsyncUniTask<TStateMachine>), () => AsyncUniTask<TStateMachine>.pool.Size);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0000FE41 File Offset: 0x0000E041
		private void Return()
		{
			this.core.Reset();
			this.stateMachine = default(TStateMachine);
			AsyncUniTask<TStateMachine>.pool.TryPush(this);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0000FE66 File Offset: 0x0000E066
		private bool TryReturn()
		{
			this.core.Reset();
			this.stateMachine = default(TStateMachine);
			return AsyncUniTask<TStateMachine>.pool.TryPush(this);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0000FE8A File Offset: 0x0000E08A
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Run()
		{
			this.stateMachine.MoveNext();
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0000FE9D File Offset: 0x0000E09D
		public UniTask Task
		{
			[DebuggerHidden]
			get
			{
				return new UniTask(this, this.core.Version);
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0000FEB0 File Offset: 0x0000E0B0
		[DebuggerHidden]
		public void SetResult()
		{
			this.core.TrySetResult(AsyncUnit.Default);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0000FEC3 File Offset: 0x0000E0C3
		[DebuggerHidden]
		public void SetException(Exception exception)
		{
			this.core.TrySetException(exception);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0000FED4 File Offset: 0x0000E0D4
		[DebuggerHidden]
		public void GetResult(short token)
		{
			try
			{
				this.core.GetResult(token);
			}
			finally
			{
				this.TryReturn();
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0000FF08 File Offset: 0x0000E108
		[DebuggerHidden]
		public UniTaskStatus GetStatus(short token)
		{
			return this.core.GetStatus(token);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0000FF16 File Offset: 0x0000E116
		[DebuggerHidden]
		public UniTaskStatus UnsafeGetStatus()
		{
			return this.core.UnsafeGetStatus();
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0000FF23 File Offset: 0x0000E123
		[DebuggerHidden]
		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			this.core.OnCompleted(continuation, state, token);
		}

		// Token: 0x04000165 RID: 357
		private static TaskPool<AsyncUniTask<TStateMachine>> pool;

		// Token: 0x04000167 RID: 359
		private TStateMachine stateMachine;

		// Token: 0x04000168 RID: 360
		private UniTaskCompletionSourceCore<AsyncUnit> core;

		// Token: 0x04000169 RID: 361
		private AsyncUniTask<TStateMachine> nextNode;
	}
}
