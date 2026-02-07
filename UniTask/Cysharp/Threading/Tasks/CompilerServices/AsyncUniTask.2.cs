using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x0200012C RID: 300
	internal sealed class AsyncUniTask<TStateMachine, T> : IStateMachineRunnerPromise<T>, IUniTaskSource<T>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>, ITaskPoolNode<AsyncUniTask<TStateMachine, T>> where TStateMachine : IAsyncStateMachine
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0000FF33 File Offset: 0x0000E133
		public Action MoveNext { get; }

		// Token: 0x060006E2 RID: 1762 RVA: 0x0000FF3B File Offset: 0x0000E13B
		private AsyncUniTask()
		{
			this.MoveNext = new Action(this.Run);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0000FF58 File Offset: 0x0000E158
		public static void SetStateMachine(ref TStateMachine stateMachine, ref IStateMachineRunnerPromise<T> runnerPromiseFieldRef)
		{
			AsyncUniTask<TStateMachine, T> asyncUniTask;
			if (!AsyncUniTask<TStateMachine, T>.pool.TryPop(out asyncUniTask))
			{
				asyncUniTask = new AsyncUniTask<TStateMachine, T>();
			}
			runnerPromiseFieldRef = asyncUniTask;
			asyncUniTask.stateMachine = stateMachine;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0000FF88 File Offset: 0x0000E188
		public ref AsyncUniTask<TStateMachine, T> NextNode
		{
			get
			{
				return ref this.nextNode;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0000FF90 File Offset: 0x0000E190
		static AsyncUniTask()
		{
			TaskPool.RegisterSizeGetter(typeof(AsyncUniTask<TStateMachine, T>), () => AsyncUniTask<TStateMachine, T>.pool.Size);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0000FFB1 File Offset: 0x0000E1B1
		private void Return()
		{
			this.core.Reset();
			this.stateMachine = default(TStateMachine);
			AsyncUniTask<TStateMachine, T>.pool.TryPush(this);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0000FFD6 File Offset: 0x0000E1D6
		private bool TryReturn()
		{
			this.core.Reset();
			this.stateMachine = default(TStateMachine);
			return AsyncUniTask<TStateMachine, T>.pool.TryPush(this);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0000FFFA File Offset: 0x0000E1FA
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Run()
		{
			this.stateMachine.MoveNext();
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001000D File Offset: 0x0000E20D
		public UniTask<T> Task
		{
			[DebuggerHidden]
			get
			{
				return new UniTask<T>(this, this.core.Version);
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00010020 File Offset: 0x0000E220
		[DebuggerHidden]
		public void SetResult(T result)
		{
			this.core.TrySetResult(result);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001002F File Offset: 0x0000E22F
		[DebuggerHidden]
		public void SetException(Exception exception)
		{
			this.core.TrySetException(exception);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00010040 File Offset: 0x0000E240
		[DebuggerHidden]
		public T GetResult(short token)
		{
			T result;
			try
			{
				result = this.core.GetResult(token);
			}
			finally
			{
				this.TryReturn();
			}
			return result;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00010078 File Offset: 0x0000E278
		[DebuggerHidden]
		void IUniTaskSource.GetResult(short token)
		{
			this.GetResult(token);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00010082 File Offset: 0x0000E282
		[DebuggerHidden]
		public UniTaskStatus GetStatus(short token)
		{
			return this.core.GetStatus(token);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00010090 File Offset: 0x0000E290
		[DebuggerHidden]
		public UniTaskStatus UnsafeGetStatus()
		{
			return this.core.UnsafeGetStatus();
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001009D File Offset: 0x0000E29D
		[DebuggerHidden]
		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			this.core.OnCompleted(continuation, state, token);
		}

		// Token: 0x0400016A RID: 362
		private static TaskPool<AsyncUniTask<TStateMachine, T>> pool;

		// Token: 0x0400016C RID: 364
		private TStateMachine stateMachine;

		// Token: 0x0400016D RID: 365
		private UniTaskCompletionSourceCore<T> core;

		// Token: 0x0400016E RID: 366
		private AsyncUniTask<TStateMachine, T> nextNode;
	}
}
