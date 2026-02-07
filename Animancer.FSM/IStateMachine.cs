using System;
using System.Collections;

namespace Animancer.FSM
{
	// Token: 0x0200000B RID: 11
	public interface IStateMachine
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002D RID: 45
		object CurrentState { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002E RID: 46
		object PreviousState { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002F RID: 47
		object NextState { get; }

		// Token: 0x06000030 RID: 48
		bool CanSetState(object state);

		// Token: 0x06000031 RID: 49
		object CanSetState(IList states);

		// Token: 0x06000032 RID: 50
		bool TrySetState(object state);

		// Token: 0x06000033 RID: 51
		bool TrySetState(IList states);

		// Token: 0x06000034 RID: 52
		bool TryResetState(object state);

		// Token: 0x06000035 RID: 53
		bool TryResetState(IList states);

		// Token: 0x06000036 RID: 54
		void ForceSetState(object state);

		// Token: 0x06000037 RID: 55
		void SetAllowNullStates(bool allow = true);
	}
}
