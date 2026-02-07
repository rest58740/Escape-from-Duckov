using System;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E2 RID: 226
	public interface IStateCallbackReceiver
	{
		// Token: 0x06000430 RID: 1072
		void OnStateEnter(IState state);

		// Token: 0x06000431 RID: 1073
		void OnStateUpdate(IState state);

		// Token: 0x06000432 RID: 1074
		void OnStateExit(IState state);
	}
}
