using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x0200012A RID: 298
	internal sealed class AsyncUniTaskVoid<TStateMachine> : IStateMachineRunner, ITaskPoolNode<AsyncUniTaskVoid<TStateMachine>>, IUniTaskSource, IValueTaskSource where TStateMachine : IAsyncStateMachine
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0000FD10 File Offset: 0x0000DF10
		public Action MoveNext { get; }

		// Token: 0x060006C8 RID: 1736 RVA: 0x0000FD18 File Offset: 0x0000DF18
		public AsyncUniTaskVoid()
		{
			this.MoveNext = new Action(this.Run);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0000FD34 File Offset: 0x0000DF34
		public static void SetStateMachine(ref TStateMachine stateMachine, ref IStateMachineRunner runnerFieldRef)
		{
			AsyncUniTaskVoid<TStateMachine> asyncUniTaskVoid;
			if (!AsyncUniTaskVoid<TStateMachine>.pool.TryPop(out asyncUniTaskVoid))
			{
				asyncUniTaskVoid = new AsyncUniTaskVoid<TStateMachine>();
			}
			runnerFieldRef = asyncUniTaskVoid;
			asyncUniTaskVoid.stateMachine = stateMachine;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0000FD64 File Offset: 0x0000DF64
		static AsyncUniTaskVoid()
		{
			TaskPool.RegisterSizeGetter(typeof(AsyncUniTaskVoid<TStateMachine>), () => AsyncUniTaskVoid<TStateMachine>.pool.Size);
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0000FD85 File Offset: 0x0000DF85
		public ref AsyncUniTaskVoid<TStateMachine> NextNode
		{
			get
			{
				return ref this.nextNode;
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0000FD8D File Offset: 0x0000DF8D
		public void Return()
		{
			this.stateMachine = default(TStateMachine);
			AsyncUniTaskVoid<TStateMachine>.pool.TryPush(this);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0000FDA7 File Offset: 0x0000DFA7
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Run()
		{
			this.stateMachine.MoveNext();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0000FDBA File Offset: 0x0000DFBA
		UniTaskStatus IUniTaskSource.GetStatus(short token)
		{
			return UniTaskStatus.Pending;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0000FDBD File Offset: 0x0000DFBD
		UniTaskStatus IUniTaskSource.UnsafeGetStatus()
		{
			return UniTaskStatus.Pending;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0000FDC0 File Offset: 0x0000DFC0
		void IUniTaskSource.OnCompleted(Action<object> continuation, object state, short token)
		{
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0000FDC2 File Offset: 0x0000DFC2
		void IUniTaskSource.GetResult(short token)
		{
		}

		// Token: 0x04000161 RID: 353
		private static TaskPool<AsyncUniTaskVoid<TStateMachine>> pool;

		// Token: 0x04000162 RID: 354
		private TStateMachine stateMachine;

		// Token: 0x04000164 RID: 356
		private AsyncUniTaskVoid<TStateMachine> nextNode;
	}
}
