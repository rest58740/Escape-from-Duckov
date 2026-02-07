using System;
using UnityEngine;

namespace Animancer.FSM
{
	// Token: 0x02000007 RID: 7
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer.FSM/StateExtensions")]
	public static class StateExtensions
	{
		// Token: 0x06000012 RID: 18 RVA: 0x0000212C File Offset: 0x0000032C
		public static TState GetPreviousState<TState>(this TState state) where TState : class, IState
		{
			return StateChange<TState>.PreviousState;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002133 File Offset: 0x00000333
		public static TState GetNextState<TState>(this TState state) where TState : class, IState
		{
			return StateChange<TState>.NextState;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000213A File Offset: 0x0000033A
		public static bool IsCurrentState<TState>(this TState state) where TState : class, IOwnedState<TState>
		{
			return state.OwnerStateMachine.CurrentState == state;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002159 File Offset: 0x00000359
		public static bool TryEnterState<TState>(this TState state) where TState : class, IOwnedState<TState>
		{
			return state.OwnerStateMachine.TrySetState(state);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000216C File Offset: 0x0000036C
		public static bool TryReEnterState<TState>(this TState state) where TState : class, IOwnedState<TState>
		{
			return state.OwnerStateMachine.TryResetState(state);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000217F File Offset: 0x0000037F
		public static void ForceEnterState<TState>(this TState state) where TState : class, IOwnedState<TState>
		{
			state.OwnerStateMachine.ForceSetState(state);
		}

		// Token: 0x04000005 RID: 5
		public const string APIDocumentationURL = "https://kybernetik.com.au/animancer/api/Animancer.FSM/";
	}
}
