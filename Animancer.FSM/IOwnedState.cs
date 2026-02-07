using System;

namespace Animancer.FSM
{
	// Token: 0x02000005 RID: 5
	public interface IOwnedState<TState> : IState where TState : class, IState
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12
		StateMachine<TState> OwnerStateMachine { get; }
	}
}
