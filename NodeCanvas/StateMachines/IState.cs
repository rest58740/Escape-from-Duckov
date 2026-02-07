using System;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E1 RID: 225
	public interface IState
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000429 RID: 1065
		string name { get; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600042A RID: 1066
		string tag { get; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600042B RID: 1067
		float elapsedTime { get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600042C RID: 1068
		FSM FSM { get; }

		// Token: 0x0600042D RID: 1069
		FSMConnection[] GetTransitions();

		// Token: 0x0600042E RID: 1070
		bool CheckTransitions();

		// Token: 0x0600042F RID: 1071
		void Finish(bool success);
	}
}
