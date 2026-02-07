using System;

namespace Animancer.FSM
{
	// Token: 0x02000004 RID: 4
	public interface IState
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8
		bool CanEnterState { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9
		bool CanExitState { get; }

		// Token: 0x0600000A RID: 10
		void OnEnterState();

		// Token: 0x0600000B RID: 11
		void OnExitState();
	}
}
