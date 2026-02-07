using System;

namespace Animancer.FSM
{
	// Token: 0x0200000A RID: 10
	public struct StateChange<TState> : IDisposable where TState : class, IState
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000022BA File Offset: 0x000004BA
		public static bool IsActive
		{
			get
			{
				return StateChange<TState>._Current._StateMachine != null;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000022C9 File Offset: 0x000004C9
		public static StateMachine<TState> StateMachine
		{
			get
			{
				return StateChange<TState>._Current._StateMachine;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000022D5 File Offset: 0x000004D5
		public static TState PreviousState
		{
			get
			{
				return StateChange<TState>._Current._PreviousState;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000022E1 File Offset: 0x000004E1
		public static TState NextState
		{
			get
			{
				return StateChange<TState>._Current._NextState;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000022ED File Offset: 0x000004ED
		internal StateChange(StateMachine<TState> stateMachine, TState previousState, TState nextState)
		{
			this = StateChange<TState>._Current;
			StateChange<TState>._Current._StateMachine = stateMachine;
			StateChange<TState>._Current._PreviousState = previousState;
			StateChange<TState>._Current._NextState = nextState;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000231B File Offset: 0x0000051B
		public void Dispose()
		{
			StateChange<TState>._Current = this;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002328 File Offset: 0x00000528
		public override string ToString()
		{
			if (!StateChange<TState>.IsActive)
			{
				return "StateChange<" + typeof(TState).FullName + "(Not Currently Active)";
			}
			return "StateChange<" + typeof(TState).FullName + string.Format(">({0}='{1}'", "PreviousState", this._PreviousState) + string.Format(", {0}='{1}')", "NextState", this._NextState);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000023A8 File Offset: 0x000005A8
		public static string CurrentToString()
		{
			return StateChange<TState>._Current.ToString();
		}

		// Token: 0x0400000A RID: 10
		[ThreadStatic]
		private static StateChange<TState> _Current;

		// Token: 0x0400000B RID: 11
		private StateMachine<TState> _StateMachine;

		// Token: 0x0400000C RID: 12
		private TState _PreviousState;

		// Token: 0x0400000D RID: 13
		private TState _NextState;
	}
}
